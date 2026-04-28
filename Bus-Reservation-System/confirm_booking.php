<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>MYBUS - Confirm Booking Details</title>
    <?php require('inc/links.php') ?>

    <style>
        .seats-container {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
            margin-top: 10px;
            justify-content: center;
        }

        .seat-btn {
            position: relative;
            border: 1px solid #ddd;
            border-radius: 8px;
            background: #fff;
            transition: 0.25s ease-in-out;
            display: inline-flex;
            justify-content: center;
            align-items: center;
            width: 55px;
            height: 55px;
            margin: 2px;
            cursor: pointer;
        }

        .seat-btn:hover:not(:disabled) {
            transform: scale(1.08);
            border-color: #0d6efd;
        }

        .seat-btn img {
            height: 38px;
        }

        .seat-number {
            position: absolute;
            top: 42%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-size: 12px;
            font-weight: bold;
            color: black;
        }

        .seat-btn.booked {
            cursor: not-allowed;
            opacity: 0.7;
        }

        .seat-btn.selected {
            background: #0d6efd;
            border-color: #0d6efd;
        }

        .seat-btn.selected .seat-number {
            color: white;
        }

        .seat-legend {
            border-top: 1px solid #eee;
            padding-top: 12px;
        }

        .selected-legend-box {
            width: 25px;
            height: 25px;
            background: #0d6efd;
            border-radius: 5px;
            display: inline-block;
        }

        .book-btn {
            width: 100%;
            padding: 12px;
            background: linear-gradient(135deg, #AD8B3A, #c9a74d);
            color: white;
            font-weight: 600;
            font-size: 16px;
            border: none;
            border-radius: 8px;
            transition: all 0.3s ease;
            letter-spacing: 0.5px;
        }

        .book-btn:hover {
            background: linear-gradient(135deg, #c9a74d, #AD8B3A);
            transform: translateY(-2px);
            box-shadow: 0 6px 15px rgba(0,0,0,0.2);
        }

        .book-btn:active {
            transform: scale(0.97);
        }

        .book-btn:disabled {
            background: #ccc;
            cursor: not-allowed;
            box-shadow: none;
            transform: none;
        }

        .trip-card {
            border-radius: 16px;
        }

        .summary-box {
            background: #f8f9fa;
            border-radius: 12px;
            padding: 15px;
        }
    </style>
</head>

<body class="bg-light">

<?php require('inc/header.php'); ?>

<?php
if (!(isset($_SESSION['login']) && $_SESSION['login'] == true)) {
    redirect('bus.php');
}

if (!isset($_GET['schedule_id'])) {
    redirect('bus.php');
}

$data = filteration($_GET);

$schedule_id = (int)$data['schedule_id'];
$passengers = isset($data['passengers']) ? (int)$data['passengers'] : 1;

if ($passengers < 1) {
    $passengers = 1;
}

if ($passengers > 9) {
    $passengers = 9;
}

$trip_res = select(
    "SELECT 
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
        b.capacity,
        b.bus_type,
        b.status AS bus_status,

        r.route_id,
        r.origin,
        r.destination,
        r.estimated_duration,
        r.status AS route_status
    FROM schedules s
    INNER JOIN buses b ON s.bus_id = b.bus_id
    INNER JOIN routes r ON s.route_id = r.route_id
    WHERE s.schedule_id = ?
    LIMIT 1",
    [$schedule_id],
    'i'
);

if (mysqli_num_rows($trip_res) == 0) {
    redirect('bus.php');
}

$trip_data = mysqli_fetch_assoc($trip_res);

$user_id = $_SESSION['user_id'] ?? $_SESSION['id'] ?? 0;

$user_res = select(
    "SELECT 
        user_id,
        full_name,
        email,
        phone_number
    FROM users
    WHERE user_id = ?
    LIMIT 1",
    [$user_id],
    'i'
);

if (mysqli_num_rows($user_res) == 0) {
    redirect('logout.php');
}

$user_data = mysqli_fetch_assoc($user_res);

$total_amount = (float)$trip_data['fare'] * $passengers;

function format_time_display($time)
{
    return date("h:i A", strtotime($time));
}
?>

<div class="container">
    <div class="row">

        <div class="col-12 my-5 mb-4 px-4">
            <h2 class="fw-bold h-font">CONFIRM BOOKING</h2>
            <div style="font-size:14px;">
                <a href="index.php" class="text-secondary text-decoration-none">HOME</a>
                <span class="text-secondary"> > </span>
                <a href="bus.php" class="text-secondary text-decoration-none">BUSES</a>
                <span class="text-secondary"> > </span>
                <span class="text-secondary">CONFIRM</span>
            </div>
        </div>

        <div class="col-lg-5 col-md-12 px-4 mb-4">
            <div class="card p-3 shadow-sm border-0 trip-card">
                <h5 class="fw-bold mb-3">Select Seat</h5>

                <div class="alert alert-info py-2">
                    Please select <strong><?php echo $passengers; ?></strong> seat(s).
                </div>

                <div id="seatLoader" class="text-center my-3">
                    <div class="spinner-border text-info" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                    <p class="text-muted mt-2">Loading seats...</p>
                </div>

                <div id="seatsContainer" class="seats-container rounded mb-3"></div>

                <div class="mt-3 d-flex gap-3 flex-wrap align-items-center seat-legend">
                    <span class="d-flex align-items-center gap-1">
                        <img src="images/seat.png" height="25"> Available
                    </span>

                    <span class="d-flex align-items-center gap-1">
                        <img src="images/book-seat.png" height="25"> Booked
                    </span>

                    <span class="d-flex align-items-center gap-1">
                        <span class="selected-legend-box"></span> Selected
                    </span>
                </div>

                <hr>

                <div class="summary-box">
                    <h6 class="fw-bold mb-2">Selected Seats</h6>
                    <div id="selectedSeats" class="text-muted">
                        No seats selected yet.
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-7 col-md-12 px-4">
            <div class="card mb-4 border-0 shadow-sm trip-card">
                <div class="card-body">
                    <h5 class="fw-bold mb-3">Trip Summary</h5>

                    <div class="row mb-3">
                        <div class="col-md-6 mb-3">
                            <label class="form-label mb-1 fw-bold">Bus Number</label>
                            <input type="text" value="<?php echo htmlspecialchars($trip_data['bus_number']); ?>"
                                class="form-control shadow-none" readonly>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label class="form-label mb-1 fw-bold">Bus Type</label>
                            <input type="text" value="<?php echo htmlspecialchars($trip_data['bus_type']); ?>"
                                class="form-control shadow-none" readonly>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label class="form-label mb-1 fw-bold">Source</label>
                            <input type="text" value="<?php echo htmlspecialchars($trip_data['origin']); ?>"
                                class="form-control shadow-none" readonly>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label class="form-label mb-1 fw-bold">Destination</label>
                            <input type="text" value="<?php echo htmlspecialchars($trip_data['destination']); ?>"
                                class="form-control shadow-none" readonly>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label class="form-label mb-1 fw-bold">Departure Date</label>
                            <input type="text" value="<?php echo htmlspecialchars($trip_data['departure_date']); ?>"
                                class="form-control shadow-none" readonly>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label class="form-label mb-1 fw-bold">Departure Time</label>
                            <input type="text" value="<?php echo format_time_display($trip_data['departure_time']); ?>"
                                class="form-control shadow-none" readonly>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label class="form-label mb-1 fw-bold">Arrival Time</label>
                            <input type="text" value="<?php echo format_time_display($trip_data['arrival_time']); ?>"
                                class="form-control shadow-none" readonly>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label class="form-label mb-1 fw-bold">Fare Per Passenger</label>
                            <input type="text" value="₱<?php echo htmlspecialchars($trip_data['fare']); ?>"
                                class="form-control shadow-none" readonly>
                        </div>
                    </div>

                    <hr>

                    <form id="booking_form">
                        <h5 class="mb-3 fw-bold">Passenger Details</h5>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1 fw-bold">Name</label>
                                <input type="text" id="passenger_name"
                                    value="<?php echo htmlspecialchars($user_data['full_name']); ?>"
                                    class="form-control shadow-none" required>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1 fw-bold">Email</label>
                                <input type="email" id="passenger_email"
                                    value="<?php echo htmlspecialchars($user_data['email']); ?>"
                                    class="form-control shadow-none" readonly>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1 fw-bold">Phone Number</label>
                                <input type="text" id="passenger_phone"
                                    value="<?php echo htmlspecialchars($user_data['phone_number']); ?>"
                                    class="form-control shadow-none" required>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1 fw-bold">No. of Passengers</label>
                                <input type="number" id="passenger_count"
                                    value="<?php echo $passengers; ?>"
                                    class="form-control shadow-none" readonly>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1 fw-bold">Payment Method</label>
                                <input type="text" value="Pay at Terminal"
                                    class="form-control shadow-none" readonly>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1 fw-bold">Total Amount</label>
                                <input type="text" id="total_amount_display"
                                    value="₱<?php echo number_format($total_amount, 2); ?>"
                                    class="form-control shadow-none fw-bold text-success" readonly>
                            </div>

                            <div class="col-12">
                                <div class="spinner-border text-info mb-3 d-none" id="info_loader" role="status">
                                    <span class="visually-hidden">Loading...</span>
                                </div>

                                <h6 class="mb-3 text-danger" id="pay_info">Please select seats.</h6>

                                <button type="submit" id="PayNow" class="book-btn" disabled>
                                    🚌 Confirm Booking
                                </button>
                            </div>
                        </div>
                    </form>

                </div>
            </div>
        </div>

    </div>
</div>

<div class="modal fade" id="bookingSuccessModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-0 shadow-lg rounded-4">
            <div class="modal-body text-center p-5">
                <div class="mb-3" style="font-size: 55px;">✅</div>

                <h4 class="fw-bold mb-2">Booking Successful!</h4>

                <p class="text-muted mb-3">
                    Your seat has been reserved successfully.<br>
                    Please pay at the terminal before departure.
                </p>

                <div class="bg-light rounded p-3 mb-4">
                    <strong>Booking Code:</strong>
                    <div id="successBookingCode" class="text-success fw-bold mt-1"></div>
                </div>

                <button type="button" class="btn w-100 text-white fw-semibold"
                    style="background:#AD8B3A;"
                    id="successOkBtn">
                    OK
                </button>
            </div>
        </div>
    </div>
</div>

<script>
const API_BASE_URL = "http://localhost:3000/api";

const scheduleId = <?php echo (int)$schedule_id; ?>;
const userId = <?php echo (int)$user_data['user_id']; ?>;
const requiredSeats = <?php echo (int)$passengers; ?>;

const seatsContainer = document.getElementById('seatsContainer');
const seatLoader = document.getElementById('seatLoader');
const selectedSeatsDiv = document.getElementById('selectedSeats');
const payInfo = document.getElementById('pay_info');
const payNowBtn = document.getElementById('PayNow');
const bookingForm = document.getElementById('booking_form');
const infoLoader = document.getElementById('info_loader');

let selectedSeats = [];

function updateSelectedSeatsDisplay() {
    if (selectedSeats.length === 0) {
        selectedSeatsDiv.innerHTML = '<span class="text-muted">No seats selected yet.</span>';
        payInfo.innerHTML = 'Please select seats.';
        payInfo.className = 'mb-3 text-danger';
        payNowBtn.disabled = true;
        return;
    }

    let seatHtml = '';

    selectedSeats.forEach(function(seat) {
        seatHtml += '<span class="badge bg-primary me-1 mb-1">Seat ' + seat + '</span>';
    });

    selectedSeatsDiv.innerHTML = seatHtml;

    if (selectedSeats.length < requiredSeats) {
        payInfo.innerHTML = 'Please select ' + (requiredSeats - selectedSeats.length) + ' more seat(s).';
        payInfo.className = 'mb-3 text-danger';
        payNowBtn.disabled = true;
    } else {
        payInfo.innerHTML = 'Ready to book seat(s): ' + selectedSeats.join(', ');
        payInfo.className = 'mb-3 text-success';
        payNowBtn.disabled = false;
    }
}

function loadSeats() {
    seatLoader.classList.remove('d-none');
    seatsContainer.innerHTML = '';

    fetch(`${API_BASE_URL}/get-seats?schedule_id=${scheduleId}`)
        .then(function(response) {
            return response.json();
        })
        .then(function(data) {
            seatLoader.classList.add('d-none');

            if (!data.success) {
                seatsContainer.innerHTML =
                    '<div class="alert alert-danger w-100 text-center">' +
                    data.message +
                    '</div>';
                return;
            }

            let html = '';

            data.seats.forEach(function(seat) {
                if (seat.status === 'booked') {
                    html +=
                        '<button type="button" class="seat-btn booked overflow-hidden" disabled>' +
                            '<img src="images/book-seat.png">' +
                            '<span class="seat-number">' + seat.seat_no + '</span>' +
                        '</button>';
                } else {
                    html +=
                        '<button type="button" class="seat-btn available overflow-hidden" data-seat="' + seat.seat_no + '">' +
                            '<img src="images/seat.png">' +
                            '<span class="seat-number">' + seat.seat_no + '</span>' +
                        '</button>';
                }
            });

            seatsContainer.innerHTML = html;

            document.querySelectorAll('.seat-btn.available').forEach(function(button) {
                button.addEventListener('click', function() {
                    const seatNo = parseInt(this.dataset.seat);

                    if (selectedSeats.includes(seatNo)) {
                        selectedSeats = selectedSeats.filter(function(s) {
                            return s !== seatNo;
                        });

                        this.classList.remove('selected');
                    } else {
                        if (selectedSeats.length >= requiredSeats) {
                            alert('You can only select ' + requiredSeats + ' seat(s).');
                            return;
                        }

                        selectedSeats.push(seatNo);
                        this.classList.add('selected');
                    }

                    selectedSeats.sort(function(a, b) {
                        return a - b;
                    });

                    updateSelectedSeatsDisplay();
                });
            });

            updateSelectedSeatsDisplay();
        })
        .catch(function(error) {
            console.error('Seat Load Error:', error);

            seatLoader.classList.add('d-none');

            seatsContainer.innerHTML =
                '<div class="alert alert-danger w-100 text-center">' +
                'Unable to load seats. Please make sure the Node API is running.' +
                '</div>';
        });
}

bookingForm.addEventListener('submit', function(e) {
    e.preventDefault();

    if (selectedSeats.length !== requiredSeats) {
        alert('Please select exactly ' + requiredSeats + ' seat(s).');
        return;
    }

    payNowBtn.disabled = true;
    infoLoader.classList.remove('d-none');
    payInfo.innerHTML = 'Processing booking...';
    payInfo.className = 'mb-3 text-info';

    const passengerName = document.getElementById('passenger_name').value.trim();
    const passengerPhone = document.getElementById('passenger_phone').value.trim();
    const passengerEmail = document.getElementById('passenger_email').value.trim();

    fetch(`${API_BASE_URL}/create-booking`, {
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
            seats: selectedSeats
        })
    })
    .then(function(response) {
        return response.json();
    })
    .then(function(data) {
        infoLoader.classList.add('d-none');

        if (data.success) {
            document.getElementById('successBookingCode').innerText = data.booking_code;

            const successModal = new bootstrap.Modal(document.getElementById('bookingSuccessModal'));
            successModal.show();

            document.getElementById('successOkBtn').onclick = function() {
                window.location.href = 'bookings.php';
            };
        } else {
            payInfo.innerHTML = data.message;
            payInfo.className = 'mb-3 text-danger';
            payNowBtn.disabled = false;
            loadSeats();
        }
    })
    .catch(function(error) {
        console.error('Booking Error:', error);

        infoLoader.classList.add('d-none');
        payInfo.innerHTML = 'Booking failed. Please make sure the Node API is running.';
        payInfo.className = 'mb-3 text-danger';
        payNowBtn.disabled = false;
    });
});

loadSeats();
</script>

<?php require('inc/footer.php'); ?>

</body>
</html>