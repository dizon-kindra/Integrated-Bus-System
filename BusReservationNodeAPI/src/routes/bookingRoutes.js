const express = require('express');
const router = express.Router();

const bookingController = require('../controllers/bookingController');

router.get('/bookings', bookingController.getAllBookings);
router.get('/all-bookings', bookingController.getAllBookings);
router.post('/create-booking', bookingController.createBooking);
router.get('/my-bookings', bookingController.myBookings);
router.post('/cancel-booking', bookingController.cancelBooking);

// Admin confirm payment / confirm booking
router.put('/admin/bookings/:id/confirm-payment', bookingController.confirmPayment);
router.post('/admin/bookings/:id/confirm-payment', bookingController.confirmPayment);

// Admin check-in passenger
router.put('/admin/bookings/:id/check-in', bookingController.checkInBooking);
router.post('/admin/bookings/:id/check-in', bookingController.checkInBooking);

// Admin boarding passenger
router.put('/admin/bookings/:id/board', bookingController.boardBooking);
router.post('/admin/bookings/:id/board', bookingController.boardBooking);

module.exports = router;