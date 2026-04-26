<?php
session_start();

require('../admin/inc/db_config.php');
require('../admin/inc/essentials.php');

if (isset($_POST['email'], $_POST['pass'])) {

    $data = filteration($_POST);

    $email = trim($data['email']);
    $password = $data['pass'];

    if ($email == '' || $password == '') {
        exit('Email and password are required.');
    }

    $u_exist = select(
        "SELECT user_id, full_name, email, phone_number, password 
         FROM users 
         WHERE email = ? 
         LIMIT 1",
        [$email],
        's'
    );

    if (mysqli_num_rows($u_exist) == 0) {
        exit('Email not registered.');
    }

    $u_exist_fetch = mysqli_fetch_assoc($u_exist);

    if (!password_verify($password, $u_exist_fetch['password'])) {
        exit('Invalid password.');
    }

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
    exit('Invalid input.');
}
?>