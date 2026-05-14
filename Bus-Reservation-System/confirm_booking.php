<?php
require_once(__DIR__ . '/inc/db_config.php');
require_once(__DIR__ . '/inc/essentials.php');

if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

if (!(isset($_SESSION['login']) && $_SESSION['login'] == true)) {
    redirect('index.php');
}

$user_id = $_SESSION['user_id'] ?? $_SESSION['id'] ?? 0;
$user_name = $_SESSION['full_name'] ?? $_SESSION['name'] ?? '';
$user_email = $_SESSION['email'] ?? '';
$user_phone = $_SESSION['phone_number'] ?? $_SESSION['phonenum'] ?? '';

$schedule_id = isset($_GET['schedule_id']) ? (int)$_GET['schedule_id'] : 0;
$passengers = isset($_GET['passengers']) ? (int)$_GET['passengers'] : 1;

if ($passengers <= 0) {
    $passengers = 1;
}

if ($schedule_id <= 0) {
    die("Invalid booking request.");
}

$query = "
    SELECT 
        s.schedule_id,
        s.departure_date,
        s.departure_time,
        s.arrival_time,
        s.fare,
        s.available_seats,
        s.trip_status,

        b.bus_id,
        b.bus_number,
        b.plate_number,
        b.bus_type,
        b.capacity,

        r.route_id,
        r.origin,
        r.destination,
        r.estimated_duration
    FROM schedules s
    INNER JOIN buses b ON s.bus_id = b.bus_id
    INNER JOIN routes r ON s.route_id = r.route_id
    WHERE s.schedule_id = ?
    LIMIT 1
";

$result = select($query, [$schedule_id], 'i');

if (!$result || mysqli_num_rows($result) == 0) {
    die("Schedule not found.");
}

$schedule = mysqli_fetch_assoc($result);

if (strtolower($schedule['trip_status']) == 'cancelled') {
    die("This trip is no longer available.");
}

if ($passengers > (int)$schedule['available_seats']) {
    die("Not enough available seats for this trip.");
}

$bookedSeats = [];

$booked_query = "
    SELECT seat_no 
    FROM bookings 
    WHERE schedule_id = ? 
    AND reservation_status != 'Cancelled'
";

$booked_result = select($booked_query, [$schedule_id], 'i');

if ($booked_result) {
    while ($row = mysqli_fetch_assoc($booked_result)) {
        $bookedSeats[] = (int)$row['seat_no'];
    }
}

$total_amount = (float)$schedule['fare'] * $passengers;

function format_time_confirm($time)
{
    if (!$time) {
        return "N/A";
    }

    return date("h:i A", strtotime($time));
}

function format_date_confirm($date)
{
    if (!$date) {
        return "N/A";
    }

    return date("F d, Y", strtotime($date));
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>MYBUS - Confirm Booking</title>
    <?php require('inc/links.php'); ?>

    <style>
        .summary-card {
            border-radius: 18px;
            overflow: hidden;
        }

        .summary-header {
            background: #172233;
            color: #fff;
            padding: 22px 28px;
        }

        .summary-body {
            padding: 28px;
        }

        .summary-label {
            font-size: 13px;
            color: #6c757d;
            margin-bottom: 3px;
        }

        .summary-value {
            font-weight: 700;
            color: #222;
        }

        .info-box {
            background: #f8f9fa;
            border-radius: 12px;
            padding: 14px;
            margin-bottom: 14px;
        }

        .gold-btn {
            background: #AD8B3A;
            color: white;
            border: none;
        }

        .gold-btn:hover {
            background: #8f722e;
            color: white;
        }

        .payment-note {
            background: #fff8e1;
            border-left: 5px solid #AD8B3A;
            padding: 12px 15px;
            border-radius: 8px;
            font-size: 14px;
        }

        .seat-note {
            background: #eef6ff;
            border-left: 5px solid #0d6efd;
            padding: 12px 15px;
            border-radius: 8px;
            font-size: 14px;
        }

        .online-payment-box {
            background: #f8f9fa;
            border: 1px solid #ddd;
            border-radius: 14px;
            padding: 18px;
        }

        .d-none {
            display: none !important;
        }

        .center-popup-icon {
            font-size: 55px;
        }

        .center-popup-btn {
            background: #AD8B3A;
            color: white;
            border: none;
        }

        .center-popup-btn:hover {
            background: #8f722e;
            color: white;
        }
    </style>
</head>

<body class="bg-light">

<?php require('inc/header.php'); ?>

<div class="container">
    <div class="row">
        <div class="col-12 my-5 px-4">
            <h2 class="fw-bold h-font">CONFIRM BOOKING</h2>
            <div style="font-size:14px;">
                <a href="index.php" class="text-secondary text-decoration-none">HOME</a>
                <span class="text-secondary"> > </span>
                <a href="bus.php" class="text-secondary text-decoration-none">SEARCH TRIPS</a>
                <span class="text-secondary"> > </span>
                <span class="text-secondary">CONFIRM BOOKING</span>
            </div>
        </div>

        <div class="col-lg-10 col-md-11 mx-auto mb-5">
            <div class="card border-0 shadow summary-card">
                <div class="summary-header">
                    <h4 class="mb-1 fw-bold">Booking Summary</h4>
                    <small>Please review your trip, seat, and payment details before confirming.</small>
                </div>

                <div class="summary-body">
                    <div class="row">
                        <div class="col-md-6">
                            <h5 class="fw-bold mb-3">Passenger Details</h5>

                            <div class="info-box">
                                <div class="summary-label">Passenger Name</div>
                                <div class="summary-value"><?php echo htmlspecialchars($user_name); ?></div>
                            </div>

                            <div class="info-box">
                                <div class="summary-label">Email</div>
                                <div class="summary-value"><?php echo htmlspecialchars($user_email); ?></div>
                            </div>

                            <div class="info-box">
                                <div class="summary-label">Phone</div>
                                <div class="summary-value"><?php echo htmlspecialchars($user_phone); ?></div>
                            </div>

                            <div class="info-box">
                                <div class="summary-label">No. of Passenger(s)</div>
                                <div class="summary-value"><?php echo $passengers; ?></div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <h5 class="fw-bold mb-3">Trip Details</h5>

                            <div class="info-box">
                                <div class="summary-label">Route</div>
                                <div class="summary-value">
                                    <?php echo htmlspecialchars($schedule['origin']); ?> → <?php echo htmlspecialchars($schedule['destination']); ?>
                                </div>
                            </div>

                            <div class="info-box">
                                <div class="summary-label">Bus</div>
                                <div class="summary-value">
                                    <?php echo htmlspecialchars($schedule['bus_number']); ?> |
                                    <?php echo htmlspecialchars($schedule['bus_type']); ?>
                                </div>
                            </div>

                            <div class="info-box">
                                <div class="summary-label">Travel Date and Time</div>
                                <div class="summary-value">
                                    <?php echo format_date_confirm($schedule['departure_date']); ?>
                                    |
                                    <?php echo format_time_confirm($schedule['departure_time']); ?>
                                    -
                                    <?php echo format_time_confirm($schedule['arrival_time']); ?>
                                </div>
                            </div>

                            <div class="info-box">
                                <div class="summary-label">Fare</div>
                                <div class="summary-value">
                                    ₱<?php echo number_format((float)$schedule['fare'], 2); ?> x <?php echo $passengers; ?> passenger(s)
                                </div>
                            </div>

                            <div class="info-box">
                                <div class="summary-label">Total Amount</div>
                                <div class="summary-value fs-5 text-success">
                                    ₱<?php echo number_format($total_amount, 2); ?>
                                </div>
                            </div>
                        </div>
                    </div>

                    <hr class="my-4">

                    <h5 class="fw-bold mb-3">Seat Selection</h5>

                    <div class="seat-note mb-3">
                        Please select <?php echo $passengers; ?> seat(s). Already booked seats are not shown.
                    </div>

                    <div class="row">
                        <?php for ($i = 1; $i <= $passengers; $i++) { ?>
                            <div class="col-md-4 mb-3">
                                <label class="form-label fw-bold">Seat for Passenger <?php echo $i; ?></label>
                                <select class="form-control shadow-none seat-select" required>
                                    <option value="">Select Seat</option>

                                    <?php
                                    for ($seat = 1; $seat <= (int)$schedule['capacity']; $seat++) {
                                        if (!in_array($seat, $bookedSeats)) {
                                            echo '<option value="' . $seat . '">Seat ' . $seat . '</option>';
                                        }
                                    }
                                    ?>
                                </select>
                            </div>
                        <?php } ?>
                    </div>

                    <hr class="my-4">

                    <h5 class="fw-bold mb-3">Payment Method</h5>

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-bold">Choose Payment Method</label>
                            <select id="payment_method" class="form-control shadow-none" required>
                                <option value="Pay at Terminal">Pay at Terminal</option>
                                <option value="Online Payment">Card / Bank Payment</option>
                            </select>
                        </div>
                    </div>

                    <div class="online-payment-box d-none mb-3" id="onlinePaymentBox">
                        <h6 class="fw-bold mb-3">Card / Bank Payment Information</h6>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Cardholder / Account Name</label>
                                <input type="text" id="account_name" class="form-control shadow-none"
                                    placeholder="Enter cardholder or account name">
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Card / Account Number</label>
                                <input type="text" id="account_number" class="form-control shadow-none"
                                    placeholder="Enter card or account number" maxlength="19">
                            </div>

                            <div class="col-md-3 mb-3">
                                <label class="form-label fw-bold">Expiry Date</label>
                                <input type="text" id="expiry_date" class="form-control shadow-none"
                                    placeholder="MM/YY" maxlength="5">
                            </div>

                            <div class="col-md-3 mb-3">
                                <label class="form-label fw-bold">CVV</label>
                                <input type="password" id="cvv" class="form-control shadow-none"
                                    placeholder="CVV" maxlength="4">
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label fw-bold">Payment Type</label>
                                <select id="online_payment_type" class="form-control shadow-none">
                                    <option value="Card">Card</option>
                                    <option value="Bank">Bank</option>
                                </select>
                            </div>
                        </div>

                        <small class="text-muted">
                            Demo payment only. Card details are validated on the page and are not stored.
                        </small>
                    </div>

                    <div class="payment-note mb-4" id="paymentNote">
                        <strong>Pay at Terminal:</strong>
                        Your booking will be marked as pending. Please pay at the terminal.
                        Your ticket will only be available after admin confirms your payment.
                    </div>

                    <div class="d-flex gap-2">
                        <button type="button" id="confirmBookingBtn" class="btn gold-btn px-4 shadow-none">
                            Confirm Booking
                        </button>

                        <a href="bus.php?view=all" class="btn btn-dark px-4 shadow-none">
                            Back
                        </a>
                    </div>

                    <div class="mt-3 d-none" id="bookingLoader">
                        <div class="spinner-border text-warning" role="status"></div>
                        <span class="ms-2">Processing your booking...</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Booking Message Modal -->
<div class="modal fade" id="bookingMessageModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-0 shadow rounded-4">
            <div class="modal-body text-center p-5">
                <div id="bookingMessageIcon" class="center-popup-icon mb-3">
                    ✅
                </div>

                <h5 class="fw-bold mb-2" id="bookingMessageTitle">
                    Booking Created
                </h5>

                <p class="text-muted mb-4" id="bookingMessageText">
                    Your booking has been created successfully.
                </p>

                <button type="button" class="btn center-popup-btn px-5 fw-semibold shadow-none"
                    id="bookingMessageOkBtn">
                    OK
                </button>
            </div>
        </div>
    </div>
</div>

<?php require('inc/footer.php'); ?>

<script>
document.addEventListener('DOMContentLoaded', function () {
    const CONFIRM_BOOKING_API_BASE_URL = "http://localhost:3000/api";

    const paymentMethod = document.getElementById('payment_method');
    const onlinePaymentBox = document.getElementById('onlinePaymentBox');
    const paymentNote = document.getElementById('paymentNote');

    const accountName = document.getElementById('account_name');
    const accountNumber = document.getElementById('account_number');
    const expiryDate = document.getElementById('expiry_date');
    const cvv = document.getElementById('cvv');
    const onlinePaymentType = document.getElementById('online_payment_type');

    const confirmBookingBtn = document.getElementById('confirmBookingBtn');
    const bookingLoader = document.getElementById('bookingLoader');

    const userId = <?php echo json_encode((int)$user_id); ?>;
    const scheduleId = <?php echo json_encode((int)$schedule_id); ?>;
    const passengerName = <?php echo json_encode($user_name); ?>;
    const passengerPhone = <?php echo json_encode($user_phone); ?>;
    const passengerEmail = <?php echo json_encode($user_email); ?>;

    function showBookingPopup(type, message, callback = null) {
        const modalEl = document.getElementById('bookingMessageModal');
        const icon = document.getElementById('bookingMessageIcon');
        const title = document.getElementById('bookingMessageTitle');
        const text = document.getElementById('bookingMessageText');
        const okBtn = document.getElementById('bookingMessageOkBtn');

        if (type === 'success') {
            icon.innerHTML = '✅';
            title.innerText = 'Booking Created';
        } else {
            icon.innerHTML = '❌';
            title.innerText = 'Booking Failed';
        }

        text.innerText = message;

        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();

        okBtn.onclick = function () {
            modal.hide();

            if (callback) {
                callback();
            }
        };
    }

    paymentMethod.addEventListener('change', function () {
        if (this.value === 'Pay at Terminal') {
            onlinePaymentBox.classList.add('d-none');

            accountName.value = '';
            accountNumber.value = '';
            expiryDate.value = '';
            cvv.value = '';

            paymentNote.innerHTML =
                "<strong>Pay at Terminal:</strong> Your booking will be marked as pending. Please pay at the terminal. Your ticket will only be available after admin confirms your payment.";
        } else {
            onlinePaymentBox.classList.remove('d-none');

            paymentNote.innerHTML =
                "<strong>Card / Bank Payment:</strong> Enter your payment details. Once the simulated payment is successful, your booking will be confirmed and your ticket will be available immediately.";
        }
    });

    function getSelectedSeats() {
        const selectedSeats = [];
        const seatSelects = document.querySelectorAll('.seat-select');

        for (let i = 0; i < seatSelects.length; i++) {
            const seatValue = seatSelects[i].value;

            if (seatValue === '') {
                showBookingPopup('error', 'Please select all required seats.');
                seatSelects[i].focus();
                return null;
            }

            if (selectedSeats.includes(seatValue)) {
                showBookingPopup('error', 'Duplicate seat selected. Please choose different seats.');
                seatSelects[i].focus();
                return null;
            }

            selectedSeats.push(seatValue);
        }

        return selectedSeats;
    }

    function validateOnlinePayment() {
        if (paymentMethod.value === 'Pay at Terminal') {
            return true;
        }

        if (accountName.value.trim() === '') {
            showBookingPopup('error', 'Please enter the cardholder or account name.');
            accountName.focus();
            return false;
        }

        if (accountNumber.value.trim().length < 8) {
            showBookingPopup('error', 'Please enter a valid card or account number.');
            accountNumber.focus();
            return false;
        }

        if (expiryDate.value.trim() === '') {
            showBookingPopup('error', 'Please enter the expiry date.');
            expiryDate.focus();
            return false;
        }

        if (cvv.value.trim().length < 3) {
            showBookingPopup('error', 'Please enter a valid CVV.');
            cvv.focus();
            return false;
        }

        return true;
    }

    function generateOnlineReference() {
        const now = Date.now();
        const type = onlinePaymentType.value.toUpperCase();
        return type + '-PAY-' + now;
    }

    confirmBookingBtn.addEventListener('click', function () {
        const selectedSeats = getSelectedSeats();

        if (selectedSeats === null) {
            return;
        }

        if (!validateOnlinePayment()) {
            return;
        }

        const selectedPaymentMethod = paymentMethod.value;
        const generatedReferenceNo =
            selectedPaymentMethod === 'Online Payment' ? generateOnlineReference() : '';

        confirmBookingBtn.disabled = true;
        bookingLoader.classList.remove('d-none');

        fetch(`${CONFIRM_BOOKING_API_BASE_URL}/create-booking`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                user_id: userId,
                schedule_id: scheduleId,
                passenger_name: passengerName,
                phone: passengerPhone,
                email: passengerEmail,
                seats: selectedSeats,
                payment_method: selectedPaymentMethod,
                reference_no: generatedReferenceNo
            })
        })
        .then(function (response) {
            return response.json();
        })
        .then(function (data) {
            console.log('Create booking response:', data);

            if (data.success) {
                showBookingPopup('success', data.message || 'Booking created successfully.', function () {
                    window.location.href = 'bookings.php';
                });
            } else {
                showBookingPopup('error', data.message || 'Failed to create booking.');
                confirmBookingBtn.disabled = false;
                bookingLoader.classList.add('d-none');
            }
        })
        .catch(function (error) {
            console.error('Booking error:', error);
            showBookingPopup('error', 'Booking failed. Please make sure the Node API is running.');
            confirmBookingBtn.disabled = false;
            bookingLoader.classList.add('d-none');
        });
    });
});
</script>

</body>
</html>