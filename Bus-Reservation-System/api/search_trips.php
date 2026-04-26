<?php
require_once "db.php";

$source = $_GET['source'] ?? '';
$destination = $_GET['destination'] ?? '';
$date = $_GET['date'] ?? '';

if ($source == '' || $destination == '' || $date == '') {
    json_response([
        "success" => false,
        "message" => "Source, destination, and date are required."
    ]);
}

$sql = "
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
    WHERE r.origin LIKE ?
      AND r.destination LIKE ?
      AND s.departure_date = ?
      AND s.trip_status = 'Scheduled'
      AND b.status = 'Active'
      AND r.status = 'Active'
    ORDER BY s.departure_time ASC
";

$stmt = $conn->prepare($sql);

$sourceLike = "%$source%";
$destinationLike = "%$destination%";

$stmt->bind_param("sss", $sourceLike, $destinationLike, $date);
$stmt->execute();

$result = $stmt->get_result();

$trips = [];

while ($row = $result->fetch_assoc()) {
    $trips[] = $row;
}

json_response([
    "success" => true,
    "count" => count($trips),
    "trips" => $trips
]);
?>