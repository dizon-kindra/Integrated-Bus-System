const db = require('../db');

async function getBuses(req, res, next) {
  try {
    const [buses] = await db.query(`
      SELECT 
        bus_id,
        bus_number,
        plate_number,
        capacity,
        bus_type,
        status
      FROM buses
      ORDER BY bus_id DESC
    `);

    res.json({
      success: true,
      count: buses.length,
      buses,
    });
  } catch (err) {
    next(err);
  }
}

async function getBusById(req, res, next) {
  try {
    const busId = Number(req.params.id);

    if (!busId) {
      return res.status(400).json({
        success: false,
        message: 'Bus ID is required.',
      });
    }

    const [buses] = await db.query(
      `SELECT 
        bus_id,
        bus_number,
        plate_number,
        capacity,
        bus_type,
        status
       FROM buses
       WHERE bus_id = ?
       LIMIT 1`,
      [busId]
    );

    if (buses.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Bus not found.',
      });
    }

    res.json({
      success: true,
      bus: buses[0],
    });
  } catch (err) {
    next(err);
  }
}

async function createBus(req, res, next) {
  try {
    const {
      bus_number = '',
      plate_number = '',
      capacity = '',
      bus_type = '',
      status = 'Active',
    } = req.body;

    if (!bus_number || !plate_number || !capacity || !bus_type) {
      return res.status(400).json({
        success: false,
        message: 'Bus number, plate number, capacity, and bus type are required.',
      });
    }

    if (Number(capacity) <= 0) {
      return res.status(400).json({
        success: false,
        message: 'Capacity must be greater than 0.',
      });
    }

    const [existing] = await db.query(
      `SELECT bus_id 
       FROM buses 
       WHERE bus_number = ? OR plate_number = ?
       LIMIT 1`,
      [bus_number, plate_number]
    );

    if (existing.length > 0) {
      return res.status(409).json({
        success: false,
        message: 'Bus number or plate number already exists.',
      });
    }

    const [result] = await db.query(
      `INSERT INTO buses 
       (bus_number, plate_number, capacity, bus_type, status)
       VALUES (?, ?, ?, ?, ?)`,
      [bus_number, plate_number, Number(capacity), bus_type, status]
    );

    res.status(201).json({
      success: true,
      message: 'Bus added successfully.',
      bus: {
        bus_id: result.insertId,
        bus_number,
        plate_number,
        capacity: Number(capacity),
        bus_type,
        status,
      },
    });
  } catch (err) {
    next(err);
  }
}

async function updateBus(req, res, next) {
  try {
    const busId = Number(req.params.id);

    if (!busId) {
      return res.status(400).json({
        success: false,
        message: 'Bus ID is required.',
      });
    }

    const {
      bus_number = '',
      plate_number = '',
      capacity = '',
      bus_type = '',
      status = '',
    } = req.body;

    if (!bus_number || !plate_number || !capacity || !bus_type || !status) {
      return res.status(400).json({
        success: false,
        message: 'All bus fields are required.',
      });
    }

    if (Number(capacity) <= 0) {
      return res.status(400).json({
        success: false,
        message: 'Capacity must be greater than 0.',
      });
    }

    const [current] = await db.query(
      'SELECT bus_id FROM buses WHERE bus_id = ? LIMIT 1',
      [busId]
    );

    if (current.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Bus not found.',
      });
    }

    const [duplicate] = await db.query(
      `SELECT bus_id 
       FROM buses 
       WHERE (bus_number = ? OR plate_number = ?)
       AND bus_id != ?
       LIMIT 1`,
      [bus_number, plate_number, busId]
    );

    if (duplicate.length > 0) {
      return res.status(409).json({
        success: false,
        message: 'Bus number or plate number already exists.',
      });
    }

    await db.query(
      `UPDATE buses
       SET bus_number = ?,
           plate_number = ?,
           capacity = ?,
           bus_type = ?,
           status = ?
       WHERE bus_id = ?`,
      [bus_number, plate_number, Number(capacity), bus_type, status, busId]
    );

    res.json({
      success: true,
      message: 'Bus updated successfully.',
    });
  } catch (err) {
    next(err);
  }
}

async function deleteBus(req, res, next) {
  try {
    const busId = Number(req.params.id);

    if (!busId) {
      return res.status(400).json({
        success: false,
        message: 'Bus ID is required.',
      });
    }

    const [used] = await db.query(
      'SELECT schedule_id FROM schedules WHERE bus_id = ? LIMIT 1',
      [busId]
    );

    if (used.length > 0) {
      return res.status(400).json({
        success: false,
        message: 'Cannot delete this bus because it is already used in a schedule. You can set it to Inactive instead.',
      });
    }

    const [result] = await db.query(
      'DELETE FROM buses WHERE bus_id = ?',
      [busId]
    );

    if (result.affectedRows === 0) {
      return res.status(404).json({
        success: false,
        message: 'Bus not found.',
      });
    }

    res.json({
      success: true,
      message: 'Bus deleted successfully.',
    });
  } catch (err) {
    next(err);
  }
}

module.exports = {
  getBuses,
  getBusById,
  createBus,
  updateBus,
  deleteBus,
};