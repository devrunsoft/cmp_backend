-- CmpAppDevelop.Admin definition

CREATE TABLE `Admin` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Email` varchar(300) NOT NULL,
  `IsActive` bit(1) DEFAULT b'1',
  `PersonId` char(36) NOT NULL,
  `Password` varchar(300) NOT NULL,
  `Role` varchar(100) DEFAULT NULL,
  `TwoFactor` tinyint(1) NOT NULL DEFAULT '0',
  `Code` varchar(100) DEFAULT NULL,
  `CodeTime` datetime DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.AdminMenuAccess definition

CREATE TABLE `AdminMenuAccess` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `MenuId` bigint NOT NULL,
  `AdminId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=600 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.AppInformation definition

CREATE TABLE `AppInformation` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyTitle` varchar(200) NOT NULL,
  `CompanyIcon` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Sign` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CompanyAddress` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CompanyPhoneNumber` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CompanyCeoLastName` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CompanyCeoFirstName` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CompanyEmail` varchar(400) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `StripeApikey` varchar(2000) NOT NULL DEFAULT '',
  `StripePaymentMethodConfiguration` varchar(2000) NOT NULL DEFAULT '',
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.AppLog definition

CREATE TABLE `AppLog` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `PersonId` char(36) NOT NULL,
  `FullName` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LogType` varchar(100) NOT NULL,
  `Action` varchar(700) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14473 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.BaseServiceAppointment definition

CREATE TABLE `BaseServiceAppointment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ServiceTypeId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `ServicePriceCrmId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` varchar(100) NOT NULL,
  `ServiceCrmId` varchar(200) DEFAULT NULL,
  `InvoiceId` bigint DEFAULT NULL,
  `IsEmegency` bit(1) NOT NULL,
  `Qty` int NOT NULL,
  `Amount` double NOT NULL,
  `ProductPriceId` bigint NOT NULL,
  `ProductId` bigint NOT NULL,
  `StartDate` datetime NOT NULL,
  `ProviderId` bigint DEFAULT NULL,
  `Subsidy` double NOT NULL DEFAULT '0',
  `DayOfWeek` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FromHour` int NOT NULL DEFAULT '480',
  `ToHour` int NOT NULL DEFAULT '1080',
  `TotalAmount` decimal(10,0) DEFAULT NULL,
  `CancelBy` varchar(100) DEFAULT NULL,
  `FactQty` int DEFAULT NULL,
  `ScaduleDate` datetime DEFAULT NULL,
  `RequestId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=701 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.BillingInformation definition

CREATE TABLE `BillingInformation` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CardholderName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CardNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Expiry` int DEFAULT NULL,
  `CVC` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Address` varchar(255) NOT NULL,
  `City` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `State` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ZIPCode` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsPaypal` tinyint(1) DEFAULT NULL,
  `CompanyId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.BillingInformationProvider definition

CREATE TABLE `BillingInformationProvider` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Address` varchar(1000) NOT NULL,
  `City` varchar(200) DEFAULT NULL,
  `State` varchar(200) DEFAULT NULL,
  `ZIPCode` varchar(200) DEFAULT NULL,
  `ProviderId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.BusinessType definition

CREATE TABLE `BusinessType` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(1000) DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Capacity definition

CREATE TABLE `Capacity` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Qty` int NOT NULL,
  `ServiceType` int NOT NULL,
  `Enable` tinyint(1) NOT NULL DEFAULT '1',
  `Order` int NOT NULL DEFAULT '1',
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatClientSession definition

CREATE TABLE `ChatClientSession` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ClientId` bigint NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ClosedAt` datetime DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatCommonMessage definition

CREATE TABLE `ChatCommonMessage` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ChatCommonSessionId` bigint NOT NULL,
  `SenderType` varchar(100) NOT NULL,
  `SenderId` bigint NOT NULL,
  `Content` text NOT NULL,
  `SentAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsInternalNote` tinyint(1) NOT NULL DEFAULT '0',
  `IsSeen` tinyint(1) NOT NULL DEFAULT '0',
  `Type` varchar(100) NOT NULL,
  `FileUrl` varchar(2000) DEFAULT NULL,
  `FileThumbnailUrl` varchar(2000) DEFAULT NULL,
  `FileExtension` varchar(200) DEFAULT NULL,
  `FileSize` bigint DEFAULT NULL,
  `DurationSeconds` double DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  `PersonId` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=168 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatCommonMessageNote definition

CREATE TABLE `ChatCommonMessageNote` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `MessageNoteType` varchar(100) NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  `Payload` longtext,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatCommonSession definition

CREATE TABLE `ChatCommonSession` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ParticipantId` bigint NOT NULL,
  `ParticipantType` varchar(100) NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ClosedAt` datetime DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `IsDelete` datetime DEFAULT NULL,
  `PersonId` char(36) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatMention definition

CREATE TABLE `ChatMention` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ChatMessageId` bigint NOT NULL,
  `MentionedType` varchar(100) NOT NULL,
  `MentionedId` bigint NOT NULL,
  `ClientId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatMessage definition

CREATE TABLE `ChatMessage` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ChatSessionId` bigint NOT NULL,
  `SenderType` varchar(100) NOT NULL,
  `SenderId` bigint NOT NULL,
  `Content` text NOT NULL,
  `SentAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsInternalNote` tinyint(1) NOT NULL DEFAULT '0',
  `IsSeen` tinyint(1) NOT NULL DEFAULT '0',
  `Type` varchar(100) NOT NULL,
  `ClientId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `FileUrl` varchar(2000) DEFAULT NULL,
  `FileThumbnailUrl` varchar(2000) DEFAULT NULL,
  `FileExtension` varchar(200) DEFAULT NULL,
  `FileSize` bigint DEFAULT NULL,
  `DurationSeconds` double DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=76 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatMessageManualNote definition

CREATE TABLE `ChatMessageManualNote` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ChatSessionId` bigint NOT NULL,
  `SenderId` bigint NOT NULL,
  `Content` text NOT NULL,
  `SentAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ClientId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `Type` varchar(100) NOT NULL,
  `FileUrl` varchar(2000) DEFAULT NULL,
  `FileExtension` varchar(200) DEFAULT NULL,
  `FileThumbnailUrl` varchar(2000) DEFAULT NULL,
  `FileSize` bigint DEFAULT NULL,
  `DurationSeconds` double DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=460 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatMessageNote definition

CREATE TABLE `ChatMessageNote` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `MessageNoteType` varchar(100) NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  `Payload` longtext,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=76 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatNotification definition

CREATE TABLE `ChatNotification` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ChatMentionId` bigint NOT NULL,
  `IsSeen` tinyint(1) NOT NULL DEFAULT '0',
  `NotifiedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatSession definition

CREATE TABLE `ChatSession` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ClientId` bigint NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ClosedAt` datetime DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL DEFAULT '1',
  `OperationalAddressId` bigint NOT NULL,
  `ChatClientSessionId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Company definition

CREATE TABLE `Company` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyName` varchar(1500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrimaryFirstName` varchar(2000) NOT NULL,
  `PrimaryLastName` varchar(2000) NOT NULL,
  `PrimaryPhonNumber` varchar(300) NOT NULL,
  `BusinessEmail` varchar(255) DEFAULT NULL,
  `Position` varchar(255) DEFAULT NULL,
  `SecondaryFirstName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SecondaryLastName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SecondaryPhoneNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ReferredBy` varchar(255) NOT NULL,
  `AccountNumber` varchar(50) NOT NULL,
  `Password` varchar(255) DEFAULT NULL,
  `Type` int NOT NULL,
  `Registered` tinyint(1) NOT NULL,
  `Accepted` tinyint(1) NOT NULL DEFAULT '0',
  `ActivationLink` char(36) DEFAULT NULL,
  `ProfilePicture` varchar(300) DEFAULT NULL,
  `PersonId` char(36) DEFAULT NULL,
  `Status` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'Approved',
  `CorporateAddress` varchar(3000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '',
  `IsDelete` datetime DEFAULT NULL,
  `Username` varchar(300) DEFAULT NULL,
  `CreateAt` datetime DEFAULT NULL,
  `EmailChangeCode` varchar(100) DEFAULT NULL,
  `PendingEmail` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2053 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.CompanyContract definition

CREATE TABLE `CompanyContract` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Content` longtext NOT NULL,
  `ContractId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `RequestId` bigint NOT NULL,
  `Status` varchar(100) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `Sign` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AdminSign` varchar(200) DEFAULT NULL,
  `ClientSignDate` datetime DEFAULT NULL,
  `ContractNumber` varchar(300) NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=147 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Contract definition

CREATE TABLE `Contract` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Title` varchar(100) NOT NULL,
  `Content` longtext NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `Active` tinyint(1) NOT NULL,
  `IsDefault` tinyint(1) NOT NULL DEFAULT '0',
  `IsDelete` datetime DEFAULT NULL,
  `Type` varchar(100) DEFAULT 'Company',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.DocumentSubmission definition

CREATE TABLE `DocumentSubmission` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `BusinessLicense` varchar(255) NOT NULL,
  `HealthDepartmentCertificate` varchar(255) NOT NULL,
  `CompanyId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Driver definition

CREATE TABLE `Driver` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `License` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `LicenseExp` datetime DEFAULT NULL,
  `BackgroundCheck` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `BackgroundCheckExp` datetime DEFAULT NULL,
  `ProfilePhoto` varchar(600) DEFAULT NULL,
  `ProviderId` bigint DEFAULT NULL,
  `Email` varchar(500) NOT NULL,
  `Password` varchar(500) NOT NULL,
  `PersonId` char(36) DEFAULT NULL,
  `Status` varchar(100) NOT NULL DEFAULT 'Approved',
  `TwoFactor` tinyint(1) NOT NULL DEFAULT '0',
  `IsDelete` datetime DEFAULT NULL,
  `ActivationLink` char(36) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.DriverManifest definition

CREATE TABLE `DriverManifest` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ManifestId` bigint NOT NULL,
  `DriverId` bigint NOT NULL,
  `ProviderId` bigint NOT NULL,
  `CreateAt` datetime NOT NULL,
  `RemoveAt` datetime DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.GoHighLevel definition

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
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Invoice definition

CREATE TABLE `Invoice` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `InvoiceCrmId` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` varchar(100) NOT NULL,
  `Link` varchar(500) DEFAULT NULL,
  `CompanyId` bigint NOT NULL,
  `InvoiceId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Amount` double NOT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `ProviderId` bigint DEFAULT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `Address` varchar(700) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SendDate` datetime DEFAULT NULL,
  `ContractId` bigint DEFAULT NULL,
  `PaymentStatus` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'Draft',
  `Comment` longtext,
  `InvoiceNumber` varchar(300) NOT NULL,
  `RequestNumber` varchar(300) NOT NULL,
  `BillingInformationId` bigint NOT NULL,
  `Type` varchar(100) NOT NULL,
  `RequestId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=422 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.InvoiceProduct definition

CREATE TABLE `InvoiceProduct` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `InvoiceId` bigint NOT NULL,
  `ProductPriceId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=330 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.InvoiceServiceAppointment definition

CREATE TABLE `InvoiceServiceAppointment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `InvoiceId` bigint NOT NULL,
  `BaseServiceAppointmentId` varchar(100) DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.InvoiceSource definition

CREATE TABLE `InvoiceSource` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `InvoiceId` varchar(200) NOT NULL,
  `CompanyId` bigint NOT NULL,
  `CreatedAt` date NOT NULL,
  `BillingInformationId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=338 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.LocationCompany definition

CREATE TABLE `LocationCompany` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Lat` double NOT NULL,
  `Long` double NOT NULL,
  `Capacity` int NOT NULL,
  `Comment` varchar(255) NOT NULL,
  `PrimaryFirstName` varchar(255) DEFAULT NULL,
  `PrimaryLastName` varchar(255) DEFAULT NULL,
  `PrimaryPhonNumber` varchar(50) DEFAULT NULL,
  `Type` int NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `CapacityId` bigint NOT NULL,
  `Address` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '',
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=214 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.LocationDateTime definition

CREATE TABLE `LocationDateTime` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `DayName` varchar(100) NOT NULL,
  `CompanyId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `FromTime` bigint NOT NULL,
  `ToTime` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Manifest definition

CREATE TABLE `Manifest` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Status` varchar(100) NOT NULL,
  `RequestId` bigint NOT NULL,
  `ProviderId` bigint DEFAULT NULL,
  `Content` longtext NOT NULL,
  `Comment` varchar(1000) DEFAULT NULL,
  `BeforeImages` varchar(300) DEFAULT NULL,
  `AfterImages` varchar(300) DEFAULT NULL,
  `StartTime` datetime DEFAULT NULL,
  `FinishTime` datetime DEFAULT NULL,
  `IsEdited` tinyint(1) DEFAULT NULL,
  `ServiceDateTime` datetime DEFAULT NULL,
  `ContractId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `ManifestNumber` varchar(300) NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `DoingStartTime` datetime DEFAULT NULL,
  `PreferredDate` datetime NOT NULL,
  `ServiceAppointmentLocationId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=178 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ManifestGreaseServiceDetail definition

CREATE TABLE `ManifestGreaseServiceDetail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ServiceAppointmentLocationId` bigint NOT NULL,
  `GreasePercentage` decimal(10,0) NOT NULL,
  `WaterPercentage` decimal(10,0) NOT NULL,
  `SolidsPercentage` decimal(10,0) NOT NULL,
  `OilPercentage` decimal(10,0) NOT NULL,
  `CodAmount` decimal(10,0) DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Menu definition

CREATE TABLE `Menu` (
  `Id` bigint NOT NULL,
  `Parent` bigint DEFAULT NULL,
  `Name` varchar(100) NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.OperationalAddress definition

CREATE TABLE `OperationalAddress` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint NOT NULL,
  `Address` varchar(3000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CrossStreet` varchar(1000) DEFAULT NULL,
  `County` varchar(1000) DEFAULT NULL,
  `LocationPhone` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `BusinessId` bigint DEFAULT NULL,
  `FirstName` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastName` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Lat` float DEFAULT NULL,
  `Long` float DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  `Username` varchar(300) DEFAULT NULL,
  `Password` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2837 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Payment definition

CREATE TABLE `Payment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Amount` bigint NOT NULL,
  `CheckoutSessionId` varchar(500) NOT NULL,
  `CreateAt` datetime NOT NULL,
  `CompanyId` bigint NOT NULL,
  `InvoiceId` bigint NOT NULL,
  `Status` varchar(200) NOT NULL,
  `Content` longtext NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Person definition

CREATE TABLE `Person` (
  `Id` char(36) NOT NULL,
  `FirstName` varchar(2000) NOT NULL,
  `LastName` varchar(2000) NOT NULL,
  `IsDelete` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Product definition

CREATE TABLE `Product` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Type` int DEFAULT NULL,
  `CollectionIds` varchar(2000) DEFAULT NULL,
  `ServiceCrmId` varchar(300) DEFAULT NULL,
  `Enable` tinyint(1) NOT NULL DEFAULT '1',
  `ProductType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ServiceType` int NOT NULL,
  `IsEmergency` tinyint(1) DEFAULT '0',
  `Order` int NOT NULL DEFAULT '1',
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ProductPrice definition

CREATE TABLE `ProductPrice` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductId` bigint NOT NULL,
  `Amount` double NOT NULL,
  `BillingPeriod` bigint NOT NULL,
  `NumberofPayments` int NOT NULL,
  `SetupFee` float NOT NULL,
  `ServiceCrmId` varchar(300) DEFAULT NULL,
  `ServicePriceCrmId` varchar(300) DEFAULT NULL,
  `Enable` tinyint(1) NOT NULL DEFAULT '1',
  `MinimumAmount` double NOT NULL DEFAULT '0',
  `Order` int NOT NULL DEFAULT '1',
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=123 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Provider definition

CREATE TABLE `Provider` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(300) NOT NULL,
  `Status` int NOT NULL,
  `Lat` float DEFAULT NULL,
  `Long` float DEFAULT NULL,
  `City` varchar(200) DEFAULT NULL,
  `Address` varchar(500) DEFAULT NULL,
  `County` varchar(200) DEFAULT NULL,
  `Rating` float DEFAULT NULL,
  `BusinessLicense` varchar(600) DEFAULT NULL,
  `BusinessLicenseExp` datetime DEFAULT NULL,
  `HealthDepartmentPermit` varchar(700) DEFAULT NULL,
  `HealthDepartmentPermitExp` datetime DEFAULT NULL,
  `WasteHaulerPermit` varchar(700) DEFAULT NULL,
  `EPACompliance` varchar(700) DEFAULT NULL,
  `EPAComplianceExp` datetime DEFAULT NULL,
  `Insurance` varchar(700) DEFAULT NULL,
  `InsuranceExp` varchar(700) DEFAULT NULL,
  `AreaLocation` double DEFAULT NULL,
  `Email` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PhoneNumber` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` varchar(300) DEFAULT NULL,
  `RegistrationStatus` varchar(100) DEFAULT NULL,
  `ActivationLink` char(36) DEFAULT NULL,
  `HasLogin` tinyint(1) DEFAULT '0',
  `ManagerFirstName` varchar(300) DEFAULT NULL,
  `ManagerLastName` varchar(300) DEFAULT NULL,
  `ManagerPhoneNumber` varchar(300) DEFAULT NULL,
  `PersonId` char(36) DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ProviderContract definition

CREATE TABLE `ProviderContract` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Content` longtext NOT NULL,
  `ContractId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `ProviderId` bigint NOT NULL,
  `ManifestIsd` varchar(500) NOT NULL,
  `RequestId` bigint DEFAULT NULL,
  `Sign` varchar(200) DEFAULT NULL,
  `AdminSign` varchar(200) DEFAULT NULL,
  `ClientSignDate` datetime DEFAULT NULL,
  `AdminSignDate` datetime DEFAULT NULL,
  `Status` varchar(300) NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `ContractNumber` varchar(300) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ProviderDriver definition

CREATE TABLE `ProviderDriver` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProviderId` bigint NOT NULL,
  `DriverId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  `IsDefault` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ProviderService definition

CREATE TABLE `ProviderService` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProviderId` bigint NOT NULL,
  `ProductId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=195 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ProviderServiceAssignment definition

CREATE TABLE `ProviderServiceAssignment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProviderId` bigint NOT NULL,
  `InvoiceId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `Status` int NOT NULL,
  `AssignTime` datetime NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ProviderVehicle definition

CREATE TABLE `ProviderVehicle` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProviderId` bigint NOT NULL,
  `VehicleId` bigint NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Request definition

CREATE TABLE `Request` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `InvoiceCrmId` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` varchar(100) NOT NULL,
  `Link` varchar(500) DEFAULT NULL,
  `CompanyId` bigint NOT NULL,
  `Amount` double NOT NULL,
  `CreatedAt` datetime DEFAULT NULL,
  `ProviderId` bigint DEFAULT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `Address` varchar(700) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SendDate` datetime DEFAULT NULL,
  `ContractId` bigint DEFAULT NULL,
  `PaymentStatus` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'Draft',
  `Comment` longtext,
  `RequestNumber` varchar(300) NOT NULL,
  `BillingInformationId` bigint NOT NULL,
  `Type` varchar(50) NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=559 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.RequestTerminate definition

CREATE TABLE `RequestTerminate` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `RequestId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `ContractId` bigint NOT NULL,
  `Message` varchar(600) NOT NULL,
  `Status` varchar(200) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `RequestTerminateNumber` varchar(300) NOT NULL,
  `RequestTerminateStatus` varchar(100) DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Route definition

CREATE TABLE `Route` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Date` datetime DEFAULT NULL,
  `Name` varchar(400) NOT NULL,
  `CreateAt` datetime NOT NULL,
  `DriverId` bigint NOT NULL,
  `Status` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderId` bigint NOT NULL,
  `VehicleId` bigint DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.RouteServiceAppointmentLocation definition

CREATE TABLE `RouteServiceAppointmentLocation` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ServiceAppointmentLocationId` bigint NOT NULL,
  `RouteId` bigint NOT NULL,
  `ManifestId` bigint NOT NULL,
  `ManifestNumber` varchar(300) NOT NULL DEFAULT '',
  `StartedAt` datetime DEFAULT NULL,
  `Comment` longtext,
  `InvoiceId` bigint DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ServiceAppointment definition

CREATE TABLE `ServiceAppointment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `FrequencyType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=701 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ServiceAppointmentEmergency definition

CREATE TABLE `ServiceAppointmentEmergency` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `FrequencyType` varchar(300) DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=635 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ServiceAppointmentLocation definition

CREATE TABLE `ServiceAppointmentLocation` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ServiceAppointmentId` bigint NOT NULL,
  `LocationCompanyId` bigint NOT NULL,
  `Qty` int DEFAULT '0',
  `FactQty` int DEFAULT NULL,
  `OilQuality` varchar(200) DEFAULT NULL,
  `Status` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'Draft',
  `FinishDate` datetime DEFAULT NULL,
  `Comment` longtext,
  `InvoiceId` bigint DEFAULT NULL,
  `StartedAt` datetime DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=696 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ServiceAppointmentLocationFile definition

CREATE TABLE `ServiceAppointmentLocationFile` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Link` varchar(2000) NOT NULL,
  `DriverId` bigint NOT NULL,
  `ServiceAppointmentLocationId` bigint NOT NULL,
  `Status` varchar(300) NOT NULL,
  `RouteId` bigint NOT NULL,
  `ProviderId` bigint DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=201 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ServiceArea definition

CREATE TABLE `ServiceArea` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `City` varchar(300) NOT NULL,
  `State` varchar(300) NOT NULL,
  `ServiceAreaType` varchar(100) NOT NULL,
  `ProviderId` bigint NOT NULL,
  `Address` longtext NOT NULL,
  `CreateAt` datetime NOT NULL,
  `Active` tinyint(1) NOT NULL DEFAULT '1',
  `Lat` double DEFAULT NULL,
  `Lng` double DEFAULT NULL,
  `Radius` double DEFAULT NULL,
  `GeoJson` longtext,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ShoppingCard definition

CREATE TABLE `ShoppingCard` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ServicePriceCrmId` varchar(200) NOT NULL,
  `ServiceCrmId` varchar(200) NOT NULL,
  `CompanyId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `FrequencyType` varchar(100) DEFAULT NULL,
  `StartDate` date DEFAULT NULL,
  `Name` varchar(300) DEFAULT NULL,
  `PriceName` varchar(300) DEFAULT NULL,
  `AddressName` varchar(300) DEFAULT NULL,
  `ServiceKind` bigint NOT NULL,
  `ServiceId` int NOT NULL,
  `LocationCompanyIds` varchar(100) DEFAULT NULL,
  `Address` varchar(600) DEFAULT NULL,
  `Qty` int NOT NULL,
  `ProductPriceId` bigint DEFAULT NULL,
  `ProductId` bigint DEFAULT NULL,
  `DayOfWeek` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FromHour` int NOT NULL DEFAULT '480',
  `ToHour` int NOT NULL DEFAULT '1080',
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=483 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.TermsConditions definition

CREATE TABLE `TermsConditions` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CreateAt` datetime NOT NULL,
  `Enable` tinyint(1) NOT NULL,
  `Content` longtext NOT NULL,
  `Type` varchar(50) NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.Vehicle definition

CREATE TABLE `Vehicle` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `VehicleRegistration` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `VehicleRegistrationExp` datetime DEFAULT NULL,
  `VehicleInsurance` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `VehicleInsuranceExp` datetime DEFAULT NULL,
  `InspectionReport` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `InspectionReportExp` datetime DEFAULT NULL,
  `Picture` varchar(600) DEFAULT NULL,
  `Capacity` int NOT NULL,
  `Weight` float NOT NULL,
  `MeasurementCertificate` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PeriodicVehicleInspections` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PeriodicVehicleInspectionsExp` datetime DEFAULT NULL,
  `ProviderId` bigint NOT NULL,
  `Name` varchar(300) NOT NULL,
  `CompartmentSize` bigint DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  `LicenseNumber` varchar(900) NOT NULL DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.VehicleCompartment definition

CREATE TABLE `VehicleCompartment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `VehicleId` bigint NOT NULL,
  `Capacity` int NOT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.VehicleService definition

CREATE TABLE `VehicleService` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `VehicleId` bigint NOT NULL,
  `VehicleServiceStatus` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Capacity` int DEFAULT NULL,
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppDevelop.ChatParticipant definition

CREATE TABLE `ChatParticipant` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ChatSessionId` bigint NOT NULL,
  `ParticipantType` varchar(100) NOT NULL,
  `ParticipantId` bigint NOT NULL,
  `JoinedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `IsRemoved` tinyint(1) NOT NULL DEFAULT '0',
  `IsDelete` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ChatSessionId` (`ChatSessionId`),
  CONSTRAINT `ChatParticipant_ibfk_1` FOREIGN KEY (`ChatSessionId`) REFERENCES `ChatSession` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;