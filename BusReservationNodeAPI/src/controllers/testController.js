const db = require('../db');

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

module.exports = {
  test,
};