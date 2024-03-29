CREATE SCHEMA IF NOT EXISTS "Operational";

CREATE TABLE "Operational"."DeviceCodes" (
    "UserCode" character varying(200) NOT NULL,
    "DeviceCode" character varying(200) NOT NULL,
    "SubjectId" character varying(200) NULL,
    "SessionId" character varying(100) NULL,
    "ClientId" character varying(200) NOT NULL,
    "Description" character varying(200) NULL,
    "CreationTime" timestamp without time zone NOT NULL,
    "Expiration" timestamp without time zone NOT NULL,
    "Data" character varying(50000) NOT NULL,
    CONSTRAINT "PK_DeviceCodes" PRIMARY KEY ("UserCode")
);

CREATE TABLE "Operational"."PersistedGrants" (
    "Key" character varying(200) NOT NULL,
    "Type" character varying(50) NOT NULL,
    "SubjectId" character varying(200) NULL,
    "SessionId" character varying(100) NULL,
    "ClientId" character varying(200) NOT NULL,
    "Description" character varying(200) NULL,
    "CreationTime" timestamp without time zone NOT NULL,
    "Expiration" timestamp without time zone NULL,
    "ConsumedTime" timestamp without time zone NULL,
    "Data" character varying(50000) NOT NULL,
    CONSTRAINT "PK_PersistedGrants" PRIMARY KEY ("Key")
);

CREATE UNIQUE INDEX "IX_DeviceCodes_DeviceCode" ON "Operational"."DeviceCodes" ("DeviceCode");

CREATE INDEX "IX_DeviceCodes_Expiration" ON "Operational"."DeviceCodes" ("Expiration");

CREATE INDEX "IX_PersistedGrants_Expiration" ON "Operational"."PersistedGrants" ("Expiration");

CREATE INDEX "IX_PersistedGrants_SubjectId_ClientId_Type" ON "Operational"."PersistedGrants" ("SubjectId", "ClientId", "Type");

CREATE INDEX "IX_PersistedGrants_SubjectId_SessionId_Type" ON "Operational"."PersistedGrants" ("SubjectId", "SessionId", "Type");
