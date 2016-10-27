CREATE DATABASE  IF NOT EXISTS `rms` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `rms`;
-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: localhost    Database: rms
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
INSERT INTO `menu_category` VALUES (1,'Basic','Basic food'),(2,'Dish','Plate Dish'),(3,'Barbeque','Barbeque'),(4,'Soft Drink','Baverage'),(5,'Alchohol',NULL);
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
  `category_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`item_id`,`category_id`),
  KEY `fk_menu_item_menu_category_idx` (`category_id`),
  CONSTRAINT `fk_menu_item_menu_category` FOREIGN KEY (`category_id`) REFERENCES `menu_category` (`category_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu_item`
--

LOCK TABLES `menu_item` WRITE;
/*!40000 ALTER TABLE `menu_item` DISABLE KEYS */;
INSERT INTO `menu_item` VALUES (1,'Rice',1.99,'R','Bowl of rice',1),(2,'Noodle',2.99,'N','Yangchun noodle',1),(3,'Coke',4.5,'C','Tin',4),(4,'Tomato Egg',11.99,'TE','',2),(5,'Mao Tai',369.99,'MT','Authentic',5),(6,'Lamb Chuan',3.99,'LC','Xingjiang Style',3),(7,'Potato Chuan',1.99,'PC','Sliced Potato',3);
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
  `staff_account` varchar(10) NOT NULL,
  `table_no` int(10) unsigned NOT NULL,
  PRIMARY KEY (`order_no`),
  KEY `fk_order_staff1_idx` (`staff_account`),
  KEY `fk_order_table1_idx` (`table_no`),
  CONSTRAINT `fk_order_staff1` FOREIGN KEY (`staff_account`) REFERENCES `staff` (`account`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_order_table1` FOREIGN KEY (`table_no`) REFERENCES `table` (`table_no`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
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
  `item_id` int(10) unsigned NOT NULL,
  `category_id` int(10) unsigned NOT NULL,
  `order_no` int(6) unsigned zerofill NOT NULL,
  PRIMARY KEY (`item_id`,`category_id`,`order_no`),
  KEY `fk_order_item_menu_item1_idx` (`item_id`,`category_id`),
  KEY `fk_order_item_order1_idx` (`order_no`),
  CONSTRAINT `fk_order_item_menu_item1` FOREIGN KEY (`item_id`, `category_id`) REFERENCES `menu_item` (`item_id`, `category_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_order_item_order1` FOREIGN KEY (`order_no`) REFERENCES `order` (`order_no`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order_item`
--

LOCK TABLES `order_item` WRITE;
/*!40000 ALTER TABLE `order_item` DISABLE KEYS */;
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
INSERT INTO `staff` VALUES ('M00001','123456','Ken Tompson',43,'15238492043','Manager',NULL),('W00001','654321','Jim Parsons',26,'18234013874','Waiter','M00001'),('W00002','654321','Gary Herington',24,'18843842104','Waiter','M00001');
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
INSERT INTO `table` VALUES (1,'4-Person Table'),(2,'4-Person Table'),(3,'4-Person Table'),(4,'4-Person Table'),(5,'8-Person Table'),(6,'8-Person Table'),(7,'8-Person Table');
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

-- Dump completed on 2016-10-28  0:51:30
