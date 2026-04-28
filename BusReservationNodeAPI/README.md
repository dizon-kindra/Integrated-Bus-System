# Bus Reservation Node API

This is the Node.js Express API version of the PHP `BusReservationAPI` folder.

## 1. Install dependencies

```powershell
cd C:\xampp\htdocs\Integrated-Bus-System\BusReservationNodeAPI
npm install
```

## 2. Create .env

Copy `.env.example` to `.env`.

```env
PORT=3000
DB_HOST=localhost
DB_USER=root
DB_PASSWORD=
DB_NAME=sr_db
```

If your MySQL password is `root`, set:

```env
DB_PASSWORD=root
```

## 3. Fix database schema

Open phpMyAdmin, select `sr_db`, click SQL, and run:

```sql
source database/sr_db_required_patch.sql
```

If phpMyAdmin cannot run `source`, open the file and paste the SQL manually.

## 4. Run API

```powershell
npm start
```

For auto restart while editing:

```powershell
npm run dev
```

## 5. Test URLs

```text
http://localhost:3000/api/test
http://localhost:3000/api/search-trips?view=all
http://localhost:3000/api/get-seats?schedule_id=1
```

PHP-style aliases also work:

```text
http://localhost:3000/api/test.php
http://localhost:3000/api/search_trips.php?view=all
http://localhost:3000/api/get_seats.php?schedule_id=1
```

## 6. Main endpoints

- POST `/api/register`
- POST `/api/login`
- GET `/api/search-trips?view=all`
- GET `/api/search-trips?source=hinungan&destination=st. bernard&date=2026-04-26`
- GET `/api/get-seats?schedule_id=1`
- POST `/api/create-booking`
- GET `/api/my-bookings?user_id=1`
- POST `/api/cancel-booking`
- PUT `/api/update-profile`
- PUT `/api/change-password`
