<?php
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