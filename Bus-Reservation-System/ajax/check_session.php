<?php
if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

header('Content-Type: application/json');

echo json_encode([
    'login' => $_SESSION['login'] ?? false,
    'user_id' => $_SESSION['user_id'] ?? null,
    'id' => $_SESSION['id'] ?? null,
    'full_name' => $_SESSION['full_name'] ?? null,
    'email' => $_SESSION['email'] ?? null,
    'all_session' => $_SESSION
]);