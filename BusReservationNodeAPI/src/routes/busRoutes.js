const express = require('express');
const router = express.Router();

const busController = require('../controllers/busController');

router.get('/admin/buses', busController.getBuses);
router.get('/admin/buses/:id', busController.getBusById);
router.post('/admin/buses', busController.createBus);
router.put('/admin/buses/:id', busController.updateBus);
router.delete('/admin/buses/:id', busController.deleteBus);

module.exports = router;