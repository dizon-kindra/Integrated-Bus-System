<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>MYBUS - Buses</title>
    <?php require('inc/links.php') ?>
</head>

<body class="bg-light">

    <?php
    require('inc/header.php');

    $source_default = '';
    $destination_default = '';
    $date_default = '';
    $passengers_default = '1';
    $view_all = false;

    if (isset($_GET['view']) && $_GET['view'] == 'all') {
        $view_all = true;

        $_SESSION['user'] = [
            "passengers" => $passengers_default,
            "date" => ''
        ];
    }

    if (isset($_GET['check_availability'])) {
        $frm_data = filteration($_GET);

        $source_default = isset($frm_data['source']) ? $frm_data['source'] : '';
        $destination_default = isset($frm_data['destination']) ? $frm_data['destination'] : '';
        $date_default = isset($frm_data['date']) ? $frm_data['date'] : '';
        $passengers_default = isset($frm_data['passengers']) ? $frm_data['passengers'] : '1';

        $_SESSION['user'] = [
            "passengers" => $passengers_default,
            "date" => $date_default
        ];
    }
    ?>

    <div class="my-5 px-4">
        <h2 class="fw-bold h-font text-center">OUR BUSES</h2>
        <div class="h-line bg-dark"></div>

        <?php if ($view_all) { ?>
            <p class="text-center text-muted mt-3">
                Browse all available trips added by the terminal admin.
            </p>
        <?php } else { ?>
            <p class="text-center text-muted mt-3">
                Search and choose from available trips based on your selected route and date.
            </p>
        <?php } ?>
    </div>

    <div class="container-fluid">
        <div class="row">

            <!-- FILTER SIDEBAR -->
            <div class="col-lg-3 col-md-12 mb-4 mb-lg-0 ps-4">
                <nav class="navbar navbar-expand-lg navbar-light bg-white rounded shadow">
                    <div class="container-fluid flex-lg-column align-items-stretch">

                        <h4 class="mt-2 h-font">FILTERS</h4>

                        <button class="navbar-toggler shadow-none" type="button" data-bs-toggle="collapse"
                            data-bs-target="#filterDropdown" aria-controls="filterDropdown" aria-expanded="false"
                            aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon"></span>
                        </button>

                        <div class="collapse navbar-collapse flex-column mt-2 align-items-stretch mx-2"
                            id="filterDropdown">

                            <div class="border bg-light p-3 rounded mb-3">
                                <h5 class="mb-3 h-font d-flex justify-content-between align-items-center"
                                    style="font-size: 18px;">
                                    <span>CHECK AVAILABILITY</span>
                                    <button id="chk_avail_btn" onclick="chk_avail_clear()"
                                        class="btn shadow-none btn-sm text-secondary d-none" type="button">Reset</button>
                                </h5>

                                <label class="form-label fw-bold">Source</label>
                                <input type="text" class="form-control shadow-none mb-3" id="source"
                                    onchange="chk_avail_filter()" value="<?php echo htmlspecialchars($source_default); ?>"
                                    placeholder="Enter origin">

                                <label class="form-label fw-bold">Destination</label>
                                <input type="text" class="form-control shadow-none mb-3" id="destination"
                                    onchange="chk_avail_filter()" value="<?php echo htmlspecialchars($destination_default); ?>"
                                    placeholder="Enter destination">
                            </div>

                            <div class="border bg-light p-3 rounded mb-3">
                                <h5 class="mb-3 h-font" style="font-size: 18px;">Date & Passengers</h5>

                                <label class="form-label fw-bold">Date</label>
                                <input type="date" class="form-control shadow-none mb-3" id="date"
                                    onchange="chk_avail_filter()" value="<?php echo htmlspecialchars($date_default); ?>">

                                <label class="form-label fw-bold">No. of Passengers</label>
                                <input type="number" onchange="chk_avail_filter()" id="passengers"
                                    class="form-control shadow-none mb-3" min="1" max="9"
                                    value="<?php echo htmlspecialchars($passengers_default); ?>">

                                <button type="button" class="btn text-white custom-bg shadow-none w-100 mb-2"
                                    onclick="chk_avail_filter()">
                                    Search Trips
                                </button>

                                <button type="button" class="btn btn-dark shadow-none w-100" onclick="view_all_trips()">
                                    View All 
                                </button>
                            </div>

                        </div>
                    </div>
                </nav>
            </div>

            <!-- BUS/TRIP RESULTS -->
            <div class="col-lg-9 col-md-12 px-4" id="bus-data">
                <!-- API results will load here -->
            </div>

        </div>
    </div>

    <script>
        let bus_data = document.getElementById('bus-data');
        let source = document.getElementById('source');
        let destination = document.getElementById('destination');
        let chk_avail_btn = document.getElementById('chk_avail_btn');
        let date = document.getElementById('date');
        let passengers = document.getElementById('passengers');

        let isLoggedIn = <?php echo (isset($_SESSION['login']) && $_SESSION['login'] == true) ? 'true' : 'false'; ?>;
        let viewAllMode = <?php echo $view_all ? 'true' : 'false'; ?>;

        function formatTime(timeValue) {
            if (!timeValue) return 'N/A';

            let parts = timeValue.split(':');
            let hour = parseInt(parts[0]);
            let minute = parts[1];

            let ampm = hour >= 12 ? 'PM' : 'AM';
            hour = hour % 12;
            hour = hour ? hour : 12;

            return hour + ':' + minute + ' ' + ampm;
        }

        function renderTrips(data) {
            if (!data.success) {
                bus_data.innerHTML = `
                    <div class="bg-white rounded shadow p-4 text-center">
                        <h4 class="text-danger mb-2">${data.message}</h4>
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

                if (isLoggedIn) {
                    bookButton = `
                        <a href="confirm_booking.php?schedule_id=${trip.schedule_id}&passengers=${passengers.value}" 
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
                    <div class="card mb-4 border-0 shadow">
                        <div class="row g-0 p-3 align-items-center">

                            <div class="col-md-4 mb-lg-0 mb-md-0 mb-3">
                                <h5 class="mb-2 fw-bold">${trip.bus_number}</h5>
                                <p class="mb-1">
                                    <span class="badge bg-dark">${trip.bus_type}</span>
                                </p>
                                <p class="mb-1 text-muted">
                                    <i class="bi bi-credit-card-2-front me-1"></i>
                                    Plate No: ${trip.plate_number}
                                </p>
                                <p class="mb-0 text-muted">
                                    <i class="bi bi-people-fill me-1"></i>
                                    Capacity: ${trip.capacity}
                                </p>
                            </div>

                            <div class="col-md-5 px-lg-3 px-md-3 px-0 mb-lg-0 mb-md-0 mb-3">
                                <h6 class="mb-2 fw-bold">Trip Details</h6>

                                <p class="mb-1">
                                    <i class="bi bi-geo-alt-fill me-1"></i>
                                    ${trip.origin} → ${trip.destination}
                                </p>

                                <p class="mb-1">
                                    <i class="bi bi-calendar-event me-1"></i>
                                    ${trip.departure_date}
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
                                    Duration: ${trip.estimated_duration}
                                </p>
                            </div>

                            <div class="col-md-3 text-center">
                                <h5 class="mb-2 text-success fw-bold">₱${trip.fare}</h5>

                                <p class="mb-2">
                                    <span class="badge bg-primary">
                                        ${trip.available_seats} seats available
                                    </span>
                                </p>

                                <p class="mb-3">
                                    <span class="badge bg-success">${trip.trip_status}</span>
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

            if (source.value === '' || destination.value === '' || date.value === '') {
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
                <div class="spinner-border d-block text-info mb-3 mx-auto" id="loader">
                    <span class="visually-hidden">Loading...</span>
                </div>
            `;

            let apiUrl = `api/search_trips.php?source=${encodeURIComponent(source.value)}&destination=${encodeURIComponent(destination.value)}&date=${encodeURIComponent(date.value)}`;

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
                            <p class="text-muted mb-0">Unable to load trips. Please try again.</p>
                        </div>
                    `;
                });
        }

        function fetch_all_trips() {
            viewAllMode = true;

            bus_data.innerHTML = `
                <div class="spinner-border d-block text-info mb-3 mx-auto" id="loader">
                    <span class="visually-hidden">Loading...</span>
                </div>
            `;

            let apiUrl = `api/search_trips.php?view=all`;

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
                            <p class="text-muted mb-0">Unable to load all available trips. Please try again.</p>
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
            if (source.value === '' || destination.value === '' || date.value === '' || passengers.value === '') {
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

    <?php require('inc/footer.php'); ?>

</body>

</html>