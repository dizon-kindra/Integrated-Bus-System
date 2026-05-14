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

if ($user_id == 0) {
    redirect('logout.php');
}

$booking_id = 0;

if (isset($_GET['booking_id'])) {
    $booking_id = (int)$_GET['booking_id'];
} else if (isset($_GET['id'])) {
    $booking_id = (int)$_GET['id'];
}

if ($booking_id <= 0) {
    die("Invalid booking ID.");
}

$query = "
    SELECT 
        bk.booking_id,
        bk.user_id,
        bk.booking_code,
        bk.passenger_name,
        bk.phone,
        bk.email,
        bk.seat_no,
        bk.total_amount,
        bk.payment_status,
        bk.reservation_status,
        bk.checkin_status,
        bk.boarding_status,
        bk.created_at,

        p.payment_id,
        p.payment_method,
        p.reference_no,
        p.paid_at,

        s.schedule_id,
        s.departure_date,
        s.departure_time,
        s.arrival_time,
        s.fare,

        b.bus_number,
        b.plate_number,
        b.bus_type,

        r.origin,
        r.destination,
        r.estimated_duration
    FROM bookings bk
    LEFT JOIN payments p ON bk.booking_id = p.booking_id
    INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
    INNER JOIN buses b ON s.bus_id = b.bus_id
    INNER JOIN routes r ON s.route_id = r.route_id
    WHERE bk.booking_id = ?
    AND bk.user_id = ?
    LIMIT 1
";

$result = select($query, [$booking_id, $user_id], 'ii');

if (mysqli_num_rows($result) == 0) {
    die("Booking not found.");
}

$data = mysqli_fetch_assoc($result);

/*
    Ticket Rule:
    Passenger can only print/view official ticket after admin confirms payment.
    Required:
    payment_status = Paid
    reservation_status = Confirmed
*/
if (
    strtolower($data['payment_status'] ?? '') !== 'paid' ||
    strtolower($data['reservation_status'] ?? '') !== 'confirmed'
) {
    die("
        <div style='font-family: Arial, sans-serif; text-align:center; margin-top:80px;'>
            <h2 style='color:#AD8B3A;'>Ticket Not Available Yet</h2>
            <p>Your ticket can only be printed after the admin confirms your payment.</p>
            <p><strong>Current Payment Status:</strong> " . htmlspecialchars($data['payment_status'] ?? 'Pending') . "</p>
            <p><strong>Current Reservation Status:</strong> " . htmlspecialchars($data['reservation_status'] ?? 'Pending') . "</p>
            <a href='bookings.php' style='display:inline-block; margin-top:15px; padding:10px 20px; background:#172233; color:white; text-decoration:none; border-radius:6px;'>Back to My Bookings</a>
        </div>
    ");
}

function format_time_ticket($time)
{
    if (!$time) {
        return "N/A";
    }

    return date("h:i A", strtotime($time));
}

function format_date_ticket($date)
{
    if (!$date) {
        return "N/A";
    }

    return date("F d, Y", strtotime($date));
}

function status_badge_class($status)
{
    $status = strtolower($status ?? '');

    if ($status == 'paid' || $status == 'confirmed' || $status == 'completed' || $status == 'checked-in' || $status == 'boarded') {
        return 'success';
    }

    if ($status == 'cancelled' || $status == 'canceled') {
        return 'danger';
    }

    return 'warning';
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>MYBUS Ticket - <?php echo htmlspecialchars($data['booking_code']); ?></title>

    <?php require('inc/links.php'); ?>

    <style>
        body {
            background: #f4f6f8;
            font-family: Arial, sans-serif;
        }

        .ticket-wrapper {
            max-width: 850px;
            margin: 40px auto;
            background: #fff;
            border-radius: 18px;
            overflow: hidden;
            box-shadow: 0 8px 25px rgba(0,0,0,0.12);
        }

        .ticket-header {
            background: #172233;
            color: #fff;
            padding: 28px 35px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .ticket-header h2 {
            margin: 0;
            font-weight: 800;
        }

        .ticket-code {
            background: #AD8B3A;
            padding: 8px 16px;
            border-radius: 30px;
            font-weight: bold;
            color: white;
        }

        .ticket-body {
            padding: 35px;
        }

        .section-title {
            font-weight: 800;
            color: #172233;
            border-bottom: 2px solid #AD8B3A;
            padding-bottom: 8px;
            margin-bottom: 18px;
        }

        .info-box {
            background: #f8f9fa;
            border-radius: 12px;
            padding: 15px;
            margin-bottom: 15px;
        }

        .info-label {
            font-size: 13px;
            color: #6c757d;
            margin-bottom: 3px;
        }

        .info-value {
            font-weight: 700;
            color: #222;
        }

        .status-pill {
            padding: 7px 12px;
            border-radius: 20px;
            color: #fff;
            font-size: 13px;
            font-weight: 700;
            display: inline-block;
        }

        .status-success {
            background: #198754;
        }

        .status-danger {
            background: #dc3545;
        }

        .status-warning {
            background: #ffc107;
            color: #222;
        }

        .ticket-footer {
            background: #f8f9fa;
            padding: 20px 35px;
            border-top: 1px dashed #ccc;
            text-align: center;
            color: #666;
        }

        .print-actions {
            max-width: 850px;
            margin: 20px auto;
            text-align: center;
        }

        .btn-print {
            background: #AD8B3A;
            color: white;
            border: none;
        }

        .btn-print:hover {
            background: #8f722e;
            color: white;
        }

        @media print {
            body {
                background: white;
            }

            .print-actions,
            .navbar,
            .container-fluid.bg-white,
            h6.bg-dark {
                display: none !important;
            }

            .ticket-wrapper {
                margin: 0;
                max-width: 100%;
                box-shadow: none;
                border: 1px solid #ddd;
            }
        }
    </style>
</head>

<body>

<div class="print-actions">
    <button onclick="window.print()" class="btn btn-print px-4 shadow-none">
        Print Ticket
    </button>

    <a href="bookings.php" class="btn btn-dark px-4 shadow-none">
        Back to My Bookings
    </a>
</div>

<div class="ticket-wrapper">
    <div class="ticket-header">
        <div>
            <h2>MYBUS Ticket</h2>
            <small>Passenger Bus Reservation System</small>
        </div>

        <div class="ticket-code">
            <?php echo htmlspecialchars($data['booking_code']); ?>
        </div>
    </div>

    <div class="ticket-body">

        <h5 class="section-title">Passenger Details</h5>

        <div class="row">
            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Passenger Name</div>
                    <div class="info-value"><?php echo htmlspecialchars($data['passenger_name']); ?></div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Email</div>
                    <div class="info-value"><?php echo htmlspecialchars($data['email']); ?></div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Phone Number</div>
                    <div class="info-value"><?php echo htmlspecialchars($data['phone']); ?></div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Seat Number</div>
                    <div class="info-value"><?php echo htmlspecialchars($data['seat_no']); ?></div>
                </div>
            </div>
        </div>

        <h5 class="section-title mt-4">Trip Details</h5>

        <div class="row">
            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Route</div>
                    <div class="info-value">
                        <?php echo htmlspecialchars($data['origin']); ?> → <?php echo htmlspecialchars($data['destination']); ?>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Travel Date</div>
                    <div class="info-value"><?php echo format_date_ticket($data['departure_date']); ?></div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Departure Time</div>
                    <div class="info-value"><?php echo format_time_ticket($data['departure_time']); ?></div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Arrival Time</div>
                    <div class="info-value"><?php echo format_time_ticket($data['arrival_time']); ?></div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Bus Number</div>
                    <div class="info-value"><?php echo htmlspecialchars($data['bus_number']); ?></div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="info-box">
                    <div class="info-label">Plate Number</div>
                    <div class="info-value"><?php echo htmlspecialchars($data['plate_number']); ?></div>
                </div>
            </div>
        </div>

        <h5 class="section-title mt-4">Payment and Status</h5>

        <div class="row">
            <div class="col-md-4">
                <div class="info-box">
                    <div class="info-label">Total Amount</div>
                    <div class="info-value">₱<?php echo number_format((float)$data['total_amount'], 2); ?></div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="info-box">
                    <div class="info-label">Payment Method</div>
                    <div class="info-value"><?php echo htmlspecialchars($data['payment_method'] ?? 'Pay at Terminal'); ?></div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="info-box">
                    <div class="info-label">Reference No.</div>
                    <div class="info-value"><?php echo htmlspecialchars($data['reference_no'] ?? $data['booking_code']); ?></div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="info-box">
                    <div class="info-label">Payment</div>
                    <span class="status-pill status-<?php echo status_badge_class($data['payment_status']); ?>">
                        <?php echo htmlspecialchars($data['payment_status']); ?>
                    </span>
                </div>
            </div>

            <div class="col-md-3">
                <div class="info-box">
                    <div class="info-label">Reservation</div>
                    <span class="status-pill status-<?php echo status_badge_class($data['reservation_status']); ?>">
                        <?php echo htmlspecialchars($data['reservation_status']); ?>
                    </span>
                </div>
            </div>

            <div class="col-md-3">
                <div class="info-box">
                    <div class="info-label">Check-in</div>
                    <span class="status-pill status-<?php echo status_badge_class($data['checkin_status']); ?>">
                        <?php echo htmlspecialchars($data['checkin_status']); ?>
                    </span>
                </div>
            </div>

            <div class="col-md-3">
                <div class="info-box">
                    <div class="info-label">Boarding</div>
                    <span class="status-pill status-<?php echo status_badge_class($data['boarding_status']); ?>">
                        <?php echo htmlspecialchars($data['boarding_status']); ?>
                    </span>
                </div>
            </div>
        </div>

    </div>

    <div class="ticket-footer">
        This ticket is valid because the payment has been confirmed by the admin.
        <br>
        Please present this ticket or booking code at the terminal before departure.
        <br>
        Generated on <?php echo date("F d, Y | h:i A"); ?>
    </div>
</div>

</body>
</html>