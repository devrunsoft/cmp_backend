CREATE TABLE `Notification` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `type` varchar(100) NOT NULL,
  `Title` varchar(800) NOT NULL,
  `Body` varchar(4000) NOT NULL,
  `Seen` datetime NULL,
  `CreateAt` datetime NOT NULL,
  `Payload` longtext NULL,
  `IsDelete` datetime NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;