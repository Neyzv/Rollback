/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 80030
Source Host           : 127.0.0.1:3306
Source Database       : rollback_auth

Target Server Type    : MYSQL
Target Server Version : 80030
File Encoding         : 65001

Date: 2023-10-09 19:39:53
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for accounts
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Login` mediumtext NOT NULL,
  `Password` mediumtext NOT NULL,
  `Nickname` text CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Ticket` mediumtext,
  `Role` int NOT NULL DEFAULT '0',
  `SecretQuestion` text NOT NULL,
  `SecretAnswer` text NOT NULL,
  `BannedDate` datetime DEFAULT NULL,
  `LastConnection` datetime DEFAULT NULL,
  `LastIP` mediumtext,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of accounts
-- ----------------------------
INSERT INTO `accounts` VALUES ('1', 'aze', '0a5b3913cbc9a9092311630e869b4442', 'Moa hehehe', 'OQVYZTDQHEHMNZIPROWJHZSVFJDXVZDQ', '4', 'Dofus ?', 'dofus', null, '2023-10-09 14:23:03', '127.0.0.1');
INSERT INTO `accounts` VALUES ('2', 'zer', 'd674a71e054c2c7a6761c366e6eb73c4', 'Moa2', 'NHRCJHEZBQZXXKWOECTHYJODKBIGRIQX', '4', 'Dofus ? ', 'dofus', null, '2023-09-19 14:24:49', '127.0.0.1');
INSERT INTO `accounts` VALUES ('4', 'ert', 'e3e84538a1b02b1cc11bf71fe3169958', 'Moa3', 'HOMVCTYWQLDPVEYPRFGOVFGMWOLNVUPS', '4', 'Dofus', 'dofus', null, '2023-08-19 15:59:19', '127.0.0.1');

-- ----------------------------
-- Table structure for accounts_gifts
-- ----------------------------
DROP TABLE IF EXISTS `accounts_gifts`;
CREATE TABLE `accounts_gifts` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AccountId` int NOT NULL,
  `Title` text NOT NULL,
  `Description` text,
  `ItemsCSV` text NOT NULL,
  `UnavailableServerIdsCSV` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;

-- ----------------------------
-- Records of accounts_gifts
-- ----------------------------

-- ----------------------------
-- Table structure for worlds
-- ----------------------------
DROP TABLE IF EXISTS `worlds`;
CREATE TABLE `worlds` (
  `Id` int NOT NULL,
  `Capacity` int NOT NULL,
  `RequiredRole` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of worlds
-- ----------------------------
INSERT INTO `worlds` VALUES ('30', '0', '0');
INSERT INTO `worlds` VALUES ('104', '0', '0');
INSERT INTO `worlds` VALUES ('111', '500', '0');
INSERT INTO `worlds` VALUES ('118', '0', '0');
INSERT INTO `worlds` VALUES ('124', '0', '0');
INSERT INTO `worlds` VALUES ('126', '0', '0');
INSERT INTO `worlds` VALUES ('670', '0', '0');
INSERT INTO `worlds` VALUES ('671', '0', '0');
INSERT INTO `worlds` VALUES ('672', '0', '0');
INSERT INTO `worlds` VALUES ('902', '0', '0');
INSERT INTO `worlds` VALUES ('903', '0', '0');
INSERT INTO `worlds` VALUES ('904', '0', '0');
INSERT INTO `worlds` VALUES ('905', '0', '0');

-- ----------------------------
-- Table structure for worlds_characters
-- ----------------------------
DROP TABLE IF EXISTS `worlds_characters`;
CREATE TABLE `worlds_characters` (
  `CharacterId` int NOT NULL,
  `AccountId` int NOT NULL,
  `WorldId` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- ----------------------------
-- Records of worlds_characters
-- ----------------------------
INSERT INTO `worlds_characters` VALUES ('1', '1', '30');
INSERT INTO `worlds_characters` VALUES ('3', '4', '30');
INSERT INTO `worlds_characters` VALUES ('5', '2', '30');
INSERT INTO `worlds_characters` VALUES ('2', '2', '30');
INSERT INTO `worlds_characters` VALUES ('6', '2', '30');
