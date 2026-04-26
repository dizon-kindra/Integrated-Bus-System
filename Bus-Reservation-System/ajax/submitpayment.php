<?php
ob_start();
session_start();

require_once('../admin/inc/db_config.php');
require('../admin/inc/essentials.php');

header('Content-Type: application/json');

function send_json($data) {
    if (ob_get_length()) {
        ob_clean();
    }

    echo json_encode($data);
    exit;
}

if (!isset($_POST['action'])) {
    send_json([
        'res' => 'failure',
        'info' => 'Invalid request.'
    ]);
}

if ($_POST['action'] == 'bookOffline') {

    if (!isset($_SESSION['id'], $_SESSION['bus'], $_SESSION['user'])) {
        send_json([
            'res' => 'failure',
            'info' => 'Session expired. Please login again.'
        ]);
    }

    if (!isset($_SESSION['user']['selectedSeatNumbers'])) {
        send_json([
            'res' => 'failure',
            'info' => 'No selected seats found.'
        ]);
    }

    $user_id = $_SESSION['id'];
    $bus_id = $_SESSION['bus']['id'];
    $source = $_SESSION['bus']['source'];
    $destination = $_SESSION['bus']['destination'];
    $date = $_SESSION['bus']['date'];
    $payAmount = $_POST['payAmount'];

    $seatNumbers = $_SESSION['user']['selectedSeatNumbers'];
    $seatNumbersString = implode(',', $seatNumbers);

    $order_id = 'OFFLINE_' . uniqid();
    $_SESSION['ORDER_ID'] = $order_id;

    $query2 = "INSERT INTO booking 
        (user_id, bus_id, user_name, phonenum, email, travel_date, seat_number, source, destination) 
        VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";

    insert($query2, [
        $user_id,
        $bus_id,
        $_SESSION['user']['name'],
        $_SESSION['user']['number'],
        $_SESSION['user']['email'],
        $date,
        $seatNumbersString,
        $source,
        $destination
    ], 'iisssssss');

    $booking_id = mysqli_insert_id($con);

    $query1 = "INSERT INTO payment 
        (booking_id, trans_amt, order_id, trans_status, trans_resp_msg) 
        VALUES (?, ?, ?, ?, ?)";

    insert($query1, [
        $booking_id,
        $payAmount,
        $order_id,
        'PENDING',
        'Pay at terminal'
    ], 'iisss');

    send_json([
        'res' => 'success',
        'message' => 'Booking created successfully. Please pay at terminal.',
        'order_id' => $order_id,
        'booking_id' => $booking_id
    ]);
}

send_json([
    'res' => 'failure',
    'info' => 'Invalid action.'
]);
?>