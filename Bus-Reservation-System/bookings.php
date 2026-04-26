<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>MYBUS - Bookings Details</title>
    <?php require('inc/links.php') ?>
</head>

<body class="bg-light">
    <?php
    require('inc/header.php');

    if (!(isset($_SESSION['login']) && $_SESSION['login'] == true)) {
        redirect('index.php');
    }
    ?>

    <div class="container">
        <div class="row">
            <div class="col-12 my-5 px-4">
                <h2 class="fw-bold h-font">BOOKINGS</h2>
                <div style="font-size:14px;">
                    <a href="index.php" class="text-secondary text-decoration-none">HOME</a>
                    <span class="text-secondary"> > </span>
                    <a href="#" class="text-secondary text-decoration-none">BOOKINGS</a>
                </div>
            </div>

            <div class="col-12 px-4 mb-5">
                <div class="card border-0 shadow-sm rounded-3">
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-hover table-bordered align-middle text-center mb-0">
                                <thead style="background:#AD8B3A; color:white;">
                                    <tr>
                                        <th>#</th>
                                        <th>Bus</th>
                                        <th>Route</th>
                                        <th>Travel Date</th>
                                        <th>Time</th>
                                        <th>Seat No.</th>
                                        <th>Amount</th>
                                        <th>Order ID</th>
                                        <th>Status</th>
                                        <th>Booked On</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <?php
                                    $query = "SELECT bd.*, p.payment_id, p.order_id, p.trans_amt, p.trans_status, p.datentime, b.bus_name, b.departuretime, b.arrivaltime
                                            FROM `booking` bd
                                            LEFT JOIN `payment` p ON p.booking_id = bd.booking_id
                                            INNER JOIN `buses` b ON bd.bus_id = b.id
                                            WHERE bd.user_id = ?
                                            ORDER BY bd.booking_id DESC";

                                    $result = select($query, [$_SESSION['id']], 'i');

                                    if (mysqli_num_rows($result) == 0) {
                                        echo '
                                        <tr>
                                            <td colspan="11" class="text-center text-muted py-4">
                                                No bookings found.
                                            </td>
                                        </tr>';
                                    }

                                    $i = 1;

                                    while ($data = mysqli_fetch_assoc($result)) {
                                        $departuretime = date("h:ia", strtotime($data['departuretime']));
                                        $arrivaltime = date("h:ia", strtotime($data['arrivaltime']));
                                        $booked_date = !empty($data['datentime']) ? date("d-m-Y | h:ia", strtotime($data['datentime'])) : 'N/A';

                                        $payment_status = strtolower($data['trans_status'] ?? 'pending');

                                        $status_badge = '<span class="badge bg-warning text-dark">Pending - Pay at Terminal</span>';
                                        $action_btn = '-';

                                        if ($payment_status == 'success') {
                                            $status_badge = '<span class="badge bg-success">Paid</span>';
                                            $action_btn = "<a href='generate_pdf.php?gen_pdf&id={$data['booking_id']}' class='btn btn-dark btn-sm shadow-none'>Download PDF</a>";
                                        } 
                                        else if ($payment_status == 'pending') {
                                            $status_badge = '<span class="badge bg-warning text-dark">Pending - Pay at Terminal</span>';
                                            $action_btn = "<button class='btn btn-danger btn-sm shadow-none cancel-booking' data-payment-id='{$data['payment_id']}'>Cancel</button>";
                                        } 
                                        else if ($payment_status == 'cancelled' || $payment_status == 'canceled') {
                                            $status_badge = '<span class="badge bg-danger">Cancelled</span>';
                                            $action_btn = '-';
                                        } 
                                        else {
                                            $status_badge = '<span class="badge bg-secondary">'.ucfirst($payment_status).'</span>';
                                            $action_btn = '-';
                                        }

                                        echo "
                                        <tr data-payment-id='{$data['payment_id']}'>
                                            <td>{$i}</td>
                                            <td class='fw-semibold'>{$data['bus_name']}</td>
                                            <td>{$data['source']} → {$data['destination']}</td>
                                            <td>{$data['travel_date']}</td>
                                            <td>{$departuretime} - {$arrivaltime}</td>
                                            <td>{$data['seat_number']}</td>
                                            <td>₹{$data['trans_amt']}</td>
                                            <td>{$data['order_id']}</td>
                                            <td>{$status_badge}</td>
                                            <td>{$booked_date}</td>
                                            <td>{$action_btn}</td>
                                        </tr>";

                                        $i++;
                                    }
                                    ?>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Cancel Booking Modal -->
    <div class="modal fade" id="cancelBookingModal" tabindex="-1" aria-labelledby="cancelBookingModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content border-0 shadow rounded-3">
                <div class="modal-header">
                    <h5 class="modal-title" id="cancelBookingModalLabel">Confirm Cancellation</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

                <div class="modal-body">
                    Are you sure you want to cancel this booking?
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" id="confirmCancelBtn">Yes, Cancel Booking</button>
                </div>
            </div>
        </div>
    </div>

    <?php require('inc/footer.php'); ?>

   <!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>MYBUS - Booking Details</title>
    <?php require('inc/links.php') ?>
</head>

<body class="bg-light">

<?php
require('inc/header.php');

if (!(isset($_SESSION['login']) && $_SESSION['login'] == true)) {
    redirect('index.php');
}
?>

<div class="container">
    <div class="row">
        <div class="col-12 my-5 px-4">
            <h2 class="fw-bold h-font">BOOKINGS</h2>
            <div style="font-size:14px;">
                <a href="index.php" class="text-secondary text-decoration-none">HOME</a>
                <span class="text-secondary"> > </span>
                <a href="#" class="text-secondary text-decoration-none">BOOKINGS</a>
            </div>
        </div>

        <div class="col-12 px-4 mb-5">
            <div class="card border-0 shadow-sm rounded-3">
                <div class="card-body">
                    <div class="table-responsive">
                        <table class="table table-hover table-bordered align-middle text-center mb-0">
                            <thead style="background:#AD8B3A; color:white;">
                                <tr>
                                    <th>#</th>
                                    <th>Bus</th>
                                    <th>Route</th>
                                    <th>Travel Date</th>
                                    <th>Time</th>
                                    <th>Seat No.</th>
                                    <th>Amount</th>
                                    <th>Order ID</th>
                                    <th>Status</th>
                                    <th>Booked On</th>
                                    <th>Action</th>
                                </tr>
                            </thead>

                            <tbody>
                            <?php
                            $query = "SELECT bd.*, p.payment_id, p.order_id, p.trans_amt, p.trans_status, p.datentime, 
                                             b.bus_name, b.departuretime, b.arrivaltime
                                      FROM `booking` bd
                                      LEFT JOIN `payment` p ON p.booking_id = bd.booking_id
                                      INNER JOIN `buses` b ON bd.bus_id = b.id
                                      WHERE bd.user_id = ?
                                      ORDER BY bd.booking_id DESC";

                            $result = select($query, [$_SESSION['id']], 'i');

                            if (mysqli_num_rows($result) == 0) {
                                echo '
                                <tr>
                                    <td colspan="11" class="text-center text-muted py-4">
                                        No bookings found.
                                    </td>
                                </tr>';
                            }

                            $i = 1;

                            while ($data = mysqli_fetch_assoc($result)) {
                                $departuretime = date("h:ia", strtotime($data['departuretime']));
                                $arrivaltime = date("h:ia", strtotime($data['arrivaltime']));
                                $booked_date = !empty($data['datentime']) ? date("d-m-Y | h:ia", strtotime($data['datentime'])) : 'N/A';

                                $payment_status = strtolower($data['trans_status'] ?? 'pending');

                                $status_badge = '<span class="badge bg-warning text-dark">Pending - Pay at Terminal</span>';
                                $action_btn = '-';

                                if ($payment_status == 'success') {
                                    $status_badge = '<span class="badge bg-success">Paid</span>';
                                    $action_btn = "<a href='generate_pdf.php?gen_pdf&id={$data['booking_id']}' class='btn btn-dark btn-sm shadow-none'>Download PDF</a>";
                                } 
                                else if ($payment_status == 'pending') {
                                    $status_badge = '<span class="badge bg-warning text-dark">Pending - Pay at Terminal</span>';

                                    if (!empty($data['payment_id'])) {
                                        $action_btn = "
                                            <button class='btn btn-danger btn-sm shadow-none cancel-booking' 
                                                data-payment-id='{$data['payment_id']}'>
                                                Cancel
                                            </button>";
                                    } else {
                                        $action_btn = '-';
                                    }
                                } 
                                else if ($payment_status == 'cancelled' || $payment_status == 'canceled') {
                                    $status_badge = '<span class="badge bg-danger">Cancelled</span>';
                                    $action_btn = '-';
                                } 
                                else {
                                    $status_badge = '<span class="badge bg-secondary">'.ucfirst($payment_status).'</span>';
                                    $action_btn = '-';
                                }

                                echo "
                                <tr>
                                    <td>{$i}</td>
                                    <td class='fw-semibold'>{$data['bus_name']}</td>
                                    <td>{$data['source']} → {$data['destination']}</td>
                                    <td>{$data['travel_date']}</td>
                                    <td>{$departuretime} - {$arrivaltime}</td>
                                    <td>{$data['seat_number']}</td>
                                    <td>₹{$data['trans_amt']}</td>
                                    <td>{$data['order_id']}</td>
                                    <td>{$status_badge}</td>
                                    <td>{$booked_date}</td>
                                    <td>{$action_btn}</td>
                                </tr>";

                                $i++;
                            }
                            ?>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Cancel Booking Modal -->
<div class="modal fade" id="cancelBookingModal" tabindex="-1" aria-labelledby="cancelBookingModalLabel"
    aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-0 shadow rounded-3">
            <div class="modal-header">
                <h5 class="modal-title" id="cancelBookingModalLabel">Confirm Cancellation</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>

            <div class="modal-body">
                Are you sure you want to cancel this booking?
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No</button>
                <button type="button" class="btn btn-danger" id="confirmCancelBtn">
                    Yes, Cancel Booking
                </button>
            </div>
        </div>
    </div>
</div>

<?php require('inc/footer.php'); ?>

<script>
document.addEventListener('DOMContentLoaded', function () {
    const cancelButtons = document.querySelectorAll('.cancel-booking');
    const confirmCancelBtn = document.getElementById('confirmCancelBtn');
    const cancelModalElement = document.getElementById('cancelBookingModal');

    let paymentIdToCancel = null;
    let cancelModal = null;

    if (cancelModalElement) {
        cancelModal = new bootstrap.Modal(cancelModalElement);
    }

    cancelButtons.forEach(button => {
        button.addEventListener('click', function () {
            paymentIdToCancel = this.getAttribute('data-payment-id');

            if (cancelModal) {
                cancelModal.show();
            }
        });
    });

    if (confirmCancelBtn) {
        confirmCancelBtn.addEventListener('click', function () {
            if (!paymentIdToCancel) {
                showAlert('danger', 'Invalid booking selected.');
                return;
            }

            confirmCancelBtn.disabled = true;
            confirmCancelBtn.innerText = 'Cancelling...';

            const formData = new FormData();
            formData.append('payment_id', paymentIdToCancel);

            fetch('ajax/cancel_booking.php', {
                method: 'POST',
                body: formData
            })
            .then(response => response.text())
            .then(data => {
                console.log('Cancel Response:', data);

                if (data.trim() === 'success') {
                    if (cancelModal) {
                        cancelModal.hide();
                    }

                    showAlert('success', 'Booking cancelled successfully!');

                    setTimeout(() => {
                        location.reload();
                    }, 1800);
                } else {
                    showAlert('danger', data);
                }
            })
            .catch(error => {
                console.error('Cancel Error:', error);
                showAlert('danger', 'Error cancelling booking.');
            })
            .finally(() => {
                confirmCancelBtn.disabled = false;
                confirmCancelBtn.innerText = 'Yes, Cancel Booking';
            });
        });
    }
});
</script>

</body>
</html>
</body>

</html>