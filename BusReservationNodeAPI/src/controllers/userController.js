const db = require('../db');

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

module.exports = {
  getUsers,
};