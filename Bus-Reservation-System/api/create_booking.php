<?php
require_once "db.php";

$data = json_decode(file_get_contents("php://input"), true);

$user_id = $data['user_id'] ?? null;
$schedule_id = $data['schedule_id'] ?? null;
$passenger_name = $data['passenger_name'] ?? '';
$phone = $data['phone'] ?? '';
$email = $data['email'] ?? '';
$seats = $data['seats'] ?? [];

if (!$schedule_id || $passenger_name == '' || $phone == '' || $email == '' || empty($seats)) {
    json_response([
        "success" => false,
        "message" => "Missing required booking data."
    ]);
}

$conn->begin_transaction();

try {
    $scheduleSql = "
        SELECT 
            s.schedule_id,
            s.fare,
            s.available_seats
        FROM schedules s
        WHERE s.schedule_id = ?
        FOR UPDATE
    ";

    $scheduleStmt = $conn->prepare($scheduleSql);
    $scheduleStmt->bind_param("i", $schedule_id);
    $scheduleStmt->execute();

    $scheduleResult = $scheduleStmt->get_result();

    if ($scheduleResult->num_rows == 0) {
        throw new Exception("Schedule not found.");
    }

    $schedule = $scheduleResult->fetch_assoc();

    $fare = (float)$schedule['fare'];
    $availableSeats = (int)$schedule['available_seats'];

    if (count($seats) > $availableSeats) {
        throw new Exception("Not enough available seats.");
    }

    $booking_code = "BK-" . strtoupper(uniqid());
    $createdBookingIds = [];

    foreach ($seats as $seat_no) {
        $seat_no = (int)$seat_no;

        $checkSql = "
            SELECT booking_id 
            FROM bookings 
            WHERE schedule_id = ? 
              AND seat_no = ? 
              AND reservation_status != 'Cancelled'
        ";

        $checkStmt = $conn->prepare($checkSql);
        $checkStmt->bind_param("ii", $schedule_id, $seat_no);
        $checkStmt->execute();

        $checkResult = $checkStmt->get_result();

        if ($checkResult->num_rows > 0) {
            throw new Exception("Seat number $seat_no is already booked.");
        }

        $insertBookingSql = "
            INSERT INTO bookings 
            (
                user_id,
                booking_code,
                schedule_id,
                passenger_name,
                phone,
                email,
                seat_no,
                total_amount,
                payment_status,
                reservation_status,
                checkin_status,
                boarding_status
            )
            VALUES (?, ?, ?, ?, ?, ?, ?, ?, 'Pending', 'Pending', 'Not Checked-in', 'Not Boarded')
        ";

        $insertBookingStmt = $conn->prepare($insertBookingSql);

        $insertBookingStmt->bind_param(
            "isisssid",
            $user_id,
            $booking_code,
            $schedule_id,
            $passenger_name,
            $phone,
            $email,
            $seat_no,
            $fare
        );

        if (!$insertBookingStmt->execute()) {
            throw new Exception("Failed to create booking.");
        }

        $booking_id = $conn->insert_id;
        $createdBookingIds[] = $booking_id;

        $paymentSql = "
            INSERT INTO payments 
            (
                booking_id,
                amount,
                payment_method,
                reference_no,
                payment_status
            )
            VALUES (?, ?, 'Pay at Terminal', ?, 'Pending')
        ";

        $paymentStmt = $conn->prepare($paymentSql);
        $paymentStmt->bind_param("ids", $booking_id, $fare, $booking_code);

        if (!$paymentStmt->execute()) {
            throw new Exception("Failed to create payment record.");
        }
    }

    $newAvailableSeats = $availableSeats - count($seats);

    $updateScheduleSql = "
        UPDATE schedules 
        SET available_seats = ? 
        WHERE schedule_id = ?
    ";

    $updateScheduleStmt = $conn->prepare($updateScheduleSql);
    $updateScheduleStmt->bind_param("ii", $newAvailableSeats, $schedule_id);
    $updateScheduleStmt->execute();

    $conn->commit();

    json_response([
        "success" => true,
        "message" => "Booking created successfully.",
        "booking_code" => $booking_code,
        "booking_ids" => $createdBookingIds,
        "total_amount" => $fare * count($seats)
    ]);

} catch (Exception $e) {
    $conn->rollback();

    json_response([
        "success" => false,
        "message" => $e->getMessage()
    ]);
}
?>