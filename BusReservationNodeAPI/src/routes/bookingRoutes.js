const express = require('express');
const router = express.Router();

const bookingController = require('../controllers/bookingController');

router.get('/bookings', bookingController.getAllBookings);
router.get('/all-bookings', bookingController.getAllBookings);
router.post('/create-booking', bookingController.createBooking);
router.get('/my-bookings', bookingController.myBookings);
router.post('/cancel-booking', bookingController.cancelBooking);

// PHP-style aliases
router.get('/bookings.php', bookingController.getAllBookings);
router.get('/all_bookings.php', bookingController.getAllBookings);
router.post('/create_booking.php', bookingController.createBooking);
router.get('/my_bookings.php', bookingController.myBookings);
router.post('/cancel_booking.php', bookingController.cancelBooking);

module.exports = router;