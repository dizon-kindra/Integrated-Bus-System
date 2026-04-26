<?php
session_start();
require_once "db.php";

$data = json_decode(file_get_contents("php://input"), true);

$email = trim($data['email'] ?? '');
$password = $data['password'] ?? '';

if ($email == '' || $password == '') {
    json_response([
        "success" => false,
        "message" => "Email and password are required."
    ]);
}

$sql = "
    SELECT 
        user_id,
        full_name,
        email,
        phone_number,
        password,
        created_at
    FROM users
    WHERE email = ?
    LIMIT 1
";

$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $email);
$stmt->execute();

$result = $stmt->get_result();

if ($result->num_rows == 0) {
    json_response([
        "success" => false,
        "message" => "Invalid email or password."
    ]);
}

$user = $result->fetch_assoc();

if (!password_verify($password, $user['password'])) {
    json_response([
        "success" => false,
        "message" => "Invalid email or password."
    ]);
}

// Store passenger login data in PHP session
$_SESSION['login'] = true;
$_SESSION['user_id'] = (int)$user['user_id'];
$_SESSION['full_name'] = $user['full_name'];
$_SESSION['email'] = $user['email'];
$_SESSION['phone_number'] = $user['phone_number'];

json_response([
    "success" => true,
    "message" => "Login successful.",
    "user" => [
        "user_id" => (int)$user['user_id'],
        "full_name" => $user['full_name'],
        "email" => $user['email'],
        "phone_number" => $user['phone_number'],
        "created_at" => $user['created_at']
    ]
]);
?>