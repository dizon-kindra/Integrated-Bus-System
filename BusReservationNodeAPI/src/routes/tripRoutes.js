const express = require('express');
const router = express.Router();

const tripController = require('../controllers/tripController');

router.get('/search-trips', tripController.searchTrips);
router.get('/trips', tripController.searchTrips);
router.get('/get-seats', tripController.getSeats);

// PHP-style aliases
router.get('/search_trips.php', tripController.searchTrips);
router.get('/get_seats.php', tripController.getSeats);

module.exports = router;