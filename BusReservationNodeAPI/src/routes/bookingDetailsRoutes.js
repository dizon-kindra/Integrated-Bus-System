const express = require('express');
const router = express.Router();

const bookingDetailsController = require('../controllers/bookingDetailsController');

router.get('/booking-details', bookingDetailsController.getBookingDetails);

module.exports = router;