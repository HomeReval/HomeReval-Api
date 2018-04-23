-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Gegenereerd op: 22 mrt 2018 om 22:00
-- Serverversie: 10.1.28-MariaDB
-- PHP-versie: 7.1.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `webshop`
--

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `adress`
--

CREATE TABLE `adress` (
  `id` bigint(20) NOT NULL,
  `housenumber` varchar(255) DEFAULT NULL,
  `street` varchar(255) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Gegevens worden geëxporteerd voor tabel `adress`
--

INSERT INTO `adress` (`id`, `housenumber`, `street`) VALUES
(1, '800', 'daltonlaan'),
(2, '400', 'daltonlaan');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `application_user`
--

CREATE TABLE `application_user` (
  `id` bigint(20) NOT NULL,
  `active` bit(1) NOT NULL DEFAULT b'1',
  `password` varchar(255) DEFAULT NULL,
  `username` varchar(255) DEFAULT NULL,
  `adress_id` bigint(20) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Gegevens worden geëxporteerd voor tabel `application_user`
--

INSERT INTO `application_user` (`id`, `active`, `password`, `username`, `adress_id`) VALUES
(1, b'1', '$2a$10$cCLV9mAeeAkuX6LMCQPKhep5Cz3kjO/R8rdXgrgzGxxy9E7m9G1HC', 'stefan', 1),
(2, b'1', '$2a$10$11McVYhf3c5H8gEL59UEeePwOsxPG3o00T0mA4GtSRNtqgTVeBm.C', 'bob', 2);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `category`
--

CREATE TABLE `category` (
  `id` bigint(20) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `path_name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Gegevens worden geëxporteerd voor tabel `category`
--

INSERT INTO `category` (`id`, `name`, `path_name`) VALUES
(1, 'Auto\'s', 'autos'),
(2, 'Vliegtuigen', 'vliegtuigen');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `discount`
--

CREATE TABLE `discount` (
  `id` bigint(20) NOT NULL,
  `begin_date` datetime DEFAULT NULL,
  `end_date` datetime DEFAULT NULL,
  `product_id` bigint(20) DEFAULT NULL,
  `discount_percentage` int(11) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Gegevens worden geëxporteerd voor tabel `discount`
--

INSERT INTO `discount` (`id`, `begin_date`, `end_date`, `product_id`, `discount_percentage`) VALUES
(1, '2018-03-13 00:00:00', '2018-03-29 00:00:00', 1, 10),
(2, '2018-03-20 00:00:00', '2018-03-30 00:00:00', 5, 90);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `product`
--

CREATE TABLE `product` (
  `id` bigint(20) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `price` float NOT NULL,
  `category_id` bigint(20) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Gegevens worden geëxporteerd voor tabel `product`
--

INSERT INTO `product` (`id`, `name`, `price`, `category_id`, `description`, `created_at`) VALUES
(1, 'Ferrari', 800000, 1, 'Een snelle auto', '2018-03-16 00:00:00'),
(4, 'Fiat', 10000, 1, 'Een minder snelle auto', '2018-03-01 00:00:00'),
(5, 'Boeing 777', 9800000, 2, 'Vliegtuig', '2018-03-13 00:00:00'),
(6, 'Airbus A300\r\nAirbus A310\r\n', 1828910, 2, 'Vliegtuig2', '2018-03-22 00:00:00'),
(7, 'Boeing 787 Dreamliner\r\n', 833333000, 2, 'Vliegtuig3', '2018-03-21 00:00:00'),
(11, 'BMW', 80000, 1, 'bmw', '2018-03-05 00:00:00'),
(12, 'Citroen', 8000, 1, 'citroen', '2018-03-22 13:31:41');

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `product_order`
--

CREATE TABLE `product_order` (
  `id` bigint(20) NOT NULL,
  `adress_id` bigint(20) DEFAULT NULL,
  `application_user_id` bigint(20) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Gegevens worden geëxporteerd voor tabel `product_order`
--

INSERT INTO `product_order` (`id`, `adress_id`, `application_user_id`) VALUES
(1, 1, 2),
(7, 1, 1),
(8, 1, 1),
(9, 1, 1);

-- --------------------------------------------------------

--
-- Tabelstructuur voor tabel `product_order_row`
--

CREATE TABLE `product_order_row` (
  `id` bigint(20) NOT NULL,
  `amount` int(11) NOT NULL,
  `product_id` bigint(20) DEFAULT NULL,
  `order_id` bigint(20) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Gegevens worden geëxporteerd voor tabel `product_order_row`
--

INSERT INTO `product_order_row` (`id`, `amount`, `product_id`, `order_id`) VALUES
(1, 5, 1, 1),
(2, 2, 1, 3),
(3, 3, 4, 3),
(4, 2, 1, 4),
(5, 3, 4, 4),
(6, 2, 1, 5),
(7, 3, 4, 5),
(8, 2, 1, 6),
(9, 3, 4, 6),
(10, 2, 1, 7),
(11, 3, 4, 7),
(12, 1, 1, 8),
(13, 1, 1, 9);

--
-- Indexen voor geëxporteerde tabellen
--

--
-- Indexen voor tabel `adress`
--
ALTER TABLE `adress`
  ADD PRIMARY KEY (`id`);

--
-- Indexen voor tabel `application_user`
--
ALTER TABLE `application_user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD KEY `FKh7vjk8kevcgo2adylosa52c7w` (`adress_id`);

--
-- Indexen voor tabel `category`
--
ALTER TABLE `category`
  ADD PRIMARY KEY (`id`);

--
-- Indexen voor tabel `discount`
--
ALTER TABLE `discount`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK9qcsopl406ufumbitfi9u7jop` (`product_id`);

--
-- Indexen voor tabel `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK1mtsbur82frn64de7balymq9s` (`category_id`);

--
-- Indexen voor tabel `product_order`
--
ALTER TABLE `product_order`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FK9x9jb8wfr3vnnyfft98wnberi` (`adress_id`),
  ADD KEY `FKlqku2v5wneq0a02bh0j84g8s6` (`application_user_id`);

--
-- Indexen voor tabel `product_order_row`
--
ALTER TABLE `product_order_row`
  ADD PRIMARY KEY (`id`),
  ADD KEY `FKpjj7o0ykmo5fcwwxg02jr3dpf` (`product_id`),
  ADD KEY `FKlby3yg7aswh7baihljlejh7sw` (`order_id`);

--
-- AUTO_INCREMENT voor geëxporteerde tabellen
--

--
-- AUTO_INCREMENT voor een tabel `adress`
--
ALTER TABLE `adress`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT voor een tabel `application_user`
--
ALTER TABLE `application_user`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT voor een tabel `category`
--
ALTER TABLE `category`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT voor een tabel `discount`
--
ALTER TABLE `discount`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT voor een tabel `product`
--
ALTER TABLE `product`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT voor een tabel `product_order`
--
ALTER TABLE `product_order`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT voor een tabel `product_order_row`
--
ALTER TABLE `product_order_row`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
