-- --------------------------------------------------------
-- Хост:                         127.0.0.1
-- Версия сервера:               8.0.19 - MySQL Community Server - GPL
-- Операционная система:         Win64
-- HeidiSQL Версия:              11.0.0.5919
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Дамп структуры базы данных site
CREATE DATABASE IF NOT EXISTS `site` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `site`;

-- Дамп структуры для таблица site.people
CREATE TABLE IF NOT EXISTS `people` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) DEFAULT NULL,
  `Surname` varchar(45) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Birthday` date DEFAULT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `Img` varchar(255) DEFAULT NULL,
  `Role` int DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы site.people: ~3 rows (приблизительно)
/*!40000 ALTER TABLE `people` DISABLE KEYS */;
INSERT INTO `people` (`id`, `Name`, `Surname`, `Email`, `Birthday`, `Password`, `Img`, `Role`) VALUES
	(6, 'Антон', 'Сухоруков', 'suh-an@ya.ru', '1996-11-23', 'lmiEInvtjsQ4XQ6v9bP25A5DMuv3L8cnqQEj+kmc0k4=', '6.png', 3),
	(8, 'Дмитрий', 'Морозов', 'SuperAdmin@Admin.ru', '1997-05-23', 'BilIUvEr+pDCkqU/2ZtI/Xk/vvq9D36s0GibFnS6Kn4=', NULL, 3),
	(10, 'Вадим', 'Селедцов', 'Admin@Admin.ru', '1980-05-23', 'XgDcQyiA9+bcCgX2CPza51gQwtbgs5u8A5NXJc2AaCo=', NULL, 2),
	(11, 'Обычный', 'Человек', 'User@User.ru', '1980-11-23', 'YqXvYdDaVjBzEXPakKZBQFlqJ6PeRezVWcdtBxsu2lY=', NULL, 1);
/*!40000 ALTER TABLE `people` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
