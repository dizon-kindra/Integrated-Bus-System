const db = require('../db');

async function getAllBookings(req, res, next) {
  try {
    const [bookings] = await db.query(`
      SELECT
        bk.booking_id,
        bk.user_id,
        bk.booking_code,
        bk.passenger_name,
        bk.phone,
        bk.email,
        bk.seat_no,
        bk.total_amount,
        bk.payment_status,
        bk.reservation_status,
        bk.checkin_status,
        bk.boarding_status,
        bk.created_at,

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

        r.route_id,
        r.origin,
        r.destination,
        r.estimated_duration,

        p.payment_id,
        p.payment_method,
        p.reference_no,
        p.payment_status AS payment_record_status,
        p.paid_at
      FROM bookings bk
      INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
      INNER JOIN buses b ON s.bus_id = b.bus_id
      INNER JOIN routes r ON s.route_id = r.route_id
      LEFT JOIN payments p ON p.booking_id = bk.booking_id
      ORDER BY bk.created_at DESC
    `);

    res.json({
      success: true,
      count: bookings.length,
      bookings,
    });
  } catch (err) {
    next(err);
  }
}

async function createBooking(req, res, next) {
  const conn = await db.getConnection();

  try {
    const {
      user_id = null,
      schedule_id,
      passenger_name = '',
      phone = '',
      email = '',
      seats = [],
    } = req.body;

    if (!schedule_id || !passenger_name || !phone || !email || !Array.isArray(seats) || seats.length === 0) {
      return res.status(400).json({
        success: false,
        message: 'Missing required booking data.',
      });
    }

    await conn.beginTransaction();

    const [schedules] = await conn.query(
      `SELECT schedule_id, fare, available_seats 
       FROM schedules 
       WHERE schedule_id = ? 
       FOR UPDATE`,
      [schedule_id]
    );

    if (schedules.length === 0) {
      throw new Error('Schedule not found.');
    }

    const schedule = schedules[0];
    const fare = Number(schedule.fare);
    const availableSeats = Number(schedule.available_seats);

    if (seats.length > availableSeats) {
      throw new Error('Not enough available seats.');
    }

    const bookingCode = `BK-${Date.now().toString(36).toUpperCase()}`;
    const createdBookingIds = [];

    for (const rawSeatNo of seats) {
      const seatNo = Number(rawSeatNo);

      const [existing] = await conn.query(
        `SELECT booking_id FROM bookings
         WHERE schedule_id = ? 
         AND seat_no = ? 
         AND reservation_status != 'Cancelled'`,
        [schedule_id, seatNo]
      );

      if (existing.length > 0) {
        throw new Error(`Seat number ${seatNo} is already booked.`);
      }

      const [bookingResult] = await conn.query(
        `INSERT INTO bookings
         (user_id, booking_code, schedule_id, passenger_name, phone, email, seat_no, total_amount,
          payment_status, reservation_status, checkin_status, boarding_status)
         VALUES (?, ?, ?, ?, ?, ?, ?, ?, 'Pending', 'Pending', 'Not Checked-in', 'Not Boarded')`,
        [user_id, bookingCode, schedule_id, passenger_name, phone, email, seatNo, fare]
      );

      const bookingId = bookingResult.insertId;
      createdBookingIds.push(bookingId);

      await conn.query(
        `INSERT INTO payments 
         (booking_id, amount, payment_method, reference_no, payment_status)
         VALUES (?, ?, 'Pay at Terminal', ?, 'Pending')`,
        [bookingId, fare, bookingCode]
      );
    }

    await conn.query(
      'UPDATE schedules SET available_seats = ? WHERE schedule_id = ?',
      [availableSeats - seats.length, schedule_id]
    );

    await conn.commit();

    res.json({
      success: true,
      message: 'Booking created successfully.',
      booking_code: bookingCode,
      booking_ids: createdBookingIds,
      total_amount: fare * seats.length,
    });
  } catch (err) {
    await conn.rollback();

    res.status(400).json({
      success: false,
      message: err.message,
    });
  } finally {
    conn.release();
  }
}

async function myBookings(req, res, next) {
  try {
    const userId = Number(req.query.user_id);

    if (!userId) {
      return res.status(400).json({
        success: false,
        message: 'User ID is required.',
      });
    }

    const [bookings] = await db.query(
      `SELECT
        bk.booking_id,
        bk.user_id,
        bk.booking_code,
        bk.passenger_name,
        bk.phone,
        bk.email,
        bk.seat_no,
        bk.total_amount,
        bk.payment_status,
        bk.reservation_status,
        bk.checkin_status,
        bk.boarding_status,
        bk.created_at,

        s.schedule_id,
        s.departure_date,
        s.departure_time,
        s.arrival_time,
        s.fare,
        s.trip_status,

        b.bus_id,
        b.bus_number,
        b.plate_number,
        b.bus_type,

        r.route_id,
        r.origin,
        r.destination,
        r.estimated_duration,

        p.payment_id,
        p.payment_method,
        p.reference_no,
        p.payment_status AS payment_record_status,
        p.paid_at
       FROM bookings bk
       INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
       INNER JOIN buses b ON s.bus_id = b.bus_id
       INNER JOIN routes r ON s.route_id = r.route_id
       LEFT JOIN payments p ON p.booking_id = bk.booking_id
       WHERE bk.user_id = ?
       ORDER BY bk.created_at DESC`,
      [userId]
    );

    res.json({
      success: true,
      count: bookings.length,
      bookings,
    });
  } catch (err) {
    next(err);
  }
}

async function cancelBooking(req, res, next) {
  const conn = await db.getConnection();

  try {
    const bookingId = Number(req.body.booking_id);
    const userId = Number(req.body.user_id);

    if (!bookingId || !userId) {
      return res.status(400).json({
        success: false,
        message: 'Booking ID and User ID are required.',
      });
    }

    await conn.beginTransaction();

    const [rows] = await conn.query(
      `SELECT booking_id, user_id, schedule_id, seat_no, payment_status, reservation_status, checkin_status, boarding_status
       FROM bookings 
       WHERE booking_id = ? 
       AND user_id = ? 
       FOR UPDATE`,
      [bookingId, userId]
    );

    if (rows.length === 0) {
      throw new Error('Booking not found.');
    }

    const booking = rows[0];

    if (booking.reservation_status === 'Cancelled') {
      throw new Error('Booking is already cancelled.');
    }

    if (booking.payment_status === 'Paid') {
      throw new Error('Paid booking cannot be cancelled from passenger side. Please contact terminal staff.');
    }

    if (booking.checkin_status === 'Checked-in' || booking.boarding_status === 'Boarded') {
      throw new Error('Checked-in or boarded booking cannot be cancelled.');
    }

    await conn.query(
      `UPDATE bookings 
       SET payment_status = 'Cancelled', reservation_status = 'Cancelled'
       WHERE booking_id = ? 
       AND user_id = ?`,
      [bookingId, userId]
    );

    await conn.query(
      `UPDATE payments 
       SET payment_status = 'Cancelled' 
       WHERE booking_id = ?`,
      [bookingId]
    );

    await conn.query(
      `UPDATE schedules 
       SET available_seats = available_seats + 1 
       WHERE schedule_id = ?`,
      [booking.schedule_id]
    );

    await conn.commit();

    res.json({
      success: true,
      message: 'Booking cancelled successfully.',
      booking_id: bookingId,
      schedule_id: booking.schedule_id,
    });
  } catch (err) {
    await conn.rollback();

    res.status(400).json({
      success: false,
      message: err.message,
    });
  } finally {
    conn.release();
  }
}

module.exports = {
  getAllBookings,
  createBooking,
  myBookings,
  cancelBooking,
};