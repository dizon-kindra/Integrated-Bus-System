const db = require('../db');

async function getSchedules(req, res, next) {
  try {
    const [schedules] = await db.query(`
      SELECT
        s.schedule_id,
        s.bus_id,
        b.bus_number,
        b.plate_number,
        b.bus_type,
        b.capacity,
        s.route_id,
        r.route_code,
        r.origin,
        r.destination,
        r.estimated_duration,
        s.departure_date,
        s.departure_time,
        s.arrival_time,
        s.fare,
        s.available_seats,
        s.trip_status
      FROM schedules s
      INNER JOIN buses b ON s.bus_id = b.bus_id
      INNER JOIN routes r ON s.route_id = r.route_id
      ORDER BY s.departure_date DESC, s.departure_time DESC
    `);

    res.json({
      success: true,
      count: schedules.length,
      schedules,
    });
  } catch (err) {
    next(err);
  }
}

async function getScheduleById(req, res, next) {
  try {
    const scheduleId = Number(req.params.id);

    const [schedules] = await db.query(
      `
      SELECT
        s.schedule_id,
        s.bus_id,
        b.bus_number,
        b.plate_number,
        b.bus_type,
        b.capacity,
        s.route_id,
        r.route_code,
        r.origin,
        r.destination,
        r.estimated_duration,
        s.departure_date,
        s.departure_time,
        s.arrival_time,
        s.fare,
        s.available_seats,
        s.trip_status
      FROM schedules s
      INNER JOIN buses b ON s.bus_id = b.bus_id
      INNER JOIN routes r ON s.route_id = r.route_id
      WHERE s.schedule_id = ?
      LIMIT 1
      `,
      [scheduleId]
    );

    if (schedules.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Schedule not found.',
      });
    }

    res.json({
      success: true,
      schedule: schedules[0],
    });
  } catch (err) {
    next(err);
  }
}

async function createSchedule(req, res, next) {
  try {
    const {
      bus_id,
      route_id,
      departure_date,
      departure_time,
      arrival_time,
      fare,
      trip_status = 'Scheduled',
    } = req.body;

    if (!bus_id || !route_id || !departure_date || !departure_time || !arrival_time || !fare) {
      return res.status(400).json({
        success: false,
        message: 'Bus, route, departure date, departure time, arrival time, and fare are required.',
      });
    }

    const [buses] = await db.query(
      'SELECT bus_id, capacity, status FROM buses WHERE bus_id = ? LIMIT 1',
      [bus_id]
    );

    if (buses.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Bus not found.',
      });
    }

    if (buses[0].status !== 'Active') {
      return res.status(400).json({
        success: false,
        message: 'Selected bus is not active.',
      });
    }

    const [routes] = await db.query(
      'SELECT route_id, status FROM routes WHERE route_id = ? LIMIT 1',
      [route_id]
    );

    if (routes.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Route not found.',
      });
    }

    if (routes[0].status !== 'Active') {
      return res.status(400).json({
        success: false,
        message: 'Selected route is not active.',
      });
    }

    const [conflict] = await db.query(
      `SELECT schedule_id
       FROM schedules
       WHERE bus_id = ?
       AND departure_date = ?
       AND departure_time = ?
       AND trip_status != 'Cancelled'
       LIMIT 1`,
      [bus_id, departure_date, departure_time]
    );

    if (conflict.length > 0) {
      return res.status(409).json({
        success: false,
        message: 'This bus already has a schedule at the same date and time.',
      });
    }

    const availableSeats = Number(buses[0].capacity);

    const [result] = await db.query(
      `INSERT INTO schedules
       (bus_id, route_id, departure_date, departure_time, arrival_time, fare, available_seats, trip_status)
       VALUES (?, ?, ?, ?, ?, ?, ?, ?)`,
      [
        Number(bus_id),
        Number(route_id),
        departure_date,
        departure_time,
        arrival_time,
        Number(fare),
        availableSeats,
        trip_status,
      ]
    );

    res.status(201).json({
      success: true,
      message: 'Schedule added successfully.',
      schedule: {
        schedule_id: result.insertId,
        bus_id: Number(bus_id),
        route_id: Number(route_id),
        departure_date,
        departure_time,
        arrival_time,
        fare: Number(fare),
        available_seats: availableSeats,
        trip_status,
      },
    });
  } catch (err) {
    next(err);
  }
}

async function updateSchedule(req, res, next) {
  try {
    const scheduleId = Number(req.params.id);

    const {
      bus_id,
      route_id,
      departure_date,
      departure_time,
      arrival_time,
      fare,
      available_seats,
      trip_status,
    } = req.body;

    if (!bus_id || !route_id || !departure_date || !departure_time || !arrival_time || !fare || available_seats === undefined || !trip_status) {
      return res.status(400).json({
        success: false,
        message: 'All schedule fields are required.',
      });
    }

    const [current] = await db.query(
      'SELECT schedule_id FROM schedules WHERE schedule_id = ? LIMIT 1',
      [scheduleId]
    );

    if (current.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Schedule not found.',
      });
    }

    await db.query(
      `UPDATE schedules
       SET bus_id = ?,
           route_id = ?,
           departure_date = ?,
           departure_time = ?,
           arrival_time = ?,
           fare = ?,
           available_seats = ?,
           trip_status = ?
       WHERE schedule_id = ?`,
      [
        Number(bus_id),
        Number(route_id),
        departure_date,
        departure_time,
        arrival_time,
        Number(fare),
        Number(available_seats),
        trip_status,
        scheduleId,
      ]
    );

    res.json({
      success: true,
      message: 'Schedule updated successfully.',
    });
  } catch (err) {
    next(err);
  }
}

async function updateTripStatus(req, res, next) {
  try {
    const scheduleId = Number(req.params.id);
    const { trip_status } = req.body;

    if (!trip_status) {
      return res.status(400).json({
        success: false,
        message: 'Trip status is required.',
      });
    }

    const allowedStatus = ['Scheduled', 'Departed', 'Arrived', 'Completed', 'Cancelled'];

    if (!allowedStatus.includes(trip_status)) {
      return res.status(400).json({
        success: false,
        message: 'Invalid trip status.',
      });
    }

    const [result] = await db.query(
      'UPDATE schedules SET trip_status = ? WHERE schedule_id = ?',
      [trip_status, scheduleId]
    );

    if (result.affectedRows === 0) {
      return res.status(404).json({
        success: false,
        message: 'Schedule not found.',
      });
    }

    res.json({
      success: true,
      message: 'Trip status updated successfully.',
    });
  } catch (err) {
    next(err);
  }
}

async function deleteSchedule(req, res, next) {
  try {
    const scheduleId = Number(req.params.id);

    const [used] = await db.query(
      `SELECT booking_id
       FROM bookings
       WHERE schedule_id = ?
       AND reservation_status != 'Cancelled'
       LIMIT 1`,
      [scheduleId]
    );

    if (used.length > 0) {
      return res.status(400).json({
        success: false,
        message: 'Cannot delete this schedule because it already has bookings. Set it to Cancelled instead.',
      });
    }

    const [result] = await db.query(
      'DELETE FROM schedules WHERE schedule_id = ?',
      [scheduleId]
    );

    if (result.affectedRows === 0) {
      return res.status(404).json({
        success: false,
        message: 'Schedule not found.',
      });
    }

    res.json({
      success: true,
      message: 'Schedule deleted successfully.',
    });
  } catch (err) {
    next(err);
  }
}

module.exports = {
  getSchedules,
  getScheduleById,
  createSchedule,
  updateSchedule,
  updateTripStatus,
  deleteSchedule,
};