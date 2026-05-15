const db = require('../db');

async function getBookingDetails(req, res, next) {
  try {
    const scheduleId = Number(req.query.schedule_id);

    if (!scheduleId) {
      return res.status(400).json({
        success: false,
        message: 'Schedule ID is required.',
      });
    }

    const [schedules] = await db.query(
      `
      SELECT 
        s.schedule_id,
        s.departure_date,
        s.departure_time,
        s.arrival_time,
        s.fare,
        s.available_seats,
        s.trip_status,

        b.bus_id,
        b.bus_number,
        b.plate_number,
        b.bus_type,
        b.capacity,

        r.route_id,
        r.origin,
        r.destination,
        r.estimated_duration
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

    const schedule = schedules[0];

    if (String(schedule.trip_status || '').toLowerCase() === 'cancelled') {
      return res.status(400).json({
        success: false,
        message: 'This trip is no longer available.',
      });
    }

    if (Number(schedule.available_seats || 0) <= 0) {
      return res.status(400).json({
        success: false,
        message: 'No available seats for this trip.',
      });
    }

    const [bookedRows] = await db.query(
      `
      SELECT seat_no 
      FROM bookings 
      WHERE schedule_id = ?
      AND reservation_status != 'Cancelled'
      `,
      [scheduleId]
    );

    const bookedSeats = bookedRows
      .map(row => Number(row.seat_no))
      .filter(seat => !Number.isNaN(seat));

    return res.json({
      success: true,
      schedule,
      bookedSeats,
    });
  } catch (err) {
    next(err);
  }
}

module.exports = {
  getBookingDetails,
};