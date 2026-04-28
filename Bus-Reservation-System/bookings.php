<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>MYBUS - Booking Details</title>
    <?php require('inc/links.php') ?>

    <style>
        .booking-card {
            border-radius: 16px;
        }

        .table thead th {
            white-space: nowrap;
            vertical-align: middle;
        }

        .table tbody td {
            vertical-align: middle;
        }

        .badge-status {
            font-size: 12px;
            padding: 7px 10px;
        }

        .action-btn {
            white-space: nowrap;
        }
    </style>
</head>

<body class="bg-light">

<?php
require('inc/header.php');

if (!(isset($_SESSION['login']) && $_SESSION['login'] == true)) {
    redirect('index.php');
}

$user_id = $_SESSION['user_id'] ?? $_SESSION['id'] ?? 0;
?>

<div class="container">
    <div class="row">
        <div class="col-12 my-5 px-4">
            <h2 class="fw-bold h-font">BOOKINGS</h2>
            <div style="font-size:14px;">
                <a href="index.php" class="text-secondary text-decoration-none">HOME</a>
                <span class="text-secondary"> > </span>
                <span class="text-secondary">BOOKINGS</span>
            </div>
        </div>

        <div class="col-12 px-4 mb-5">
            <div class="card border-0 shadow-sm booking-card">
                <div class="card-body">

                    <div id="bookingsLoader" class="text-center py-5">
                        <div class="spinner-border text-info" role="status">
                            <span class="visually-hidden">Loading...</span>
                        </div>
                        <p class="text-muted mt-3 mb-0">Loading your bookings...</p>
                    </div>

                    <div class="table-responsive d-none" id="bookingsTableWrapper">
                        <table class="table table-hover table-bordered align-middle text-center mb-0">
                            <thead style="background:#AD8B3A; color:white;">
                                <tr>
                                    <th>#</th>
                                    <th>Booking Code</th>
                                    <th>Bus</th>
                                    <th>Route</th>
                                    <th>Travel Date</th>
                                    <th>Time</th>
                                    <th>Seat No.</th>
                                    <th>Amount</th>
                                    <th>Payment</th>
                                    <th>Reservation</th>
                                    <th>Check-in</th>
                                    <th>Boarding</th>
                                    <th>Booked On</th>
                                    <th>Action</th>
                                </tr>
                            </thead>

                            <tbody id="bookingsTableBody">
                            </tbody>
                        </table>
                    </div>

                    <div id="noBookingsBox" class="text-center py-5 d-none">
                        <h5 class="text-muted">No bookings found.</h5>
                        <p class="text-muted mb-3">You have not made any reservations yet.</p>
                        <a href="index.php#searchTrip" class="btn text-white custom-bg shadow-none">
                            Search Trips
                        </a>
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
                <br>
                <small class="text-muted">Only pending and unpaid bookings can be cancelled.</small>
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-secondary shadow-none" data-bs-dismiss="modal">No</button>
                <button type="button" class="btn btn-danger shadow-none" id="confirmCancelBtn">
                    Yes, Cancel Booking
                </button>
            </div>
        </div>
    </div>
</div>

<script>
document.addEventListener('DOMContentLoaded', function () {
    const API_BASE_URL = 'http://localhost:3000/api';
    const userId = <?php echo (int)$user_id; ?>;

    const bookingsLoader = document.getElementById('bookingsLoader');
    const bookingsTableWrapper = document.getElementById('bookingsTableWrapper');
    const bookingsTableBody = document.getElementById('bookingsTableBody');
    const noBookingsBox = document.getElementById('noBookingsBox');

    const cancelModalElement = document.getElementById('cancelBookingModal');
    const confirmCancelBtn = document.getElementById('confirmCancelBtn');

    let cancelModal = null;
    let selectedBookingId = null;

    if (cancelModalElement) {
        cancelModal = new bootstrap.Modal(cancelModalElement);
    }

    function formatTime(timeValue) {
        if (!timeValue) {
            return 'N/A';
        }

        const parts = timeValue.split(':');
        let hour = parseInt(parts[0]);
        const minute = parts[1];

        const ampm = hour >= 12 ? 'PM' : 'AM';
        hour = hour % 12;
        hour = hour ? hour : 12;

        return hour + ':' + minute + ' ' + ampm;
    }

    function formatDateTime(dateTimeValue) {
        if (!dateTimeValue) {
            return 'N/A';
        }

        const date = new Date(String(dateTimeValue).replace(' ', 'T'));

        if (isNaN(date.getTime())) {
            return dateTimeValue;
        }

        return date.toLocaleDateString() + ' | ' + date.toLocaleTimeString([], {
            hour: '2-digit',
            minute: '2-digit'
        });
    }

    function getPaymentBadge(status) {
        const value = (status || 'Pending').toLowerCase();

        if (value === 'paid') {
            return '<span class="badge bg-success badge-status">Paid</span>';
        }

        if (value === 'cancelled' || value === 'canceled') {
            return '<span class="badge bg-danger badge-status">Cancelled</span>';
        }

        if (value === 'pending') {
            return '<span class="badge bg-warning text-dark badge-status">Pending - Pay at Terminal</span>';
        }

        return '<span class="badge bg-secondary badge-status">' + status + '</span>';
    }

    function getReservationBadge(status) {
        const value = (status || 'Pending').toLowerCase();

        if (value === 'confirmed') {
            return '<span class="badge bg-success badge-status">Confirmed</span>';
        }

        if (value === 'completed') {
            return '<span class="badge bg-primary badge-status">Completed</span>';
        }

        if (value === 'cancelled' || value === 'canceled') {
            return '<span class="badge bg-danger badge-status">Cancelled</span>';
        }

        if (value === 'pending') {
            return '<span class="badge bg-warning text-dark badge-status">Pending</span>';
        }

        return '<span class="badge bg-secondary badge-status">' + status + '</span>';
    }

    function getSimpleBadge(status) {
        const value = (status || '').toLowerCase();

        if (value.includes('checked') || value.includes('boarded')) {
            return '<span class="badge bg-success badge-status">' + status + '</span>';
        }

        if (value.includes('not')) {
            return '<span class="badge bg-secondary badge-status">' + status + '</span>';
        }

        return '<span class="badge bg-info text-dark badge-status">' + status + '</span>';
    }

    function loadBookings() {
        bookingsLoader.classList.remove('d-none');
        bookingsTableWrapper.classList.add('d-none');
        noBookingsBox.classList.add('d-none');
        bookingsTableBody.innerHTML = '';

        fetch(`${API_BASE_URL}/my-bookings?user_id=${userId}`)
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                bookingsLoader.classList.add('d-none');

                if (!data.success) {
                    bookingsTableWrapper.classList.remove('d-none');
                    bookingsTableBody.innerHTML =
                        '<tr>' +
                            '<td colspan="14" class="text-center text-danger py-4">' +
                                data.message +
                            '</td>' +
                        '</tr>';
                    return;
                }

                if (data.count === 0) {
                    noBookingsBox.classList.remove('d-none');
                    return;
                }

                let html = '';

                data.bookings.forEach(function (booking, index) {
                    const departureTime = formatTime(booking.departure_time);
                    const arrivalTime = formatTime(booking.arrival_time);
                    const bookedOn = formatDateTime(booking.created_at);

                    const paymentStatus = booking.payment_status || 'Pending';
                    const reservationStatus = booking.reservation_status || 'Pending';
                    const checkinStatus = booking.checkin_status || 'Not Checked-in';
                    const boardingStatus = booking.boarding_status || 'Not Boarded';

                    let actionBtn = '-';

                    if (
                        paymentStatus.toLowerCase() === 'pending' &&
                        reservationStatus.toLowerCase() === 'pending' &&
                        checkinStatus.toLowerCase() !== 'checked-in' &&
                        boardingStatus.toLowerCase() !== 'boarded'
                    ) {
                        actionBtn =
                            '<button class="btn btn-danger btn-sm shadow-none action-btn cancel-booking" ' +
                                'data-booking-id="' + booking.booking_id + '">' +
                                'Cancel' +
                            '</button>';
                    } else if (
                        paymentStatus.toLowerCase() === 'paid' ||
                        reservationStatus.toLowerCase() === 'confirmed' ||
                        reservationStatus.toLowerCase() === 'completed'
                    ) {
                        actionBtn =
                            '<a href="generate_pdf.php?booking_id=' + booking.booking_id + '" ' +
                                'class="btn btn-dark btn-sm shadow-none action-btn">' +
                                'Print Ticket' +
                            '</a>';
                    }

                    html +=
                        '<tr>' +
                            '<td>' + (index + 1) + '</td>' +
                            '<td class="fw-semibold">' + (booking.booking_code || 'N/A') + '</td>' +
                            '<td>' + (booking.bus_number || 'N/A') + '</td>' +
                            '<td>' + (booking.origin || 'N/A') + ' → ' + (booking.destination || 'N/A') + '</td>' +
                            '<td>' + (booking.departure_date || 'N/A') + '</td>' +
                            '<td>' + departureTime + ' - ' + arrivalTime + '</td>' +
                            '<td>' + (booking.seat_no || 'N/A') + '</td>' +
                            '<td>₱' + parseFloat(booking.total_amount || booking.fare || 0).toFixed(2) + '</td>' +
                            '<td>' + getPaymentBadge(paymentStatus) + '</td>' +
                            '<td>' + getReservationBadge(reservationStatus) + '</td>' +
                            '<td>' + getSimpleBadge(checkinStatus) + '</td>' +
                            '<td>' + getSimpleBadge(boardingStatus) + '</td>' +
                            '<td>' + bookedOn + '</td>' +
                            '<td>' + actionBtn + '</td>' +
                        '</tr>';
                });

                bookingsTableBody.innerHTML = html;
                bookingsTableWrapper.classList.remove('d-none');

                attachCancelEvents();
            })
            .catch(function (error) {
                console.error('Bookings Load Error:', error);

                bookingsLoader.classList.add('d-none');
                bookingsTableWrapper.classList.remove('d-none');

                bookingsTableBody.innerHTML =
                    '<tr>' +
                        '<td colspan="14" class="text-center text-danger py-4">' +
                            'Unable to load bookings. Please make sure the Node API is running.' +
                        '</td>' +
                    '</tr>';
            });
    }

    function attachCancelEvents() {
        document.querySelectorAll('.cancel-booking').forEach(function (button) {
            button.addEventListener('click', function () {
                selectedBookingId = this.getAttribute('data-booking-id');

                if (cancelModal) {
                    cancelModal.show();
                }
            });
        });
    }

    if (confirmCancelBtn) {
        confirmCancelBtn.addEventListener('click', function () {
            if (!selectedBookingId) {
                showAlert('danger', 'Invalid booking selected.');
                return;
            }

            confirmCancelBtn.disabled = true;
            confirmCancelBtn.innerText = 'Cancelling...';

            fetch(`${API_BASE_URL}/cancel-booking`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    booking_id: parseInt(selectedBookingId),
                    user_id: userId
                })
            })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                if (data.success) {
                    if (cancelModal) {
                        cancelModal.hide();
                    }

                    showAlert('success', data.message);

                    selectedBookingId = null;

                    setTimeout(function () {
                        loadBookings();
                    }, 1000);
                } else {
                    showAlert('danger', data.message);
                }
            })
            .catch(function (error) {
                console.error('Cancel Error:', error);
                showAlert('danger', 'Error cancelling booking.');
            })
            .finally(function () {
                confirmCancelBtn.disabled = false;
                confirmCancelBtn.innerText = 'Yes, Cancel Booking';
            });
        });
    }

    loadBookings();
});
</script>

<?php require('inc/footer.php'); ?>

</body>
</html>