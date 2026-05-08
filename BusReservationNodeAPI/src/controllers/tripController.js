const db = require('../db');

async function searchTrips(req, res, next) {
  try {
    const { view = '', source = '', destination = '', date = '' } = req.query;

    let sql = `
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
        b.capacity,
        b.bus_type,
        b.status AS bus_status,
        r.route_id,
        r.origin,
        r.destination,
        r.estimated_duration,
        r.status AS route_status
      FROM schedules s
      INNER JOIN buses b ON s.bus_id = b.bus_id
      INNER JOIN routes r ON s.route_id = r.route_id
      WHERE s.trip_status = 'Scheduled'
        AND b.status = 'Active'
        AND r.status = 'Active'
        AND s.available_seats > 0
    `;

    const values = [];

    if (view === 'all') {
      sql += ' AND s.departure_date >= CURDATE() ORDER BY s.departure_date ASC, s.departure_time ASC';
    } else {
      if (!source || !destination || !date) {
        return res.status(400).json({
          success: false,
          message: 'Source, destination, and date are required.',
        });
      }

      sql += `
        AND r.origin LIKE ?
        AND r.destination LIKE ?
        AND s.departure_date = ?
        ORDER BY s.departure_time ASC
      `;

      values.push(`%${source}%`, `%${destination}%`, date);
    }

    const [trips] = await db.query(sql, values);

    res.json({
      success: true,
      mode: view === 'all' ? 'all' : 'search',
      count: trips.length,
      trips,
    });
  } catch (err) {
    next(err);
  }
}

async function getSeats(req, res, next) {
  try {
    const scheduleId = Number(req.query.schedule_id);

    if (!scheduleId) {
      return res.status(400).json({
        success: false,
        message: 'Schedule ID is required.',
      });
    }

    const [schedules] = await db.query(
      `SELECT s.schedule_id, s.available_seats, b.capacity
       FROM schedules s
       INNER JOIN buses b ON s.bus_id = b.bus_id
       WHERE s.schedule_id = ?`,
      [scheduleId]
    );

    if (schedules.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Schedule not found.',
      });
    }

    const capacity = Number(schedules[0].capacity);

    const [booked] = await db.query(
      `SELECT seat_no FROM bookings
       WHERE schedule_id = ? 
       AND reservation_status != 'Cancelled'`,
      [scheduleId]
    );

    const bookedSeats = booked.map(row => Number(row.seat_no));
    const seats = [];

    for (let i = 1; i <= capacity; i++) {
      seats.push({
        seat_no: i,
        status: bookedSeats.includes(i) ? 'booked' : 'available',
      });
    }

    res.json({
      success: true,
      schedule_id: scheduleId,
      capacity,
      available_count: capacity - bookedSeats.length,
      booked_count: bookedSeats.length,
      seats,
    });
  } catch (err) {
    next(err);
  }
}

module.exports = {
  searchTrips,
  getSeats,
};