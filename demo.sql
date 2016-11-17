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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu_category`
--

LOCK TABLES `menu_category` WRITE;
/*!40000 ALTER TABLE `menu_category` DISABLE KEYS */;
INSERT INTO `menu_category` VALUES (1,'Main Course','chn'),(2,'Dish','chn'),(3,'Western',''),(4,'Alchohol','Sell to adults'),(5,'Snacks','no nutrition'),(6,'Barbecue','Hot'),(7,'Hot Pot','spicy'),(8,'buffet','expensive'),(9,'Seafood','may be allergic'),(10,'Fruit','healthy');
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
  UNIQUE KEY `item_code_UNIQUE` (`item_code`),
  KEY `fk_menu_item_menu_category_idx` (`category_id`),
  CONSTRAINT `fk_menu_item_menu_category` FOREIGN KEY (`category_id`) REFERENCES `menu_category` (`category_id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menu_item`
--

LOCK TABLES `menu_item` WRITE;
/*!40000 ALTER TABLE `menu_item` DISABLE KEYS */;
INSERT INTO `menu_item` VALUES (1,'Rice',1.99,'R','bb',1),(2,'Noodle',6.99,'N','123',1),(3,'Egg fried tomatoes',12.99,'T',NULL,2),(4,'Pizza',32.99,'P','',3),(5,'XO',119.99,'X','',4),(6,'Hamberger',21.99,'H','',3),(7,'Fried potato',2.99,'F',NULL,2),(8,'Mutton',55.99,'M','mild',7),(9,'Crispy chips',1.99,'C',NULL,5),(10,'Drumsticks',9.99,'D','meat',6),(11,'Oyster',66.88,'O','seafood',8),(12,'Lobster',222.99,'L','delicious',9),(13,'Banana',1.99,'B',NULL,10),(14,'Eggplant',4.66,'E',NULL,6),(15,'Octopus',333.44,'Z',NULL,9);
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
  `table_no` int(10) unsigned DEFAULT NULL,
  PRIMARY KEY (`order_no`),
  KEY `fk_order_staff1_idx` (`staff_account`),
  KEY `fk_order_table1_idx` (`table_no`),
  CONSTRAINT `fk_order_staff1` FOREIGN KEY (`staff_account`) REFERENCES `staff` (`account`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `fk_order_table1` FOREIGN KEY (`table_no`) REFERENCES `table` (`table_no`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order`
--

LOCK TABLES `order` WRITE;
/*!40000 ALTER TABLE `order` DISABLE KEYS */;
INSERT INTO `order` VALUES (000001,'2016-10-02 22:34:59',122,'qwe',2),(000002,'2016-11-01 13:23:48',70,'qwe',1),(000003,'2016-11-01 13:32:19',200,'qwe',2),(000004,'2016-10-12 22:34:59',200,'qwe',3),(000005,'2016-10-01 22:34:59',200,'qwe',4),(000006,'2016-09-02 22:34:59',200,'qwe',5),(000007,'2016-09-22 22:34:59',200,'qwe',4),(000008,'2016-08-02 22:34:59',200,'qwe',3),(000009,'2016-07-02 22:34:59',200,'qwe',5),(000010,'2016-07-14 22:34:59',200,'qwe',3),(000011,'2016-11-17 17:58:28',30,'M00001',3),(000012,'2016-11-17 18:24:41',40,'qwe',NULL);
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
  `total_price` double NOT NULL,
  PRIMARY KEY (`order_item_id`),
  KEY `fk_order_item_menu_item1_idx` (`item_id`),
  KEY `fk_order_item_order1_idx` (`order_no`),
  CONSTRAINT `fk_order_item_menu_item1` FOREIGN KEY (`item_id`) REFERENCES `menu_item` (`item_id`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `fk_order_item_order1` FOREIGN KEY (`order_no`) REFERENCES `order` (`order_no`) ON DELETE CASCADE ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order_item`
--

LOCK TABLES `order_item` WRITE;
/*!40000 ALTER TABLE `order_item` DISABLE KEYS */;
INSERT INTO `order_item` VALUES (2,4,000003,7,65.98),(2,2,000003,9,13.98),(2,4,000003,10,65.98),(2,6,000004,11,43.98),(6,4,000004,12,197.94),(1,2,000004,13,6.99),(1,12,000004,14,222.99),(4,6,000005,15,87.96),(8,1,000005,16,15.92),(5,5,000006,17,599.9499999999999),(3,8,000006,18,167.97),(1,11,000006,19,66.88),(2,8,000007,20,111.98),(7,13,000007,21,13.93),(16,14,000007,22,74.56),(16,14,000008,23,74.56),(2,13,000008,24,3.98),(2,15,000009,25,666.88),(4,4,000009,26,131.96),(2,7,000009,27,5.98),(8,2,000009,28,55.92),(2,9,000010,29,3.98),(2,2,000001,30,13.98),(5,4,000001,31,164.95000000000002),(1,3,000002,32,12.99),(2,1,000002,33,3.98),(3,10,000011,34,29.97),(1,2,000012,35,6.99),(1,4,000012,36,32.99);
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
INSERT INTO `staff` VALUES ('M00001','123','Ken Tompson',45,'01012321','Manager',NULL),('qwe','123','Kurt',25,'+8613213','Manager',NULL),('W00001','321','Jim Parsons',27,'010842719','Waiter','M00001');
/*!40000 ALTER TABLE `staff` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `table`
--

DROP TABLE IF EXISTS `table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `table` (
  `table_no` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `table_info` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`table_no`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `table`
--

LOCK TABLES `table` WRITE;
/*!40000 ALTER TABLE `table` DISABLE KEYS */;
INSERT INTO `table` VALUES (1,'2 seats'),(2,'2 seats'),(3,'2 seats'),(4,'2 seats'),(5,'4 seats'),(6,'4 seats'),(7,'4 seats'),(8,'4 seats'),(9,'5 seats'),(10,'5 seats'),(11,'6 seats'),(12,'6 seats'),(13,'7 seats'),(14,'8 seats'),(15,'7 seats');
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

-- Dump completed on 2016-11-18  2:27:53
