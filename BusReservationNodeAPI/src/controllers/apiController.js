const bcrypt = require('bcryptjs');
const db = require('../db');
const { normalizePhpHash, requireFields } = require('../utils');

async function test(req, res, next) {
  try {
    const [rows] = await db.query('SELECT DATABASE() AS database_name');
    res.json({
      success: true,
      message: 'Bus Reservation Node API is working',
      database: rows[0].database_name,
    });
  } catch (err) {
    next(err);
  }
}

async function getUsers(req, res, next) {
  try {
    const [users] = await db.query(`
      SELECT 
        user_id,
        full_name,
        email,
        phone_number,
        created_at
      FROM users
      ORDER BY user_id DESC
    `);

    res.json({
      success: true,
      count: users.length,
      users,
    });
  } catch (err) {
    next(err);
  }
}

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

async function register(req, res, next) {
  try {
    const missing = requireFields(req.body, ['full_name', 'email', 'phone_number', 'password', 'confirm_password']);
    if (missing) return res.status(400).json({ success: false, message: 'All fields are required.' });

    const { full_name, email, phone_number, password, confirm_password } = req.body;

    if (!String(email).includes('@')) {
      return res.status(400).json({ success: false, message: 'Invalid email address.' });
    }

    if (password !== confirm_password) {
      return res.status(400).json({ success: false, message: 'Passwords do not match.' });
    }

    if (String(password).length < 6) {
      return res.status(400).json({ success: false, message: 'Password must be at least 6 characters.' });
    }

    const [existing] = await db.query('SELECT user_id FROM users WHERE email = ? LIMIT 1', [email]);

    if (existing.length > 0) {
      return res.status(409).json({ success: false, message: 'Email already registered.' });
    }

    const hashedPassword = await bcrypt.hash(password, 10);

    const [result] = await db.query(
      'INSERT INTO users (full_name, email, phone_number, password) VALUES (?, ?, ?, ?)',
      [full_name, email, phone_number, hashedPassword]
    );

    res.json({
      success: true,
      message: 'Registration successful.',
      user: {
        user_id: result.insertId,
        full_name,
        email,
        phone_number,
      },
    });
  } catch (err) {
    next(err);
  }
}

async function login(req, res, next) {
  try {
    const { email = '', password = '' } = req.body;

    if (!email || !password) {
      return res.status(400).json({ success: false, message: 'Email and password are required.' });
    }

    const [users] = await db.query(
      `SELECT user_id, full_name, email, phone_number, password, created_at
       FROM users 
       WHERE email = ? 
       LIMIT 1`,
      [email]
    );

    if (users.length === 0) {
      return res.status(401).json({ success: false, message: 'Invalid email or password.' });
    }

    const user = users[0];
    const isValid = await bcrypt.compare(password, normalizePhpHash(user.password));

    if (!isValid) {
      return res.status(401).json({ success: false, message: 'Invalid email or password.' });
    }

    res.json({
      success: true,
      message: 'Login successful.',
      user: {
        user_id: user.user_id,
        full_name: user.full_name,
        email: user.email,
        phone_number: user.phone_number,
        created_at: user.created_at,
      },
    });
  } catch (err) {
    next(err);
  }
}

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

async function updateProfile(req, res, next) {
  try {
    const userId = Number(req.body.user_id);
    const { full_name = '', phone_number = '' } = req.body;

    if (!userId || !full_name || !phone_number) {
      return res.status(400).json({
        success: false,
        message: 'All fields are required.',
      });
    }

    const [result] = await db.query(
      'UPDATE users SET full_name = ?, phone_number = ? WHERE user_id = ?',
      [full_name, phone_number, userId]
    );

    if (result.affectedRows === 0) {
      return res.status(404).json({
        success: false,
        message: 'User not found.',
      });
    }

    res.json({
      success: true,
      message: 'Profile updated successfully.',
    });
  } catch (err) {
    next(err);
  }
}

async function changePassword(req, res, next) {
  try {
    const userId = Number(req.body.user_id);

    const {
      current_password = '',
      new_password = '',
      confirm_password = '',
    } = req.body;

    if (!userId || !current_password || !new_password || !confirm_password) {
      return res.status(400).json({
        success: false,
        message: 'All password fields are required.',
      });
    }

    if (new_password !== confirm_password) {
      return res.status(400).json({
        success: false,
        message: 'New password and confirm password do not match.',
      });
    }

    if (String(new_password).length < 6) {
      return res.status(400).json({
        success: false,
        message: 'New password must be at least 6 characters.',
      });
    }

    const [users] = await db.query(
      'SELECT password FROM users WHERE user_id = ? LIMIT 1',
      [userId]
    );

    if (users.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'User not found.',
      });
    }

    const isValid = await bcrypt.compare(current_password, normalizePhpHash(users[0].password));

    if (!isValid) {
      return res.status(401).json({
        success: false,
        message: 'Current password is incorrect.',
      });
    }

    const hashedPassword = await bcrypt.hash(new_password, 10);

    await db.query(
      'UPDATE users SET password = ? WHERE user_id = ?',
      [hashedPassword, userId]
    );

    res.json({
      success: true,
      message: 'Password changed successfully.',
    });
  } catch (err) {
    next(err);
  }
}

module.exports = {
  test,
  getUsers,
  getAllBookings,
  register,
  login,
  searchTrips,
  getSeats,
  createBooking,
  myBookings,
  cancelBooking,
  updateProfile,
  changePassword,
};