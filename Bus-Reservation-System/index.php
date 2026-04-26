<!DOCTYPE html>
<html lang='en'>

<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>MYBUS - Passenger Bus Reservation</title>
    <?php require('inc/links.php') ?>

    <style>
        .availability-form {
            margin-top: -70px;
            z-index: 2;
            position: relative;
        }

        .hero-img {
            height: 520px;
            object-fit: cover;
            filter: brightness(55%);
        }

        .carousel-caption-custom {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 85%;
            color: #fff;
            text-align: center;
            z-index: 5;
        }

        .carousel-caption-custom h1 {
            font-size: 3rem;
            font-weight: 800;
        }

        .carousel-caption-custom p {
            font-size: 1.1rem;
            max-width: 750px;
            margin: 15px auto;
        }

        .search-card {
            border-radius: 18px;
        }

        .how-card,
        .facility-card,
        .contact-card {
            border-radius: 18px;
            transition: .3s;
        }

        .how-card:hover,
        .facility-card:hover {
            transform: translateY(-6px);
        }

        .how-icon {
            font-size: 2.3rem;
            color: #0d6efd;
        }

        .contact-icon {
            font-size: 1.3rem;
            color: #0d6efd;
            margin-right: 12px;
        }

        @media screen and (max-width:575px) {
            .availability-form {
                margin-top: 25px;
                padding: 0 20px;
            }

            .carousel-caption-custom h1 {
                font-size: 2rem;
            }

            .hero-img {
                height: 420px;
            }
        }
    </style>
</head>

<body class='bg-light'>
 <?php
    require('inc/header.php');
    $today = date('Y-m-d');
    ?>

    <!-- HERO / CAROUSEL -->
    <div class='container-fluid px-lg-4 mt-4 position-relative'>
        <div class='swiper swiper-container'>
            <div class='swiper-wrapper'>
                <div class='swiper-slide position-relative'>
                    <img src='images/carousel/bus.jpg' class='w-100 d-block hero-img' />
                    <div class="carousel-caption-custom">
                        <h1>Book Your Bus Trip Easily</h1>
                        <p>
                            Search available schedules, choose your route, reserve seats, and manage your bookings
                            through MYBUS Passenger Reservation System.
                        </p>
                        <a href="#searchTrip" class="btn btn-primary px-4 py-2 mt-2">Search Trips</a>
                    </div>
                </div>
                <div class='swiper-slide position-relative'>
                    <img src='images/carousel/bus.jpg' class='w-100 d-block hero-img' />
                    <div class="carousel-caption-custom">
                        <h1>Fast, Simple, and Convenient</h1>
                        <p>
                            Find your preferred bus schedule and reserve your seat in just a few steps.
                        </p>
                        <a href="#searchTrip" class="btn btn-primary px-4 py-2 mt-2">Start Booking</a>
                    </div>
                </div>
                <div class='swiper-slide position-relative'>
                    <img src='images/carousel/bus.jpg' class='w-100 d-block hero-img' />
                    <div class="carousel-caption-custom">
                        <h1>Manage Your Reservation</h1>
                        <p>
                            View booking status, check trip details, and print or download your ticket anytime.
                        </p>
                        <a href="bookings.php" class="btn btn-primary px-4 py-2 mt-2">My Bookings</a>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- SEARCH / CHECK AVAILABILITY FORM -->
    <div class='container availability-form' id="searchTrip">
        <div class='row'>
            <div class='col-lg-12 bg-white p-4 rounded shadow search-card'>
                <h4 class='mb-2 h-font fw-bold'>Search Available Trips</h4>
                <p class="text-muted mb-4">Enter your travel details to find available bus schedules.</p>

                <form action='bus.php' method="GET">
                    <div class='row align-items-end'>
                        <div class='col-lg-3 mb-3'>
                            <label class='form-label fw-bold'>From</label>
                            <input type='text' class='form-control shadow-none' name="source" required placeholder="Enter origin">
                        </div>

                        <div class='col-lg-3 mb-3'>
                            <label class='form-label fw-bold'>To</label>
                            <input type='text' class='form-control shadow-none' name="destination" required placeholder="Enter destination">
                        </div>

                        <div class='col-lg-3 mb-3'>
                            <label class='form-label fw-bold'>Departure Date</label>
                            <input type='date' min="<?php echo $today; ?>" class='form-control shadow-none' name="date" required value="<?php echo $today; ?>">
                        </div>

                        <div class='col-lg-2 mb-3'>
                            <label class='form-label fw-bold'>Passengers</label>
                            <input type='number' min="1" max="9" class='form-control shadow-none' name="passengers" required value="1">
                        </div>

                        <input type="hidden" name="check_availability">

                        <div class='col-lg-1 mb-lg-3 mt-2 d-grid'>
                            <button type='submit' class='btn text-white shadow-none custom-bg'>Search</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <!-- HOW IT WORKS -->
    <h2 class='mt-5 pt-4 mb-4 text-center fw-bold h-font'>HOW IT WORKS</h2>

    <div class='container'>
        <div class='row g-4'>
            <div class='col-lg-3 col-md-6'>
                <div class='bg-white text-center shadow p-4 how-card h-100'>
                    <i class='bi bi-search how-icon'></i>
                    <h5 class='mt-3 fw-bold'>Search Schedule</h5>
                    <p class='text-muted'>Choose your origin, destination, date, and number of passengers.</p>
                </div>
            </div>

            <div class='col-lg-3 col-md-6'>
                <div class='bg-white text-center shadow p-4 how-card h-100'>
                    <i class='bi bi-bus-front how-icon'></i>
                    <h5 class='mt-3 fw-bold'>Choose Bus</h5>
                    <p class='text-muted'>View available buses, departure time, fare, and available seats.</p>
                </div>
            </div>

            <div class='col-lg-3 col-md-6'>
                <div class='bg-white text-center shadow p-4 how-card h-100'>
                    <i class='bi bi-ticket-perforated how-icon'></i>
                    <h5 class='mt-3 fw-bold'>Reserve Seat</h5>
                    <p class='text-muted'>Select your preferred seat and confirm your reservation details.</p>
                </div>
            </div>

            <div class='col-lg-3 col-md-6'>
                <div class='bg-white text-center shadow p-4 how-card h-100'>
                    <i class='bi bi-printer how-icon'></i>
                    <h5 class='mt-3 fw-bold'>Print Ticket</h5>
                    <p class='text-muted'>View your booking status and print or download your ticket.</p>
                </div>
            </div>
        </div>
    </div>

    <!-- BUS FACILITIES -->
    <h2 id="facilities" class='mt-5 pt-4 mb-4 text-center fw-bold h-font'>OUR BUS FACILITIES</h2>

    <div class='container'>
        <div class='row justify-content-center g-4 px-lg-0 px-md-0 px-5'>
            <div class='col-lg-3 col-md-4'>
                <div class='text-center bg-white rounded shadow py-4 my-3 facility-card h-100'>
                    <img src='images/facilities/recliner.png' alt='' width='80px'>
                    <h5 class='mt-3 fw-bold'>Reclining Seats</h5>
                    <p class="text-muted px-3">Comfortable seats for a relaxing trip.</p>
                </div>
            </div>

            <div class='col-lg-3 col-md-4'>
                <div class='text-center bg-white rounded shadow py-4 my-3 facility-card h-100'>
                    <img src='images/facilities/IMG_43553.svg' alt='' width='80px'>
                    <h5 class='mt-3 fw-bold'>Free WiFi</h5>
                    <p class="text-muted px-3">Stay connected during your travel.</p>
                </div>
            </div>

            <div class='col-lg-3 col-md-4'>
                <div class='text-center bg-white rounded shadow py-4 my-3 facility-card h-100'>
                    <img src='images/facilities/IMG_49949.svg' alt='' width='80px'>
                    <h5 class='mt-3 fw-bold'>AC Comfort</h5>
                    <p class="text-muted px-3">Enjoy cool and comfortable rides.</p>
                </div>
            </div>
        </div>
    </div>

    <!-- TESTIMONIALS -->
    <h2 class='mt-5 pt-4 mb-4 text-center fw-bold h-font'>CUSTOMER TESTIMONIALS</h2>

    <div class='container mt-5'>
        <div class='swiper swiper-testimonials'>
            <div class='swiper-wrapper mb-5'>
                <div class='swiper-slide bg-white mb-3 px-4 rounded shadow-sm'>
                    <div class='profile d-flex align-items-center p-4'>
                        <i class="bi bi-person-circle fs-4"></i>
                        <h6 class='m-0 ms-2'>Jane Doe</h6>
                    </div>
                    <p>
                        MYBUS made my trip booking easy and fast. The reservation process was simple and convenient.
                    </p>
                    <div class='rating mb-3'>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                    </div>
                </div>

                <div class='swiper-slide bg-white mb-3 px-4 rounded shadow-sm'>
                    <div class='profile d-flex align-items-center p-4'>
                        <i class="bi bi-person-circle fs-4"></i>
                        <h6 class='m-0 ms-2'>John Smith</h6>
                    </div>
                    <p>
                        I found my schedule quickly, booked my seat, and checked my booking status easily.
                    </p>
                    <div class='rating mb-3'>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                    </div>
                </div>

                <div class='swiper-slide bg-white mb-3 px-4 rounded shadow-sm'>
                    <div class='profile d-flex align-items-center p-4'>
                        <i class="bi bi-person-circle fs-4"></i>
                        <h6 class='m-0 ms-2'>Michael Johnson</h6>
                    </div>
                    <p>
                        The bus was punctual and the booking confirmation was clear and easy to understand.
                    </p>
                    <div class='rating mb-3'>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                        <i class='bi bi-star-fill text-warning'></i>
                    </div>
                </div>
            </div>
            <div class='swiper-pagination'></div>
        </div>
    </div>
    <!-- ABOUT US -->
<h2 class='mt-5 pt-4 mb-4 text-center fw-bold h-font' id="about">ABOUT MYBUS</h2>

<div class='container mb-5'>
    <div class='bg-white shadow p-4 rounded'>
        <h4 class="fw-bold mb-3">Passenger Bus Reservation System</h4>
        <p class='text-muted mb-0'>
            MYBUS is a passenger bus reservation system designed to help users search available trips,
            reserve seats, manage bookings, and print or download tickets in a simple and convenient way.
            The system provides passengers with an easy booking process and quick access to their reservation status.
        </p>
    </div>
</div>

    <!-- CONTACT US -->
    <h2 class='mt-5 pt-4 mb-4 text-center fw-bold h-font' id="contactus">CONTACT US</h2>

    <div class='container mb-5'>
        <div class='row g-4'>
            <div class='col-lg-5'>
                <div class='bg-white shadow p-4 contact-card h-100'>
                    <h4 class='fw-bold mb-4'>Passenger Support</h4>

                    <div class='d-flex mb-3'>
                        <i class='bi bi-geo-alt-fill contact-icon'></i>
                        <div>
                            <h6 class='fw-bold mb-1'>Terminal Address</h6>
                            <p class='text-muted mb-0'>San Juan Bus Terminal, Southern Leyte, Philippines</p>
                        </div>
                    </div>

                    <div class='d-flex mb-3'>
                        <i class='bi bi-telephone-fill contact-icon'></i>
                        <div>
                            <h6 class='fw-bold mb-1'>Phone Number</h6>
                            <p class='text-muted mb-0'>+63 912 345 6789</p>
                        </div>
                    </div>

                    <div class='d-flex mb-3'>
                        <i class='bi bi-envelope-fill contact-icon'></i>
                        <div>
                            <h6 class='fw-bold mb-1'>Email Address</h6>
                            <p class='text-muted mb-0'>support@mybus.com</p>
                        </div>
                    </div>

                    <div class='d-flex'>
                        <i class='bi bi-clock-fill contact-icon'></i>
                        <div>
                            <h6 class='fw-bold mb-1'>Support Hours</h6>
                            <p class='text-muted mb-0'>Monday - Sunday | 8:00 AM - 8:00 PM</p>
                        </div>
                    </div>
                </div>
            </div>

            <div class='col-lg-7'>
                <div class='bg-white shadow p-4 contact-card'>
                    <h4 class='fw-bold'>Send Us a Message</h4>
                    <p class='text-muted'>
                        For booking concerns, schedule inquiries, ticket issues, or reservation assistance, send us a message.
                    </p>

                    <form action=''>
                        <div class='row'>
                            <div class='col-md-6 mb-3'>
                                <label class='form-label fw-bold'>Name</label>
                                <input type='text' class='form-control shadow-none' placeholder="Your Name">
                            </div>

                            <div class='col-md-6 mb-3'>
                                <label class='form-label fw-bold'>Email</label>
                                <input type='email' class='form-control shadow-none' placeholder="your-email@example.com">
                            </div>
                        </div>

                        <div class='mb-3'>
                            <label class='form-label fw-bold'>Subject</label>
                            <input type='text' class='form-control shadow-none' placeholder="Booking concern / Inquiry">
                        </div>

                        <div class='mb-3'>
                            <label class='form-label fw-bold'>Message</label>
                            <textarea class='form-control shadow-none' rows='4' placeholder="Write your message here..."></textarea>
                        </div>

                        <div class='text-end'>
                            <button type='submit' class='btn custom-bg text-white px-4'>Send Message</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <?php require('inc/footer.php') ?>

    <script src='https://cdn.jsdelivr.net/npm/swiper@11/swiper-bundle.min.js'></script>
    <script>
        var swiper = new Swiper(".swiper-container", {
            spaceBetween: 30,
            effect: "fade",
            loop: true,
            autoplay: {
                delay: 3000,
                disableOnInteraction: false,
            }
        });

        var swiper = new Swiper(".swiper-testimonials", {
            effect: "coverflow",
            grabCursor: true,
            centeredSlides: true,
            slidesPerView: "3",
            coverflowEffect: {
                rotate: 40,
                stretch: 0,
                depth: 100,
                modifier: 1,
                slideShadows: false,
            },
            pagination: {
                el: ".swiper-pagination",
            },
            loop: true,
            autoplay: {
                delay: 2500,
                disableOnInteraction: false,
            },
            breakpoints: {
                320: {
                    slidesPerView: 1,
                },
                640: {
                    slidesPerView: 2,
                },
                768: {
                    slidesPerView: 2,
                },
                1024: {
                    slidesPerView: 3,
                },
            },
        });
    </script>

</body>

</html>