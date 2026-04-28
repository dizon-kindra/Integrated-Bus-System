const express = require('express');
const controller = require('../controllers/apiController');

const router = express.Router();

// Clean Node API routes
router.get('/users', controller.getUsers);
router.get('/bookings', controller.getAllBookings);
router.get('/test', controller.test);
router.get('/health', controller.test);
router.post('/register', controller.register);
router.post('/login', controller.login);
router.get('/search-trips', controller.searchTrips);
router.get('/trips', controller.searchTrips);
router.get('/get-seats', controller.getSeats);
router.post('/create-booking', controller.createBooking);
router.get('/my-bookings', controller.myBookings);
router.post('/cancel-booking', controller.cancelBooking);
router.put('/update-profile', controller.updateProfile);
router.put('/change-password', controller.changePassword);

// PHP-style aliases so your existing web/C# URLs are easier to update gradually
router.get('/test.php', controller.test);
router.post('/register.php', controller.register);
router.post('/login.php', controller.login);
router.get('/search_trips.php', controller.searchTrips);
router.get('/get_seats.php', controller.getSeats);
router.post('/create_booking.php', controller.createBooking);
router.get('/my_bookings.php', controller.myBookings);
router.post('/cancel_booking.php', controller.cancelBooking);
router.put('/update_profile.php', controller.updateProfile);
router.put('/change_password.php', controller.changePassword);

module.exports = router;
