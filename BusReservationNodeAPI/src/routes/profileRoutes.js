const express = require('express');
const router = express.Router();

const profileController = require('../controllers/profileController');

router.put('/update-profile', profileController.updateProfile);
router.post('/update-profile', profileController.updateProfile);

router.put('/change-password', profileController.changePassword);
router.post('/change-password', profileController.changePassword);

module.exports = router;