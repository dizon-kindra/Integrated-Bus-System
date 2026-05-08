const bcrypt = require('bcryptjs');
const db = require('../db');
const { normalizePhpHash } = require('../utils');

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

    const isValid = await bcrypt.compare(
      current_password,
      normalizePhpHash(users[0].password)
    );

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
  updateProfile,
  changePassword,
};