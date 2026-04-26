<?php
session_start();
require_once "db.php";

$data = json_decode(file_get_contents("php://input"), true);

$user_id = isset($data['user_id']) ? (int)$data['user_id'] : 0;
$current_password = $data['current_password'] ?? '';
$new_password = $data['new_password'] ?? '';
$confirm_password = $data['confirm_password'] ?? '';

if ($user_id <= 0 || $current_password == '' || $new_password == '' || $confirm_password == '') {
    json_response([
        "success" => false,
        "message" => "All password fields are required."
    ]);
}

if ($new_password !== $confirm_password) {
    json_response([
        "success" => false,
        "message" => "New password and confirm password do not match."
    ]);
}

if (strlen($new_password) < 6) {
    json_response([
        "success" => false,
        "message" => "New password must be at least 6 characters."
    ]);
}

$sql = "SELECT password FROM users WHERE user_id = ? LIMIT 1";
$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $user_id);
$stmt->execute();

$result = $stmt->get_result();

if ($result->num_rows == 0) {
    json_response([
        "success" => false,
        "message" => "User not found."
    ]);
}

$user = $result->fetch_assoc();

if (!password_verify($current_password, $user['password'])) {
    json_response([
        "success" => false,
        "message" => "Current password is incorrect."
    ]);
}

$hashedPassword = password_hash($new_password, PASSWORD_DEFAULT);

$updateSql = "UPDATE users SET password = ? WHERE user_id = ?";
$updateStmt = $conn->prepare($updateSql);
$updateStmt->bind_param("si", $hashedPassword, $user_id);

if ($updateStmt->execute()) {
    json_response([
        "success" => true,
        "message" => "Password changed successfully."
    ]);
}

json_response([
    "success" => false,
    "message" => "Failed to change password."
]);
?>