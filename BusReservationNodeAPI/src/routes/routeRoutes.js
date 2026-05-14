const express = require('express');
const router = express.Router();

const routeController = require('../controllers/routeController');

router.get('/admin/routes', routeController.getRoutes);
router.get('/admin/routes/:id', routeController.getRouteById);
router.post('/admin/routes', routeController.createRoute);
router.put('/admin/routes/:id', routeController.updateRoute);
router.delete('/admin/routes/:id', routeController.deleteRoute);

module.exports = router;