const bcrypt = require('bcryptjs');
const db = require('../db');
const { normalizePhpHash, requireFields } = require('../utils');

async function register(req, res, next) {
  try {
    const missing = requireFields(req.body, [
      'full_name',
      'email',
      'phone_number',
      'password',
      'confirm_password',
    ]);

    if (missing) {
      return res.status(400).json({
        success: false,
        message: 'All fields are required.',
      });
    }

    const { full_name, email, phone_number, password, confirm_password } = req.body;

    if (!String(email).includes('@')) {
      return res.status(400).json({
        success: false,
        message: 'Invalid email address.',
      });
    }

    if (password !== confirm_password) {
      return res.status(400).json({
        success: false,
        message: 'Passwords do not match.',
      });
    }

    if (String(password).length < 6) {
      return res.status(400).json({
        success: false,
        message: 'Password must be at least 6 characters.',
      });
    }

    const [existing] = await db.query(
      'SELECT user_id FROM users WHERE email = ? LIMIT 1',
      [email]
    );

    if (existing.length > 0) {
      return res.status(409).json({
        success: false,
        message: 'Email already registered.',
      });
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
      return res.status(400).json({
        success: false,
        message: 'Email and password are required.',
      });
    }

    const [users] = await db.query(
      `SELECT user_id, full_name, email, phone_number, password, created_at
       FROM users 
       WHERE email = ? 
       LIMIT 1`,
      [email]
    );

    if (users.length === 0) {
      return res.status(401).json({
        success: false,
        message: 'Invalid email or password.',
      });
    }

    const user = users[0];
    const isValid = await bcrypt.compare(password, normalizePhpHash(user.password));

    if (!isValid) {
      return res.status(401).json({
        success: false,
        message: 'Invalid email or password.',
      });
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
async function adminLogin(req, res, next) {
  try {
    const { username = '', password = '' } = req.body;

    if (!username || !password) {
      return res.status(400).json({
        success: false,
        message: 'Admin username and password are required.',
      });
    }

    const [admins] = await db.query(
      `SELECT admin_id, username, password
       FROM admins
       WHERE username = ?
       LIMIT 1`,
      [username]
    );

    if (admins.length === 0) {
      return res.status(401).json({
        success: false,
        message: 'Invalid admin username or password.',
      });
    }

    const admin = admins[0];

    const isValid = await bcrypt.compare(
      password,
      normalizePhpHash(admin.password)
    );

    if (!isValid) {
      return res.status(401).json({
        success: false,
        message: 'Invalid admin username or password.',
      });
    }

    res.json({
      success: true,
      message: 'Admin login successful.',
      admin: {
        admin_id: admin.admin_id,
        username: admin.username,
      },
    });
  } catch (err) {
    next(err);
  }
}
module.exports = {
  register,
  login,
  adminLogin,
};