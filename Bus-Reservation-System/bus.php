<?php
require_once(__DIR__ . '/inc/db_config.php');
require_once(__DIR__ . '/inc/essentials.php');

if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

$view_all = false;

if (isset($_GET['view']) && $_GET['view'] == 'all') {
    $view_all = true;
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>MYBUS - Search Trips</title>

    <?php require('inc/links.php'); ?>

    <style>
        .bus-card {
            border-radius: 16px;
        }

        .filter-card {
            border-radius: 12px;
        }

        .custom-bg {
            background-color: #AD8B3A;
            border: 1px solid #AD8B3A;
        }

        .custom-bg:hover {
            background-color: #8f722e;
            border-color: #8f722e;
        }

        .h-font {
            font-family: 'Cinzel', serif;
        }

        .trip-title {
            color: #172233;
        }
    </style>
</head>

<body class="bg-light">

<?php require('inc/header.php'); ?>

<div class="container-fluid">
    <div class="row">

        <div class="col-lg-3 col-md-12 mb-lg-0 mb-4 px-lg-4 mt-4">
            <nav class="navbar navbar-expand-lg navbar-light bg-white rounded shadow">
                <div class="container-fluid flex-lg-column align-items-stretch">
                    <h4 class="mt-2 h-font">FILTERS</h4>

                    <button class="navbar-toggler shadow-none" type="button" data-bs-toggle="collapse"
                        data-bs-target="#filterDropdown" aria-controls="filterDropdown" aria-expanded="false"
                        aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>

                    <div class="collapse navbar-collapse flex-column align-items-stretch mt-2" id="filterDropdown">

                        <div class="border bg-light p-3 rounded mb-3 filter-card">
                            <h5 class="mb-3 h-font">ROUTE</h5>

                            <label class="form-label fw-bold">Source</label>
                            <input type="text" id="source" class="form-control shadow-none mb-3"
                                placeholder="Enter origin">

                            <label class="form-label fw-bold">Destination</label>
                            <input type="text" id="destination" class="form-control shadow-none"
                                placeholder="Enter destination">
                        </div>

                        <div class="border bg-light p-3 rounded mb-3 filter-card">
                            <h5 class="mb-3 h-font">DATE & PASSENGERS</h5>

                            <label class="form-label fw-bold">Date</label>
                            <input type="date" id="date" class="form-control shadow-none mb-3">

                            <label class="form-label fw-bold">No. of Passengers</label>
                            <input type="number" id="passengers" class="form-control shadow-none mb-3"
                                value="1" min="1" max="9">

                            <button type="button" id="searchBtn" onclick="chk_avail_filter()"
                                class="btn text-white custom-bg shadow-none w-100 mb-2">
                                Search Trips
                            </button>

                            <button type="button" onclick="view_all_trips()"
                                class="btn btn-dark shadow-none w-100 mb-2">
                                View All
                            </button>

                            <button type="button" id="chk_avail_btn" onclick="chk_avail_clear()"
                                class="btn btn-outline-dark shadow-none w-100 d-none">
                                Clear Search
                            </button>
                        </div>

                    </div>
                </div>
            </nav>
        </div>

        <div class="col-lg-9 col-md-12 px-4 mt-4">
            <div class="mb-4">
                <h2 class="fw-bold h-font">AVAILABLE TRIPS</h2>
                <div style="font-size:14px;">
                    <a href="index.php" class="text-secondary text-decoration-none">HOME</a>
                    <span class="text-secondary"> > </span>
                    <span class="text-secondary">SEARCH TRIPS</span>
                </div>
            </div>

            <div id="bus-data">
                <div class="bg-white rounded shadow p-4 text-center">
                    <h4 class="text-muted mb-2">Loading trips...</h4>
                    <p class="text-muted mb-0">Please wait while we fetch available trips.</p>
                </div>
            </div>
        </div>

    </div>
</div>

<?php require('inc/footer.php'); ?>

<script>
    /*
        Use a unique variable name here to avoid conflict with other included scripts
        that may already declare API_BASE_URL.
    */
    var BUS_API_BASE_URL = "http://localhost:3000/api";

    let bus_data = document.getElementById('bus-data');
    let source = document.getElementById('source');
    let destination = document.getElementById('destination');
    let chk_avail_btn = document.getElementById('chk_avail_btn');
    let date = document.getElementById('date');
    let passengers = document.getElementById('passengers');

    let isLoggedIn = <?php echo (isset($_SESSION['login']) && $_SESSION['login'] == true) ? 'true' : 'false'; ?>;
    let viewAllMode = <?php echo $view_all ? 'true' : 'false'; ?>;

    function getPassengerCount() {
        let count = parseInt(passengers.value);

        if (isNaN(count) || count <= 0) {
            count = 1;
        }

        if (count > 9) {
            count = 9;
        }

        passengers.value = count;

        return count;
    }

    function formatTime(timeValue) {
        if (!timeValue) {
            return 'N/A';
        }

        let parts = timeValue.split(':');
        let hour = parseInt(parts[0]);
        let minute = parts[1];

        let ampm = hour >= 12 ? 'PM' : 'AM';
        hour = hour % 12;
        hour = hour ? hour : 12;

        return hour + ':' + minute + ' ' + ampm;
    }

    function formatDate(dateValue) {
        if (!dateValue) {
            return 'N/A';
        }

        const dateObj = new Date(dateValue);

        if (isNaN(dateObj.getTime())) {
            return dateValue;
        }

        return dateObj.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: '2-digit'
        });
    }

    function renderTrips(data) {
        if (!data.success) {
            bus_data.innerHTML = `
                <div class="bg-white rounded shadow p-4 text-center">
                    <h4 class="text-danger mb-2">${data.message || 'Unable to load trips.'}</h4>
                    <p class="text-muted mb-3">Please check your search details.</p>
                    <button class="btn btn-dark shadow-none" onclick="view_all_trips()">
                        View All Available Trips
                    </button>
                </div>
            `;
            return;
        }

        if (data.count === 0) {
            bus_data.innerHTML = `
                <div class="bg-white rounded shadow p-4 text-center">
                    <h4 class="text-danger mb-2">No trips found.</h4>
                    <p class="text-muted mb-3">
                        Try another route or departure date, or browse all available trips.
                    </p>
                    <button class="btn btn-dark shadow-none" onclick="view_all_trips()">
                        View All Available Trips
                    </button>
                </div>
            `;
            return;
        }

        let html = '';

        if (viewAllMode) {
            html += `
                <div class="alert alert-info shadow-sm">
                    Showing all available trips from the terminal schedule.
                </div>
            `;
        }

        data.trips.forEach(trip => {
            let bookButton = '';
            let passengerCount = getPassengerCount();
            let availableSeats = parseInt(trip.available_seats || 0);

            if (availableSeats <= 0) {
                bookButton = `
                    <button type="button" class="btn btn-sm btn-secondary shadow-none" disabled>
                        Fully Booked
                    </button>
                `;
            } else if (passengerCount > availableSeats) {
                bookButton = `
                    <button type="button" class="btn btn-sm btn-secondary shadow-none" disabled>
                        Not Enough Seats
                    </button>
                `;
            } else if (isLoggedIn) {
                bookButton = `
                    <a href="confirm_booking.php?schedule_id=${trip.schedule_id}&passengers=${passengerCount}" 
                       class="btn btn-sm text-white custom-bg shadow-none">
                        Book Now
                    </a>
                `;
            } else {
                bookButton = `
                    <button type="button" class="btn btn-sm text-white custom-bg shadow-none" 
                        data-bs-toggle="modal" data-bs-target="#loginModal">
                        Login to Book
                    </button>
                `;
            }

            html += `
                <div class="card mb-4 border-0 shadow bus-card">
                    <div class="row g-0 p-3 align-items-center">

                        <div class="col-md-4 mb-lg-0 mb-md-0 mb-3">
                            <h5 class="mb-2 fw-bold trip-title">${trip.bus_number || 'N/A'}</h5>
                            <p class="mb-1">
                                <span class="badge bg-dark">${trip.bus_type || 'N/A'}</span>
                            </p>
                            <p class="mb-1 text-muted">
                                <i class="bi bi-credit-card-2-front me-1"></i>
                                Plate No: ${trip.plate_number || 'N/A'}
                            </p>
                            <p class="mb-0 text-muted">
                                <i class="bi bi-people-fill me-1"></i>
                                Capacity: ${trip.capacity || 'N/A'}
                            </p>
                        </div>

                        <div class="col-md-5 px-lg-3 px-md-3 px-0 mb-lg-0 mb-md-0 mb-3">
                            <h6 class="mb-2 fw-bold">Trip Details</h6>

                            <p class="mb-1">
                                <i class="bi bi-geo-alt-fill me-1"></i>
                                ${trip.origin || 'N/A'} → ${trip.destination || 'N/A'}
                            </p>

                            <p class="mb-1">
                                <i class="bi bi-calendar-event me-1"></i>
                                ${formatDate(trip.departure_date)}
                            </p>

                            <p class="mb-1">
                                <i class="bi bi-clock me-1"></i>
                                Departure: ${formatTime(trip.departure_time)}
                            </p>

                            <p class="mb-1">
                                <i class="bi bi-clock-history me-1"></i>
                                Arrival: ${formatTime(trip.arrival_time)}
                            </p>

                            <p class="mb-0">
                                <i class="bi bi-hourglass-split me-1"></i>
                                Duration: ${trip.estimated_duration || 'N/A'}
                            </p>
                        </div>

                        <div class="col-md-3 text-center">
                            <h5 class="mb-2 text-success fw-bold">₱${parseFloat(trip.fare || 0).toFixed(2)}</h5>

                            <p class="mb-2">
                                <span class="badge bg-primary">
                                    ${trip.available_seats || 0} seats available
                                </span>
                            </p>

                            <p class="mb-3">
                                <span class="badge bg-success">${trip.trip_status || 'Scheduled'}</span>
                            </p>

                            ${bookButton}
                        </div>

                    </div>
                </div>
            `;
        });

        bus_data.innerHTML = html;
    }

    function fetch_bus() {
        viewAllMode = false;

        if (source.value.trim() === '' || destination.value.trim() === '' || date.value === '') {
            bus_data.innerHTML = `
                <div class="bg-white rounded shadow p-4 text-center">
                    <h4 class="text-danger mb-2">Please fill the trip information.</h4>
                    <p class="text-muted mb-3">Enter source, destination, and travel date.</p>
                    <button class="btn btn-dark shadow-none" onclick="view_all_trips()">
                        View All Available Trips
                    </button>
                </div>
            `;
            return;
        }

        bus_data.innerHTML = `
            <div class="text-center py-5">
                <div class="spinner-border text-info mb-3" id="loader">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p class="text-muted">Searching trips...</p>
            </div>
        `;

        let apiUrl = `${BUS_API_BASE_URL}/search-trips?source=${encodeURIComponent(source.value.trim())}&destination=${encodeURIComponent(destination.value.trim())}&date=${encodeURIComponent(date.value)}`;

        fetch(apiUrl)
            .then(response => response.json())
            .then(data => {
                console.log("Trip Search API Response:", data);
                renderTrips(data);
            })
            .catch(error => {
                console.error("Trip Search Error:", error);

                bus_data.innerHTML = `
                    <div class="bg-white rounded shadow p-4 text-center">
                        <h4 class="text-danger mb-2">Something went wrong.</h4>
                        <p class="text-muted mb-0">Unable to load trips. Please make sure the Node API is running.</p>
                    </div>
                `;
            });
    }

    function fetch_all_trips() {
        viewAllMode = true;

        bus_data.innerHTML = `
            <div class="text-center py-5">
                <div class="spinner-border text-info mb-3" id="loader">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p class="text-muted">Loading all available trips...</p>
            </div>
        `;

        let apiUrl = `${BUS_API_BASE_URL}/search-trips?view=all`;

        fetch(apiUrl)
            .then(response => response.json())
            .then(data => {
                console.log("All Trips API Response:", data);
                renderTrips(data);
            })
            .catch(error => {
                console.error("All Trips Error:", error);

                bus_data.innerHTML = `
                    <div class="bg-white rounded shadow p-4 text-center">
                        <h4 class="text-danger mb-2">Something went wrong.</h4>
                        <p class="text-muted mb-0">Unable to load all available trips. Please make sure the Node API is running.</p>
                    </div>
                `;
            });
    }

    function view_all_trips() {
        window.history.pushState({}, '', 'bus.php?view=all');

        source.value = '';
        destination.value = '';
        date.value = '';
        passengers.value = '1';

        chk_avail_btn.classList.add('d-none');

        fetch_all_trips();
    }

    function chk_avail_filter() {
        let passengerCount = getPassengerCount();

        if (source.value.trim() === '' || destination.value.trim() === '' || date.value === '' || passengerCount <= 0) {
            bus_data.innerHTML = `
                <div class="bg-white rounded shadow p-4 text-center">
                    <h4 class="text-danger mb-2">Please fill the information!</h4>
                    <p class="text-muted mb-3">Source, destination, date, and passengers are required.</p>
                    <button class="btn btn-dark shadow-none" onclick="view_all_trips()">
                        View All Available Trips
                    </button>
                </div>
            `;
            chk_avail_btn.classList.add('d-none');
        } else {
            chk_avail_btn.classList.remove('d-none');
            fetch_bus();
        }
    }

    function chk_avail_clear() {
        source.value = '';
        destination.value = '';
        date.value = '';
        passengers.value = '1';

        chk_avail_btn.classList.add('d-none');

        bus_data.innerHTML = `
            <div class="bg-white rounded shadow p-4 text-center">
                <h4 class="text-muted mb-2">Search cleared.</h4>
                <p class="text-muted mb-3">Enter your travel details to search again.</p>
                <button class="btn btn-dark shadow-none" onclick="view_all_trips()">
                    View All Available Trips
                </button>
            </div>
        `;
    }

    window.onload = function () {
        if (viewAllMode) {
            fetch_all_trips();
        } else {
            fetch_bus();
        }
    };
</script>

</body>
</html>