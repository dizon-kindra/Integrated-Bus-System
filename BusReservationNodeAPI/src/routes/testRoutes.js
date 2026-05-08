const express = require('express');
const router = express.Router();

const testController = require('../controllers/testController');

router.get('/test', testController.test);
router.get('/health', testController.test);
router.get('/test.php', testController.test);

module.exports = router;