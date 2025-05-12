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
  `Endereco` varchar(255) NOT NULL,
  `Telefone` varchar(15) NOT NULL,
  `Score` int NOT NULL,
  `Classificacao` varchar(50) NOT NULL,
  `DataCadastro` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Cpf` (`Cpf`)
) ENGINE=InnoDB AUTO_INCREMENT=67 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clientes`
--

LOCK TABLES `clientes` WRITE;
/*!40000 ALTER TABLE `clientes` DISABLE KEYS */;
INSERT INTO `clientes` VALUES (57,'Carlos Silva','1985-03-12','42811229144','carlos.silva@example.com',85000.00,'Rua das Palmeiras, 120 - Campinas SP','11987654321',350,'Cliente Regular','2025-05-11 22:38:54'),(58,'Ana Souza','1992-08-23','30417062080','ana.souza@example.com',120000.00,'Av. Atlântica, 55 - Salvador BA','71991234567',350,'Cliente Regular','2025-05-11 22:38:54'),(59,'João Pedro','2000-01-01','10610345070','joao.pedro@example.com',40000.00,'Rua do Sol, 340 - Fortaleza CE','85999887766',150,'Mau Cliente','2025-05-11 22:38:54'),(60,'Fernanda Lima','1975-11-30','08254151070','fernanda.lima@example.com',160000.00,'Rua da Paz, 890 - Belo Horizonte MG','31988776655',500,'Bom Cliente','2025-05-11 22:38:54'),(61,'Bruno Castro','1988-05-09','39053354809','bruno.castro@example.com',65000.00,'Rua das Hortênsias, 99 - Curitiba PR','41997766554',350,'Cliente Regular','2025-05-11 22:38:54'),(62,'Juliana Alves','1997-06-18','01489666070','juliana.alves@example.com',52000.00,'Rua Dom Pedro, 10 - Porto Alegre RS','51981234567',250,'Mau Cliente','2025-05-11 22:38:54'),(63,'Marcos Dias','1980-10-22','06806449090','marcos.dias@example.com',135000.00,'Av. Principal, 444 - João Pessoa PB','83991112233',500,'Bom Cliente','2025-05-11 22:38:54'),(64,'Patrícia Gomes','1994-04-14','22645406080','patricia.gomes@example.com',72000.00,'Rua Nova, 88 - Recife PE','81996655443',350,'Cliente Regular','2025-05-11 22:38:54'),(65,'Lucas Ferreira','2002-02-02','50344230030','lucas.ferreira@example.com',35000.00,'Rua São João, 300 - Maceió AL','82992233445',150,'Mau Cliente','2025-05-11 22:38:54'),(66,'Bianca Melo','1983-12-20','35089097050','bianca.melo@example.com',98000.00,'Rua Bela Vista, 777 - Brasília DF','61991234567',350,'Cliente Regular','2025-05-11 22:38:54');
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

-- Dump completed on 2025-05-11 22:40:58
