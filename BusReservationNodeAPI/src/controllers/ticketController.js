const db = require('../db');

async function getTicketDetails(req, res, next) {
  try {
    const bookingId = Number(req.query.booking_id);
    const userId = Number(req.query.user_id);

    if (!bookingId || !userId) {
      return res.status(400).json({
        success: false,
        message: 'Booking ID and User ID are required.',
      });
    }

    const [rows] = await db.query(
      `
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

        p.payment_id,
        p.payment_method,
        p.reference_no,
        p.paid_at,

        s.schedule_id,
        s.departure_date,
        s.departure_time,
        s.arrival_time,
        s.fare,

        b.bus_number,
        b.plate_number,
        b.bus_type,

        r.origin,
        r.destination,
        r.estimated_duration
      FROM bookings bk
      LEFT JOIN payments p ON bk.booking_id = p.booking_id
      INNER JOIN schedules s ON bk.schedule_id = s.schedule_id
      INNER JOIN buses b ON s.bus_id = b.bus_id
      INNER JOIN routes r ON s.route_id = r.route_id
      WHERE bk.booking_id = ?
      AND bk.user_id = ?
      LIMIT 1
      `,
      [bookingId, userId]
    );

    if (rows.length === 0) {
      return res.status(404).json({
        success: false,
        message: 'Booking not found.',
      });
    }

    const ticket = rows[0];

    if (
      String(ticket.payment_status || '').toLowerCase() !== 'paid' ||
      String(ticket.reservation_status || '').toLowerCase() !== 'confirmed'
    ) {
      return res.status(403).json({
        success: false,
        message: 'Ticket is not available yet.',
        payment_status: ticket.payment_status || 'Pending',
        reservation_status: ticket.reservation_status || 'Pending',
      });
    }

    return res.json({
      success: true,
      ticket,
    });
  } catch (err) {
    next(err);
  }
}

module.exports = {
  getTicketDetails,
};