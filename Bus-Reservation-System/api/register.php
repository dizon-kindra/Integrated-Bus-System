<?php
require_once "db.php";

$data = json_decode(file_get_contents("php://input"), true);

$full_name = trim($data['full_name'] ?? '');
$email = trim($data['email'] ?? '');
$phone_number = trim($data['phone_number'] ?? '');
$password = $data['password'] ?? '';
$confirm_password = $data['confirm_password'] ?? '';

if ($full_name == '' || $email == '' || $phone_number == '' || $password == '' || $confirm_password == '') {
    json_response([
        "success" => false,
        "message" => "All fields are required."
    ]);
}

if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
    json_response([
        "success" => false,
        "message" => "Invalid email address."
    ]);
}

if ($password !== $confirm_password) {
    json_response([
        "success" => false,
        "message" => "Passwords do not match."
    ]);
}

if (strlen($password) < 6) {
    json_response([
        "success" => false,
        "message" => "Password must be at least 6 characters."
    ]);
}

$checkSql = "SELECT user_id FROM users WHERE email = ?";
$checkStmt = $conn->prepare($checkSql);
$checkStmt->bind_param("s", $email);
$checkStmt->execute();
$checkResult = $checkStmt->get_result();

if ($checkResult->num_rows > 0) {
    json_response([
        "success" => false,
        "message" => "Email already registered."
    ]);
}

$hashedPassword = password_hash($password, PASSWORD_DEFAULT);

$insertSql = "
    INSERT INTO users 
    (
        full_name,
        email,
        phone_number,
        password
    )
    VALUES (?, ?, ?, ?)
";

$insertStmt = $conn->prepare($insertSql);
$insertStmt->bind_param("ssss", $full_name, $email, $phone_number, $hashedPassword);

if ($insertStmt->execute()) {
    json_response([
        "success" => true,
        "message" => "Registration successful.",
        "user" => [
            "user_id" => $conn->insert_id,
            "full_name" => $full_name,
            "email" => $email,
            "phone_number" => $phone_number
        ]
    ]);
} else {
    json_response([
        "success" => false,
        "message" => "Registration failed."
    ]);
}
?>