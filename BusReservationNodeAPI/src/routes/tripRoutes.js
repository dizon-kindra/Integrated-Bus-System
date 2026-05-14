const express = require('express');
const router = express.Router();

const tripController = require('../controllers/tripController');

router.get('/search-trips', tripController.searchTrips);
router.get('/trips', tripController.searchTrips);
router.get('/get-seats', tripController.getSeats);


module.exports = router;