<!-- Footer -->
<div class="container-fluid bg-white mt-5">
    <div class="row">
        <div class="col-lg-4 p-4">
            <h3 class="h-font fw-bold fs-3 mb-2">MYBUS</h3>
            <p>
                At MYBUS, we offer a seamless travel experience with top-notch amenities and exceptional customer service.
                Whether for business or leisure, we ensure your journey is comfortable and hassle-free.
            </p>
        </div>

        <div class="col-lg-4 p-4">
            <h5 class="mb-3 h-font">Quick Links</h5>

            <a href="index.php" class="d-inline-block mb-2 text-dark text-decoration-none">
                Home
            </a><br>

            <a href="index.php#searchTrip" class="d-inline-block mb-2 text-dark text-decoration-none">
                Search Trips
            </a><br>

            <a href="bookings.php" class="d-inline-block mb-2 text-dark text-decoration-none">
                My Bookings
            </a><br>

            <a href="index.php#contactus" class="d-inline-block mb-2 text-dark text-decoration-none">
                Contact Us
            </a><br>
        </div>

        <div class="col-lg-4 p-4">
            <h5 class="mb-3 h-font">Follow Us</h5>

            <a href="#" class="d-inline-block mb-3 text-dark text-decoration-none">
                <i class="bi bi-twitter me-1"></i>Twitter
            </a><br>

            <a href="#" class="d-inline-block mb-3 text-dark text-decoration-none">
                <i class="bi bi-facebook me-1"></i>Facebook
            </a><br>

            <a href="#" class="d-inline-block mb-3 text-dark text-decoration-none">
                <i class="bi bi-instagram me-1"></i>Instagram
            </a><br>
        </div>
    </div>
</div>

<h6 class="text-center bg-dark text-white p-3 m-0 h-font">
    Designed and Developed by MYBUS
</h6>

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"
    integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz"
    crossorigin="anonymous"></script>

<script>
function showAlert(type, message) {
    const alertType = type === 'success' ? 'success' : 'danger';

    const alertDiv = document.createElement('div');
    alertDiv.className = 'alert alert-' + alertType + ' alert-dismissible fade show position-fixed top-50 start-50 translate-middle shadow text-center px-5 py-4';
    alertDiv.style.zIndex = '9999';
    alertDiv.style.minWidth = '350px';
    alertDiv.style.borderRadius = '16px';

    alertDiv.innerHTML =
        '<h5 class="mb-2">' + (alertType === 'success' ? 'Success!' : 'Error!') + '</h5>' +
        '<p class="mb-0">' + message + '</p>' +
        '<button type="button" class="btn-close position-absolute top-0 end-0 m-3" data-bs-dismiss="alert"></button>';

    document.body.appendChild(alertDiv);

    setTimeout(function () {
        if (alertDiv) {
            alertDiv.remove();
        }
    }, 1800);
}

document.addEventListener('DOMContentLoaded', function () {
    const loginForm = document.getElementById('login_form');
    const registerForm = document.getElementById('register_form');

    if (loginForm) {
        loginForm.addEventListener('submit', function (e) {
            e.preventDefault();

            const formData = new FormData(this);

            fetch('ajax/login.php', {
                method: 'POST',
                body: formData
            })
            .then(function (response) {
                return response.text();
            })
            .then(function (data) {
                console.log('Login Response:', data);

                if (data.trim() === 'success') {
                    const modalElement = document.getElementById('loginModal');

                    if (modalElement) {
                        const modalInstance = bootstrap.Modal.getOrCreateInstance(modalElement);
                        modalInstance.hide();
                    }

                    showAlert('success', 'Login successful! Welcome to MYBUS.');

                    loginForm.reset();

                    setTimeout(function () {
                        window.location.href = 'index.php';
                    }, 1800);
                } else {
                    showAlert('danger', data);
                }
            })
            .catch(function (error) {
                console.error('Login Error:', error);
                showAlert('danger', 'Login failed. Please try again.');
            });
        });
    }

    if (registerForm) {
        registerForm.addEventListener('submit', function (e) {
            e.preventDefault();

            const formData = new FormData(this);

            fetch('ajax/register.php', {
                method: 'POST',
                body: formData
            })
            .then(function (response) {
                return response.text();
            })
            .then(function (data) {
                console.log('Register Response:', data);

                if (data.trim() === 'success') {
                    const modalElement = document.getElementById('registerModal');

                    if (modalElement) {
                        const modalInstance = bootstrap.Modal.getOrCreateInstance(modalElement);
                        modalInstance.hide();
                    }

                    showAlert('success', 'Registration successful! You can now login.');

                    registerForm.reset();

                    setTimeout(function () {
                        const loginModalElement = document.getElementById('loginModal');

                        if (loginModalElement) {
                            const loginModal = new bootstrap.Modal(loginModalElement);
                            loginModal.show();
                        }
                    }, 700);
                } else {
                    showAlert('danger', data);
                }
            })
            .catch(function (error) {
                console.error('Registration Error:', error);
                showAlert('danger', 'Registration failed. Please try again.');
            });
        });
    }
});

function checkLoginToBook(status, schedule_id, passengers) {
    if (status) {
        window.location.href = 'confirm_booking.php?schedule_id=' + schedule_id + '&passengers=' + passengers;
    } else {
        showAlert('danger', 'Please log in to book a bus!');
    }
}
</script>