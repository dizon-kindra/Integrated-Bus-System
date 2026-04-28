function normalizePhpHash(hash) {
  // PHP password_hash sometimes starts with $2y$.
  // bcryptjs expects $2a$/$2b$, so this converts it safely for verification.
  if (typeof hash === 'string' && hash.startsWith('$2y$')) {
    return '$2b$' + hash.slice(4);
  }
  return hash;
}

function requireFields(body, fields) {
  for (const field of fields) {
    if (body[field] === undefined || body[field] === null || String(body[field]).trim() === '') {
      return field;
    }
  }
  return null;
}

module.exports = { normalizePhpHash, requireFields };
