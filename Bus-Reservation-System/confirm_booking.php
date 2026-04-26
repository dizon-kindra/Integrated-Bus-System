<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>MYBUS - Confirm Booking Details</title>
    <?php require('inc/links.php') ?>

    <style>
        .seats-container {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 5px;
            margin-top: 7px;
        }

        .seat-btn {
            position: relative;
            border: var(--bs-btn-border-width) solid var(--bs-btn-border-color);
            border-radius: var(--bs-btn-border-radius);
            background-color: var(--bs-btn-bg);
            transition: color 0.25s ease-in-out, background-color 0.25s ease-in-out, border-color 0.25s ease-in-out, box-shadow 0.25s ease-in-out;
            display: inline-block;
            justify-content: center;
            align-items: center;
            width: 50px;
            height: 50px;
            margin: 2px;
        }

        .seat-btn:hover {
            transform: scale(1.1);
        }

        .seat-number {
            position: absolute;
            top: 35%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-size: 12px;
            font-weight: bold;
            color: black;
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

        @media (max-width: 768px) {
            .seats-container {
                grid-template-columns: repeat(3, 1fr);
            }
        }

        @media (max-width: 480px) {
            .seats-container {
                grid-template-columns: repeat(2, 1fr);
            }
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
        }
    </style>
</head>

<body class="bg-light">

<?php require('inc/header.php'); ?>

<?php
if (!isset($_GET['id'])) {
    redirect('bus.php');
} else if (!(isset($_SESSION['login']) && $_SESSION['login'] == true)) {
    redirect('bus.php');
}

$data = filteration($_GET);

$bus_res = select("SELECT * FROM `buses` WHERE `id`=? ORDER BY `id` DESC", [$data['id']], 'i');

if (mysqli_num_rows($bus_res) == 0) {
    redirect('bus.php');
}

$bus_data = mysqli_fetch_assoc($bus_res);

$_SESSION['bus'] = [
    "id" => $bus_data['id'],
    "name" => $bus_data['bus_name'],
    "price" => $bus_data['price'],
    "payment" => null,
    "available" => false
];

$user_res = select("SELECT * FROM `users` WHERE `id` = ? LIMIT 1", [$_SESSION['id']], 'i');
$user_data = mysqli_fetch_assoc($user_res);

$seat_res = select("SELECT * FROM `seats` WHERE `bus_id`=? ORDER BY `id` ASC", [$data['id']], 'i');

if (mysqli_num_rows($seat_res) > 0) {
    $seats = [];
    while ($seat_data = mysqli_fetch_assoc($seat_res)) {
        $seats[] = $seat_data;
    }
} else {
    redirect('bus.php');
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
                <a href="confirm_booking.php" class="text-secondary text-decoration-none">CONFIRM</a>
            </div>
        </div>

        <div class="col-lg-4 col-md-12 px-4 mb-4">
            <div class="card p-3 shadow-sm rounded">
                <h5 class="fw-bold mb-3">Select Seat</h5>

                <div class="seats-container rounded mb-3 d-flex flex-wrap align-items-center justify-content-center">
                    <form>
                        <?php
                        $seat_count = 0;

                        foreach ($seats as $seat) {
                            if ($seat['status'] == 'available') {
                                echo '<button type="button" class="seat-btn available overflow-hidden seat" data-seat-id="' . $seat['id'] . '" data-seat-number="' . $seat['seat_number'] . '">
                                        <img height="40px" src="images/seat.png">
                                        <span class="seat-number">' . $seat['seat_number'] . '</span>
                                    </button>';
                            } else {
                                echo '<button type="button" class="seat-btn booked seat overflow-hidden" disabled>
                                        <img height="40px" src="images/book-seat.png">
                                        <span class="seat-number">' . $seat['seat_number'] . '</span>
                                    </button>';
                            }

                            $seat_count++;

                            if ($seat_count % 2 == 0) {
                                echo '<span class="mx-3"></span>';
                            }

                            if ($seat_count % 4 == 0) {
                                echo '<div class="w-100"></div>';
                            }
                        }
                        ?>
                    </form>
                </div>

                <!-- Seat Legend -->
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

                <h5><?php echo $bus_data['bus_name']; ?></h5>
                <h6>₹<?php echo $bus_data['price']; ?></h6>

                <div class="selected-seats mt-3">
                    <h6 class="mb-2">Selected Seats</h6>
                    <div id="selectedSeats" class="mb-3">
                        <!-- The selected seats will be displayed here -->
                    </div>
                </div>
            </div>

            <?php $_SESSION['ORDER_ID'] = 'ORD_' . $_SESSION['id'] . random_int(11111, 9999999); ?>
        </div>

        <div class="col-lg-6 col-md-12 px-4">
            <div class="card mb-4 border-0 shadow-sm rounded-3">
                <div class="card-body">
                    <form id="booking_form">
                        <h6 class="mb-3 fw-bold">BOOKING DETAILS</h6>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1">Name</label>
                                <input type="text" name="name" id="name" value="<?php echo $user_data['name'] ?>"
                                    class="form-control shadow-none" required>
                            </div>

                            <input type="hidden" id="email" name="email" value="<?php echo $user_data['email'] ?>">

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1">Phone Number</label>
                                <input type="text" value="<?php echo $user_data['phonenum'] ?>" id="phonenum"
                                    name="phonenum" class="form-control shadow-none" onchange="check_availability()"
                                    required>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1">Source</label>
                                <input type="text" onchange="check_availability()"
                                    value="<?php echo $bus_data['source']; ?>" name="source" id="source"
                                    class="form-control shadow-none" required>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1">Destination</label>
                                <input type="text" onchange="check_availability()"
                                    value="<?php echo $bus_data['destination']; ?>" name="destination" id="destination"
                                    class="form-control shadow-none" required>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1">Date</label>
                                <input type="date" onchange="check_availability()"
                                    value="<?php echo $_SESSION['user']['date']; ?>" name="date" id="date"
                                    class="form-control shadow-none" required>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label mb-1">No. of Passengers</label>
                                <input type="text" onchange="check_availability()"
                                    value="<?php echo $_SESSION['user']['passengers']; ?>" name="passengers"
                                    id="passengers" class="form-control shadow-none" required>
                            </div>

                            <div class="col-12">
                                <div class="spinner-border text-info mb-3 d-none" id="info_loader" role="status">
                                    <span class="visually-hidden">Loading...</span>
                                </div>

                                <h6 class="mb-3 text-danger" id="pay_info">Please select seats</h6>

                                <button id="PayNow" class="book-btn" disabled>
                                    🚌 Book Now
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>

    </div>
</div>

<!-- Booking Success Modal -->
<div class="modal fade" id="bookingSuccessModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-0 shadow-lg rounded-4">
            <div class="modal-body text-center p-5">
                <div class="mb-3" style="font-size: 55px;">✅</div>

                <h4 class="fw-bold mb-2">Booking Successful!</h4>

                <p class="text-muted mb-4">
                    Your seat has been reserved successfully.<br>
                    Please pay at the terminal before departure.
                </p>

                <button type="button" class="btn w-100 text-white fw-semibold"
                    style="background:#AD8B3A;"
                    id="successOkBtn">
                    OK
                </button>
            </div>
        </div>
    </div>
</div>

<?php require('inc/footer.php'); ?>

<script src="scripts/confirm_booking.js"></script>

</body>
</html>