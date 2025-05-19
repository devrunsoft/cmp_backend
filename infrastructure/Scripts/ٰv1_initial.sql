-- CmpAppLocal.Admin definition

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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO Admin (Email, IsActive, PersonId , Password , Role , TwoFactor) VALUES
('devrunsoft@gmail.com', 1 , 'e374845c-8ad0-40a2-b24b-e8e3fc697f12' , '66355366' , 'SuperAdmin' , 1);


-- CmpAppLocal.AppInformation definition

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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.AppLog definition

CREATE TABLE `AppLog` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `PersonId` char(36) NOT NULL,
  `FullName` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LogType` varchar(100) NOT NULL,
  `Action` varchar(700) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1090 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.BusinessType definition

CREATE TABLE `BusinessType` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Capacity definition

CREATE TABLE `Capacity` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `Qty` int NOT NULL,
  `ServiceType` int NOT NULL,
  `Enable` tinyint(1) NOT NULL DEFAULT '1',
  `Order` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Company definition

CREATE TABLE `Company` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyName` varchar(255) NOT NULL,
  `PrimaryFirstName` varchar(255) NOT NULL,
  `PrimaryLastName` varchar(255) NOT NULL,
  `PrimaryPhonNumber` varchar(50) NOT NULL,
  `BusinessEmail` varchar(255) NOT NULL,
  `Position` varchar(255) NOT NULL,
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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=94 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Contract definition

CREATE TABLE `Contract` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Title` varchar(100) NOT NULL,
  `Content` longtext NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `Active` tinyint(1) NOT NULL,
  `IsDefault` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.InvoiceProduct definition

CREATE TABLE `InvoiceProduct` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `InvoiceId` bigint NOT NULL,
  `ProductPriceId` bigint NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=305 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.InvoiceServiceAppointment definition

CREATE TABLE `InvoiceServiceAppointment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `InvoiceId` bigint NOT NULL,
  `BaseServiceAppointmentId` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.InvoiceSource definition

CREATE TABLE `InvoiceSource` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `InvoiceId` varchar(200) NOT NULL,
  `CompanyId` bigint NOT NULL,
  `CreatedAt` date NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=298 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.LocationCompany definition

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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=193 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Menu definition

CREATE TABLE `Menu` (
  `Id` bigint NOT NULL,
  `Parent` bigint DEFAULT NULL,
  `Name` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


INSERT INTO Menu (Id, Parent, Name) VALUES
(1, NULL, 'Dashboard'),
(2, NULL, 'Assigned Services'),
(3, NULL, 'Client'),
(4, NULL, 'Invoices'),
(5, NULL, 'Product'),
(6, NULL, 'Clients Detail'),
(7, NULL, 'Client Dashboard'),
(8, NULL, 'Client Services'),
(9, NULL, 'Client Enroll Service'),
(10, NULL, 'Client Shopping Card'),
(11, NULL, 'Provider Detail'),
(12, NULL, 'Product Price'),
(13, NULL, 'Config'),
(14, NULL, 'Capacity'),
(15, NULL, 'Invoice Detail'),
(16, NULL, 'Invoice Assign'),
(17, NULL, 'Providers'),
(18, NULL, 'Admin Management'),
(19, NULL, 'Contract'),
(20, NULL, 'Company Contract'),
(21, NULL, 'Information'),
(22, NULL, 'Access Management'),
(24, NULL, 'Requests'),
(25, NULL, 'Manifests'),
(26, NULL, 'Manifest Draft'),
(29, NULL, 'TermsConditions'),
(30, NULL, 'Transactions'),
(31, NULL, 'Logs');



-- CmpAppLocal.Person definition

CREATE TABLE `Person` (
  `Id` char(36) NOT NULL,
  `FirstName` varchar(300) NOT NULL,
  `LastName` varchar(300) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO Person (Id, FirstName, LastName) VALUES
('e374845c-8ad0-40a2-b24b-e8e3fc697f12', 'system', 'admin');

-- CmpAppLocal.Product definition

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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Provider definition

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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.ProviderServiceAssignment definition

CREATE TABLE `ProviderServiceAssignment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProviderId` bigint NOT NULL,
  `InvoiceId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `Status` int NOT NULL,
  `AssignTime` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.ServiceAppointment definition

CREATE TABLE `ServiceAppointment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `FrequencyType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=544 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.ServiceAppointmentEmergency definition

CREATE TABLE `ServiceAppointmentEmergency` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `FrequencyType` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=237 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.ServiceAppointmentLocation definition

CREATE TABLE `ServiceAppointmentLocation` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ServiceAppointmentId` bigint NOT NULL,
  `LocationCompanyId` bigint NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=530 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.ServiceArea definition

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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.TermsConditions definition

CREATE TABLE `TermsConditions` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CreateAt` datetime NOT NULL,
  `Enable` tinyint(1) NOT NULL,
  `Content` longtext NOT NULL,
  `Type` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.AdminMenuAccess definition

CREATE TABLE `AdminMenuAccess` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `MenuId` bigint NOT NULL,
  `AdminId` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `AdminMenuAccess_Menu_FK` (`MenuId`),
  KEY `AdminMenuAccess_Admin_FK` (`AdminId`),
  CONSTRAINT `AdminMenuAccess_Admin_FK` FOREIGN KEY (`AdminId`) REFERENCES `Admin` (`Id`),
  CONSTRAINT `AdminMenuAccess_Menu_FK` FOREIGN KEY (`MenuId`) REFERENCES `Menu` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.BillingInformation definition

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
  PRIMARY KEY (`Id`),
  KEY `BillingInformation_Company_FK` (`CompanyId`),
  CONSTRAINT `BillingInformation_Company_FK` FOREIGN KEY (`CompanyId`) REFERENCES `Company` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.BillingInformationProvider definition

CREATE TABLE `BillingInformationProvider` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Address` varchar(1000) NOT NULL,
  `City` varchar(200) DEFAULT NULL,
  `State` varchar(200) DEFAULT NULL,
  `ZIPCode` varchar(200) DEFAULT NULL,
  `ProviderId` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `BillingInformationProvider_Provider_FK` (`ProviderId`),
  CONSTRAINT `BillingInformationProvider_Provider_FK` FOREIGN KEY (`ProviderId`) REFERENCES `Provider` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.DocumentSubmission definition

CREATE TABLE `DocumentSubmission` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `BusinessLicense` varchar(255) NOT NULL,
  `HealthDepartmentCertificate` varchar(255) NOT NULL,
  `CompanyId` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `DocumentSubmission_Company_FK` (`CompanyId`),
  CONSTRAINT `DocumentSubmission_Company_FK` FOREIGN KEY (`CompanyId`) REFERENCES `Company` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Driver definition

CREATE TABLE `Driver` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(200) NOT NULL,
  `LastName` varchar(200) NOT NULL,
  `License` varchar(600) NOT NULL,
  `LicenseExp` datetime NOT NULL,
  `BackgroundCheck` varchar(600) NOT NULL,
  `BackgroundCheckExp` datetime NOT NULL,
  `ProfilePhoto` varchar(600) DEFAULT NULL,
  `ProviderId` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Driver_Provider_FK` (`ProviderId`),
  CONSTRAINT `Driver_Provider_FK` FOREIGN KEY (`ProviderId`) REFERENCES `Provider` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Payment definition

CREATE TABLE `Payment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Amount` bigint NOT NULL,
  `CheckoutSessionId` varchar(500) NOT NULL,
  `CreateAt` datetime NOT NULL,
  `CompanyId` bigint NOT NULL,
  `InvoiceId` bigint NOT NULL,
  `Status` varchar(200) NOT NULL,
  `Content` longtext NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Payment_Company_FK` (`CompanyId`),
  KEY `Payment_Driver_FK` (`InvoiceId`),
  CONSTRAINT `Payment_Company_FK` FOREIGN KEY (`CompanyId`) REFERENCES `Company` (`Id`),
  CONSTRAINT `Payment_Driver_FK` FOREIGN KEY (`InvoiceId`) REFERENCES `Driver` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.ProductPrice definition

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
  PRIMARY KEY (`Id`),
  KEY `ProductPrice_ServiceArea_FK` (`ProductId`),
  CONSTRAINT `ProductPrice_ServiceArea_FK` FOREIGN KEY (`ProductId`) REFERENCES `ServiceArea` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=123 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.ProviderService definition

CREATE TABLE `ProviderService` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProviderId` bigint NOT NULL,
  `ProductId` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProviderService_Provider_FK` (`ProviderId`),
  KEY `ProviderService_Product_FK` (`ProductId`),
  CONSTRAINT `ProviderService_Product_FK` FOREIGN KEY (`ProductId`) REFERENCES `Product` (`Id`),
  CONSTRAINT `ProviderService_Provider_FK` FOREIGN KEY (`ProviderId`) REFERENCES `Provider` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=138 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.ShoppingCard definition

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
  PRIMARY KEY (`Id`),
  KEY `ShoppingCard_Company_FK` (`CompanyId`),
  KEY `ShoppingCard_Product_FK` (`ProductId`),
  KEY `ShoppingCard_ProductPrice_FK` (`ProductPriceId`),
  CONSTRAINT `ShoppingCard_Company_FK` FOREIGN KEY (`CompanyId`) REFERENCES `Company` (`Id`),
  CONSTRAINT `ShoppingCard_Product_FK` FOREIGN KEY (`ProductId`) REFERENCES `Product` (`Id`),
  CONSTRAINT `ShoppingCard_ProductPrice_FK` FOREIGN KEY (`ProductPriceId`) REFERENCES `ProductPrice` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=432 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Vehicle definition

CREATE TABLE `Vehicle` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `VehicleRegistration` varchar(600) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `VehicleRegistrationExp` datetime NOT NULL,
  `VehicleInsurance` varchar(600) NOT NULL,
  `VehicleInsuranceExp` datetime NOT NULL,
  `InspectionReport` varchar(600) NOT NULL,
  `InspectionReportExp` datetime NOT NULL,
  `Picture` varchar(600) DEFAULT NULL,
  `Capacity` int NOT NULL,
  `Weight` float NOT NULL,
  `MeasurementCertificate` varchar(600) NOT NULL,
  `PeriodicVehicleInspections` varchar(600) NOT NULL,
  `PeriodicVehicleInspectionsExp` datetime NOT NULL,
  `ProviderId` bigint NOT NULL,
  `Name` varchar(300) NOT NULL,
  `CompartmentSize` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Vehicle_Provider_FK` (`ProviderId`),
  CONSTRAINT `Vehicle_Provider_FK` FOREIGN KEY (`ProviderId`) REFERENCES `Provider` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.VehicleCompartment definition

CREATE TABLE `VehicleCompartment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `VehicleId` bigint NOT NULL,
  `Capacity` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `VehicleCompartment_Vehicle_FK` (`VehicleId`),
  CONSTRAINT `VehicleCompartment_Vehicle_FK` FOREIGN KEY (`VehicleId`) REFERENCES `Vehicle` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.VehicleService definition

CREATE TABLE `VehicleService` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `VehicleId` bigint NOT NULL,
  `VehicleServiceStatus` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Capacity` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `VehicleService_Vehicle_FK` (`VehicleId`),
  CONSTRAINT `VehicleService_Vehicle_FK` FOREIGN KEY (`VehicleId`) REFERENCES `Vehicle` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.BaseServiceAppointment definition

CREATE TABLE `BaseServiceAppointment` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ServiceTypeId` bigint NOT NULL COMMENT 'ProductCollection Enum',
  `CompanyId` bigint NOT NULL,
  `OperationalAddressId` bigint NOT NULL,
  `ServicePriceCrmId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Status` varchar(100) NOT NULL,
  `ServiceCrmId` varchar(200) DEFAULT NULL,
  `InvoiceId` bigint NOT NULL,
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
  `OilQuality` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `BaseServiceAppointment_Company_FK` (`CompanyId`),
  KEY `BaseServiceAppointment_ProviderServiceAssignment_FK` (`OperationalAddressId`),
  KEY `BaseServiceAppointment_Invoice_FK` (`InvoiceId`),
  KEY `BaseServiceAppointment_Product_FK` (`ProductId`),
  KEY `BaseServiceAppointment_ProductPrice_FK` (`ProductPriceId`),
  KEY `BaseServiceAppointment_Provider_FK` (`ProviderId`),
  CONSTRAINT `BaseServiceAppointment_Company_FK` FOREIGN KEY (`CompanyId`) REFERENCES `Company` (`Id`),
  CONSTRAINT `BaseServiceAppointment_Invoice_FK` FOREIGN KEY (`InvoiceId`) REFERENCES `Invoice` (`Id`),
  CONSTRAINT `BaseServiceAppointment_Product_FK` FOREIGN KEY (`ProductId`) REFERENCES `Product` (`Id`),
  CONSTRAINT `BaseServiceAppointment_ProductPrice_FK` FOREIGN KEY (`ProductPriceId`) REFERENCES `ProductPrice` (`Id`),
  CONSTRAINT `BaseServiceAppointment_Provider_FK` FOREIGN KEY (`ProviderId`) REFERENCES `Provider` (`Id`),
  CONSTRAINT `BaseServiceAppointment_ProviderServiceAssignment_FK` FOREIGN KEY (`OperationalAddressId`) REFERENCES `ProviderServiceAssignment` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=547 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.CompanyContract definition

CREATE TABLE `CompanyContract` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Content` longtext NOT NULL,
  `ContractId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `InvoiceId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` varchar(100) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `Sign` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AdminSign` varchar(200) DEFAULT NULL,
  `ClientSignDate` datetime DEFAULT NULL,
  `ContractNumber` varchar(300) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `CompanyContract_Manifest_FK` (`ContractId`),
  KEY `CompanyContract_Company_FK` (`CompanyId`),
  CONSTRAINT `CompanyContract_Company_FK` FOREIGN KEY (`CompanyId`) REFERENCES `Company` (`Id`),
  CONSTRAINT `CompanyContract_Manifest_FK` FOREIGN KEY (`ContractId`) REFERENCES `Manifest` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=130 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Invoice definition

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
  PRIMARY KEY (`Id`),
  KEY `Invoice_Company_FK` (`CompanyId`),
  KEY `Invoice_Provider_FK` (`ProviderId`),
  KEY `Invoice_OperationalAddress_FK` (`OperationalAddressId`),
  KEY `Invoice_CompanyContract_FK` (`ContractId`),
  CONSTRAINT `Invoice_Company_FK` FOREIGN KEY (`CompanyId`) REFERENCES `Company` (`Id`),
  CONSTRAINT `Invoice_CompanyContract_FK` FOREIGN KEY (`ContractId`) REFERENCES `CompanyContract` (`Id`),
  CONSTRAINT `Invoice_Menu_FK` FOREIGN KEY (`ContractId`) REFERENCES `Menu` (`Id`),
  CONSTRAINT `Invoice_OperationalAddress_FK` FOREIGN KEY (`OperationalAddressId`) REFERENCES `OperationalAddress` (`Id`),
  CONSTRAINT `Invoice_Provider_FK` FOREIGN KEY (`ProviderId`) REFERENCES `Provider` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=328 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.Manifest definition

CREATE TABLE `Manifest` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Status` varchar(100) NOT NULL,
  `InvoiceId` bigint NOT NULL,
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
  PRIMARY KEY (`Id`),
  KEY `Manifest_Invoice_FK` (`InvoiceId`),
  CONSTRAINT `Manifest_Invoice_FK` FOREIGN KEY (`InvoiceId`) REFERENCES `Invoice` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=58 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- CmpAppLocal.OperationalAddress definition

CREATE TABLE `OperationalAddress` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint NOT NULL,
  `Address` varchar(3000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CrossStreet` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `County` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LocationPhone` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `BusinessId` bigint NOT NULL,
  `FirstName` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LastName` varchar(300) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Lat` float NOT NULL,
  `Long` float NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `OperationalAddress_Manifest_FK` (`CompanyId`),
  CONSTRAINT `OperationalAddress_Manifest_FK` FOREIGN KEY (`CompanyId`) REFERENCES `Manifest` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=99 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;