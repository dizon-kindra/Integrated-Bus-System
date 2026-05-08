const express = require('express');
const cors = require('cors');
require('dotenv').config();

const { notFound, errorHandler } = require('./middleware/error');

const testRoutes = require('./routes/testRoutes');
const userRoutes = require('./routes/userRoutes');
const authRoutes = require('./routes/authRoutes');
const tripRoutes = require('./routes/tripRoutes');
const bookingRoutes = require('./routes/bookingRoutes');
const profileRoutes = require('./routes/profileRoutes');

const app = express();

app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.get('/', (req, res) => {
  res.json({
    success: true,
    message: 'Bus Reservation Node API is running.',
    test_url: '/api/test'
  });
});

// Direct separated routes
app.use('/api', testRoutes);
app.use('/api', userRoutes);
app.use('/api', authRoutes);
app.use('/api', tripRoutes);
app.use('/api', bookingRoutes);
app.use('/api', profileRoutes);

app.use(notFound);
app.use(errorHandler);

const port = process.env.PORT || 3000;

app.listen(port, () => {
  console.log(`Bus Reservation Node API running on http://localhost:${port}`);
  console.log(`Test URL: http://localhost:${port}/api/test`);
});