-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: clientesdb
-- ------------------------------------------------------
-- Server version	8.0.42

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
-- Table structure for table `clientes`
--

DROP TABLE IF EXISTS `clientes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clientes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) NOT NULL,
  `DataNascimento` date NOT NULL,
  `Cpf` char(11) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `RendimentoAnual` decimal(15,2) NOT NULL,
  `Estado` char(2) NOT NULL,
  `Telefone` varchar(15) NOT NULL,
  `Score` int NOT NULL,
  `Classificacao` varchar(50) NOT NULL,
  `DataCadastro` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Cpf` (`Cpf`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clientes`
--

LOCK TABLES `clientes` WRITE;
/*!40000 ALTER TABLE `clientes` DISABLE KEYS */;
INSERT INTO `clientes` VALUES (1,'Camila Andrade','1992-04-10','12345678901','camila.andrade@email.com',85000.00,'SP','11988887777',350,'Cliente Regular','2025-05-10 21:11:46'),(2,'Pablo Alves','2000-03-01','49796868857','teste@gmail.com',10000.00,'SP','13997332255',250,'Mau Cliente','2025-05-10 22:28:23'),(36,'Joana Ribeiro','1970-07-15','20607881020','joana.ribeiro@email.com',150000.00,'RJ','21988887777',500,'Bom Cliente','2025-05-11 05:25:07'),(37,'Carlos Tavares','1985-02-20','59849431075','carlos.tavares@email.com',130000.00,'MG','31977776666',450,'Bom Cliente','2025-05-11 05:25:32'),(38,'Ana Costa','1990-06-10','12170104089','ana.costa@email.com',90000.00,'SP','11966665555',350,'Cliente Regular','2025-05-11 05:26:09'),(39,'Diego Matos','2003-11-05','89463330062','diego.matos@email.com',130000.00,'RS','51944443333',350,'Cliente Regular','2025-05-11 05:26:26'),(40,'Julia Mendes','2005-12-25','16770166046','julia.mendes@email.com',30000.00,'PR','41922221111',150,'Mau Cliente','2025-05-11 05:26:41'),(41,'Fernando Lima','1975-03-30','65217248009','fernando.lima@email.com',40000.00,'PE','81911112222',300,'Cliente Regular','2025-05-11 05:26:57');
/*!40000 ALTER TABLE `clientes` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-11  5:32:58
