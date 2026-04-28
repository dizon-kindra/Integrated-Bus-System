<?php
if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

header('Content-Type: application/json');

$raw_data = file_get_contents('php://input');
$data = json_decode($raw_data, true);

if (!$data) {
    echo json_encode([
        'success' => false,
        'message' => 'No user data received.'
    ]);
    exit;
}

$user_id = isset($data['user_id']) ? (int)$data['user_id'] : 0;
$full_name = isset($data['full_name']) ? trim($data['full_name']) : '';
$email = isset($data['email']) ? trim($data['email']) : '';
$phone_number = isset($data['phone_number']) ? trim($data['phone_number']) : '';

if ($user_id <= 0 || $full_name === '' || $email === '') {
    echo json_encode([
        'success' => false,
        'message' => 'Invalid user data.',
        'received' => $data
    ]);
    exit;
}

$_SESSION['login'] = true;
$_SESSION['user_id'] = $user_id;
$_SESSION['id'] = $user_id;

$_SESSION['email'] = $email;
$_SESSION['uEmail'] = $email;

$_SESSION['full_name'] = $full_name;
$_SESSION['name'] = $full_name;
$_SESSION['uName'] = $full_name;

$_SESSION['phone_number'] = $phone_number;
$_SESSION['phonenum'] = $phone_number;
$_SESSION['uPhone'] = $phone_number;

session_write_close();

echo json_encode([
    'success' => true,
    'message' => 'Session saved successfully.',
    'session' => [
        'login' => true,
        'user_id' => $user_id,
        'full_name' => $full_name,
        'email' => $email
    ]
]);