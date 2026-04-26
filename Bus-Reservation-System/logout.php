<?php
require('admin/inc/essentials.php');
session_start();
session_unset(); 
session_destroy();  

?>
<!DOCTYPE html>
<html>
<head>
    <title>Logout</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>

<script>
function showAlert(type, message) {
    const alertDiv = document.createElement('div');
    alertDiv.className = "alert alert-success position-fixed top-50 start-50 translate-middle text-center px-5 py-4 shadow";
    alertDiv.style.zIndex = "9999";
    alertDiv.style.borderRadius = "16px";

    alertDiv.innerHTML = `
        <h5 class="mb-2">Success!</h5>
        <p>${message}</p>
    `;

    document.body.appendChild(alertDiv);

    setTimeout(() => {
        window.location.href = "index.php";
    }, 1500);
}

showAlert('success', 'Logout successful!');
</script>

</body>
</html>