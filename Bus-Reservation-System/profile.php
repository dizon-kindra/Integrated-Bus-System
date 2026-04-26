<?php
session_start();
require('admin/inc/db_config.php');
require('admin/inc/essentials.php');

if (!isset($_SESSION['login']) || $_SESSION['login'] != true) {
    header("Location: index.php");
    exit;
}

$user_id = $_SESSION['id'];

// FETCH USER DATA
$query = "SELECT * FROM users WHERE id=?";
$stmt = $con->prepare($query);
$stmt->bind_param("i", $user_id);
$stmt->execute();
$result = $stmt->get_result();
$user = $result->fetch_assoc();

// UPDATE PROFILE
if (isset($_POST['update_profile'])) {
    $name = $_POST['name'];
    $phone = $_POST['phone'];

    $query = "UPDATE users SET name=?, phonenum=? WHERE id=?";
    $stmt = $con->prepare($query);
    $stmt->bind_param("ssi", $name, $phone, $user_id);

    if ($stmt->execute()) {
        echo "<script>alert('Profile updated successfully'); window.location='profile.php';</script>";
    }
}

// CHANGE PASSWORD
if (isset($_POST['change_password'])) {
    $new_pass = password_hash($_POST['new_pass'], PASSWORD_DEFAULT);

    $query = "UPDATE users SET password=? WHERE id=?";
    $stmt = $con->prepare($query);
    $stmt->bind_param("si", $new_pass, $user_id);

    if ($stmt->execute()) {
        echo "<script>alert('Password changed successfully'); window.location='profile.php';</script>";
    }
}
?>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <title>My Profile - MYBUS</title>
    <?php require('inc/links.php'); ?>
</head>

<body class="bg-light">

<?php require('inc/header.php'); ?>

<div class="container mt-5">
    <div class="row">

        <!-- LEFT: USER INFO -->
        <div class="col-lg-4 mb-4">
            <div class="bg-white p-4 shadow rounded text-center">
                <i class="bi bi-person-circle fs-1"></i>
                <h4 class="mt-2"><?php echo $user['name']; ?></h4>
                <p class="text-muted"><?php echo $user['email']; ?></p>
                <p><strong>Phone:</strong> <?php echo $user['phonenum']; ?></p>
            </div>
        </div>

        <!-- RIGHT: EDIT FORM -->
        <div class="col-lg-8">

            <!-- UPDATE PROFILE -->
            <div class="bg-white p-4 shadow rounded mb-4">
                <h5 class="fw-bold mb-3">Update Profile</h5>

                <form method="POST">
                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label>Name</label>
                            <input type="text" name="name" class="form-control" value="<?php echo $user['name']; ?>" required>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label>Phone</label>
                            <input type="text" name="phone" class="form-control" value="<?php echo $user['phonenum']; ?>" required>
                        </div>
                    </div>

                    <button type="submit" name="update_profile" class="btn btn-primary">
                        Update Profile
                    </button>
                </form>
            </div>

            <!-- CHANGE PASSWORD -->
            <div class="bg-white p-4 shadow rounded">
                <h5 class="fw-bold mb-3">Change Password</h5>

                <form method="POST">
                    <div class="mb-3">
                        <label>New Password</label>
                        <input type="password" name="new_pass" class="form-control" required>
                    </div>

                    <button type="submit" name="change_password" class="btn btn-dark">
                        Change Password
                    </button>
                </form>
            </div>

        </div>
    </div>
</div>

<?php require('inc/footer.php'); ?>

</body>
</html>