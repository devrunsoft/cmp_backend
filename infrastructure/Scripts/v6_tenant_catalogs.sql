ALTER TABLE `Product`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Product_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `ProductPrice`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_ProductPrice_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `Contract`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Contract_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `Capacity`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Capacity_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

ALTER TABLE `TermsConditions`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_TermsConditions_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);
        
ALTER TABLE `Route`
    ADD COLUMN `TenantId` BIGINT NULL,
    ADD CONSTRAINT `FK_Route_Tenant_TenantId`
        FOREIGN KEY (`TenantId`) REFERENCES `Tenant` (`Id`);

CREATE INDEX `IX_Route_TenantId` ON `Route` (`TenantId`);
CREATE INDEX `IX_Product_TenantId` ON `Product` (`TenantId`);
CREATE INDEX `IX_ProductPrice_TenantId` ON `ProductPrice` (`TenantId`);
CREATE INDEX `IX_Contract_TenantId` ON `Contract` (`TenantId`);
CREATE INDEX `IX_Capacity_TenantId` ON `Capacity` (`TenantId`);
CREATE INDEX `IX_TermsConditions_TenantId` ON `TermsConditions` (`TenantId`);
