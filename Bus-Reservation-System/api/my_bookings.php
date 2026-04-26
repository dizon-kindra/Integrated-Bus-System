<?php
require_once "db.php";

$user_id = $_GET['user_id'] ?? '';

if ($user_id == '') {
    json_response([
        "success" => false,
        "message" => "User ID is required."
    ]);
}

$sql = "
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

        s.schedule_id,
        s.departure_date,
        s.departure_time,
        s.arrival_time,
        s.fare,
        s.trip_status,

        b.bus_id,
        b.bus_number,
        b.plate_number,
        b.bus_type,

        r.route_id,
        r.origin,
        r.destination,
        r.estimated_duration,

        p.payment_id,
        p.payment_method,
        p.reference_no,
        p.paid_at
    FROM bookings bk
    INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
    INNER JOIN buses b ON s.bus_id = b.bus_id
    INNER JOIN routes r ON s.route_id = r.route_id
    LEFT JOIN payments p ON p.booking_id = bk.booking_id
    WHERE bk.user_id = ?
    ORDER BY bk.created_at DESC
";

$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $user_id);
$stmt->execute();

$result = $stmt->get_result();

$bookings = [];

while ($row = $result->fetch_assoc()) {
    $bookings[] = $row;
}

json_response([
    "success" => true,
    "count" => count($bookings),
    "bookings" => $bookings
]);
?>