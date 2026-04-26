<?php
session_start();
require_once "db.php";

$data = json_decode(file_get_contents("php://input"), true);

$user_id = isset($data['user_id']) ? (int)$data['user_id'] : 0;
$full_name = trim($data['full_name'] ?? '');
$phone_number = trim($data['phone_number'] ?? '');

if ($user_id <= 0 || $full_name == '' || $phone_number == '') {
    json_response([
        "success" => false,
        "message" => "All fields are required."
    ]);
}

$sql = "UPDATE users SET full_name = ?, phone_number = ? WHERE user_id = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ssi", $full_name, $phone_number, $user_id);

if ($stmt->execute()) {
    $_SESSION['full_name'] = $full_name;
    $_SESSION['name'] = $full_name;
    $_SESSION['phone_number'] = $phone_number;
    $_SESSION['phonenum'] = $phone_number;

    json_response([
        "success" => true,
        "message" => "Profile updated successfully."
    ]);
}

json_response([
    "success" => false,
    "message" => "Failed to update profile."
]);
?>