<?php
if (session_status() === PHP_SESSION_NONE) {
    session_start();
}
?>

<!-- Navbar -->
<nav class="navbar navbar-expand-lg navbar-light bg-white px-lg-3 py-lg-2 shadow-sm sticky-top">
    <div class="container-fluid">
        <a class="navbar-brand me-5 fw-bold fs-3 h-font" href="index.php">MYBUS</a>

        <button class="navbar-toggler shadow-none" type="button" data-bs-toggle="collapse"
            data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent"
            aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarSupportedContent">

            <ul class="navbar-nav me-auto mb-2 mb-lg-0">

                <?php if (isset($_SESSION['login']) && $_SESSION['login'] == true) { ?>

                    <li class="nav-item">
                        <a class="nav-link h-font me-2 chover" href="index.php">Home</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link h-font me-2 chover" href="index.php#searchTrip">Search Trips</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link h-font me-2 chover" href="bookings.php">My Bookings</a>
                    </li>

                <?php } else { ?>

                    <li class="nav-item">
                        <a class="nav-link h-font me-2 chover" href="index.php">Home</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link h-font me-2 chover" href="index.php#about">About</a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link h-font me-2 chover" href="index.php#contactus">Contact</a>
                    </li>

                <?php } ?>

            </ul>

            <?php if (isset($_SESSION['login']) && $_SESSION['login'] == true) { ?>

                <div class="d-flex align-items-center">
                    <a href="profile.php" class="btn btn-outline-dark shadow-none me-lg-3 me-2 h-font">
                        Profile
                    </a>

                    <a href="logout.php" class="btn btn-dark shadow-none h-font">
                        Logout
                    </a>
                </div>

            <?php } else { ?>

                <div class="d-flex">
                    <button class="btn btn-outline-dark shadow-none me-lg-3 me-2 h-font"
                        data-bs-toggle="modal" data-bs-target="#loginModal">
                        Login
                    </button>

                    <button class="btn btn-dark shadow-none h-font"
                        data-bs-toggle="modal" data-bs-target="#registerModal">
                        Register
                    </button>
                </div>

            <?php } ?>

        </div>
    </div>
</nav>

<!-- Login Modal -->
<div class="modal fade" id="loginModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1"
    aria-labelledby="loginModalLabel" aria-hidden="true">

    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-0 rounded-4 overflow-hidden">

            <form id="login_form" method="POST">

                <div class="modal-header bg-dark text-white">
                    <div>
                        <h1 class="modal-title fs-5" id="loginModalLabel">
                            <i class="bi bi-person-circle fs-3 me-2"></i>Passenger Login
                        </h1>
                        <small>Login to book trips and view your reservations.</small>
                    </div>

                    <button type="reset" class="btn-close btn-close-white shadow-none"
                        data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

                <div class="modal-body p-4">

                    <div class="mb-3">
                        <label for="login_email" class="form-label fw-bold">Email</label>
                        <input type="email" id="login_email" name="email"
                            class="form-control shadow-none"
                            placeholder="Enter your email" required>
                    </div>

                    <div class="mb-3">
                        <label for="pass" class="form-label fw-bold">Password</label>

                        <div class="input-group">
                            <input type="password" id="pass" name="password"
                                class="form-control shadow-none"
                                placeholder="Enter your password" required>

                            <button class="btn btn-outline-secondary" type="button" id="toggleLoginPassword">
                                <i class="bi bi-eye" id="loginEyeIcon"></i>
                            </button>
                        </div>
                    </div>

                    <button type="submit" class="btn btn-dark shadow-none w-100 py-2">
                        Login
                    </button>

                    <div class="text-center mt-3">
                        <small>
                            Don’t have an account?
                            <a href="#" data-bs-toggle="modal" data-bs-target="#registerModal"
                                data-bs-dismiss="modal">
                                Register here
                            </a>
                        </small>
                    </div>

                </div>

            </form>

        </div>
    </div>
</div>

<!-- Register Modal -->
<div class="modal fade" id="registerModal" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1"
    aria-labelledby="registerModalLabel" aria-hidden="true">

    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-0 rounded-4 overflow-hidden">

            <form id="register_form" method="POST">

                <div class="modal-header bg-dark text-white">
                    <div>
                        <h1 class="modal-title fs-5" id="registerModalLabel">
                            <i class="bi bi-person-plus fs-3 me-2"></i>Create Passenger Account
                        </h1>
                        <small>Register to reserve seats and manage your bookings.</small>
                    </div>

                    <button type="reset" class="btn-close btn-close-white shadow-none"
                        data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

                <div class="modal-body p-4">

                    <div class="mb-3">
                        <label for="name" class="form-label fw-bold">Full Name</label>
                        <input type="text" id="name" name="full_name"
                            class="form-control shadow-none"
                            placeholder="Enter your full name" required>
                    </div>

                    <div class="mb-3">
                        <label for="register_email" class="form-label fw-bold">Email</label>
                        <input type="email" id="register_email" name="email"
                            class="form-control shadow-none"
                            placeholder="Enter your email" required>
                    </div>

                    <div class="mb-3">
                        <label for="phonenum" class="form-label fw-bold">Phone Number</label>
                        <input type="tel" id="phonenum" name="phone_number"
                            class="form-control shadow-none"
                            placeholder="Enter your phone number" required>
                    </div>

                    <div class="mb-3">
                        <label for="register_pass" class="form-label fw-bold">Password</label>

                        <div class="input-group">
                            <input type="password" id="register_pass" name="password"
                                class="form-control shadow-none"
                                placeholder="Create password" required>

                            <button class="btn btn-outline-secondary" type="button" id="toggleRegisterPassword">
                                <i class="bi bi-eye" id="registerEyeIcon"></i>
                            </button>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label for="cpass" class="form-label fw-bold">Confirm Password</label>
                        <input type="password" id="cpass" name="confirm_password"
                            class="form-control shadow-none"
                            placeholder="Confirm password" required>
                    </div>

                    <button type="submit" class="btn btn-dark shadow-none w-100 py-2">
                        Register
                    </button>

                    <div class="text-center mt-3">
                        <small>
                            Already have an account?
                            <a href="#" data-bs-toggle="modal" data-bs-target="#loginModal"
                                data-bs-dismiss="modal">
                                Login here
                            </a>
                        </small>
                    </div>

                </div>

            </form>

        </div>
    </div>
</div>

<script>
document.addEventListener("DOMContentLoaded", function () {
    const toggleLoginPassword = document.getElementById("toggleLoginPassword");
    const loginPassword = document.getElementById("pass");
    const loginEyeIcon = document.getElementById("loginEyeIcon");

    if (toggleLoginPassword && loginPassword && loginEyeIcon) {
        toggleLoginPassword.addEventListener("click", function () {
            const type = loginPassword.getAttribute("type") === "password" ? "text" : "password";
            loginPassword.setAttribute("type", type);

            loginEyeIcon.classList.toggle("bi-eye");
            loginEyeIcon.classList.toggle("bi-eye-slash");
        });
    }

    const toggleRegisterPassword = document.getElementById("toggleRegisterPassword");
    const registerPassword = document.getElementById("register_pass");
    const registerEyeIcon = document.getElementById("registerEyeIcon");

    if (toggleRegisterPassword && registerPassword && registerEyeIcon) {
        toggleRegisterPassword.addEventListener("click", function () {
            const type = registerPassword.getAttribute("type") === "password" ? "text" : "password";
            registerPassword.setAttribute("type", type);

            registerEyeIcon.classList.toggle("bi-eye");
            registerEyeIcon.classList.toggle("bi-eye-slash");
        });
    }
});
</script>