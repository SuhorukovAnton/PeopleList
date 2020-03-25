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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Дамп данных таблицы site.people: ~3 rows (приблизительно)
/*!40000 ALTER TABLE `people` DISABLE KEYS */;
INSERT INTO `people` (`id`, `Name`, `Surname`, `Email`, `Birthday`, `Password`, `Img`, `Role`) VALUES
	(3, 'Мария', 'Нарюкова', 'masha16965@mail.ru', '1111-11-11', 'u1SR1JyZfP1JGmkpgKhf4dnNvxnISvwAlt5PHlspSqo=', '3.jpg', 1),
	(5, 'Влад', 'Зуев', 'masha16966@mail.ru', '3221-03-12', 'dRDhgynk6c2x5+6jr4szEyKqI7sc48zyQaqPztD8Wng=', NULL, 2),
	(6, 'Антон', 'Сухоруков', 'suh-an@ya.ru', '1996-11-23', 'Pj/Ou/iPbNHsri4SXZX90X15u3MzZlK+jc6Yz6Za8QQ=', '6.png', 3),
	(7, 'Александр', 'Ололоев', 'ololo@ra.ri', '1983-07-15', 'lCWmx8UPwtQMWtYbdVUxhXBY3+94i6sA1vWaNQIzDPE=', NULL, 1);
/*!40000 ALTER TABLE `people` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
