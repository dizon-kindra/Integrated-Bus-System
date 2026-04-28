USE sr_db;

-- Required by the passenger API login/register/profile endpoints.
CREATE TABLE IF NOT EXISTS users (
  user_id INT AUTO_INCREMENT PRIMARY KEY,
  full_name VARCHAR(100) NOT NULL,
  email VARCHAR(100) NOT NULL UNIQUE,
  phone_number VARCHAR(50) NOT NULL,
  password VARCHAR(255) NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Required by the API booking endpoints.
-- Your uploaded sr_db.sql is missing these columns in bookings.
ALTER TABLE bookings
  ADD COLUMN IF NOT EXISTS user_id INT NULL AFTER booking_id,
  ADD COLUMN IF NOT EXISTS booking_code VARCHAR(100) NULL AFTER user_id,
  ADD COLUMN IF NOT EXISTS total_amount DECIMAL(10,2) NULL AFTER seat_no;

-- Optional but recommended indexes.
ALTER TABLE bookings
  ADD INDEX IF NOT EXISTS idx_bookings_user_id (user_id),
  ADD INDEX IF NOT EXISTS idx_bookings_booking_code (booking_code);
