const express = require('express');
const cors = require('cors');
require('dotenv').config();

const apiRoutes = require('./routes/api');
const { notFound, errorHandler } = require('./middleware/error');

const app = express();
const port = process.env.PORT || 3000;

app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.get('/', (req, res) => {
  res.json({
    success: true,
    message: 'Bus Reservation Node API',
    test_url: `http://localhost:${port}/api/test`,
  });
});

app.use('/api', apiRoutes);

app.use(notFound);
app.use(errorHandler);

app.listen(port, () => {
  console.log(`Bus Reservation Node API running on port ${port}`);
  console.log(`Test: http://localhost:${port}/api/test`);
});
