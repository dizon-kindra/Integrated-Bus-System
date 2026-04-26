<?php
require('../admin/inc/db_config.php');
require('../admin/inc/essentials.php');

if (isset($_POST['name'], $_POST['email'], $_POST['phonenum'], $_POST['pass'], $_POST['cpass'])) {

    $data = filteration($_POST);

    $full_name = trim($data['name']);
    $email = trim($data['email']);
    $phone_number = trim($data['phonenum']);
    $password = $data['pass'];
    $confirm_password = $data['cpass'];

    if ($full_name == '' || $email == '' || $phone_number == '' || $password == '' || $confirm_password == '') {
        exit('All fields are required.');
    }

    if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
        exit('Invalid email address.');
    }

    if ($password !== $confirm_password) {
        exit('Password and confirm password do not match.');
    }

    if (strlen($password) < 6) {
        exit('Password must be at least 6 characters.');
    }

    $u_exist = select(
        "SELECT user_id FROM users WHERE email = ? OR phone_number = ? LIMIT 1",
        [$email, $phone_number],
        'ss'
    );

    if (mysqli_num_rows($u_exist) > 0) {
        exit('Email or phone number already registered.');
    }

    $hashed_password = password_hash($password, PASSWORD_DEFAULT);

    $query = "
        INSERT INTO users 
            (full_name, email, phone_number, password)
        VALUES 
            (?, ?, ?, ?)
    ";

    $values = [$full_name, $email, $phone_number, $hashed_password];

    if (insert($query, $values, 'ssss')) {
        exit('success');
    } else {
        exit('Registration failed. Please try again.');
    }

} else {
    exit('Invalid input.');
}
?>