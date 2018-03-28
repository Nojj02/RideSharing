\connect postgres

 -- ridesharing Schema
CREATE SCHEMA "ridesharing"
    AUTHORIZATION postgres;

-- ride table
CREATE TABLE "ridesharing"."ride"
(
    "db_id" bigserial NOT NULL,
    "id" uuid NOT NULL,
    "version" bigint NOT NULL,
    "event_type" CHARACTER VARYING(100) COLLATE pg_catalog."default"  NOT NULL,
    "event" jsonb NOT NULL,
    "timestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "rideevent_pkey" PRIMARY KEY ("db_id"),
    CONSTRAINT "version_uq" UNIQUE ("id", "version")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE "ridesharing"."ride"
    OWNER to postgres;