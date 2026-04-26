-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 25, 2026 at 09:22 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `sr_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `bookings`
--

CREATE TABLE `bookings` (
  `booking_id` int(11) NOT NULL,
  `schedule_id` int(11) NOT NULL,
  `passenger_name` varchar(100) NOT NULL,
  `phone` varchar(50) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `seat_no` int(11) NOT NULL,
  `payment_status` varchar(20) DEFAULT 'Pending',
  `reservation_status` varchar(20) DEFAULT 'Pending',
  `checkin_status` varchar(30) DEFAULT 'Not Checked-in',
  `boarding_status` varchar(30) DEFAULT 'Not Boarded',
  `created_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bookings`
--

INSERT INTO `bookings` (`booking_id`, `schedule_id`, `passenger_name`, `phone`, `email`, `seat_no`, `payment_status`, `reservation_status`, `checkin_status`, `boarding_status`, `created_at`) VALUES
(1, 1, 'Juan Dela Cruz', '09123456789', 'juan@example.com', 1, 'Pending', 'Pending', 'Not Checked-in', 'Not Boarded', '2026-04-25 16:58:48'),
(2, 1, 'Maria Santos', '09987654321', 'maria@example.com', 2, 'Paid', 'Completed', 'Checked-in', 'Boarded', '2026-04-25 16:58:48');

-- --------------------------------------------------------

--
-- Table structure for table `buses`
--

CREATE TABLE `buses` (
  `bus_id` int(11) NOT NULL,
  `bus_number` varchar(50) NOT NULL,
  `plate_number` varchar(50) NOT NULL,
  `capacity` int(11) NOT NULL,
  `bus_type` varchar(50) NOT NULL,
  `status` varchar(20) DEFAULT 'Active'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `buses`
--

INSERT INTO `buses` (`bus_id`, `bus_number`, `plate_number`, `capacity`, `bus_type`, `status`) VALUES
(1, 'BUS-001', 'ABC-1234', 45, 'Airconditioned', 'Active'),
(2, 'BUS-002', 'XYZ-5678', 40, 'Ordinary', 'Active'),
(3, 'BUS-003', 'DEF-9012', 50, 'Airconditioned', 'Active'),
(4, 'BUS-001', 'ABC-1234', 40, 'Airconditioned', 'Active');

-- --------------------------------------------------------

--
-- Table structure for table `bus_add`
--

CREATE TABLE `bus_add` (
  `ID` int(11) NOT NULL,
  `b_no` varchar(100) DEFAULT NULL,
  `b_sou` varchar(100) DEFAULT NULL,
  `b_des` varchar(100) DEFAULT NULL,
  `b_ty` varchar(100) DEFAULT NULL,
  `b_time` varchar(100) DEFAULT NULL,
  `b_price` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bus_add`
--

INSERT INTO `bus_add` (`ID`, `b_no`, `b_sou`, `b_des`, `b_ty`, `b_time`, `b_price`) VALUES
(1, '123', 'Anahawan', 'Cabalian', 'Local', '7:00 am', '50');

-- --------------------------------------------------------

--
-- Table structure for table `bus_status`
--

CREATE TABLE `bus_status` (
  `seatno` int(11) NOT NULL,
  `status` varchar(10) DEFAULT 'A'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bus_status`
--

INSERT INTO `bus_status` (`seatno`, `status`) VALUES
(1, 'B'),
(2, 'B'),
(3, 'A'),
(4, 'A'),
(5, 'A'),
(6, 'A'),
(7, 'A'),
(8, 'A'),
(9, 'A'),
(10, 'A'),
(11, 'A'),
(12, 'A'),
(13, 'A'),
(14, 'A'),
(15, 'A'),
(16, 'A'),
(17, 'A'),
(18, 'A'),
(19, 'A'),
(20, 'A'),
(21, 'A'),
(22, 'A'),
(23, 'A'),
(24, 'A'),
(25, 'A'),
(26, 'A'),
(27, 'A'),
(28, 'A');

-- --------------------------------------------------------

--
-- Table structure for table `login`
--

CREATE TABLE `login` (
  `id` int(11) NOT NULL,
  `uname` varchar(100) NOT NULL,
  `pass` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `login`
--

INSERT INTO `login` (`id`, `uname`, `pass`) VALUES
(1, 'admin', '123');

-- --------------------------------------------------------

--
-- Table structure for table `passenger`
--

CREATE TABLE `passenger` (
  `ID` int(11) NOT NULL,
  `s_no` int(11) DEFAULT NULL,
  `b_no` varchar(100) DEFAULT NULL,
  `b_sr` varchar(100) DEFAULT NULL,
  `b_des` varchar(100) DEFAULT NULL,
  `b_ty` varchar(100) DEFAULT NULL,
  `b_ar` varchar(100) DEFAULT NULL,
  `b_price` decimal(10,2) DEFAULT NULL,
  `b_trav` varchar(100) DEFAULT NULL,
  `p_nm` varchar(100) DEFAULT NULL,
  `p_em` varchar(100) DEFAULT NULL,
  `p_ag` int(11) DEFAULT NULL,
  `p_mob` varchar(50) DEFAULT NULL,
  `p_gen` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `payments`
--

CREATE TABLE `payments` (
  `payment_id` int(11) NOT NULL,
  `booking_id` int(11) NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `payment_method` varchar(50) NOT NULL,
  `reference_no` varchar(100) DEFAULT NULL,
  `payment_status` varchar(20) DEFAULT 'Pending',
  `paid_at` datetime DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `payments`
--

INSERT INTO `payments` (`payment_id`, `booking_id`, `amount`, `payment_method`, `reference_no`, `payment_status`, `paid_at`, `created_at`) VALUES
(1, 2, 1000.00, 'Cash', '32435', 'Paid', '2026-04-26 01:52:50', '2026-04-25 17:52:26'),
(2, 2, 1000.00, 'Card', '32435', 'Paid', '2026-04-26 02:52:48', '2026-04-25 17:53:15');

-- --------------------------------------------------------

--
-- Table structure for table `routes`
--

CREATE TABLE `routes` (
  `route_id` int(11) NOT NULL,
  `route_code` varchar(20) NOT NULL,
  `origin` varchar(100) NOT NULL,
  `destination` varchar(100) NOT NULL,
  `fare` decimal(10,2) NOT NULL,
  `estimated_duration` varchar(50) NOT NULL,
  `status` varchar(20) NOT NULL DEFAULT 'Active',
  `created_at` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `routes`
--

INSERT INTO `routes` (`route_id`, `route_code`, `origin`, `destination`, `fare`, `estimated_duration`, `status`, `created_at`) VALUES
(1, 'RT-001', 'hinungan', 'st. bernard', 400.00, '4:00 am', 'Active', '2026-04-25 15:47:17'),
(2, 'RT-002', 'Hinunangan', 'Hilongos', 700.00, '4:00 pm', 'Active', '2026-04-25 18:49:07');

-- --------------------------------------------------------

--
-- Table structure for table `schedules`
--

CREATE TABLE `schedules` (
  `schedule_id` int(11) NOT NULL,
  `bus_id` int(11) NOT NULL,
  `route_id` int(11) NOT NULL,
  `departure_date` date NOT NULL,
  `departure_time` time NOT NULL,
  `arrival_time` time NOT NULL,
  `fare` decimal(10,2) NOT NULL,
  `available_seats` int(11) NOT NULL,
  `trip_status` varchar(20) DEFAULT 'Scheduled'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `schedules`
--

INSERT INTO `schedules` (`schedule_id`, `bus_id`, `route_id`, `departure_date`, `departure_time`, `arrival_time`, `fare`, `available_seats`, `trip_status`) VALUES
(1, 1, 1, '2026-04-26', '00:50:36', '20:50:36', 400.00, 46, 'Scheduled'),
(2, 1, 1, '2026-04-26', '02:50:05', '02:50:05', 400.00, 45, 'Scheduled'),
(3, 3, 2, '2026-04-27', '02:50:05', '04:50:05', 700.00, 50, 'Scheduled');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bookings`
--
ALTER TABLE `bookings`
  ADD PRIMARY KEY (`booking_id`),
  ADD KEY `schedule_id` (`schedule_id`);

--
-- Indexes for table `buses`
--
ALTER TABLE `buses`
  ADD PRIMARY KEY (`bus_id`);

--
-- Indexes for table `bus_add`
--
ALTER TABLE `bus_add`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `bus_status`
--
ALTER TABLE `bus_status`
  ADD PRIMARY KEY (`seatno`);

--
-- Indexes for table `login`
--
ALTER TABLE `login`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `passenger`
--
ALTER TABLE `passenger`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `payments`
--
ALTER TABLE `payments`
  ADD PRIMARY KEY (`payment_id`),
  ADD KEY `booking_id` (`booking_id`);

--
-- Indexes for table `routes`
--
ALTER TABLE `routes`
  ADD PRIMARY KEY (`route_id`),
  ADD UNIQUE KEY `route_code` (`route_code`);

--
-- Indexes for table `schedules`
--
ALTER TABLE `schedules`
  ADD PRIMARY KEY (`schedule_id`),
  ADD KEY `bus_id` (`bus_id`),
  ADD KEY `route_id` (`route_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bookings`
--
ALTER TABLE `bookings`
  MODIFY `booking_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `buses`
--
ALTER TABLE `buses`
  MODIFY `bus_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `bus_add`
--
ALTER TABLE `bus_add`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `login`
--
ALTER TABLE `login`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `passenger`
--
ALTER TABLE `passenger`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `payments`
--
ALTER TABLE `payments`
  MODIFY `payment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `routes`
--
ALTER TABLE `routes`
  MODIFY `route_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `schedules`
--
ALTER TABLE `schedules`
  MODIFY `schedule_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bookings`
--
ALTER TABLE `bookings`
  ADD CONSTRAINT `bookings_ibfk_1` FOREIGN KEY (`schedule_id`) REFERENCES `schedules` (`schedule_id`);

--
-- Constraints for table `payments`
--
ALTER TABLE `payments`
  ADD CONSTRAINT `payments_ibfk_1` FOREIGN KEY (`booking_id`) REFERENCES `bookings` (`booking_id`);

--
-- Constraints for table `schedules`
--
ALTER TABLE `schedules`
  ADD CONSTRAINT `schedules_ibfk_1` FOREIGN KEY (`bus_id`) REFERENCES `buses` (`bus_id`),
  ADD CONSTRAINT `schedules_ibfk_2` FOREIGN KEY (`route_id`) REFERENCES `routes` (`route_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
