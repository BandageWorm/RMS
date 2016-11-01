CREATE DATABASE  IF NOT EXISTS `rms` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `rms`;
-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: rms
-- ------------------------------------------------------
-- Server version	5.7.15-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `menu_category`
--

DROP TABLE IF EXISTS `menu_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `menu_category` (
  `category_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `category` varchar(20) NOT NULL,
  `category_info` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu_category`
--

LOCK TABLES `menu_category` WRITE;
/*!40000 ALTER TABLE `menu_category` DISABLE KEYS */;
INSERT INTO `menu_category` VALUES (1,'Main Course','Chinese style'),(2,'Side dishes','Hong Kong style'),(3,'Drinks','Soft drinks'),(4,'Dessert','Cakes'),(5,'Wine','XO');
/*!40000 ALTER TABLE `menu_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `menu_item`
--

DROP TABLE IF EXISTS `menu_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `menu_item` (
  `item_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `item_name` varchar(60) NOT NULL,
  `price` double NOT NULL,
  `item_code` varchar(10) NOT NULL,
  `item_info` varchar(100) DEFAULT NULL,
  `category_id` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`item_id`),
  KEY `fk_menu_item_menu_category_idx` (`category_id`),
  CONSTRAINT `fk_menu_item_menu_category` FOREIGN KEY (`category_id`) REFERENCES `menu_category` (`category_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu_item`
--

LOCK TABLES `menu_item` WRITE;
/*!40000 ALTER TABLE `menu_item` DISABLE KEYS */;
INSERT INTO `menu_item` VALUES (1,'Rice',1.99,'R','2 bowls will kill you',1),(2,'Noodle',3.99,'N','Very long',1),(3,'Chicken wings',14.99,'CW','Can fly',2),(4,'Egg',5.99,'E','Supplement protein',2),(5,'Coke',1.99,'C','CO2',3),(6,'Sprite',1.99,'S','CO2',3);
/*!40000 ALTER TABLE `menu_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order`
--

DROP TABLE IF EXISTS `order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `order` (
  `order_no` int(6) unsigned zerofill NOT NULL AUTO_INCREMENT,
  `order_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `actual_payment` double NOT NULL,
  `staff_account` varchar(10) DEFAULT NULL,
  `table_no` int(10) unsigned NOT NULL,
  PRIMARY KEY (`order_no`),
  KEY `fk_order_staff1_idx` (`staff_account`),
  KEY `fk_order_table1_idx` (`table_no`),
  CONSTRAINT `fk_order_staff1` FOREIGN KEY (`staff_account`) REFERENCES `staff` (`account`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_order_table1` FOREIGN KEY (`table_no`) REFERENCES `table` (`table_no`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
INSERT INTO `order` VALUES (000001,'2016-11-01 11:36:47',110,'tony',1),(000002,'2016-11-01 11:36:50',0,'tony',2),(000003,'2016-11-01 11:36:52',0,'tony',3),(000004,'2016-11-01 11:46:55',0,'tony',4);
/*!40000 ALTER TABLE `order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order_item`
--

DROP TABLE IF EXISTS `order_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `order_item` (
  `quantity` int(10) unsigned NOT NULL,
  `item_id` int(10) unsigned DEFAULT NULL,
  `order_no` int(6) unsigned zerofill DEFAULT NULL,
  `order_item_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`order_item_id`),
  KEY `fk_order_item_menu_item1_idx` (`item_id`),
  KEY `fk_order_item_order1_idx` (`order_no`),
  CONSTRAINT `fk_order_item_menu_item1` FOREIGN KEY (`item_id`) REFERENCES `menu_item` (`item_id`) ON DELETE SET NULL ON UPDATE NO ACTION,
  CONSTRAINT `fk_order_item_order1` FOREIGN KEY (`order_no`) REFERENCES `order` (`order_no`) ON DELETE SET NULL ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order_item`
--

LOCK TABLES `order_item` WRITE;
/*!40000 ALTER TABLE `order_item` DISABLE KEYS */;
INSERT INTO `order_item` VALUES (2,1,000001,1),(2,2,000001,2),(2,4,000001,5),(2,3,000001,6),(2,3,000001,7),(2,NULL,000001,8);
/*!40000 ALTER TABLE `order_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `staff`
--

DROP TABLE IF EXISTS `staff`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `staff` (
  `account` varchar(10) NOT NULL,
  `password` varchar(20) NOT NULL,
  `staff_name` varchar(50) NOT NULL,
  `age` int(11) NOT NULL,
  `contact_no` varchar(15) NOT NULL,
  `role` varchar(10) NOT NULL,
  `manager_account` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`account`),
  KEY `fk_staff_staff1_idx` (`manager_account`),
  CONSTRAINT `fk_staff_staff1` FOREIGN KEY (`manager_account`) REFERENCES `staff` (`account`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `staff`
--

LOCK TABLES `staff` WRITE;
/*!40000 ALTER TABLE `staff` DISABLE KEYS */;
INSERT INTO `staff` VALUES ('tony','123','liuzhaozhi',23,'32131','Manager',NULL);
/*!40000 ALTER TABLE `staff` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table`
--

DROP TABLE IF EXISTS `table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `table` (
  `table_no` int(10) unsigned NOT NULL,
  `table_info` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`table_no`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table`
--

LOCK TABLES `table` WRITE;
/*!40000 ALTER TABLE `table` DISABLE KEYS */;
INSERT INTO `table` VALUES (1,'2 seats'),(2,'3 seats'),(3,'2 seats'),(4,'2 seats'),(5,'2 seats'),(6,'3 seats'),(7,'3 seats'),(8,'3 seats');
/*!40000 ALTER TABLE `table` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-11-01 19:59:03
