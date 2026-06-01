CREATE TABLE `Tenant` (
    `Id` BIGINT NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(255) NOT NULL,
    `Slug` VARCHAR(128) NULL,
    `SubDomain` VARCHAR(255) NULL,
    `Host` VARCHAR(255) NULL,
    `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
    `WantsDispatchManagement` TINYINT(1) NOT NULL DEFAULT 0,
    `HasAdminPortal` TINYINT(1) NOT NULL DEFAULT 1,
    `HasClientPortal` TINYINT(1) NOT NULL DEFAULT 1,
    `HasProviderPortal` TINYINT(1) NOT NULL DEFAULT 1,
    `AdminCanViewAllRecords` TINYINT(1) NOT NULL DEFAULT 0,
    `AllowMainAdminAccess` TINYINT(1) NOT NULL DEFAULT 1,
    -- `AllowTenantAdminAccess` TINYINT(1) NOT NULL DEFAULT 1,
    `PrimaryColor` VARCHAR(32) NULL,
    `SecondaryColor` VARCHAR(32) NULL,
    `LogoUrl` VARCHAR(1024) NULL,
    `SupportEmail` VARCHAR(255) NULL,
    `Verified` TINYINT(1) NOT NULL DEFAULT 0,
    `ProviderId` BIGINT NOT NULL,
    `WhiteLabelRequestId` BIGINT NOT NULL,
    `IsDelete` DATETIME NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UX_Tenant_Slug` (`Slug`),
    UNIQUE KEY `UX_Tenant_Host` (`Host`)
);

CREATE TABLE `TenantDomain` (
    `Id` BIGINT NOT NULL AUTO_INCREMENT,
    `TenantId` BIGINT NOT NULL,
    `SubDomain` VARCHAR(255) NULL,
    `Host` VARCHAR(255) NOT NULL,
    `PortalType` VARCHAR(32) NOT NULL,
    `IsPrimary` TINYINT(1) NOT NULL DEFAULT 0,
    `IsVerified` TINYINT(1) NOT NULL DEFAULT 0,
    `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
    `Verified` TINYINT(1) NOT NULL DEFAULT 0,
    `IsDelete` DATETIME NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UX_TenantDomain_Host` (`Host`),
    KEY `IX_TenantDomain_TenantId_IsPrimary` (`TenantId`, `IsPrimary`),
    CONSTRAINT `FK_TenantDomain_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`)
);

CREATE TABLE `TenantAccess` (
    `Id` BIGINT NOT NULL AUTO_INCREMENT,
    `TenantId` BIGINT NOT NULL,
    `AccessibleTenantId` BIGINT NOT NULL,
    `CanViewRecords` TINYINT(1) NOT NULL DEFAULT 0,
    `CanManageDispatch` TINYINT(1) NOT NULL DEFAULT 0,
    `IsActive` TINYINT(1) NOT NULL DEFAULT 1,
    `IsDelete` DATETIME NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UX_TenantAccess_Tenant_AccessibleTenant` (`TenantId`, `AccessibleTenantId`),
    CONSTRAINT `FK_TenantAccess_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`),
    CONSTRAINT `FK_TenantAccess_Tenant_AccessibleTenantId`
        FOREIGN KEY (`AccessibleTenantId`) REFERENCES `Tenant` (`Id`)
);

CREATE TABLE `WhiteLabelRequest` (
    `Id` BIGINT NOT NULL AUTO_INCREMENT,
    `ProviderId` BIGINT NOT NULL,
    `TenantId` BIGINT NULL,
    `CreatedAt` DATETIME NOT NULL,
    `UpdatedAt` DATETIME NULL,
    `ManageClientsDirectly` TINYINT(1) NOT NULL DEFAULT 0,
    `WantsClientPortal` TINYINT(1) NOT NULL DEFAULT 0,
    `WantsDispatchManagement` TINYINT(1) NOT NULL DEFAULT 0,
    `WantsInvoicing` TINYINT(1) NOT NULL DEFAULT 0,
    `WantsWhiteLabelBranding` TINYINT(1) NOT NULL DEFAULT 0,
    `WantsCustomDomain` TINYINT(1) NOT NULL DEFAULT 0,
    `CustomDomain` VARCHAR(255) NULL,
    `SubDomain` VARCHAR(255) NULL,
    `Status` VARCHAR(32) NOT NULL DEFAULT 'Pending',
    `AdminReviewNote` VARCHAR(2000) NULL,
    `IsDelete` DATETIME NULL,
    PRIMARY KEY (`Id`),
    UNIQUE KEY `UX_WhiteLabelRequest_ProviderId` (`ProviderId`),
    CONSTRAINT `FK_WhiteLabelRequest_Provider_ProviderId`
        FOREIGN KEY (`ProviderId`) REFERENCES `Provider` (`Id`),
    CONSTRAINT `FK_WhiteLabelRequest_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`)
);

ALTER TABLE `Admin`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Admin_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `Company`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Company_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `Provider`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Provider_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `AppInformation`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_AppInformation_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `Request`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Request_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `Invoice`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Invoice_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `Manifest`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Manifest_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `CompanyContract`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_CompanyContract_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `ProviderContract`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_ProviderContract_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `Payment`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Payment_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `ChatClientSession`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_ChatClientSession_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `ChatCommonSession`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_ChatCommonSession_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);
        
ALTER TABLE `ChatCommonMessage`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_ChatCommonMessage_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `ChatMessage`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_ChatMessage_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `ChatSession`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_ChatSession_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `Notification`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Notification_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

UPDATE `Request` r
JOIN `Company` c ON c.`Id` = r.`CompanyId`
SET r.`TenantId` = c.`TenantId`
WHERE r.`TenantId` IS NULL;

UPDATE `Invoice` i
JOIN `Company` c ON c.`Id` = i.`CompanyId`
SET i.`TenantId` = c.`TenantId`
WHERE i.`TenantId` IS NULL;

UPDATE `Manifest` m
JOIN `Company` c ON c.`Id` = m.`CompanyId`
SET m.`TenantId` = c.`TenantId`
WHERE m.`TenantId` IS NULL;

UPDATE `CompanyContract` cc
JOIN `Company` c ON c.`Id` = cc.`CompanyId`
SET cc.`TenantId` = c.`TenantId`
WHERE cc.`TenantId` IS NULL;

UPDATE `ProviderContract` pc
JOIN `Company` c ON c.`Id` = pc.`CompanyId`
SET pc.`TenantId` = c.`TenantId`
WHERE pc.`TenantId` IS NULL;

UPDATE `Payment` p
JOIN `Company` c ON c.`Id` = p.`CompanyId`
SET p.`TenantId` = c.`TenantId`
WHERE p.`TenantId` IS NULL;

UPDATE `ChatSession` cs
JOIN `Company` c ON c.`Id` = cs.`ClientId`
SET cs.`TenantId` = c.`TenantId`
WHERE cs.`TenantId` IS NULL;

UPDATE `ChatMessage` cm
JOIN `ChatSession` cs ON cs.`Id` = cm.`ChatSessionId`
SET cm.`TenantId` = cs.`TenantId`
WHERE cm.`TenantId` IS NULL;

UPDATE `ChatClientSession` ccs
JOIN `Company` c ON c.`Id` = ccs.`ClientId`
SET ccs.`TenantId` = c.`TenantId`
WHERE ccs.`TenantId` IS NULL;

UPDATE `ChatCommonSession` ccs
JOIN `Company` c ON c.`Id` = ccs.`ClientId`
SET ccs.`TenantId` = c.`TenantId`
WHERE ccs.`TenantId` IS NULL;

UPDATE `ChatCommonMessage` ccm
JOIN `ChatCommonSession` ccs ON ccs.`Id` = ccm.`ChatCommonSessionId`
SET ccm.`TenantId` = ccs.`TenantId`
WHERE ccm.`TenantId` IS NULL;

CREATE INDEX `IX_Admin_TenantId` ON `Admin` (`TenantId`);
CREATE INDEX `IX_Company_TenantId` ON `Company` (`TenantId`);
CREATE INDEX `IX_Provider_TenantId` ON `Provider` (`TenantId`);
CREATE INDEX `IX_AppInformation_TenantId` ON `AppInformation` (`TenantId`);
CREATE INDEX `IX_Request_TenantId` ON `Request` (`TenantId`);
CREATE INDEX `IX_Invoice_TenantId` ON `Invoice` (`TenantId`);
CREATE INDEX `IX_Manifest_TenantId` ON `Manifest` (`TenantId`);
CREATE INDEX `IX_CompanyContract_TenantId` ON `CompanyContract` (`TenantId`);
CREATE INDEX `IX_ProviderContract_TenantId` ON `ProviderContract` (`TenantId`);
CREATE INDEX `IX_Payment_TenantId` ON `Payment` (`TenantId`);
CREATE INDEX `IX_ChatClientSession_TenantId` ON `ChatClientSession` (`TenantId`);
CREATE INDEX `IX_ChatCommonSession_TenantId` ON `ChatCommonSession` (`TenantId`);
CREATE INDEX `IX_ChatCommonMessage_TenantId` ON `ChatCommonMessage` (`TenantId`);
CREATE INDEX `IX_ChatMessage_TenantId` ON `ChatMessage` (`TenantId`);
CREATE INDEX `IX_ChatSession_TenantId` ON `ChatSession` (`TenantId`);
CREATE INDEX `IX_Notification_TenantId` ON `Notification` (`TenantId`);
CREATE INDEX `IX_WhiteLabelRequest_TenantId` ON `WhiteLabelRequest` (`TenantId`);
CREATE INDEX `IX_TenantDomain_TenantId` ON `TenantDomain` (`TenantId`);
CREATE INDEX `IX_TenantAccess_TenantId` ON `TenantAccess` (`TenantId`);
CREATE INDEX `IX_TenantAccess_AccessibleTenantId` ON `TenantAccess` (`AccessibleTenantId`);
