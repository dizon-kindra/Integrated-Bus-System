const db = require('../db');

async function getRoutes(req, res, next) {
  try {
    const [routes] = await db.query(`
      SELECT
        route_id,
        route_code,
        origin,
        destination,
        fare,
        estimated_duration,
        status,
        created_at
      FROM routes
      ORDER BY route_id DESC
    `);

    res.json({
      success: true,
      count: routes.length,
      routes,
    });
  } catch (err) {
    next(err);
  }
}

async function getRouteById(req, res, next) {
  try {
    const routeId = Number(req.params.id);

    if (!routeId) {
      return res.status(400).json({
        success: false,
        message: 'Route ID is required.',
      });
    }

    const [routes] = await db.query(
      `SELECT
        route_id,
        route_code,
        origin,
        destination,
        fare,
        estimated_duration,
        status,
        created_at
       FROM routes
       WHERE route_id = ?
       LIMIT 1`,
      [routeId]
    );

    if (routes.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Route not found.',
      });
    }

    res.json({
      success: true,
      route: routes[0],
    });
  } catch (err) {
    next(err);
  }
}

async function createRoute(req, res, next) {
  try {
    const {
      route_code = '',
      origin = '',
      destination = '',
      fare = '',
      estimated_duration = '',
      status = 'Active',
    } = req.body;

    if (!route_code || !origin || !destination || !fare || !estimated_duration) {
      return res.status(400).json({
        success: false,
        message: 'Route code, origin, destination, fare, and estimated duration are required.',
      });
    }

    if (Number(fare) <= 0) {
      return res.status(400).json({
        success: false,
        message: 'Fare must be greater than 0.',
      });
    }

    const [existing] = await db.query(
      `SELECT route_id
       FROM routes
       WHERE route_code = ?
       LIMIT 1`,
      [route_code]
    );

    if (existing.length > 0) {
      return res.status(409).json({
        success: false,
        message: 'Route code already exists.',
      });
    }

    const [result] = await db.query(
      `INSERT INTO routes
       (route_code, origin, destination, fare, estimated_duration, status)
       VALUES (?, ?, ?, ?, ?, ?)`,
      [route_code, origin, destination, Number(fare), estimated_duration, status]
    );

    res.status(201).json({
      success: true,
      message: 'Route added successfully.',
      route: {
        route_id: result.insertId,
        route_code,
        origin,
        destination,
        fare: Number(fare),
        estimated_duration,
        status,
      },
    });
  } catch (err) {
    next(err);
  }
}

async function updateRoute(req, res, next) {
  try {
    const routeId = Number(req.params.id);

    if (!routeId) {
      return res.status(400).json({
        success: false,
        message: 'Route ID is required.',
      });
    }

    const {
      route_code = '',
      origin = '',
      destination = '',
      fare = '',
      estimated_duration = '',
      status = '',
    } = req.body;

    if (!route_code || !origin || !destination || !fare || !estimated_duration || !status) {
      return res.status(400).json({
        success: false,
        message: 'All route fields are required.',
      });
    }

    if (Number(fare) <= 0) {
      return res.status(400).json({
        success: false,
        message: 'Fare must be greater than 0.',
      });
    }

    const [current] = await db.query(
      'SELECT route_id FROM routes WHERE route_id = ? LIMIT 1',
      [routeId]
    );

    if (current.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Route not found.',
      });
    }

    const [duplicate] = await db.query(
      `SELECT route_id
       FROM routes
       WHERE route_code = ?
       AND route_id != ?
       LIMIT 1`,
      [route_code, routeId]
    );

    if (duplicate.length > 0) {
      return res.status(409).json({
        success: false,
        message: 'Route code already exists.',
      });
    }

    await db.query(
      `UPDATE routes
       SET route_code = ?,
           origin = ?,
           destination = ?,
           fare = ?,
           estimated_duration = ?,
           status = ?
       WHERE route_id = ?`,
      [route_code, origin, destination, Number(fare), estimated_duration, status, routeId]
    );

    res.json({
      success: true,
      message: 'Route updated successfully.',
    });
  } catch (err) {
    next(err);
  }
}

async function deleteRoute(req, res, next) {
  try {
    const routeId = Number(req.params.id);

    if (!routeId) {
      return res.status(400).json({
        success: false,
        message: 'Route ID is required.',
      });
    }

    const [used] = await db.query(
      'SELECT schedule_id FROM schedules WHERE route_id = ? LIMIT 1',
      [routeId]
    );

    if (used.length > 0) {
      return res.status(400).json({
        success: false,
        message: 'Cannot delete this route because it is already used in a schedule. You can set it to Inactive instead.',
      });
    }

    const [result] = await db.query(
      'DELETE FROM routes WHERE route_id = ?',
      [routeId]
    );

    if (result.affectedRows === 0) {
      return res.status(404).json({
        success: false,
        message: 'Route not found.',
      });
    }

    res.json({
      success: true,
      message: 'Route deleted successfully.',
    });
  } catch (err) {
    next(err);
  }
}

module.exports = {
  getRoutes,
  getRouteById,
  createRoute,
  updateRoute,
  deleteRoute,
};