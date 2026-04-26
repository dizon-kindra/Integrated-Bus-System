<?php
session_start();

require('../admin/inc/db_config.php');
require('../admin/inc/essentials.php');

if (isset($_POST['email'], $_POST['pass'])) {

    $data = filteration($_POST);

    $u_exist = select(
        "SELECT * FROM `users` WHERE `email` = ? LIMIT 1",
        [$data['email']],
        's'
    );

    if (mysqli_num_rows($u_exist) > 0) {
        $u_exist_fetch = mysqli_fetch_assoc($u_exist);

        if (password_verify($data['pass'], $u_exist_fetch['password'])) {

            $_SESSION['login'] = true;
            $_SESSION['user_id'] = $u_exist_fetch['user_id'];
            $_SESSION['id'] = $u_exist_fetch['user_id'];
            $_SESSION['email'] = $u_exist_fetch['email'];
            $_SESSION['full_name'] = $u_exist_fetch['full_name'];
            $_SESSION['name'] = $u_exist_fetch['full_name'];
            $_SESSION['phone_number'] = $u_exist_fetch['phone_number'];
            $_SESSION['phonenum'] = $u_exist_fetch['phone_number'];

            exit('success');

        } else {
            exit('Invalid password.');
        }

    } else {
        exit('Email not registered.');
    }

} else {
    exit('Invalid input.');
}
?>