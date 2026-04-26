<?php
require('admin/inc/db_config.php');
require('admin/inc/essentials.php');

session_start();

if (!(isset($_SESSION['login']) && $_SESSION['login'] == true)) {
    redirect('index.php');
}

$user_id = $_SESSION['user_id'] ?? $_SESSION['id'] ?? 0;

if ($user_id == 0) {
    redirect('logout.php');
}

$user_res = select(
    "SELECT user_id, full_name, email, phone_number, created_at 
     FROM users 
     WHERE user_id = ? 
     LIMIT 1",
    [$user_id],
    'i'
);

if (mysqli_num_rows($user_res) == 0) {
    redirect('logout.php');
}

$user_data = mysqli_fetch_assoc($user_res);
?>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>MYBUS - Profile</title>
    <?php require('inc/links.php'); ?>

    <style>
        .profile-card {
            border-radius: 18px;
        }

        .profile-icon {
            font-size: 70px;
            color: #AD8B3A;
        }

        .profile-label {
            font-weight: 700;
            color: #333;
        }

        .update-btn {
            background: #AD8B3A;
            color: white;
            border: none;
        }

        .update-btn:hover {
            background: #8f722e;
            color: white;
        }

        .center-popup-icon {
            font-size: 52px;
        }

        .center-popup-btn {
            background: #AD8B3A;
            color: white;
            border: none;
        }

        .center-popup-btn:hover {
            background: #8f722e;
            color: white;
        }
    </style>
</head>

<body class="bg-light">

<?php require('inc/header.php'); ?>

<div class="container">
    <div class="row">
        <div class="col-12 my-5 px-4">
            <h2 class="fw-bold h-font">PROFILE</h2>
            <div style="font-size:14px;">
                <a href="index.php" class="text-secondary text-decoration-none">HOME</a>
                <span class="text-secondary"> > </span>
                <span class="text-secondary">PROFILE</span>
            </div>
        </div>

        <div class="col-lg-8 col-md-10 mx-auto mb-5">
            <div class="card border-0 shadow profile-card">
                <div class="card-body p-4">

                    <div class="text-center mb-4">
                        <i class="bi bi-person-circle profile-icon"></i>
                        <h4 class="fw-bold mt-2 mb-0">
                            <?php echo htmlspecialchars($user_data['full_name']); ?>
                        </h4>
                        <p class="text-muted mb-0">
                            Passenger Account
                        </p>
                    </div>

                    <hr>

                    <form id="profileForm">
                        <input type="hidden" id="user_id" value="<?php echo (int)$user_data['user_id']; ?>">

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label profile-label">Full Name</label>
                                <input type="text" id="full_name" class="form-control shadow-none"
                                    value="<?php echo htmlspecialchars($user_data['full_name']); ?>" required>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label profile-label">Email</label>
                                <input type="email" id="email" class="form-control shadow-none"
                                    value="<?php echo htmlspecialchars($user_data['email']); ?>" readonly>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label profile-label">Phone Number</label>
                                <input type="text" id="phone_number" class="form-control shadow-none"
                                    value="<?php echo htmlspecialchars($user_data['phone_number']); ?>" required>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label class="form-label profile-label">Registered On</label>
                                <input type="text" class="form-control shadow-none"
                                    value="<?php echo date('F d, Y | h:i A', strtotime($user_data['created_at'])); ?>" readonly>
                            </div>
                        </div>

                        <button type="submit" class="btn update-btn shadow-none px-4">
                            Update Profile
                        </button>
                    </form>

                    <hr class="my-4">

                    <h5 class="fw-bold mb-3">Change Password</h5>

                    <form id="passwordForm">
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <label class="form-label profile-label">Current Password</label>
                                <input type="password" id="current_password" class="form-control shadow-none" required>
                            </div>

                            <div class="col-md-4 mb-3">
                                <label class="form-label profile-label">New Password</label>
                                <input type="password" id="new_password" class="form-control shadow-none" required>
                            </div>

                            <div class="col-md-4 mb-3">
                                <label class="form-label profile-label">Confirm Password</label>
                                <input type="password" id="confirm_password" class="form-control shadow-none" required>
                            </div>
                        </div>

                        <button type="submit" class="btn btn-dark shadow-none px-4">
                            Change Password
                        </button>
                    </form>

                </div>
            </div>
        </div>
    </div>
</div>

<!-- Center Popup Modal -->
<div class="modal fade" id="messageModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-0 shadow rounded-4">
            <div class="modal-body text-center p-5">
                <div id="messageIcon" class="center-popup-icon mb-3">✅</div>

                <h5 class="fw-bold mb-2" id="messageTitle">
                    Success
                </h5>

                <p class="text-muted mb-4" id="messageText">
                    Profile updated successfully.
                </p>

                <button type="button" class="btn center-popup-btn px-5 fw-semibold"
                    id="messageOkBtn">
                    OK
                </button>
            </div>
        </div>
    </div>
</div>

<script>
document.addEventListener('DOMContentLoaded', function () {
    console.log("Profile page JS loaded");

    const profileForm = document.getElementById('profileForm');
    const passwordForm = document.getElementById('passwordForm');

    function showCenterPopup(type, message, callback = null) {
        const modalEl = document.getElementById('messageModal');
        const messageIcon = document.getElementById('messageIcon');
        const messageTitle = document.getElementById('messageTitle');
        const messageText = document.getElementById('messageText');
        const messageOkBtn = document.getElementById('messageOkBtn');

        if (type === 'success') {
            messageIcon.innerHTML = '✅';
            messageTitle.innerText = 'Success';
        } else {
            messageIcon.innerHTML = '❌';
            messageTitle.innerText = 'Error';
        }

        messageText.innerText = message;

        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();

        messageOkBtn.onclick = function () {
            modal.hide();

            if (callback) {
                callback();
            }
        };
    }

    if (profileForm) {
        profileForm.addEventListener('submit', function(e) {
            e.preventDefault();

            const userId = document.getElementById('user_id').value;
            const fullName = document.getElementById('full_name').value.trim();
            const phoneNumber = document.getElementById('phone_number').value.trim();

            fetch('api/update_profile.php', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    user_id: parseInt(userId),
                    full_name: fullName,
                    phone_number: phoneNumber
                })
            })
            .then(function(response) {
                return response.text();
            })
            .then(function(text) {
                console.log("Update profile raw response:", text);

                const data = JSON.parse(text);

                if (data.success) {
                    showCenterPopup('success', data.message, function () {
                        location.reload();
                    });
                } else {
                    showCenterPopup('error', data.message);
                }
            })
            .catch(function(error) {
                console.error("Profile update error:", error);
                showCenterPopup('error', 'Profile update failed.');
            });
        });
    }

    if (passwordForm) {
        passwordForm.addEventListener('submit', function(e) {
            e.preventDefault();

            const userId = document.getElementById('user_id').value;
            const currentPassword = document.getElementById('current_password').value;
            const newPassword = document.getElementById('new_password').value;
            const confirmPassword = document.getElementById('confirm_password').value;

            fetch('api/change_password.php', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    user_id: parseInt(userId),
                    current_password: currentPassword,
                    new_password: newPassword,
                    confirm_password: confirmPassword
                })
            })
            .then(function(response) {
                return response.text();
            })
            .then(function(text) {
                console.log("Change password raw response:", text);

                const data = JSON.parse(text);

                if (data.success) {
                    showCenterPopup('success', data.message, function () {
                        passwordForm.reset();
                    });
                } else {
                    showCenterPopup('error', data.message);
                }
            })
            .catch(function(error) {
                console.error("Password change error:", error);
                showCenterPopup('error', 'Password change failed.');
            });
        });
    }
});
</script>

<?php require('inc/footer.php'); ?>

</body>
</html>