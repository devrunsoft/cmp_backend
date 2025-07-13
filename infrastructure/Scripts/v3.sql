
CREATE TABLE `RequestTerminate` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `InvoiceNumber` varchar(300) NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `ContractId` bigint NOT NULL,
  `Message` varchar(600) NOT NULL,
  `Status` varchar(200) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `RequestTerminateNumber` varchar(300) NOT NULL,
  `RequestTerminateStatus` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE `LocationDateTime` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `DayName` varchar(100) NOT NULL,
  `CompanyId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `FromTime` bigint NOT NULL,
  `ToTime` bigint NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- Add BillingInformationId to InvoiceSource
ALTER TABLE InvoiceSource ADD COLUMN BillingInformationId BIGINT NOT NULL;

-- Add OperationalAddressId to InvoiceSource
ALTER TABLE InvoiceSource ADD COLUMN OperationalAddressId BIGINT NOT NULL;

-- Add BillingInformationId to Invoice
ALTER TABLE Invoice ADD COLUMN BillingInformationId BIGINT NOT NULL;

-- Add CorporateAddress to Company
ALTER TABLE Company ADD COLUMN CorporateAddress VARCHAR(3000) NOT NULL DEFAULT '';
