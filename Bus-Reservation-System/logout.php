<?php
require_once(__DIR__ . '/inc/essentials.php');

if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

session_unset();
session_destroy();
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Logout</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>

<body>

<script>
function showAlert(message) {
    const alertDiv = document.createElement('div');

    alertDiv.className = "alert alert-success position-fixed top-50 start-50 translate-middle text-center px-5 py-4 shadow";
    alertDiv.style.zIndex = "9999";
    alertDiv.style.borderRadius = "16px";

    alertDiv.innerHTML = `
        <h5 class="mb-2">Success!</h5>
        <p class="mb-0">${message}</p>
    `;

    document.body.appendChild(alertDiv);

    setTimeout(() => {
        window.location.href = "index.php";
    }, 1500);
}

showAlert('Logout successful!');
</script>

</body>
</html>