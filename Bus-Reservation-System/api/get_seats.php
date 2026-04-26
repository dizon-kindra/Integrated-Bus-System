<?php
require_once "db.php";

$schedule_id = $_GET['schedule_id'] ?? '';

if ($schedule_id == '') {
    json_response([
        "success" => false,
        "message" => "Schedule ID is required."
    ]);
}

$sql = "
    SELECT 
        s.schedule_id,
        s.available_seats,
        b.capacity
    FROM schedules s
    INNER JOIN buses b ON s.bus_id = b.bus_id
    WHERE s.schedule_id = ?
";

$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $schedule_id);
$stmt->execute();

$result = $stmt->get_result();

if ($result->num_rows == 0) {
    json_response([
        "success" => false,
        "message" => "Schedule not found."
    ]);
}

$schedule = $result->fetch_assoc();

$capacity = (int)$schedule['capacity'];

$bookedSql = "
    SELECT seat_no
    FROM bookings
    WHERE schedule_id = ?
      AND reservation_status != 'Cancelled'
";

$bookedStmt = $conn->prepare($bookedSql);
$bookedStmt->bind_param("i", $schedule_id);
$bookedStmt->execute();

$bookedResult = $bookedStmt->get_result();

$bookedSeats = [];

while ($row = $bookedResult->fetch_assoc()) {
    $bookedSeats[] = (int)$row['seat_no'];
}

$seats = [];

for ($i = 1; $i <= $capacity; $i++) {
    $seats[] = [
        "seat_no" => $i,
        "status" => in_array($i, $bookedSeats) ? "booked" : "available"
    ];
}

json_response([
    "success" => true,
    "schedule_id" => (int)$schedule_id,
    "capacity" => $capacity,
    "available_count" => $capacity - count($bookedSeats),
    "booked_count" => count($bookedSeats),
    "seats" => $seats
]);
?>