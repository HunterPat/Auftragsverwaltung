-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: order_management
-- ------------------------------------------------------
-- Server version	8.0.37

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `customers`
--

DROP TABLE IF EXISTS `customers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customers` (
  `CustomerId` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`CustomerId`)
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customers`
--

LOCK TABLES `customers` WRITE;
/*!40000 ALTER TABLE `customers` DISABLE KEYS */;
INSERT INTO `customers` VALUES (2,'abc'),(3,'abdd'),(4,'22'),(5,'fred'),(6,'drag'),(7,'ss'),(8,'dda'),(9,'ass'),(10,'112s'),(11,'1ss'),(12,'eyeo'),(13,'JM'),(14,'Motherson'),(15,'Levi'),(16,'Scrum Alliance'),(17,'TAP'),(18,'Adobe'),(19,'Zoetis'),(20,'Hilton'),(21,'Laura Ashley'),(22,'Teneo'),(23,'Caterpillar'),(24,'Adelphi'),(25,'Dassault'),(26,'Credit Suisse'),(27,'Hemsley Fraser'),(28,'Test Review'),(29,'Inbox Insight'),(30,'McKinsey'),(31,'Bridgestone'),(32,'PRS InVivo'),(33,'Kantar'),(34,'Pearl'),(35,'Amex'),(36,'UBS'),(37,'GWI'),(38,'Manor'),(39,'Exyte'),(40,'Rolls Royce'),(41,'FieldworkHub'),(42,'Magenta'),(43,'Nestle'),(44,'BAT'),(45,'Aston Martin'),(46,'The Ripple Effect'),(47,'Fellowes'),(48,'Global Lingo'),(49,'Pirelli'),(50,'Ashworth'),(51,'ddrr'),(52,'122a'),(53,'sfd1'),(54,'ssa1');
/*!40000 ALTER TABLE `customers` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-05 11:54:20
