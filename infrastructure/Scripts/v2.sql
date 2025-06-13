-- CmpAppLocal.GoHighLevel definition

CREATE TABLE `GoHighLevel` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `LocationId` varchar(300) NOT NULL,
  `Authorization` varchar(600) NOT NULL,
  `RestApi` varchar(300) NOT NULL,
  `Version` varchar(200) NOT NULL,
  `UpdateContactApi` varchar(500) NOT NULL,
  `ForgotPasswordApi` varchar(500) NOT NULL,
  `ActivationLinkApi` varchar(500) NOT NULL,
  `SendEmailApi` varchar(500) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;