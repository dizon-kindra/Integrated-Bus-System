<!-- Footer  -->
<div class="container-fluid bg-white mt-5">
    <div class="row">
        <div class="col-lg-4 p-4">
            <h3 class="h-font fw-bold fs-3 mb-2">MYBUS</h3>
            <p>
                At MYBUS, we provide a convenient and reliable bus reservation experience.
                Passengers can search available trips, reserve seats, and manage bookings easily.
            </p>
        </div>

        <div class="col-lg-4 p-4">
            <h5 class="mb-3 h-font">Links</h5>
            <a href="index.php" class="d-inline-block mb-2 text-dark text-decoration-none">Home</a><br>
            <a href="bus.php?view=all" class="d-inline-block mb-2 text-dark text-decoration-none">Buses</a><br>
            <a href="bookings.php" class="d-inline-block mb-2 text-dark text-decoration-none">My Bookings</a><br>
            <a href="index.php#contact" class="d-inline-block mb-2 text-dark text-decoration-none">Contact Us</a><br>
            <a href="index.php#about" class="d-inline-block mb-2 text-dark text-decoration-none">About</a><br>
        </div>

        <div class="col-lg-4 p-4">
            <h5 class="mb-3 h-font">Follow Us</h5>

            <a href="#" class="d-inline-block mb-3 text-dark text-decoration-none mb-2">
                <i class="bi bi-twitter me-1"></i>Twitter
            </a><br>

            <a href="#" class="d-inline-block mb-3 text-dark text-decoration-none mb-2">
                <i class="bi bi-facebook me-1"></i>Facebook
            </a><br>

            <a href="#" class="d-inline-block mb-3 text-dark text-decoration-none">
                <i class="bi bi-instagram me-1"></i>Instagram
            </a>
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
const API_BASE_URL = "http://localhost:3000/api";

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

    // =========================
    // PASSENGER LOGIN - NODE API
    // =========================
    if (loginForm) {
        loginForm.addEventListener('submit', function (e) {
            e.preventDefault();

            const formData = new FormData(loginForm);

            // Your old header.php may use name="pass"
            // Node API requires "password"
            const loginData = {
                email: formData.get('email') || '',
                password: formData.get('password') || formData.get('pass') || ''
            };

            console.log('Login Data Sent:', loginData);

            fetch(`${API_BASE_URL}/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(loginData)
            })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                console.log('Node Login Response:', data);

                if (!data.success) {
                    showAlert('danger', data.message || 'Login failed.');
                    return;
                }

                return fetch('ajax/set_node_session.php', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data.user)
                })
                .then(function (response) {
                    return response.json();
                })
                .then(function (sessionData) {
                    console.log('Session Save Response:', sessionData);

                    if (!sessionData.success) {
                        showAlert('danger', sessionData.message || 'Unable to save login session.');
                        return;
                    }

                    const modalElement = document.getElementById('loginModal');

                    if (modalElement) {
                        const modalInstance = bootstrap.Modal.getOrCreateInstance(modalElement);
                        modalInstance.hide();
                    }

                    showAlert('success', 'Login successful!');

                    loginForm.reset();

                    setTimeout(function () {
                        window.location.href = 'index.php';
                    }, 1000);
                });
            })
            .catch(function (error) {
                console.error('Login Error:', error);
                showAlert('danger', 'Login failed. Make sure Node API is running.');
            });
        });
    }

    // =============================
    // PASSENGER REGISTER - NODE API
    // =============================
    if (registerForm) {
        registerForm.addEventListener('submit', function (e) {
            e.preventDefault();

            const formData = new FormData(registerForm);

            // Your old header.php may use:
            // name, phonenum, pass, cpass
            // Node API requires:
            // full_name, phone_number, password, confirm_password
            const registerData = {
                full_name: formData.get('full_name') || formData.get('name') || '',
                email: formData.get('email') || '',
                phone_number: formData.get('phone_number') || formData.get('phonenum') || '',
                password: formData.get('password') || formData.get('pass') || '',
                confirm_password: formData.get('confirm_password') || formData.get('cpass') || ''
            };

            console.log('Register Data Sent:', registerData);

            fetch(`${API_BASE_URL}/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(registerData)
            })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                console.log('Node Register Response:', data);

                if (!data.success) {
                    showAlert('danger', data.message || 'Registration failed.');
                    return;
                }

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
                        const loginModal = bootstrap.Modal.getOrCreateInstance(loginModalElement);
                        loginModal.show();
                    }
                }, 800);
            })
            .catch(function (error) {
                console.error('Registration Error:', error);
                showAlert('danger', 'Registration failed. Make sure Node API is running.');
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