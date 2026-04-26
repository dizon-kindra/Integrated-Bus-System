<?php
require_once "db.php";

$data = json_decode(file_get_contents("php://input"), true);

$booking_id = $data['booking_id'] ?? null;
$user_id = $data['user_id'] ?? null;

if (!$booking_id || !$user_id) {
    json_response([
        "success" => false,
        "message" => "Booking ID and User ID are required."
    ]);
}

$conn->begin_transaction();

try {
    $bookingSql = "
        SELECT 
            booking_id,
            user_id,
            schedule_id,
            seat_no,
            payment_status,
            reservation_status,
            checkin_status,
            boarding_status
        FROM bookings
        WHERE booking_id = ?
          AND user_id = ?
        FOR UPDATE
    ";

    $bookingStmt = $conn->prepare($bookingSql);
    $bookingStmt->bind_param("ii", $booking_id, $user_id);
    $bookingStmt->execute();

    $bookingResult = $bookingStmt->get_result();

    if ($bookingResult->num_rows == 0) {
        throw new Exception("Booking not found.");
    }

    $booking = $bookingResult->fetch_assoc();

    if ($booking['reservation_status'] == 'Cancelled') {
        throw new Exception("Booking is already cancelled.");
    }

    if ($booking['payment_status'] == 'Paid') {
        throw new Exception("Paid booking cannot be cancelled from passenger side. Please contact terminal staff.");
    }

    if ($booking['checkin_status'] == 'Checked-in' || $booking['boarding_status'] == 'Boarded') {
        throw new Exception("Checked-in or boarded booking cannot be cancelled.");
    }

    $schedule_id = (int)$booking['schedule_id'];

    $cancelBookingSql = "
        UPDATE bookings
        SET 
            payment_status = 'Cancelled',
            reservation_status = 'Cancelled'
        WHERE booking_id = ?
          AND user_id = ?
    ";

    $cancelBookingStmt = $conn->prepare($cancelBookingSql);
    $cancelBookingStmt->bind_param("ii", $booking_id, $user_id);

    if (!$cancelBookingStmt->execute()) {
        throw new Exception("Failed to cancel booking.");
    }

    $cancelPaymentSql = "
        UPDATE payments
        SET payment_status = 'Cancelled'
        WHERE booking_id = ?
    ";

    $cancelPaymentStmt = $conn->prepare($cancelPaymentSql);
    $cancelPaymentStmt->bind_param("i", $booking_id);

    if (!$cancelPaymentStmt->execute()) {
        throw new Exception("Failed to cancel payment.");
    }

    $updateScheduleSql = "
        UPDATE schedules
        SET available_seats = available_seats + 1
        WHERE schedule_id = ?
    ";

    $updateScheduleStmt = $conn->prepare($updateScheduleSql);
    $updateScheduleStmt->bind_param("i", $schedule_id);

    if (!$updateScheduleStmt->execute()) {
        throw new Exception("Failed to update available seats.");
    }

    $conn->commit();

    json_response([
        "success" => true,
        "message" => "Booking cancelled successfully.",
        "booking_id" => (int)$booking_id,
        "schedule_id" => $schedule_id
    ]);

} catch (Exception $e) {
    $conn->rollback();

    json_response([
        "success" => false,
        "message" => $e->getMessage()
    ]);
}
?>