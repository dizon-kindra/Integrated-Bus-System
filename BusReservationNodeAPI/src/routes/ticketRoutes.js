const express = require('express');
const router = express.Router();

const ticketController = require('../controllers/ticketController');

router.get('/ticket-details', ticketController.getTicketDetails);

module.exports = router;