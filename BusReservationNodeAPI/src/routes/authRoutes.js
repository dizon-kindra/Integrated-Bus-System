const express = require('express');
const router = express.Router();

const authController = require('../controllers/authController');

router.post('/register', authController.register);
router.post('/login', authController.login);

// PHP-style aliases
router.post('/register.php', authController.register);
router.post('/login.php', authController.login);

module.exports = router;