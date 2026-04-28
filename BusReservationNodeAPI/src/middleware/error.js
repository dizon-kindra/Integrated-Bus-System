function notFound(req, res) {
  res.status(404).json({ success: false, message: 'Endpoint not found.' });
}

function errorHandler(err, req, res, next) {
  console.error(err);
  res.status(err.status || 500).json({
    success: false,
    message: err.message || 'Server error.',
  });
}

module.exports = { notFound, errorHandler };
