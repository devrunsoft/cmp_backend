ALTER TABLE `Manifest`
ADD COLUMN `COD` bit(1) DEFAULT b'0';

ALTER TABLE `Manifest`
ADD COLUMN `AddtitionalInformation` longtext null;