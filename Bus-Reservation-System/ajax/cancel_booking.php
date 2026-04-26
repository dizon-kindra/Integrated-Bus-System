<?php
session_start();

require('../admin/inc/db_config.php');
require('../admin/inc/essentials.php');

$response = 0;

if (isset($_POST['payment_id']) && isset($_SESSION['id'])) {

    $payment_id = $_POST['payment_id'];
    $user_id = $_SESSION['id'];

    $query = "SELECT bd.booking_id, bd.seat_number, bd.bus_id
              FROM booking bd
              INNER JOIN payment p ON p.booking_id = bd.booking_id
              WHERE p.payment_id = ? AND bd.user_id = ?
              LIMIT 1";

    $res = select($query, [$payment_id, $user_id], "ii");

    if ($res && mysqli_num_rows($res) > 0) {

        $data = mysqli_fetch_assoc($res);

        $booking_id = $data['booking_id'];
        $bus_id = $data['bus_id'];
        $seat_numbers = explode(",", $data['seat_number']);

        $query1 = "UPDATE booking 
                   SET status = 'cancelled' 
                   WHERE booking_id = ?";

        update($query1, [$booking_id], "i");

        $query2 = "UPDATE payment 
                   SET trans_status = 'cancelled', 
                       trans_resp_msg = 'Booking cancelled by user'
                   WHERE payment_id = ?";

        update($query2, [$payment_id], "i");

        foreach ($seat_numbers as $seatnum) {
            $seatnum = trim($seatnum);

            if ($seatnum !== '') {
                $query3 = "UPDATE seats 
                           SET status = 'available', booking_id = NULL 
                           WHERE seat_number = ? AND bus_id = ?";

                update($query3, [$seatnum, $bus_id], "ii");
            }
        }

        $response = 1;
    }
}
echo ($response == 1) ? 'success' : 'Failed to cancel booking.';
?>