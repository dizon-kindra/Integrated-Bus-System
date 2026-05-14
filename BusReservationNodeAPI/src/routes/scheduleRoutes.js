const express = require('express');
const router = express.Router();

const scheduleController = require('../controllers/scheduleController');

router.get('/admin/schedules', scheduleController.getSchedules);
router.get('/admin/schedules/:id', scheduleController.getScheduleById);
router.post('/admin/schedules', scheduleController.createSchedule);
router.put('/admin/schedules/:id', scheduleController.updateSchedule);
router.put('/admin/schedules/:id/status', scheduleController.updateTripStatus);
router.delete('/admin/schedules/:id', scheduleController.deleteSchedule);

module.exports = router;