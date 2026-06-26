--
-- PostgreSQL database dump
--

\restrict sXkIw7g5IVBbvRh4RkBI1rXifAgjM68lnBeMm1M6ZApwTge3xkzgFjKQ0pPEg9B

-- Dumped from database version 16.14
-- Dumped by pg_dump version 16.14

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: hangfire; Type: SCHEMA; Schema: -; Owner: kanvasuser
--

CREATE SCHEMA hangfire;


ALTER SCHEMA hangfire OWNER TO kanvasuser;

--
-- Name: public; Type: SCHEMA; Schema: -; Owner: kanvasuser
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO kanvasuser;

--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: kanvasuser
--

COMMENT ON SCHEMA public IS '';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: aggregatedcounter; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.aggregatedcounter (
    id bigint NOT NULL,
    key text NOT NULL,
    value bigint NOT NULL,
    expireat timestamp with time zone
);


ALTER TABLE hangfire.aggregatedcounter OWNER TO kanvasuser;

--
-- Name: aggregatedcounter_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: kanvasuser
--

CREATE SEQUENCE hangfire.aggregatedcounter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.aggregatedcounter_id_seq OWNER TO kanvasuser;

--
-- Name: aggregatedcounter_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: kanvasuser
--

ALTER SEQUENCE hangfire.aggregatedcounter_id_seq OWNED BY hangfire.aggregatedcounter.id;


--
-- Name: counter; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.counter (
    id bigint NOT NULL,
    key text NOT NULL,
    value bigint NOT NULL,
    expireat timestamp with time zone
);


ALTER TABLE hangfire.counter OWNER TO kanvasuser;

--
-- Name: counter_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: kanvasuser
--

CREATE SEQUENCE hangfire.counter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.counter_id_seq OWNER TO kanvasuser;

--
-- Name: counter_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: kanvasuser
--

ALTER SEQUENCE hangfire.counter_id_seq OWNED BY hangfire.counter.id;


--
-- Name: hash; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.hash (
    id bigint NOT NULL,
    key text NOT NULL,
    field text NOT NULL,
    value text,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.hash OWNER TO kanvasuser;

--
-- Name: hash_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: kanvasuser
--

CREATE SEQUENCE hangfire.hash_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.hash_id_seq OWNER TO kanvasuser;

--
-- Name: hash_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: kanvasuser
--

ALTER SEQUENCE hangfire.hash_id_seq OWNED BY hangfire.hash.id;


--
-- Name: job; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.job (
    id bigint NOT NULL,
    stateid bigint,
    statename text,
    invocationdata jsonb NOT NULL,
    arguments jsonb NOT NULL,
    createdat timestamp with time zone NOT NULL,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.job OWNER TO kanvasuser;

--
-- Name: job_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: kanvasuser
--

CREATE SEQUENCE hangfire.job_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.job_id_seq OWNER TO kanvasuser;

--
-- Name: job_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: kanvasuser
--

ALTER SEQUENCE hangfire.job_id_seq OWNED BY hangfire.job.id;


--
-- Name: jobparameter; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.jobparameter (
    id bigint NOT NULL,
    jobid bigint NOT NULL,
    name text NOT NULL,
    value text,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.jobparameter OWNER TO kanvasuser;

--
-- Name: jobparameter_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: kanvasuser
--

CREATE SEQUENCE hangfire.jobparameter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.jobparameter_id_seq OWNER TO kanvasuser;

--
-- Name: jobparameter_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: kanvasuser
--

ALTER SEQUENCE hangfire.jobparameter_id_seq OWNED BY hangfire.jobparameter.id;


--
-- Name: jobqueue; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.jobqueue (
    id bigint NOT NULL,
    jobid bigint NOT NULL,
    queue text NOT NULL,
    fetchedat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.jobqueue OWNER TO kanvasuser;

--
-- Name: jobqueue_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: kanvasuser
--

CREATE SEQUENCE hangfire.jobqueue_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.jobqueue_id_seq OWNER TO kanvasuser;

--
-- Name: jobqueue_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: kanvasuser
--

ALTER SEQUENCE hangfire.jobqueue_id_seq OWNED BY hangfire.jobqueue.id;


--
-- Name: list; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.list (
    id bigint NOT NULL,
    key text NOT NULL,
    value text,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.list OWNER TO kanvasuser;

--
-- Name: list_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: kanvasuser
--

CREATE SEQUENCE hangfire.list_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.list_id_seq OWNER TO kanvasuser;

--
-- Name: list_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: kanvasuser
--

ALTER SEQUENCE hangfire.list_id_seq OWNED BY hangfire.list.id;


--
-- Name: lock; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.lock (
    resource text NOT NULL,
    updatecount integer DEFAULT 0 NOT NULL,
    acquired timestamp with time zone
);


ALTER TABLE hangfire.lock OWNER TO kanvasuser;

--
-- Name: schema; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.schema (
    version integer NOT NULL
);


ALTER TABLE hangfire.schema OWNER TO kanvasuser;

--
-- Name: server; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.server (
    id text NOT NULL,
    data jsonb,
    lastheartbeat timestamp with time zone NOT NULL,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.server OWNER TO kanvasuser;

--
-- Name: set; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.set (
    id bigint NOT NULL,
    key text NOT NULL,
    score double precision NOT NULL,
    value text NOT NULL,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.set OWNER TO kanvasuser;

--
-- Name: set_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: kanvasuser
--

CREATE SEQUENCE hangfire.set_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.set_id_seq OWNER TO kanvasuser;

--
-- Name: set_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: kanvasuser
--

ALTER SEQUENCE hangfire.set_id_seq OWNED BY hangfire.set.id;


--
-- Name: state; Type: TABLE; Schema: hangfire; Owner: kanvasuser
--

CREATE TABLE hangfire.state (
    id bigint NOT NULL,
    jobid bigint NOT NULL,
    name text NOT NULL,
    reason text,
    createdat timestamp with time zone NOT NULL,
    data jsonb,
    updatecount integer DEFAULT 0 NOT NULL
);


ALTER TABLE hangfire.state OWNER TO kanvasuser;

--
-- Name: state_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: kanvasuser
--

CREATE SEQUENCE hangfire.state_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE hangfire.state_id_seq OWNER TO kanvasuser;

--
-- Name: state_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: kanvasuser
--

ALTER SEQUENCE hangfire.state_id_seq OWNED BY hangfire.state.id;


--
-- Name: Adresler; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."Adresler" (
    "Id" integer NOT NULL,
    "Baslik" text NOT NULL,
    "AdSoyad" text NOT NULL,
    "Telefon" text NOT NULL,
    "Sehir" text NOT NULL,
    "Ilce" text NOT NULL,
    "AcikAdres" text NOT NULL,
    "AppUserId" text NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."Adresler" OWNER TO kanvasuser;

--
-- Name: Adresler_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."Adresler" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Adresler_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: AspNetRoleClaims; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."AspNetRoleClaims" (
    "Id" integer NOT NULL,
    "RoleId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


ALTER TABLE public."AspNetRoleClaims" OWNER TO kanvasuser;

--
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."AspNetRoleClaims" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."AspNetRoleClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: AspNetRoles; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text
);


ALTER TABLE public."AspNetRoles" OWNER TO kanvasuser;

--
-- Name: AspNetUserClaims; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."AspNetUserClaims" (
    "Id" integer NOT NULL,
    "UserId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


ALTER TABLE public."AspNetUserClaims" OWNER TO kanvasuser;

--
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."AspNetUserClaims" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."AspNetUserClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: AspNetUserLogins; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" text NOT NULL
);


ALTER TABLE public."AspNetUserLogins" OWNER TO kanvasuser;

--
-- Name: AspNetUserRoles; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL
);


ALTER TABLE public."AspNetUserRoles" OWNER TO kanvasuser;

--
-- Name: AspNetUserTokens; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text
);


ALTER TABLE public."AspNetUserTokens" OWNER TO kanvasuser;

--
-- Name: AspNetUsers; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."AspNetUsers" (
    "Id" text NOT NULL,
    "AdSoyad" text NOT NULL,
    "Sehir" text NOT NULL,
    "SifreSifirlamaToken" text,
    "SifreSifirlamaGecerlilik" timestamp with time zone,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    "DogumTarihi" timestamp with time zone,
    "KimlikFotografYolu" text DEFAULT ''::text NOT NULL,
    "KimlikNo" text DEFAULT ''::text NOT NULL,
    "Adres" text DEFAULT ''::text NOT NULL,
    "WholesaleStatus" integer DEFAULT 0 NOT NULL,
    "BasvuruTarihi" timestamp with time zone
);


ALTER TABLE public."AspNetUsers" OWNER TO kanvasuser;

--
-- Name: BankaHesaplari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."BankaHesaplari" (
    "Id" integer NOT NULL,
    "BankaAdi" text NOT NULL,
    "HesapSahibi" text NOT NULL,
    "IBAN" text NOT NULL,
    "SubeKodu" text,
    "HesapNo" text,
    "AktifMi" boolean NOT NULL,
    "Sira" integer NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."BankaHesaplari" OWNER TO kanvasuser;

--
-- Name: BankaHesaplari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."BankaHesaplari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."BankaHesaplari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: BultenAbonelikleri; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."BultenAbonelikleri" (
    "Id" integer NOT NULL,
    "Email" text NOT NULL,
    "KayitTarihi" timestamp with time zone NOT NULL,
    "AktifMi" boolean NOT NULL,
    "IpAdresi" text
);


ALTER TABLE public."BultenAbonelikleri" OWNER TO kanvasuser;

--
-- Name: BultenAbonelikleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."BultenAbonelikleri" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."BultenAbonelikleri_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: CarkOdulleri; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."CarkOdulleri" (
    "Id" integer NOT NULL,
    "LabelTr" character varying(50) NOT NULL,
    "LabelEn" character varying(50) NOT NULL,
    "LabelAr" character varying(50) NOT NULL,
    "Tip" character varying(20) NOT NULL,
    "Deger" integer DEFAULT 0 NOT NULL,
    "Renk" character varying(10) DEFAULT '#313511'::character varying NOT NULL,
    "MesajTr" character varying(200),
    "MesajEn" character varying(200),
    "MesajAr" character varying(200),
    "Sira" integer DEFAULT 0 NOT NULL,
    "AktifMi" boolean DEFAULT true NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone DEFAULT now() NOT NULL,
    "GuncellenmeTarihi" timestamp with time zone,
    "SilindiMi" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."CarkOdulleri" OWNER TO kanvasuser;

--
-- Name: CarkOdulleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."CarkOdulleri" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."CarkOdulleri_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Favoriler; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."Favoriler" (
    "Id" integer NOT NULL,
    "AppUserId" text NOT NULL,
    "UrunId" integer NOT NULL,
    "FiyatDustugundaBildir" boolean NOT NULL,
    "EskiFiyat" numeric,
    "SonBildirimTarihi" timestamp with time zone,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."Favoriler" OWNER TO kanvasuser;

--
-- Name: Favoriler_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."Favoriler" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Favoriler_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: HomePageSectionProducts; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."HomePageSectionProducts" (
    "Id" integer NOT NULL,
    "SectionId" integer NOT NULL,
    "UrunId" integer NOT NULL,
    "SortOrder" integer NOT NULL
);


ALTER TABLE public."HomePageSectionProducts" OWNER TO kanvasuser;

--
-- Name: HomePageSectionProducts_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."HomePageSectionProducts" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."HomePageSectionProducts_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: HomePageSections; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."HomePageSections" (
    "Id" integer NOT NULL,
    "SectionType" integer NOT NULL,
    "Enabled" boolean NOT NULL,
    "SortOrder" integer NOT NULL,
    "Title" text NOT NULL,
    "Subtitle" text NOT NULL,
    "ViewAllText" text,
    "ViewAllUrl" text,
    "ImageUrl" text,
    "Description" text,
    "ButtonText" text,
    "ButtonUrl" text
);


ALTER TABLE public."HomePageSections" OWNER TO kanvasuser;

--
-- Name: HomePageSections_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."HomePageSections" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."HomePageSections_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: IadeTalepleri; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."IadeTalepleri" (
    "Id" integer NOT NULL,
    "SiparisId" integer NOT NULL,
    "MusteriId" text NOT NULL,
    "Neden" text NOT NULL,
    "Aciklama" text,
    "IBAN" text,
    "Durum" integer NOT NULL,
    "AdminNotu" text,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."IadeTalepleri" OWNER TO kanvasuser;

--
-- Name: IadeTalepleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."IadeTalepleri" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."IadeTalepleri_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: IletisimMesajlari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."IletisimMesajlari" (
    "Id" integer NOT NULL,
    "AdSoyad" text NOT NULL,
    "Email" text NOT NULL,
    "Konu" text NOT NULL,
    "Mesaj" text NOT NULL,
    "Tarih" timestamp with time zone NOT NULL,
    "IpAdresi" text,
    "OkunduMu" boolean NOT NULL
);


ALTER TABLE public."IletisimMesajlari" OWNER TO kanvasuser;

--
-- Name: IletisimMesajlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."IletisimMesajlari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."IletisimMesajlari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: KargoBolgeFiyatlari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."KargoBolgeFiyatlari" (
    "Id" integer NOT NULL,
    "KargoFirmasiId" integer NOT NULL,
    "BolgeId" integer NOT NULL,
    "Fiyat" numeric NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."KargoBolgeFiyatlari" OWNER TO kanvasuser;

--
-- Name: KargoBolgeFiyatlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."KargoBolgeFiyatlari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."KargoBolgeFiyatlari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: KargoBolgeSehirler; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."KargoBolgeSehirler" (
    "Id" integer NOT NULL,
    "BolgeId" integer NOT NULL,
    "SehirAdi" text NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."KargoBolgeSehirler" OWNER TO kanvasuser;

--
-- Name: KargoBolgeSehirler_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."KargoBolgeSehirler" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."KargoBolgeSehirler_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: KargoBolgeler; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."KargoBolgeler" (
    "Id" integer NOT NULL,
    "Ad" text NOT NULL,
    "Sira" integer NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL,
    "Ulke" character varying(100),
    "Aciklama" character varying(500),
    "Fiyat" numeric DEFAULT 0 NOT NULL
);


ALTER TABLE public."KargoBolgeler" OWNER TO kanvasuser;

--
-- Name: KargoBolgeler_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."KargoBolgeler" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."KargoBolgeler_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: KargoFirmalari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."KargoFirmalari" (
    "Id" integer NOT NULL,
    "Ad" text NOT NULL,
    "Kod" text NOT NULL,
    "LogoUrl" text,
    "Telefon" text,
    "TakipUrl" text,
    "GondericiUnvan" text NOT NULL,
    "GondericiAdres" text NOT NULL,
    "GondericiTelefon" text NOT NULL,
    "AktifMi" boolean NOT NULL,
    "VarsayilanMi" boolean NOT NULL,
    "Fiyat" numeric NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."KargoFirmalari" OWNER TO kanvasuser;

--
-- Name: KargoFirmalari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."KargoFirmalari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."KargoFirmalari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Kategoriler; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."Kategoriler" (
    "Id" integer NOT NULL,
    "Ad" text NOT NULL,
    "KisaAciklama" text NOT NULL,
    "Aciklama" text NOT NULL,
    "Slug" text,
    "GorselUrl" text,
    "BannerUrl" text,
    "SeoTitle" text NOT NULL,
    "SeoDescription" text NOT NULL,
    "UstMetin" text NOT NULL,
    "AltMetin" text NOT NULL,
    "KampanyaEtiketi" text NOT NULL,
    "UrunSiralamaTipi" text NOT NULL,
    "Sira" integer NOT NULL,
    "AktifMi" boolean NOT NULL,
    "ParentKategoriId" integer,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL,
    "AciklamaAr" text DEFAULT ''::text NOT NULL,
    "AciklamaEn" text DEFAULT ''::text NOT NULL,
    "AdAr" text DEFAULT ''::text NOT NULL,
    "AdEn" text DEFAULT ''::text NOT NULL,
    "AltMetinAr" text DEFAULT ''::text NOT NULL,
    "AltMetinEn" text DEFAULT ''::text NOT NULL,
    "KampanyaEtiketiAr" text DEFAULT ''::text NOT NULL,
    "KampanyaEtiketiEn" text DEFAULT ''::text NOT NULL,
    "KisaAciklamaAr" text DEFAULT ''::text NOT NULL,
    "KisaAciklamaEn" text DEFAULT ''::text NOT NULL,
    "SeoDescriptionAr" text DEFAULT ''::text NOT NULL,
    "SeoDescriptionEn" text DEFAULT ''::text NOT NULL,
    "SeoTitleAr" text DEFAULT ''::text NOT NULL,
    "SeoTitleEn" text DEFAULT ''::text NOT NULL,
    "UstMetinAr" text DEFAULT ''::text NOT NULL,
    "UstMetinEn" text DEFAULT ''::text NOT NULL,
    "ReceteGerekliMi" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Kategoriler" OWNER TO kanvasuser;

--
-- Name: Kategoriler_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."Kategoriler" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Kategoriler_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Kuponlar; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."Kuponlar" (
    "Id" integer NOT NULL,
    "Kod" character varying(20) NOT NULL,
    "Tip" integer NOT NULL,
    "Deger" numeric NOT NULL,
    "MinSepetTutari" numeric NOT NULL,
    "SonKullanmaTarihi" timestamp with time zone NOT NULL,
    "KullanimLimiti" integer NOT NULL,
    "KullanilanMiktar" integer NOT NULL,
    "AktifMi" boolean NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."Kuponlar" OWNER TO kanvasuser;

--
-- Name: Kuponlar_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."Kuponlar" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Kuponlar_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: KurumsalSayfalar; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."KurumsalSayfalar" (
    "Id" integer NOT NULL,
    "Baslik" text NOT NULL,
    "Icerik" text NOT NULL,
    "UrlSlug" text NOT NULL,
    "Sira" integer NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."KurumsalSayfalar" OWNER TO kanvasuser;

--
-- Name: KurumsalSayfalar_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."KurumsalSayfalar" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."KurumsalSayfalar_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: PushAbonelikleri; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."PushAbonelikleri" (
    "Id" integer NOT NULL,
    "Token" character varying(500) NOT NULL,
    "AppUserId" text,
    "Tarayici" character varying(200),
    "Platform" character varying(100),
    "AktifMi" boolean NOT NULL,
    "SonGorulmeTarihi" timestamp with time zone,
    "Tercihler" character varying(500),
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."PushAbonelikleri" OWNER TO kanvasuser;

--
-- Name: PushAbonelikleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."PushAbonelikleri" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."PushAbonelikleri_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: SenTasarla; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."SenTasarla" (
    "Id" integer NOT NULL,
    "KullaniciId" text,
    "DosyaYolu" character varying(200) NOT NULL,
    "Olcu" character varying(50) NOT NULL,
    "Fiyat" numeric(18,2) NOT NULL,
    "Efekt" character varying(20) NOT NULL,
    "CercveliMi" boolean NOT NULL,
    "ParcaSayisi" character varying(50) NOT NULL,
    "OlusturmaTarihi" timestamp with time zone NOT NULL,
    "SepeteEklendi" boolean NOT NULL,
    "SessionId" character varying(50) NOT NULL
);


ALTER TABLE public."SenTasarla" OWNER TO kanvasuser;

--
-- Name: SenTasarla_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."SenTasarla" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."SenTasarla_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: SepetItems; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."SepetItems" (
    "Id" integer NOT NULL,
    "SepetId" integer NOT NULL,
    "UrunId" integer NOT NULL,
    "UrunSecenekId" integer,
    "Adet" integer NOT NULL,
    "Fiyat" numeric NOT NULL,
    "UrunBaslik" text NOT NULL,
    "UrunResimUrl" text NOT NULL,
    "CerceveModeli" text NOT NULL,
    "SecenekAdi" text,
    "MusteriNotu" character varying(500),
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL,
    "HediyePaketFiyati" numeric DEFAULT 0.0 NOT NULL,
    "HediyePaketi" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."SepetItems" OWNER TO kanvasuser;

--
-- Name: SepetItems_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."SepetItems" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."SepetItems_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Sepetler; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."Sepetler" (
    "Id" integer NOT NULL,
    "AppUserId" text,
    "SessionId" text,
    "SonGuncellemeTarihi" timestamp with time zone NOT NULL,
    "TerkEdildi" boolean NOT NULL,
    "TerkEdilmeTarihi" timestamp with time zone,
    "HatirlatmaGonderildi" boolean NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."Sepetler" OWNER TO kanvasuser;

--
-- Name: Sepetler_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."Sepetler" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Sepetler_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: SiparisDetaylari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."SiparisDetaylari" (
    "Id" integer NOT NULL,
    "SiparisId" integer NOT NULL,
    "UrunSecenekId" integer,
    "Adet" integer NOT NULL,
    "BirimFiyat" numeric NOT NULL,
    "UrunId" integer NOT NULL,
    "CerceveModeli" text NOT NULL,
    "MusteriNotu" character varying(500),
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL,
    "HediyePaketFiyati" numeric DEFAULT 0.0 NOT NULL,
    "HediyePaketi" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."SiparisDetaylari" OWNER TO kanvasuser;

--
-- Name: SiparisDetaylari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."SiparisDetaylari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."SiparisDetaylari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Siparisler; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."Siparisler" (
    "Id" integer NOT NULL,
    "AppUserId" text,
    "SiparisNo" text NOT NULL,
    "KuponKodu" text,
    "IndirimTutari" numeric NOT NULL,
    "MusteriAdSoyad" text NOT NULL,
    "Telefon" text NOT NULL,
    "Eposta" text NOT NULL,
    "Sehir" text NOT NULL,
    "Ilce" text NOT NULL,
    "AcikAdres" text NOT NULL,
    "ToplamTutar" numeric NOT NULL,
    "Durum" integer NOT NULL,
    "KargoTakipNo" text,
    "KargoFirmasiId" integer,
    "KargoFirmasi" text,
    "KargoUcreti" numeric NOT NULL,
    "Aciklama" text,
    "EmailHashKodu" text,
    "FaturaDosyaYolu" text,
    "FaturaDosyaAdi" text,
    "FaturaYuklendiMi" boolean NOT NULL,
    "FaturaYuklenmeTarihi" timestamp with time zone,
    "FaturaMailGonderildiMi" boolean NOT NULL,
    "FaturaMailGonderimTarihi" timestamp with time zone,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL,
    "ReceteDosyaYolu" text,
    "TeslimatTipi" text DEFAULT ''::text NOT NULL,
    "KimlikFotoYolu" text,
    "KapidaOdemeHizmetBedeli" numeric DEFAULT 0.0 NOT NULL,
    "OdemeYontemi" text DEFAULT ''::text NOT NULL,
    "ReceteOnayDurumu" integer DEFAULT 0 NOT NULL,
    "ReceteRedSebebi" text
);


ALTER TABLE public."Siparisler" OWNER TO kanvasuser;

--
-- Name: Siparisler_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."Siparisler" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Siparisler_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: SiteAyarlari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."SiteAyarlari" (
    "Id" integer NOT NULL,
    "SiteAdi" text NOT NULL,
    "MarkaAdi" text NOT NULL,
    "SiteBasligi" text NOT NULL,
    "SiteAciklamasi" text NOT NULL,
    "SiteLogoUrl" text NOT NULL,
    "FaviconUrl" text NOT NULL,
    "BaseUrl" text NOT NULL,
    "TemaRengi" text NOT NULL,
    "UstBarMesaji" text NOT NULL,
    "KampanyaMesaji" text NOT NULL,
    "UstBarEtkin" boolean NOT NULL,
    "UstBarHizi" double precision NOT NULL,
    "FooterAciklamasi" text NOT NULL,
    "Telefon" text NOT NULL,
    "Email" text NOT NULL,
    "Adres" text NOT NULL,
    "WhatsappNumarasi" text NOT NULL,
    "CalismaSaatleri" text NOT NULL,
    "FacebookUrl" text NOT NULL,
    "InstagramUrl" text NOT NULL,
    "TwitterUrl" text NOT NULL,
    "YoutubeUrl" text NOT NULL,
    "TiktokUrl" text NOT NULL,
    "PinterestUrl" text NOT NULL,
    "ParaBirimi" text NOT NULL,
    "KargoBedeli" numeric NOT NULL,
    "UcretsizKargoLimiti" numeric NOT NULL,
    "StokUyariLimiti" integer NOT NULL,
    "StoktaYokSatisIzni" boolean NOT NULL,
    "PaytrAktifMi" boolean NOT NULL,
    "PaytrTestModu" boolean NOT NULL,
    "PaytrMerchantId" text NOT NULL,
    "PaytrMerchantKeyProtected" text NOT NULL,
    "PaytrMerchantSaltProtected" text NOT NULL,
    "PaytrCallbackUrl" text NOT NULL,
    "PaytrBasariliDonusUrl" text NOT NULL,
    "PaytrBasarisizDonusUrl" text NOT NULL,
    "KargoFirmasi" text NOT NULL,
    "KargoTakipUrl" text NOT NULL,
    "SiparisTeslimSuresiGun" integer NOT NULL,
    "IadeHakkiGun" integer NOT NULL,
    "MetaTitle" text NOT NULL,
    "MetaDescription" text NOT NULL,
    "MetaKeywords" text NOT NULL,
    "GoogleAnalyticsId" text NOT NULL,
    "FacebookPixelId" text NOT NULL,
    "VarsayilanSosyalPaylasimGorseliUrl" text NOT NULL,
    "CookieMetni" text NOT NULL,
    "YeniSiparisMailBildirimi" boolean NOT NULL,
    "StokUyarisiMailBildirimi" boolean NOT NULL,
    "IadeTalebiMailBildirimi" boolean NOT NULL,
    "BildirimAliciEmail" text NOT NULL,
    "BakimModuAktif" boolean NOT NULL,
    "BakimModuMesaji" text NOT NULL,
    "GirisZorunluMu" boolean DEFAULT false NOT NULL,
    "StokBiteniGriGoster" boolean DEFAULT true NOT NULL,
    "KapidaOdemeAktifMi" boolean DEFAULT false NOT NULL,
    "KapidaOdemeHizmetBedeli" numeric DEFAULT 0.0 NOT NULL,
    "KapidaOdemeLimiti" numeric DEFAULT 0.0 NOT NULL,
    "ToptanciMinSiparisTutari" numeric DEFAULT 500 NOT NULL,
    "IptalSuresiSaat" integer DEFAULT 3 NOT NULL
);


ALTER TABLE public."SiteAyarlari" OWNER TO kanvasuser;

--
-- Name: SiteAyarlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."SiteAyarlari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."SiteAyarlari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: SiteDegerlendirmeleri; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."SiteDegerlendirmeleri" (
    "Id" integer NOT NULL,
    "AdSoyad" character varying(100) NOT NULL,
    "Eposta" character varying(200),
    "Puan" integer NOT NULL,
    "Baslik" character varying(200),
    "YorumMetni" character varying(2000) NOT NULL,
    "OnayliMi" boolean NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."SiteDegerlendirmeleri" OWNER TO kanvasuser;

--
-- Name: SiteDegerlendirmeleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."SiteDegerlendirmeleri" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."SiteDegerlendirmeleri_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Slaytlar; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."Slaytlar" (
    "Id" integer NOT NULL,
    "Baslik" text NOT NULL,
    "AltBaslik" text NOT NULL,
    "Aciklama" text NOT NULL,
    "ResimUrl" text,
    "VideoUrl" text,
    "Tur" text NOT NULL,
    "Sira" integer NOT NULL,
    "AktifMi" boolean NOT NULL,
    "OlusturmaTarihi" timestamp with time zone NOT NULL,
    "BaslikEn" text DEFAULT ''::text NOT NULL,
    "BaslikAr" text DEFAULT ''::text NOT NULL,
    "AltBaslikEn" text DEFAULT ''::text NOT NULL,
    "AltBaslikAr" text DEFAULT ''::text NOT NULL,
    "AciklamaEn" text DEFAULT ''::text NOT NULL,
    "AciklamaAr" text DEFAULT ''::text NOT NULL
);


ALTER TABLE public."Slaytlar" OWNER TO kanvasuser;

--
-- Name: Slaytlar_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."Slaytlar" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Slaytlar_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ToptanciIskontoOranlari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."ToptanciIskontoOranlari" (
    "Id" integer NOT NULL,
    "ToptanciUrunGrubuId" integer NOT NULL,
    "MinAdet" integer DEFAULT 1 NOT NULL,
    "IskontoYuzdesi" numeric DEFAULT 0 NOT NULL,
    "AktifMi" boolean DEFAULT true NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone DEFAULT now() NOT NULL,
    "GuncellemeTarihi" timestamp with time zone,
    "SilindiMi" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."ToptanciIskontoOranlari" OWNER TO kanvasuser;

--
-- Name: ToptanciIskontoOranlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."ToptanciIskontoOranlari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."ToptanciIskontoOranlari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ToptanciUrunGruplari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."ToptanciUrunGruplari" (
    "Id" integer NOT NULL,
    "Ad" text NOT NULL,
    "Aciklama" text,
    "AktifMi" boolean DEFAULT true NOT NULL,
    "Sira" integer DEFAULT 0 NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone DEFAULT now() NOT NULL,
    "GuncellemeTarihi" timestamp with time zone,
    "SilindiMi" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."ToptanciUrunGruplari" OWNER TO kanvasuser;

--
-- Name: ToptanciUrunGruplari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."ToptanciUrunGruplari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."ToptanciUrunGruplari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: UrunOzellikDegerleri; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."UrunOzellikDegerleri" (
    "Id" integer NOT NULL,
    "UrunId" integer NOT NULL,
    "UrunOzellikTanimiId" integer NOT NULL,
    "Deger" text NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."UrunOzellikDegerleri" OWNER TO kanvasuser;

--
-- Name: UrunOzellikDegerleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."UrunOzellikDegerleri" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."UrunOzellikDegerleri_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: UrunOzellikTanimlari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."UrunOzellikTanimlari" (
    "Id" integer NOT NULL,
    "Ad" text NOT NULL,
    "Kod" text NOT NULL,
    "UrunTipi" text NOT NULL,
    "AlanTipi" text NOT NULL,
    "YardimMetni" text NOT NULL,
    "Secenekler" text NOT NULL,
    "FiltredeGoster" boolean NOT NULL,
    "DetaySayfasindaGoster" boolean NOT NULL,
    "TeknikTablodaGoster" boolean NOT NULL,
    "AktifMi" boolean NOT NULL,
    "Sira" integer NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."UrunOzellikTanimlari" OWNER TO kanvasuser;

--
-- Name: UrunOzellikTanimlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."UrunOzellikTanimlari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."UrunOzellikTanimlari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: UrunResimleri; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."UrunResimleri" (
    "Id" integer NOT NULL,
    "ResimYolu" text NOT NULL,
    "Baslik" text NOT NULL,
    "AltMetin" text NOT NULL,
    "MedyaTipi" text NOT NULL,
    "MedyaAlani" text NOT NULL,
    "VideoUrl" text NOT NULL,
    "ThumbnailYolu" text NOT NULL,
    "MobilResimYolu" text NOT NULL,
    "Etiketler" text NOT NULL,
    "Sira" integer NOT NULL,
    "VarsayilanMi" boolean NOT NULL,
    "UrunSecenekId" integer,
    "UrunId" integer NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."UrunResimleri" OWNER TO kanvasuser;

--
-- Name: UrunResimleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."UrunResimleri" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."UrunResimleri_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: UrunSecenekleri; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."UrunSecenekleri" (
    "Id" integer NOT NULL,
    "UrunId" integer NOT NULL,
    "Olcu" text NOT NULL,
    "CerceveTipi" text NOT NULL,
    "CerceveRengi" text NOT NULL,
    "CerceveKalinligi" text NOT NULL,
    "MalzemeTuru" text NOT NULL,
    "Yon" text NOT NULL,
    "ParcaSayisi" integer NOT NULL,
    "VaryantSku" text NOT NULL,
    "KisilestirmeMetni" text NOT NULL,
    "OzelTasarimNotu" text NOT NULL,
    "FiyatFarki" numeric NOT NULL,
    "SatisFiyati" numeric NOT NULL,
    "MaliyetFiyati" numeric NOT NULL,
    "StokAdedi" integer NOT NULL,
    "UretimSuresiGun" integer NOT NULL,
    "Desi" numeric NOT NULL,
    "GorselUrl" text NOT NULL,
    "AktifMi" boolean NOT NULL,
    "VarsayilanMi" boolean NOT NULL,
    "TukeninceGizle" boolean NOT NULL,
    "OnSipariseAcikMi" boolean NOT NULL,
    "Sira" integer NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."UrunSecenekleri" OWNER TO kanvasuser;

--
-- Name: UrunSecenekleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."UrunSecenekleri" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."UrunSecenekleri_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Urunler; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."Urunler" (
    "Id" integer NOT NULL,
    "Baslik" text NOT NULL,
    "KisaAd" text NOT NULL,
    "Slug" text,
    "UrlYolu" text NOT NULL,
    "SKU" text NOT NULL,
    "Barkod" text NOT NULL,
    "Marka" text NOT NULL,
    "UrunTipi" text NOT NULL,
    "Etiketler" text NOT NULL,
    "KisaAciklama" text NOT NULL,
    "Aciklama" text NOT NULL,
    "TeknikOzellikler" text NOT NULL,
    "MalzemeBilgisi" text NOT NULL,
    "BakimTalimati" text NOT NULL,
    "PaketlemeBilgisi" text NOT NULL,
    "AnaGorselUrl" text NOT NULL,
    "StokDurumu" text NOT NULL,
    "Fiyat" numeric NOT NULL,
    "IndirimliFiyat" numeric,
    "Maliyet" numeric NOT NULL,
    "KdvOrani" numeric NOT NULL,
    "UretimSuresiGun" integer NOT NULL,
    "KargoyaVerilisSuresiGun" integer NOT NULL,
    "TahminiTeslimSuresiGun" integer NOT NULL,
    "AktifMi" boolean NOT NULL,
    "OneCikanMi" boolean NOT NULL,
    "YeniUrunMu" boolean NOT NULL,
    "KampanyaliMi" boolean NOT NULL,
    "AnaSayfadaGoster" boolean NOT NULL,
    "Sira" integer NOT NULL,
    "GoruntulenmeSayisi" integer NOT NULL,
    "SatisSayisi" integer NOT NULL,
    "FavoriSayisi" integer NOT NULL,
    "MinSiparisAdedi" integer NOT NULL,
    "MaxSiparisAdedi" integer,
    "SeoTitle" text NOT NULL,
    "SeoDescription" text NOT NULL,
    "SeoKeywords" text NOT NULL,
    "KategoriId" integer NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL,
    "TopFiyat" numeric,
    "AciklamaAr" text DEFAULT ''::text NOT NULL,
    "AciklamaEn" text DEFAULT ''::text NOT NULL,
    "BaslikAr" text DEFAULT ''::text NOT NULL,
    "BaslikEn" text DEFAULT ''::text NOT NULL,
    "KisaAciklamaAr" text DEFAULT ''::text NOT NULL,
    "KisaAciklamaEn" text DEFAULT ''::text NOT NULL,
    "SeoDescriptionAr" text DEFAULT ''::text NOT NULL,
    "SeoDescriptionEn" text DEFAULT ''::text NOT NULL,
    "SeoTitleAr" text DEFAULT ''::text NOT NULL,
    "SeoTitleEn" text DEFAULT ''::text NOT NULL,
    "HediyePaketFiyati" numeric DEFAULT 0.0 NOT NULL,
    "HediyePaketiVarMi" boolean DEFAULT false NOT NULL,
    "WhatsappSiparisVarMi" boolean DEFAULT false NOT NULL,
    "FiyatGizliMi" boolean DEFAULT false NOT NULL,
    "ToptanciUrunGrubuId" integer,
    "KampanyaBitisTarihi" timestamp with time zone,
    "YayindaMi" boolean DEFAULT true NOT NULL
);


ALTER TABLE public."Urunler" OWNER TO kanvasuser;

--
-- Name: Urunler_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."Urunler" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Urunler_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Yorumlar; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."Yorumlar" (
    "Id" integer NOT NULL,
    "UrunId" integer NOT NULL,
    "AppUserId" text,
    "AdSoyad" character varying(50) NOT NULL,
    "YorumMetni" text NOT NULL,
    "Puan" integer NOT NULL,
    "OnayliMi" boolean NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."Yorumlar" OWNER TO kanvasuser;

--
-- Name: Yorumlar_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."Yorumlar" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."Yorumlar_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: ZiyaretciLoglari; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."ZiyaretciLoglari" (
    "Id" integer NOT NULL,
    "IpAdresi" text NOT NULL,
    "Url" text NOT NULL,
    "CihazBilgisi" text NOT NULL,
    "ReferansUrl" text,
    "KullaniciAdi" text,
    "Metod" text NOT NULL,
    "Sehir" text,
    "Ulke" text,
    "Tarayici" text,
    "CihazModeli" text,
    "IsletimSistemi" text,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


ALTER TABLE public."ZiyaretciLoglari" OWNER TO kanvasuser;

--
-- Name: ZiyaretciLoglari_Id_seq; Type: SEQUENCE; Schema: public; Owner: kanvasuser
--

ALTER TABLE public."ZiyaretciLoglari" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public."ZiyaretciLoglari_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: kanvasuser
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO kanvasuser;

--
-- Name: aggregatedcounter id; Type: DEFAULT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.aggregatedcounter ALTER COLUMN id SET DEFAULT nextval('hangfire.aggregatedcounter_id_seq'::regclass);


--
-- Name: counter id; Type: DEFAULT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.counter ALTER COLUMN id SET DEFAULT nextval('hangfire.counter_id_seq'::regclass);


--
-- Name: hash id; Type: DEFAULT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.hash ALTER COLUMN id SET DEFAULT nextval('hangfire.hash_id_seq'::regclass);


--
-- Name: job id; Type: DEFAULT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.job ALTER COLUMN id SET DEFAULT nextval('hangfire.job_id_seq'::regclass);


--
-- Name: jobparameter id; Type: DEFAULT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.jobparameter ALTER COLUMN id SET DEFAULT nextval('hangfire.jobparameter_id_seq'::regclass);


--
-- Name: jobqueue id; Type: DEFAULT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.jobqueue ALTER COLUMN id SET DEFAULT nextval('hangfire.jobqueue_id_seq'::regclass);


--
-- Name: list id; Type: DEFAULT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.list ALTER COLUMN id SET DEFAULT nextval('hangfire.list_id_seq'::regclass);


--
-- Name: set id; Type: DEFAULT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.set ALTER COLUMN id SET DEFAULT nextval('hangfire.set_id_seq'::regclass);


--
-- Name: state id; Type: DEFAULT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.state ALTER COLUMN id SET DEFAULT nextval('hangfire.state_id_seq'::regclass);


--
-- Data for Name: aggregatedcounter; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.aggregatedcounter (id, key, value, expireat) FROM stdin;
\.


--
-- Data for Name: counter; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.counter (id, key, value, expireat) FROM stdin;
\.


--
-- Data for Name: hash; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.hash (id, key, field, value, expireat, updatecount) FROM stdin;
\.


--
-- Data for Name: job; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.job (id, stateid, statename, invocationdata, arguments, createdat, expireat, updatecount) FROM stdin;
\.


--
-- Data for Name: jobparameter; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.jobparameter (id, jobid, name, value, updatecount) FROM stdin;
\.


--
-- Data for Name: jobqueue; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.jobqueue (id, jobid, queue, fetchedat, updatecount) FROM stdin;
\.


--
-- Data for Name: list; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.list (id, key, value, expireat, updatecount) FROM stdin;
\.


--
-- Data for Name: lock; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.lock (resource, updatecount, acquired) FROM stdin;
\.


--
-- Data for Name: schema; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.schema (version) FROM stdin;
22
\.


--
-- Data for Name: server; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.server (id, data, lastheartbeat, updatecount) FROM stdin;
abdulmuin:40204:d36a11d1-8b41-4812-997c-9ccf725735de	{"Queues": ["default"], "StartedAt": "2026-06-25T22:59:20.6279923Z", "WorkerCount": 20}	2026-06-26 13:18:19.854198+00	0
\.


--
-- Data for Name: set; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.set (id, key, score, value, expireat, updatecount) FROM stdin;
\.


--
-- Data for Name: state; Type: TABLE DATA; Schema: hangfire; Owner: kanvasuser
--

COPY hangfire.state (id, jobid, name, reason, createdat, data, updatecount) FROM stdin;
\.


--
-- Data for Name: Adresler; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."Adresler" ("Id", "Baslik", "AdSoyad", "Telefon", "Sehir", "Ilce", "AcikAdres", "AppUserId", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: AspNetRoleClaims; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."AspNetRoleClaims" ("Id", "RoleId", "ClaimType", "ClaimValue") FROM stdin;
\.


--
-- Data for Name: AspNetRoles; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."AspNetRoles" ("Id", "Name", "NormalizedName", "ConcurrencyStamp") FROM stdin;
fe42d608-671b-46c9-add1-ee9711fda800	Admin	ADMIN	\N
2bd01a53-b102-45c8-ae17-4a20b989d8ad	SuperAdmin	SUPERADMIN	\N
9e8c7c63-546d-4363-b400-136a00cd421e	Yonetici	YONETICI	\N
52158fe6-1632-4ae7-a399-96e2d33ed840	SiparisYoneticisi	SIPARISYONETICISI	\N
dc3d77d3-68e1-453f-8ce4-29451840331c	UrunYoneticisi	URUNYONETICISI	\N
6a59347e-97d1-48ce-a747-da234f452412	IcerikYoneticisi	ICERIKYONETICISI	\N
950f188b-e336-4679-93c0-9516226c0d04	KargoYoneticisi	KARGOYONETICISI	\N
86f160eb-4652-4965-8288-db828bc2738d	Goruntuleyici	GORUNTULEYICI	\N
1f8065f6-cdac-4708-b780-414442ded4df	Wholesale	WHOLESALE	\N
e5b7cdf7-fb08-48f8-a0b4-76b5db17d04c	Uye	UYE	\N
\.


--
-- Data for Name: AspNetUserClaims; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."AspNetUserClaims" ("Id", "UserId", "ClaimType", "ClaimValue") FROM stdin;
\.


--
-- Data for Name: AspNetUserLogins; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."AspNetUserLogins" ("LoginProvider", "ProviderKey", "ProviderDisplayName", "UserId") FROM stdin;
\.


--
-- Data for Name: AspNetUserRoles; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."AspNetUserRoles" ("UserId", "RoleId") FROM stdin;
57039d3f-f6b2-498e-9a3b-0e985154adad	fe42d608-671b-46c9-add1-ee9711fda800
fed886bd-cd51-4cb1-9a15-c5447b1c63f4	e5b7cdf7-fb08-48f8-a0b4-76b5db17d04c
\.


--
-- Data for Name: AspNetUserTokens; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."AspNetUserTokens" ("UserId", "LoginProvider", "Name", "Value") FROM stdin;
\.


--
-- Data for Name: AspNetUsers; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."AspNetUsers" ("Id", "AdSoyad", "Sehir", "SifreSifirlamaToken", "SifreSifirlamaGecerlilik", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "DogumTarihi", "KimlikFotografYolu", "KimlikNo", "Adres", "WholesaleStatus", "BasvuruTarihi") FROM stdin;
57039d3f-f6b2-498e-9a3b-0e985154adad	7ANRPS48 Admin	Ramallah	\N	\N	admin@7anrps48.com	ADMIN@7ANRPS48.COM	admin@7anrps48.com	ADMIN@7ANRPS48.COM	t	AQAAAAIAAYagAAAAEF8daTB4ytBcgFB+xxdkYXvabwhIW1ycnDohe7CA3yK9uqGH5UrFbkDnMV7Bn2lSUA==	ZA3GTOI7WYGZ4PTIUN3SY6SCWWJQFCBG	d9da7a54-ffe2-4384-a977-6ef1efbc39f2	\N	f	f	\N	t	0	\N				0	\N
fed886bd-cd51-4cb1-9a15-c5447b1c63f4	Test Kullan─▒c─▒	Gaza	\N	\N	testuser2026@example.com	TESTUSER2026@EXAMPLE.COM	testuser2026@example.com	TESTUSER2026@EXAMPLE.COM	f	AQAAAAIAAYagAAAAEOGAZMSuwPG1w4GrdCTWgqN513JQJOPUhE4ZZXCTA1tSUNHsTTMsLTOflL3Hkab2aw==	YMUQQYAAX7OMGSOK5BHOG2BYMZS6J5NS	8ad46df0-1762-4555-aea3-9f57fc872f08	+970599123456	f	f	\N	t	0	1990-01-15 00:00:00+00	/uploads/kimlikler/f91e1730c21844be86099bce352d6829.png	12345678901	Test Adres, Gaza, Filistin	0	\N
\.


--
-- Data for Name: BankaHesaplari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."BankaHesaplari" ("Id", "BankaAdi", "HesapSahibi", "IBAN", "SubeKodu", "HesapNo", "AktifMi", "Sira", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
1	Bank of Palestine	7ANRPS48 Trading Co.	PS99000102030405060708090	001	102030	t	1	2026-06-25 21:44:12.781099+00	f
2	Palestine National Bank	7ANRPS48 Trading Co.	PS88010203040506070809100	010	304050	t	2	2026-06-25 21:44:12.781099+00	f
\.


--
-- Data for Name: BultenAbonelikleri; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."BultenAbonelikleri" ("Id", "Email", "KayitTarihi", "AktifMi", "IpAdresi") FROM stdin;
\.


--
-- Data for Name: CarkOdulleri; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."CarkOdulleri" ("Id", "LabelTr", "LabelEn", "LabelAr", "Tip", "Deger", "Renk", "MesajTr", "MesajEn", "MesajAr", "Sira", "AktifMi", "OlusturulmaTarihi", "GuncellenmeTarihi", "SilindiMi") FROM stdin;
1	%5 ─░ndirim	5% Off	Ï«ÏÁ┘à 5%	discount	5	#313511	Harika! Sepetinde %5 indirim kazand─▒n!	\N	Ï▒ÏğÏĞÏ╣! ┘ä┘éÏ» Ï▒Ï¿Ï¡Ï¬ Ï«ÏÁ┘à 5%!	1	t	2026-06-23 20:59:42.878242+00	\N	f
2	%10 ─░ndirim	10% Off	Ï«ÏÁ┘à 10%	discount	10	#b58735	Harika! Sepetinde %10 indirim kazand─▒n!	\N	Ï▒ÏğÏĞÏ╣! ┘ä┘éÏ» Ï▒Ï¿Ï¡Ï¬ Ï«ÏÁ┘à 10%!	2	t	2026-06-23 20:59:42.878242+00	\N	f
3	%15 ─░ndirim	15% Off	Ï«ÏÁ┘à 15%	discount	15	#4a4a2e	S├╝per! Sepetinde %15 indirim kazand─▒n!	\N	┘à┘àÏ¬ÏğÏ▓! ┘ä┘éÏ» Ï▒Ï¿Ï¡Ï¬ Ï«ÏÁ┘à 15%!	3	t	2026-06-23 20:59:42.878242+00	\N	f
4	%20 ─░ndirim	20% Off	Ï«ÏÁ┘à 20%	discount	20	#c49a3c	M├╝thi┼ş! Sepetinde %20 indirim kazand─▒n!	\N	Ï▒ÏğÏĞÏ╣! ┘ä┘éÏ» Ï▒Ï¿Ï¡Ï¬ Ï«ÏÁ┘à 20%!	4	t	2026-06-23 20:59:42.878242+00	\N	f
5	%25 ─░ndirim	25% Off	Ï«ÏÁ┘à 25%	discount	25	#313511	Harika! Sepetinde %25 indirim kazand─▒n!	\N	┘àÏ░┘ç┘ä! ┘ä┘éÏ» Ï▒Ï¿Ï¡Ï¬ Ï«ÏÁ┘à 25%!	5	t	2026-06-23 20:59:42.878242+00	\N	f
6	├£cretsiz Kargo	Free Shipping	Ï┤Ï¡┘å ┘àÏ¼Ïğ┘å┘è	freeship	0	#b58735	Tebrikler! ├£cretsiz kargo kazand─▒n!	\N	Ï¬┘çÏğ┘å┘è┘åÏğ! ┘ä┘éÏ» Ï▒Ï¿Ï¡Ï¬ Ï┤Ï¡┘å┘ïÏğ ┘àÏ¼Ïğ┘å┘è┘ïÏğ!	6	t	2026-06-23 20:59:42.878242+00	\N	f
7	Tekrar Dene	Try Again	Ï¡Ïğ┘ê┘ä ┘àÏ▒Ï® ÏúÏ«Ï▒┘ë	none	0	#6f6a5e	\N	\N	Ï¡Ï© Ïú┘ê┘üÏ▒ ┘ü┘è Ïğ┘ä┘àÏ▒Ï® Ïğ┘ä┘éÏğÏ»┘àÏ®!	7	t	2026-06-23 20:59:42.878242+00	\N	f
8	%10 ─░ndirim	10% Off	Ï«ÏÁ┘à 10%	discount	10	#d4a84b	Harika! Sepetinde %10 indirim kazand─▒n!	\N	Ï▒ÏğÏĞÏ╣! ┘ä┘éÏ» Ï▒Ï¿Ï¡Ï¬ Ï«ÏÁ┘à 10%!	8	t	2026-06-23 20:59:42.878242+00	\N	f
\.


--
-- Data for Name: Favoriler; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."Favoriler" ("Id", "AppUserId", "UrunId", "FiyatDustugundaBildir", "EskiFiyat", "SonBildirimTarihi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: HomePageSectionProducts; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."HomePageSectionProducts" ("Id", "SectionId", "UrunId", "SortOrder") FROM stdin;
\.


--
-- Data for Name: HomePageSections; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."HomePageSections" ("Id", "SectionType", "Enabled", "SortOrder", "Title", "Subtitle", "ViewAllText", "ViewAllUrl", "ImageUrl", "Description", "ButtonText", "ButtonUrl") FROM stdin;
1	4	t	10				/Urun	\N	\N	\N	\N
2	5	t	20				/Urun?sort=popularity	\N	\N	\N	\N
3	6	t	30				/Urun?sort=price_asc	\N	\N	\N	\N
\.


--
-- Data for Name: IadeTalepleri; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."IadeTalepleri" ("Id", "SiparisId", "MusteriId", "Neden", "Aciklama", "IBAN", "Durum", "AdminNotu", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: IletisimMesajlari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."IletisimMesajlari" ("Id", "AdSoyad", "Email", "Konu", "Mesaj", "Tarih", "IpAdresi", "OkunduMu") FROM stdin;
\.


--
-- Data for Name: KargoBolgeFiyatlari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."KargoBolgeFiyatlari" ("Id", "KargoFirmasiId", "BolgeId", "Fiyat", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
1	1	1	0.0	2026-06-25 21:44:12.566969+00	f
2	1	2	0.0	2026-06-25 21:44:12.566969+00	f
3	1	3	0.0	2026-06-25 21:44:12.566969+00	f
4	1	4	0.0	2026-06-25 21:44:12.566969+00	f
5	1	5	0.0	2026-06-25 21:44:12.566969+00	f
6	1	6	0.0	2026-06-25 21:44:12.566969+00	f
7	1	7	0.0	2026-06-25 21:44:12.566969+00	f
8	1	8	0.0	2026-06-25 21:44:12.566969+00	f
9	1	9	0.0	2026-06-25 21:44:12.566969+00	f
10	1	10	0.0	2026-06-25 21:44:12.566969+00	f
11	2	1	0.0	2026-06-25 21:44:12.566969+00	f
12	2	2	0.0	2026-06-25 21:44:12.566969+00	f
13	2	3	0.0	2026-06-25 21:44:12.566969+00	f
14	2	4	0.0	2026-06-25 21:44:12.566969+00	f
15	2	5	0.0	2026-06-25 21:44:12.566969+00	f
16	2	6	0.0	2026-06-25 21:44:12.566969+00	f
17	2	7	0.0	2026-06-25 21:44:12.566969+00	f
18	2	8	0.0	2026-06-25 21:44:12.566969+00	f
19	2	9	0.0	2026-06-25 21:44:12.566969+00	f
20	2	10	0.0	2026-06-25 21:44:12.566969+00	f
\.


--
-- Data for Name: KargoBolgeSehirler; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."KargoBolgeSehirler" ("Id", "BolgeId", "SehirAdi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
1	1	Gazze	2026-06-25 21:44:12.566969+00	f
2	1	┼Şimali Gazze	2026-06-25 21:44:12.566969+00	f
3	1	Beyt Hanun	2026-06-25 21:44:12.566969+00	f
4	1	Beyt Lahiya	2026-06-25 21:44:12.566969+00	f
5	1	Cablus	2026-06-25 21:44:12.566969+00	f
6	2	Ramallah	2026-06-25 21:44:12.566969+00	f
7	2	El-Bireh	2026-06-25 21:44:12.566969+00	f
8	2	Birzeit	2026-06-25 21:44:12.566969+00	f
9	2	Abu Dis	2026-06-25 21:44:12.566969+00	f
10	2	Yabrud	2026-06-25 21:44:12.566969+00	f
11	3	Nablus	2026-06-25 21:44:12.566969+00	f
12	3	Tubas	2026-06-25 21:44:12.566969+00	f
13	3	Salfit	2026-06-25 21:44:12.566969+00	f
14	3	Asira	2026-06-25 21:44:12.566969+00	f
15	4	El Halil	2026-06-25 21:44:12.566969+00	f
16	4	Yatta	2026-06-25 21:44:12.566969+00	f
17	4	Dura	2026-06-25 21:44:12.566969+00	f
18	4	Halhul	2026-06-25 21:44:12.566969+00	f
19	4	Sa'ir	2026-06-25 21:44:12.566969+00	f
20	5	Beyt├╝llahim	2026-06-25 21:44:12.566969+00	f
21	5	Beyt Jala	2026-06-25 21:44:12.566969+00	f
22	5	Artas	2026-06-25 21:44:12.566969+00	f
23	5	Husan	2026-06-25 21:44:12.566969+00	f
24	6	Cenin	2026-06-25 21:44:12.566969+00	f
25	6	Ajjah	2026-06-25 21:44:12.566969+00	f
26	6	Kabatiya	2026-06-25 21:44:12.566969+00	f
27	6	Ya'bad	2026-06-25 21:44:12.566969+00	f
28	7	Eriha	2026-06-25 21:44:12.566969+00	f
29	7	El-Auja	2026-06-25 21:44:12.566969+00	f
30	7	Fasayil	2026-06-25 21:44:12.566969+00	f
31	7	Nabi Musa	2026-06-25 21:44:12.566969+00	f
32	8	Tulkerim	2026-06-25 21:44:12.566969+00	f
33	8	Anabta	2026-06-25 21:44:12.566969+00	f
34	8	Nur ┼Şems	2026-06-25 21:44:12.566969+00	f
35	8	Kafr Sur	2026-06-25 21:44:12.566969+00	f
36	9	Kalkilya	2026-06-25 21:44:12.566969+00	f
37	9	Azzun	2026-06-25 21:44:12.566969+00	f
38	9	Kafr Thulth	2026-06-25 21:44:12.566969+00	f
39	9	Hable	2026-06-25 21:44:12.566969+00	f
40	10	Kud├╝s	2026-06-25 21:44:12.566969+00	f
41	10	┼Şarkiyye Kud├╝s	2026-06-25 21:44:12.566969+00	f
42	10	Al-Eizariya	2026-06-25 21:44:12.566969+00	f
43	10	Al-Ram	2026-06-25 21:44:12.566969+00	f
44	10	Abu Dis	2026-06-25 21:44:12.566969+00	f
\.


--
-- Data for Name: KargoBolgeler; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."KargoBolgeler" ("Id", "Ad", "Sira", "OlusturulmaTarihi", "SilindiMi", "Ulke", "Aciklama", "Fiyat") FROM stdin;
1	Gazze	1	2026-06-25 21:44:12.566969+00	f	Filistin	Kuzey Gazze ve Gazze ┼Şehri b├Âlgesi	0
2	Ramallah	2	2026-06-25 21:44:12.566969+00	f	Filistin	Orta Bat─▒ ┼Şeria b├Âlgesi	0
3	Nablus	3	2026-06-25 21:44:12.566969+00	f	Filistin	Kuzey Bat─▒ ┼Şeria b├Âlgesi	0
4	El Halil	4	2026-06-25 21:44:12.566969+00	f	Filistin	G├╝ney Bat─▒ ┼Şeria b├Âlgesi	0
5	Beyt├╝llahim	5	2026-06-25 21:44:12.566969+00	f	Filistin	Bat─▒ ┼Şeria - Kud├╝s'e yak─▒n	0
6	Cenin	6	2026-06-25 21:44:12.566969+00	f	Filistin	Kuzey Filistin b├Âlgesi	0
7	Eriha	7	2026-06-25 21:44:12.566969+00	f	Filistin	├£rd├╝n Vadisi ve Eriha b├Âlgesi	0
8	Tulkerim	8	2026-06-25 21:44:12.566969+00	f	Filistin	Kuzeybat─▒ Bat─▒ ┼Şeria b├Âlgesi	0
9	Kalkilya	9	2026-06-25 21:44:12.566969+00	f	Filistin	Kuzeybat─▒ Bat─▒ ┼Şeria - Kalkilya b├Âlgesi	0
10	Kud├╝s	10	2026-06-25 21:44:12.566969+00	f	Filistin	Kud├╝s ve ├ğevresi	0
\.


--
-- Data for Name: KargoFirmalari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."KargoFirmalari" ("Id", "Ad", "Kod", "LogoUrl", "Telefon", "TakipUrl", "GondericiUnvan", "GondericiAdres", "GondericiTelefon", "AktifMi", "VarsayilanMi", "Fiyat", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
1	Aras Kargo	aras	/aras-logo.svg	444 25 52	https://www.araskargo.com.tr/tr/online-islemler/kargo-takip	Canvasia	Merkez Mah. Sanat Sok. No:5 ─░stanbul / T├╝rkiye	0850 123 45 67	t	t	0	2026-06-14 08:15:13.705997+00	f
2	Filistin Kargo	filistin-kargo	/filistin-kargo-logo.svg	0599 123 456	https://track.7anrps48.com/kargo/	7ANRPS48	Ramallah, El-Bireh, Palestine	+970 59 912 3456	t	t	0	2026-06-25 21:44:11.841216+00	f
\.


--
-- Data for Name: Kategoriler; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."Kategoriler" ("Id", "Ad", "KisaAciklama", "Aciklama", "Slug", "GorselUrl", "BannerUrl", "SeoTitle", "SeoDescription", "UstMetin", "AltMetin", "KampanyaEtiketi", "UrunSiralamaTipi", "Sira", "AktifMi", "ParentKategoriId", "OlusturulmaTarihi", "SilindiMi", "AciklamaAr", "AciklamaEn", "AdAr", "AdEn", "AltMetinAr", "AltMetinEn", "KampanyaEtiketiAr", "KampanyaEtiketiEn", "KisaAciklamaAr", "KisaAciklamaEn", "SeoDescriptionAr", "SeoDescriptionEn", "SeoTitleAr", "SeoTitleEn", "UstMetinAr", "UstMetinEn", "ReceteGerekliMi") FROM stdin;
4	Kud├╝s Koleksiyonu	Kud├╝s esintili ├Âzel ├╝r├╝nler.	Kud├╝s temal─▒ se├ğili ├╝r├╝nler.	kudus-koleksiyonu			Kud├╝s Koleksiyonu	Kud├╝s esintili ├Âzel ├╝r├╝nler.				manual	1	t	1	2026-06-19 11:21:33.814335+00	f	┘à┘åÏ¬Ï¼ÏğÏ¬ ┘àÏ«Ï¬ÏğÏ▒Ï® Ï¿ÏÀÏğÏ¿Ï╣ Ïğ┘ä┘éÏ»Ï│.	Selected Jerusalem themed products.	┘àÏ¼┘à┘êÏ╣Ï® Ïğ┘ä┘éÏ»Ï│	Jerusalem Collection					┘à┘åÏ¬Ï¼ÏğÏ¬ Ï«ÏğÏÁÏ® ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å Ïğ┘ä┘éÏ»Ï│.	Jerusalem inspired special products.	┘à┘åÏ¬Ï¼ÏğÏ¬ Ï«ÏğÏÁÏ® ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å Ïğ┘ä┘éÏ»Ï│.	Jerusalem inspired special products.	┘àÏ¼┘à┘êÏ╣Ï® Ïğ┘ä┘éÏ»Ï│	Jerusalem Collection			f
5	Gazze Dayan─▒┼şma	Gazze ruhunu ta┼ş─▒yan anlaml─▒ ├╝r├╝nler.	Gazze temal─▒ ├╝r├╝nler.	gazze-dayanisma			Gazze Dayan─▒┼şma	Gazze ruhunu ta┼ş─▒yan anlaml─▒ ├╝r├╝nler.				manual	2	t	1	2026-06-19 11:21:33.814335+00	f	┘à┘åÏ¬Ï¼ÏğÏ¬ Ï¿ÏÀÏğÏ¿Ï╣ Ï║Ï▓Ï®.	Gaza themed products.	Ï¬ÏÂÏğ┘à┘å Ï║Ï▓Ï®	Gaza Solidarity					┘à┘åÏ¬Ï¼ÏğÏ¬ ┘àÏ╣Ï¿Ï▒Ï® Ï¬Ï¡┘à┘ä Ï▒┘êÏ¡ Ï║Ï▓Ï®.	Meaningful products carrying the spirit of Gaza.	┘à┘åÏ¬Ï¼ÏğÏ¬ ┘àÏ╣Ï¿Ï▒Ï® Ï¬Ï¡┘à┘ä Ï▒┘êÏ¡ Ï║Ï▓Ï®.	Meaningful products carrying the spirit of Gaza.	Ï¬ÏÂÏğ┘à┘å Ï║Ï▓Ï®	Gaza Solidarity			f
6	Harita & Bayrak	Harita ve bayrak temal─▒ tasar─▒mlar.	Filistin haritas─▒ ve bayra─ş─▒ temal─▒ ├╝r├╝nler.	harita-bayrak			Harita & Bayrak	Harita ve bayrak temal─▒ tasar─▒mlar.				manual	3	t	1	2026-06-19 11:21:33.814335+00	f	┘à┘åÏ¬Ï¼ÏğÏ¬ Ï¿ÏÀÏğÏ¿Ï╣ Ï«Ï▒┘èÏÀÏ® ┘êÏ╣┘ä┘à ┘ü┘äÏ│ÏÀ┘è┘å.	Palestine map and flag themed products.	Ïğ┘äÏ«Ï▒┘èÏÀÏ® ┘êÏğ┘äÏ╣┘ä┘à	Map & Flag					Ï¬ÏÁÏğ┘à┘è┘à Ï¿ÏÀÏğÏ¿Ï╣ Ïğ┘äÏ«Ï▒┘èÏÀÏ® ┘êÏğ┘äÏ╣┘ä┘à.	Map and flag themed designs.	Ï¬ÏÁÏğ┘à┘è┘à Ï¿ÏÀÏğÏ¿Ï╣ Ïğ┘äÏ«Ï▒┘èÏÀÏ® ┘êÏğ┘äÏ╣┘ä┘à.	Map and flag themed designs.	Ïğ┘äÏ«Ï▒┘èÏÀÏ® ┘êÏğ┘äÏ╣┘ä┘à	Map & Flag			f
7	Salon Dekoru	Salon i├ğin ┼ş─▒k tamamlay─▒c─▒lar.	Salon dekorasyon ├╝r├╝nleri.	salon-dekoru			Salon Dekoru	Salon i├ğin ┼ş─▒k tamamlay─▒c─▒lar.				manual	1	t	2	2026-06-19 11:21:33.814335+00	f	┘à┘åÏ¬Ï¼ÏğÏ¬ Ï»┘è┘â┘êÏ▒ Ï║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®.	Living room decoration products.	Ï»┘è┘â┘êÏ▒ Ï║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®	Living Room Decor					┘ä┘àÏ│ÏğÏ¬ Ïú┘å┘è┘éÏ® ┘äÏ║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®.	Stylish accents for living rooms.	┘ä┘àÏ│ÏğÏ¬ Ïú┘å┘è┘éÏ® ┘äÏ║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®.	Stylish accents for living rooms.	Ï»┘è┘â┘êÏ▒ Ï║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®	Living Room Decor			f
8	Duvar Dekoru	Duvarlara karakter katan ├╝r├╝nler.	Duvar dekorasyonu se├ğkisi.	duvar-dekoru			Duvar Dekoru	Duvarlara karakter katan ├╝r├╝nler.				manual	2	t	2	2026-06-19 11:21:33.814335+00	f	┘àÏ«Ï¬ÏğÏ▒ÏğÏ¬ Ï»┘è┘â┘êÏ▒ Ïğ┘äÏ¼Ï»Ï▒Ïğ┘å.	Wall decoration selection.	Ï»┘è┘â┘êÏ▒ Ïğ┘äÏ¼Ï»Ï▒Ïğ┘å	Wall Decor					┘à┘åÏ¬Ï¼ÏğÏ¬ Ï¬ÏÂ┘è┘ü ÏÀÏğÏ¿Ï╣Ïğ┘ï ┘ä┘äÏ¼Ï»Ï▒Ïğ┘å.	Products that add character to walls.	┘à┘åÏ¬Ï¼ÏğÏ¬ Ï¬ÏÂ┘è┘ü ÏÀÏğÏ¿Ï╣Ïğ┘ï ┘ä┘äÏ¼Ï»Ï▒Ïğ┘å.	Products that add character to walls.	Ï»┘è┘â┘êÏ▒ Ïğ┘äÏ¼Ï»Ï▒Ïğ┘å	Wall Decor			f
9	Mutfak & Sofra	Mutfak ve sofra i├ğin zarif ├╝r├╝nler.	Mutfak ve sofra ├╝r├╝nleri.	mutfak-sofra			Mutfak & Sofra	Mutfak ve sofra i├ğin zarif ├╝r├╝nler.				manual	3	t	2	2026-06-19 11:21:33.814335+00	f	┘à┘åÏ¬Ï¼ÏğÏ¬ Ïğ┘ä┘àÏÀÏ¿Ï« ┘êÏğ┘ä┘àÏğÏĞÏ»Ï®.	Kitchen and table products.	Ïğ┘ä┘àÏÀÏ¿Ï« ┘êÏğ┘ä┘àÏğÏĞÏ»Ï®	Kitchen & Table					┘à┘åÏ¬Ï¼ÏğÏ¬ Ïú┘å┘è┘éÏ® ┘ä┘ä┘àÏÀÏ¿Ï« ┘êÏğ┘ä┘àÏğÏĞÏ»Ï®.	Elegant products for kitchen and table.	┘à┘åÏ¬Ï¼ÏğÏ¬ Ïú┘å┘è┘éÏ® ┘ä┘ä┘àÏÀÏ¿Ï« ┘êÏğ┘ä┘àÏğÏĞÏ»Ï®.	Elegant products for kitchen and table.	Ïğ┘ä┘àÏÀÏ¿Ï« ┘êÏğ┘ä┘àÏğÏĞÏ»Ï®	Kitchen & Table			f
10	Ki┼şiye ├ûzel Hediyeler	Ki┼şiye ├Âzel anlaml─▒ hediyeler.	Ki┼şiselle┼ştirilebilir hediyelik ├╝r├╝nler.	kisiye-ozel-hediyeler			Ki┼şiye ├ûzel Hediyeler	Ki┼şiye ├Âzel anlaml─▒ hediyeler.				manual	1	t	3	2026-06-19 11:21:33.814335+00	f	┘à┘åÏ¬Ï¼ÏğÏ¬ ┘çÏ»Ïğ┘èÏğ ┘éÏğÏ¿┘äÏ® ┘ä┘äÏ¬Ï«ÏÁ┘èÏÁ.	Customizable gift products.	┘çÏ»Ïğ┘èÏğ ┘àÏ«ÏÁÏÁÏ®	Personalized Gifts					┘çÏ»Ïğ┘èÏğ ┘àÏ«ÏÁÏÁÏ® ┘êÏ░ÏğÏ¬ ┘àÏ╣┘å┘ë.	Meaningful personalized gifts.	┘çÏ»Ïğ┘èÏğ ┘àÏ«ÏÁÏÁÏ® ┘êÏ░ÏğÏ¬ ┘àÏ╣┘å┘ë.	Meaningful personalized gifts.	┘çÏ»Ïğ┘èÏğ ┘àÏ«ÏÁÏÁÏ®	Personalized Gifts			f
11	Anahtarl─▒k & Magnet	K├╝├ğ├╝k ama anlaml─▒ hediyeler.	Anahtarl─▒k ve magnet ├╝r├╝nleri.	anahtarlik-magnet			Anahtarl─▒k & Magnet	K├╝├ğ├╝k ama anlaml─▒ hediyeler.				manual	2	t	3	2026-06-19 11:21:33.814335+00	f	┘à┘åÏ¬Ï¼ÏğÏ¬ Ïğ┘ä┘à┘èÏ»Ïğ┘ä┘èÏğÏ¬ ┘êÏğ┘ä┘àÏ║┘åÏğÏÀ┘èÏ│.	Keychain and magnet products.	┘à┘èÏ»Ïğ┘ä┘èÏğÏ¬ ┘ê┘àÏ║┘åÏğÏÀ┘èÏ│	Keychain & Magnet					┘çÏ»Ïğ┘èÏğ ÏÁÏ║┘èÏ▒Ï® ┘ä┘â┘å┘çÏğ ┘àÏ╣Ï¿Ï▒Ï®.	Small but meaningful gifts.	┘çÏ»Ïğ┘èÏğ ÏÁÏ║┘èÏ▒Ï® ┘ä┘â┘å┘çÏğ ┘àÏ╣Ï¿Ï▒Ï®.	Small but meaningful gifts.	┘à┘èÏ»Ïğ┘ä┘èÏğÏ¬ ┘ê┘àÏ║┘åÏğÏÀ┘èÏ│	Keychain & Magnet			f
12	├çanta & Aksesuar	G├╝nl├╝k kullan─▒ma uygun aksesuarlar.	├çanta ve aksesuar se├ğkisi.	canta-aksesuar			├çanta & Aksesuar	G├╝nl├╝k kullan─▒ma uygun aksesuarlar.				manual	3	t	3	2026-06-19 11:21:33.814335+00	f	┘àÏ«Ï¬ÏğÏ▒ÏğÏ¬ Ïğ┘äÏ¡┘éÏğÏĞÏ¿ ┘êÏğ┘äÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬.	Bags and accessories selection.	Ïğ┘äÏ¡┘éÏğÏĞÏ¿ ┘êÏğ┘äÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬	Bags & Accessories					ÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬ ┘à┘åÏğÏ│Ï¿Ï® ┘ä┘äÏğÏ│Ï¬Ï«Ï»Ïğ┘à Ïğ┘ä┘è┘ê┘à┘è.	Accessories for daily use.	ÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬ ┘à┘åÏğÏ│Ï¿Ï® ┘ä┘äÏğÏ│Ï¬Ï«Ï»Ïğ┘à Ïğ┘ä┘è┘ê┘à┘è.	Accessories for daily use.	Ïğ┘äÏ¡┘éÏğÏĞÏ¿ ┘êÏğ┘äÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬	Bags & Accessories			f
1	Filistin Temal─▒	Filistin ruhunu ta┼ş─▒yan ├Âzel se├ğkiler.	Filistin k├╝lt├╝r├╝, ┼şehirleri ve sembollerinden ilham alan ├╝r├╝nler.	filistin-temali	\N	\N	Filistin Temal─▒ ├£r├╝nler	Filistin temal─▒ ├Âzel ├╝r├╝n koleksiyonu.				manual	1	t	\N	2026-06-19 11:21:33.814335+00	f	┘à┘åÏ¬Ï¼ÏğÏ¬ ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å Ïğ┘äÏ½┘éÏğ┘üÏ® ┘êÏğ┘ä┘àÏ»┘å ┘êÏğ┘äÏ▒┘à┘êÏ▓ Ïğ┘ä┘ü┘äÏ│ÏÀ┘è┘å┘èÏ®.	Products inspired by Palestinian culture, cities and symbols.	┘àÏ│Ï¬┘êÏ¡┘ë ┘à┘å ┘ü┘äÏ│ÏÀ┘è┘å	Palestine Inspired					┘àÏ«Ï¬ÏğÏ▒ÏğÏ¬ ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å ┘ü┘äÏ│ÏÀ┘è┘å.	Curated pieces inspired by Palestine.	┘àÏ¼┘à┘êÏ╣Ï® ┘à┘åÏ¬Ï¼ÏğÏ¬ Ï«ÏğÏÁÏ® ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å ┘ü┘äÏ│ÏÀ┘è┘å.	A special Palestine inspired product collection.	┘à┘åÏ¬Ï¼ÏğÏ¬ ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å ┘ü┘äÏ│ÏÀ┘è┘å	Palestine Inspired Products			f
2	Ev & Dekorasyon	Ya┼şam alanlar─▒ i├ğin zarif dekorasyon ├╝r├╝nleri.	Evinize s─▒cakl─▒k ve karakter katan dekoratif ├╝r├╝nler.	ev-dekorasyon	\N	\N	Ev ve Dekorasyon	Ev dekorasyonu i├ğin se├ğili ├╝r├╝nler.				manual	2	t	\N	2026-06-19 11:21:33.814335+00	f	┘à┘åÏ¬Ï¼ÏğÏ¬ Ï»┘è┘â┘êÏ▒ Ï¬ÏÂ┘è┘ü Ïğ┘äÏ»┘üÏí ┘êÏğ┘äÏ┤Ï«ÏÁ┘èÏ® ÏÑ┘ä┘ë ┘à┘åÏ▓┘ä┘â.	Decorative products that add warmth and character to your home.	Ïğ┘ä┘à┘åÏ▓┘ä ┘êÏğ┘äÏ»┘è┘â┘êÏ▒	Home & Decoration					┘éÏÀÏ╣ Ï»┘è┘â┘êÏ▒ Ïú┘å┘è┘éÏ® ┘ä┘àÏ│ÏğÏ¡ÏğÏ¬ Ïğ┘ä┘àÏ╣┘èÏ┤Ï®.	Elegant decoration pieces for living spaces.	┘à┘åÏ¬Ï¼ÏğÏ¬ ┘àÏ«Ï¬ÏğÏ▒Ï® ┘äÏ»┘è┘â┘êÏ▒ Ïğ┘ä┘à┘åÏ▓┘ä.	Selected products for home decoration.	Ïğ┘ä┘à┘åÏ▓┘ä ┘êÏğ┘äÏ»┘è┘â┘êÏ▒	Home and Decoration			f
3	Hediyelik & Aksesuar	Anlaml─▒ hediyeler ve tamamlay─▒c─▒ aksesuarlar.	Sevdikleriniz i├ğin ├Âzel hediyeler ve g├╝nl├╝k kullan─▒ma uygun aksesuarlar.	hediyelik-aksesuar	\N	\N	Hediyelik ve Aksesuar	Hediyelik ├╝r├╝nler ve aksesuar koleksiyonu.				manual	3	t	\N	2026-06-19 11:21:33.814335+00	f	┘çÏ»Ïğ┘èÏğ Ï«ÏğÏÁÏ® ┘ä┘äÏúÏ¡Ï¿Ï® ┘êÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬ ┘ä┘äÏğÏ│Ï¬Ï«Ï»Ïğ┘à Ïğ┘ä┘è┘ê┘à┘è.	Special gifts for loved ones and everyday accessories.	Ïğ┘ä┘çÏ»Ïğ┘èÏğ ┘êÏğ┘äÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬	Gifts & Accessories					┘çÏ»Ïğ┘èÏğ ┘à┘à┘èÏ▓Ï® ┘êÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬ ┘à┘â┘à┘äÏ®.	Meaningful gifts and complementary accessories.	┘àÏ¼┘à┘êÏ╣Ï® Ïğ┘ä┘çÏ»Ïğ┘èÏğ ┘êÏğ┘äÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬.	Gift products and accessories collection.	Ïğ┘ä┘çÏ»Ïğ┘èÏğ ┘êÏğ┘äÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬	Gifts and Accessories			f
13	├à┬Şehir ve Mimari			sehir-ve-mimari	\N	\N	├à┬Şehir ve Mimari - 7ANRPS48	├à┬Şehir ve Mimari kategorisindeki kanvas tablolar				manual	1	t	\N	2026-06-25 21:44:12.150666+00	f																	f
14	Manzara Temal├ä┬▒			manzara-temali	\N	\N	Manzara Temal├ä┬▒ - 7ANRPS48	Manzara Temal├ä┬▒ kategorisindeki kanvas tablolar				manual	2	t	\N	2026-06-25 21:44:12.156326+00	f																	f
15	├âÔÇíi├â┬ğekli ve Dekoratif			cicekli-ve-dekoratif	\N	\N	├âÔÇíi├â┬ğekli ve Dekoratif - 7ANRPS48	├âÔÇíi├â┬ğekli ve Dekoratif kategorisindeki kanvas tablolar				manual	3	t	\N	2026-06-25 21:44:12.156865+00	f																	f
16	Hayvanlar Alemi			hayvanlar-alemi	\N	\N	Hayvanlar Alemi - 7ANRPS48	Hayvanlar Alemi kategorisindeki kanvas tablolar				manual	4	t	\N	2026-06-25 21:44:12.157049+00	f																	f
17	Modern & Soyut			modern-ve-soyut	\N	\N	Modern & Soyut - 7ANRPS48	Modern & Soyut kategorisindeki kanvas tablolar				manual	5	t	\N	2026-06-25 21:44:12.157163+00	f																	f
18	├âÔÇíocuk Odas├ä┬▒			cocuk-odasi	\N	\N	├âÔÇíocuk Odas├ä┬▒ - 7ANRPS48	├âÔÇíocuk Odas├ä┬▒ kategorisindeki kanvas tablolar				manual	6	t	\N	2026-06-25 21:44:12.157216+00	f																	f
19	Farkl├ä┬▒ Tasar├ä┬▒mlar			farkli-tasarimlar	\N	\N	Farkl├ä┬▒ Tasar├ä┬▒mlar - 7ANRPS48	Farkl├ä┬▒ Tasar├ä┬▒mlar kategorisindeki kanvas tablolar				manual	7	t	\N	2026-06-25 21:44:12.157257+00	f																	f
20	Dini ve Hat Sanat├ä┬▒			dini-ve-hat-sanati	\N	\N	Dini ve Hat Sanat├ä┬▒ - 7ANRPS48	Dini ve Hat Sanat├ä┬▒ kategorisindeki kanvas tablolar				manual	8	t	\N	2026-06-25 21:44:12.157303+00	f																	f
21	Klasik Eserler			klasik-eserler	\N	\N	Klasik Eserler - 7ANRPS48	Klasik Eserler kategorisindeki kanvas tablolar				manual	9	t	\N	2026-06-25 21:44:12.157348+00	f																	f
22	Atat├â┬╝rk ve T├â┬╝rkiye			ataturk-ve-turkiye	\N	\N	Atat├â┬╝rk ve T├â┬╝rkiye - 7ANRPS48	Atat├â┬╝rk ve T├â┬╝rkiye kategorisindeki kanvas tablolar				manual	10	t	\N	2026-06-25 21:44:12.157394+00	f																	f
23	Kad├ä┬▒n Temal├ä┬▒			kadin-temali	\N	\N	Kad├ä┬▒n Temal├ä┬▒ - 7ANRPS48	Kad├ä┬▒n Temal├ä┬▒ kategorisindeki kanvas tablolar				manual	11	t	\N	2026-06-25 21:44:12.157443+00	f																	f
24	Yiyecek ve ├ä┬░├â┬ğecek			yiyecek-ve-─▒cecek	\N	\N	Yiyecek ve ├ä┬░├â┬ğecek - 7ANRPS48	Yiyecek ve ├ä┬░├â┬ğecek kategorisindeki kanvas tablolar				manual	12	t	\N	2026-06-25 21:44:12.157496+00	f																	f
25	Osmanl├ä┬▒ ve Tu├ä┼©ra			osmanli-ve-tugra	\N	\N	Osmanl├ä┬▒ ve Tu├ä┼©ra - 7ANRPS48	Osmanl├ä┬▒ ve Tu├ä┼©ra kategorisindeki kanvas tablolar				manual	13	t	\N	2026-06-25 21:44:12.157537+00	f																	f
26	Soyut ve Sanatsal			soyut-ve-sanatsal	\N	\N	Soyut ve Sanatsal - 7ANRPS48	Soyut ve Sanatsal kategorisindeki kanvas tablolar				manual	14	t	\N	2026-06-25 21:44:12.157584+00	f																	f
\.


--
-- Data for Name: Kuponlar; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."Kuponlar" ("Id", "Kod", "Tip", "Deger", "MinSepetTutari", "SonKullanmaTarihi", "KullanimLimiti", "KullanilanMiktar", "AktifMi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: KurumsalSayfalar; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."KurumsalSayfalar" ("Id", "Baslik", "Icerik", "UrlSlug", "Sira", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: PushAbonelikleri; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."PushAbonelikleri" ("Id", "Token", "AppUserId", "Tarayici", "Platform", "AktifMi", "SonGorulmeTarihi", "Tercihler", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: SenTasarla; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."SenTasarla" ("Id", "KullaniciId", "DosyaYolu", "Olcu", "Fiyat", "Efekt", "CercveliMi", "ParcaSayisi", "OlusturmaTarihi", "SepeteEklendi", "SessionId") FROM stdin;
\.


--
-- Data for Name: SepetItems; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."SepetItems" ("Id", "SepetId", "UrunId", "UrunSecenekId", "Adet", "Fiyat", "UrunBaslik", "UrunResimUrl", "CerceveModeli", "SecenekAdi", "MusteriNotu", "OlusturulmaTarihi", "SilindiMi", "HediyePaketFiyati", "HediyePaketi") FROM stdin;
1	5	3	3	1	75.00	Filistin Hat─▒ra Hediye Kutusu	/slider4.png		Premium / Dikey | SKU: DEMO-003-STD	\N	2026-06-19 20:09:33.094699+00	f	0	f
2	5	2	2	1	120.00	Zeytin Dal─▒ Dekoratif Set	/slider5.png		Premium / Dikey | SKU: DEMO-002-STD	\N	2026-06-19 20:09:39.116367+00	f	0	f
3	6	3	3	3	75.00	Filistin Hat─▒ra Hediye Kutusu	/slider4.png		Premium / Dikey | SKU: DEMO-003-STD	\N	2026-06-23 20:12:00.576289+00	f	8.00	t
4	5	1	1	1	95.00	Filistin Motifli Duvar Tablosu	/slider4.png		Premium / Dikey | SKU: DEMO-001-STD	\N	2026-06-25 21:48:52.307333+00	f	0	f
5	16	13	13	1	200	Modern Soyut Kanvas Tablo			30x40 cm	\N	2026-06-25 22:19:22.903784+00	f	0	f
6	17	13	13	1	200	Modern Soyut Kanvas Tablo			30x40 cm	\N	2026-06-25 22:19:34.427194+00	f	0	f
7	28	13	13	1	200	Modern Soyut Kanvas Tablo			30x40 cm	\N	2026-06-25 23:00:10.183984+00	f	0	f
8	28	1	1	1	95.00	Filistin Motifli Duvar Tablosu	/slider4.png		Premium / Dikey | SKU: DEMO-001-STD	\N	2026-06-25 23:00:12.468193+00	f	0	f
9	33	13	13	1	200	Modern Soyut Kanvas Tablo			30x40 cm	\N	2026-06-25 23:02:34.068169+00	f	0	f
10	36	13	13	1	200	Modern Soyut Kanvas Tablo			30x40 cm	\N	2026-06-25 23:03:19.146587+00	f	0	f
11	36	1	1	1	95.00	Filistin Motifli Duvar Tablosu	/slider4.png		Premium / Dikey | SKU: DEMO-001-STD	\N	2026-06-25 23:03:20.929485+00	f	0	f
\.


--
-- Data for Name: Sepetler; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."Sepetler" ("Id", "AppUserId", "SessionId", "SonGuncellemeTarihi", "TerkEdildi", "TerkEdilmeTarihi", "HatirlatmaGonderildi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
1	\N	c1a0ef8c-17a5-ea0d-898e-c3081436b8aa	2026-06-14 08:18:15.908051+00	f	\N	f	2026-06-14 08:18:15.908051+00	f
2	\N	a0ea19f4-fbc9-6fe0-6b2c-8ebfd14992a3	2026-06-14 08:19:36.515153+00	f	\N	f	2026-06-14 08:19:36.515153+00	f
3	\N	dd9501ac-371a-e43d-956f-9565e194202d	2026-06-19 11:15:44.495134+00	f	\N	f	2026-06-19 11:15:44.495134+00	f
4	\N	e470c2d4-5b04-13cf-b8f7-0c2e02cffa41	2026-06-19 19:22:11.430437+00	f	\N	f	2026-06-19 19:22:11.430437+00	f
6	\N	e076be36-d2da-2c12-beb2-68a51fb65a46	2026-06-23 20:12:00.627844+00	f	\N	f	2026-06-23 20:12:00.424862+00	f
5	57039d3f-f6b2-498e-9a3b-0e985154adad	\N	2026-06-25 21:48:52.312383+00	f	\N	f	2026-06-19 20:08:54.846332+00	f
7	\N	0205a138-f008-0dd1-0297-87c445513188	2026-06-25 22:10:09.138358+00	f	\N	f	2026-06-25 22:10:09.138356+00	f
8	\N	5cf46efa-6f3c-0ed7-50fd-992d43311710	2026-06-25 22:10:11.89857+00	f	\N	f	2026-06-25 22:10:11.89857+00	f
9	\N	49fbb7c1-a955-98c0-35ff-87b101cad52a	2026-06-25 22:10:11.915931+00	f	\N	f	2026-06-25 22:10:11.915931+00	f
10	\N	66bfcd9d-4722-1089-057f-4a4f5e97d2c3	2026-06-25 22:10:39.151459+00	f	\N	f	2026-06-25 22:10:39.151459+00	f
11	\N	6b33b69d-e0b4-fdfb-a2c1-45ae3ae48a90	2026-06-25 22:10:42.116689+00	f	\N	f	2026-06-25 22:10:42.116688+00	f
12	\N	2c7e9bc5-fc52-ce71-bfdc-ff66b9906f0a	2026-06-25 22:10:42.134622+00	f	\N	f	2026-06-25 22:10:42.134621+00	f
13	\N	94931f2a-fd97-6bd2-c8a6-95b338e13613	2026-06-25 22:11:10.515626+00	f	\N	f	2026-06-25 22:11:10.515625+00	f
14	\N	561110f0-2f9d-0586-c1f8-5b6e42f0295a	2026-06-25 22:11:13.422608+00	f	\N	f	2026-06-25 22:11:13.422607+00	f
15	\N	22c6e8c6-3935-83b2-1709-4d44e80e0fb9	2026-06-25 22:11:13.438429+00	f	\N	f	2026-06-25 22:11:13.438429+00	f
16	\N	74af3397-fe68-0b60-c87b-f26d8b5d8fb4	2026-06-25 22:19:22.903978+00	f	\N	f	2026-06-25 22:19:22.886499+00	f
17	\N	8ff748cb-914f-d2c8-23d7-27b1af8e5e90	2026-06-25 22:19:34.427314+00	f	\N	f	2026-06-25 22:19:34.408851+00	f
18	\N	4869f9f6-8639-ad5a-f9e3-64bb09c56b23	2026-06-25 22:22:19.203304+00	f	\N	f	2026-06-25 22:22:19.203303+00	f
19	\N	fdcea3ed-fd9c-2ed4-c047-7986bf71ace3	2026-06-25 22:22:20.570273+00	f	\N	f	2026-06-25 22:22:20.570272+00	f
20	\N	953e69f6-f266-46c2-17dc-78d8f2d8c1f6	2026-06-25 22:22:20.589652+00	f	\N	f	2026-06-25 22:22:20.589651+00	f
21	\N	caacce29-0beb-2c9c-ffe4-434faa8f4cd7	2026-06-25 22:22:32.824742+00	f	\N	f	2026-06-25 22:22:32.824741+00	f
22	\N	cdf6c78b-f70a-3680-bca1-bb1f2600a73e	2026-06-25 22:22:34.194318+00	f	\N	f	2026-06-25 22:22:34.194317+00	f
23	\N	c0437b95-145d-4e2f-dd6c-cd80149771d4	2026-06-25 22:22:34.212528+00	f	\N	f	2026-06-25 22:22:34.212528+00	f
24	\N	c02e807a-adef-a350-cae0-b65ca976e2ef	2026-06-25 22:22:46.801016+00	f	\N	f	2026-06-25 22:22:46.801016+00	f
25	\N	18be6f3a-1592-1a1e-0691-a6a558ada6cc	2026-06-25 22:22:48.203988+00	f	\N	f	2026-06-25 22:22:48.203987+00	f
26	\N	a01fcc33-d018-768b-efb6-23d35f32f159	2026-06-25 22:22:48.219291+00	f	\N	f	2026-06-25 22:22:48.219291+00	f
27	\N	194b8062-6702-9b16-ab29-40f80521c0a4	2026-06-25 22:59:26.319944+00	f	\N	f	2026-06-25 22:59:26.319944+00	f
28	\N	1d03aceb-4d4a-2269-9e50-1f3b6a9b3bd3	2026-06-25 23:00:12.468332+00	f	\N	f	2026-06-25 23:00:10.164298+00	f
29	\N	87bd2a4c-6667-ee01-242e-42cc3ff40bdb	2026-06-25 23:00:44.542973+00	f	\N	f	2026-06-25 23:00:44.542973+00	f
30	\N	b147ab75-04ee-050d-8980-f381326b52eb	2026-06-25 23:00:44.560141+00	f	\N	f	2026-06-25 23:00:44.560141+00	f
31	\N	f918beb7-e4c3-67d5-3313-9b4388ba7fae	2026-06-25 23:01:03.432292+00	f	\N	f	2026-06-25 23:01:03.432292+00	f
32	\N	6e11b636-d754-61fc-3be8-28bb6ff598cb	2026-06-25 23:01:03.444589+00	f	\N	f	2026-06-25 23:01:03.444589+00	f
33	\N	f9201567-68b8-5b8f-1a84-368c78a2f309	2026-06-25 23:02:34.068244+00	f	\N	f	2026-06-25 23:02:34.062527+00	f
34	\N	d8730bd6-0528-4491-587b-b369450f348e	2026-06-25 23:02:45.616129+00	f	\N	f	2026-06-25 23:02:45.616129+00	f
35	\N	2b08cf8f-9cb6-2c2a-051a-df00ee569abb	2026-06-25 23:02:53.089075+00	f	\N	f	2026-06-25 23:02:53.089075+00	f
36	\N	04d5c621-3015-8139-39e4-b948bcacd7f1	2026-06-25 23:03:20.929606+00	f	\N	f	2026-06-25 23:03:19.14094+00	f
37	\N	a9fa5d90-ef57-47a1-e29e-79aae4d10d67	2026-06-25 23:03:37.25925+00	f	\N	f	2026-06-25 23:03:37.25925+00	f
\.


--
-- Data for Name: SiparisDetaylari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."SiparisDetaylari" ("Id", "SiparisId", "UrunSecenekId", "Adet", "BirimFiyat", "UrunId", "CerceveModeli", "MusteriNotu", "OlusturulmaTarihi", "SilindiMi", "HediyePaketFiyati", "HediyePaketi") FROM stdin;
\.


--
-- Data for Name: Siparisler; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."Siparisler" ("Id", "AppUserId", "SiparisNo", "KuponKodu", "IndirimTutari", "MusteriAdSoyad", "Telefon", "Eposta", "Sehir", "Ilce", "AcikAdres", "ToplamTutar", "Durum", "KargoTakipNo", "KargoFirmasiId", "KargoFirmasi", "KargoUcreti", "Aciklama", "EmailHashKodu", "FaturaDosyaYolu", "FaturaDosyaAdi", "FaturaYuklendiMi", "FaturaYuklenmeTarihi", "FaturaMailGonderildiMi", "FaturaMailGonderimTarihi", "OlusturulmaTarihi", "SilindiMi", "ReceteDosyaYolu", "TeslimatTipi", "KimlikFotoYolu", "KapidaOdemeHizmetBedeli", "OdemeYontemi", "ReceteOnayDurumu", "ReceteRedSebebi") FROM stdin;
\.


--
-- Data for Name: SiteAyarlari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."SiteAyarlari" ("Id", "SiteAdi", "MarkaAdi", "SiteBasligi", "SiteAciklamasi", "SiteLogoUrl", "FaviconUrl", "BaseUrl", "TemaRengi", "UstBarMesaji", "KampanyaMesaji", "UstBarEtkin", "UstBarHizi", "FooterAciklamasi", "Telefon", "Email", "Adres", "WhatsappNumarasi", "CalismaSaatleri", "FacebookUrl", "InstagramUrl", "TwitterUrl", "YoutubeUrl", "TiktokUrl", "PinterestUrl", "ParaBirimi", "KargoBedeli", "UcretsizKargoLimiti", "StokUyariLimiti", "StoktaYokSatisIzni", "PaytrAktifMi", "PaytrTestModu", "PaytrMerchantId", "PaytrMerchantKeyProtected", "PaytrMerchantSaltProtected", "PaytrCallbackUrl", "PaytrBasariliDonusUrl", "PaytrBasarisizDonusUrl", "KargoFirmasi", "KargoTakipUrl", "SiparisTeslimSuresiGun", "IadeHakkiGun", "MetaTitle", "MetaDescription", "MetaKeywords", "GoogleAnalyticsId", "FacebookPixelId", "VarsayilanSosyalPaylasimGorseliUrl", "CookieMetni", "YeniSiparisMailBildirimi", "StokUyarisiMailBildirimi", "IadeTalebiMailBildirimi", "BildirimAliciEmail", "BakimModuAktif", "BakimModuMesaji", "GirisZorunluMu", "StokBiteniGriGoster", "KapidaOdemeAktifMi", "KapidaOdemeHizmetBedeli", "KapidaOdemeLimiti", "ToptanciMinSiparisTutari", "IptalSuresiSaat") FROM stdin;
1	7ANRPS48	7ANRPS48	7ANRPS48 - ┘àÏ¬Ï¼Ï▒┘â Ïğ┘äÏÑ┘ä┘âÏ¬Ï▒┘ê┘å┘è	┘à┘åÏ¬Ï¼ÏğÏ¬ ┘àÏ¬┘å┘êÏ╣Ï® Ï¿Ïú┘üÏÂ┘ä Ïğ┘äÏúÏ│Ï╣ÏğÏ▒ ┘àÏ╣ Ï«Ï»┘àÏ® Ï¬┘êÏÁ┘è┘ä Ï│Ï▒┘èÏ╣Ï®	/74anrps48logo2.svg	/74anrps48logo2.svg	https://www.7anrps48.com	#1a5632	Ï¬┘êÏÁ┘è┘ä ┘àÏ¼Ïğ┘å┘è ┘ä┘äÏÀ┘äÏ¿ÏğÏ¬ ┘ü┘ê┘é 200 Ôé¬	Ïğ┘äÏ»┘üÏ╣ Ï╣┘åÏ» Ïğ┘äÏğÏ│Ï¬┘äÏğ┘à ┘àÏ¬ÏğÏ¡	t	34	┘àÏ¬Ï¼Ï▒ ÏÑ┘ä┘âÏ¬Ï▒┘ê┘å┘è ┘ü┘äÏ│ÏÀ┘è┘å┘è ┘è┘éÏ»┘à ┘à┘åÏ¬Ï¼ÏğÏ¬ ┘àÏ¬┘å┘êÏ╣Ï® Ï¿ÏúÏ│Ï╣ÏğÏ▒ ┘à┘åÏğ┘üÏ│Ï® ┘êÏ¬┘êÏÁ┘è┘ä Ï│Ï▒┘èÏ╣ ┘äÏ¼┘à┘èÏ╣ Ïğ┘ä┘àÏ»┘å	+970-599-000-000	info@7anrps48.com	┘ü┘äÏ│ÏÀ┘è┘å	+970599000000	Ïğ┘äÏ│Ï¿Ï¬ - Ïğ┘äÏ«┘à┘èÏ│: 9:00 - 21:00							Ôé¬	15	200	5	f	f	f							Ï¬┘êÏÁ┘è┘ä ┘àÏ¡┘ä┘è		3	7	7ANRPS48 - ┘àÏ¬Ï¼Ï▒ ÏÑ┘ä┘âÏ¬Ï▒┘ê┘å┘è ┘ü┘äÏ│ÏÀ┘è┘å┘è | Ï¬Ï│┘ê┘é Ïú┘ê┘å┘äÏğ┘è┘å	Ï¬Ï│┘ê┘é Ïú┘ê┘å┘äÏğ┘è┘å ┘à┘å 7ANRPS48. ┘à┘åÏ¬Ï¼ÏğÏ¬ ┘àÏ¬┘å┘êÏ╣Ï® Ï¿Ïú┘üÏÂ┘ä Ïğ┘äÏúÏ│Ï╣ÏğÏ▒ ┘àÏ╣ Ï¬┘êÏÁ┘è┘ä Ï│Ï▒┘èÏ╣ ┘äÏ¼┘à┘èÏ╣ ┘àÏ»┘å ┘ü┘äÏ│ÏÀ┘è┘å. Ïğ┘äÏ»┘üÏ╣ Ï╣┘åÏ» Ïğ┘äÏğÏ│Ï¬┘äÏğ┘à Ïú┘ê Ï¬Ï¡┘ê┘è┘ä Ï¿┘å┘â┘è.	Ï¬Ï│┘ê┘é Ïú┘ê┘å┘äÏğ┘è┘å, ┘ü┘äÏ│ÏÀ┘è┘å, ┘àÏ¬Ï¼Ï▒ ÏÑ┘ä┘âÏ¬Ï▒┘ê┘å┘è, Ï¬┘êÏÁ┘è┘ä, 7ANRPS48			/74anrps48logo2.svg	┘åÏ│Ï¬Ï«Ï»┘à ┘à┘ä┘üÏğÏ¬ Ï¬Ï╣Ï▒┘è┘ü Ïğ┘äÏğÏ▒Ï¬Ï¿ÏğÏÀ ┘äÏ¬Ï¡Ï│┘è┘å Ï¬Ï¼Ï▒Ï¿Ï¬┘â ┘êÏ¬Ï¡┘ä┘è┘ä Ï¡Ï▒┘âÏ® Ïğ┘ä┘à┘ê┘éÏ╣.	t	t	t	info@7anrps48.com	f	┘åÏ¡┘å ┘åÏ╣┘à┘ä Ï╣┘ä┘ë Ï¬Ï¡Ï│┘è┘å Ïğ┘ä┘à┘ê┘éÏ╣ ┘äÏ¬┘éÏ»┘è┘à Ï¬Ï¼Ï▒Ï¿Ï® Ï¬Ï│┘ê┘é Ïú┘üÏÂ┘ä. Ï│┘åÏ╣┘êÏ» ┘éÏ▒┘èÏ¿Ïğ┘ï!	f	t	f	0.0	2000	500	3
\.


--
-- Data for Name: SiteDegerlendirmeleri; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."SiteDegerlendirmeleri" ("Id", "AdSoyad", "Eposta", "Puan", "Baslik", "YorumMetni", "OnayliMi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: Slaytlar; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."Slaytlar" ("Id", "Baslik", "AltBaslik", "Aciklama", "ResimUrl", "VideoUrl", "Tur", "Sira", "AktifMi", "OlusturmaTarihi", "BaslikEn", "BaslikAr", "AltBaslikEn", "AltBaslikAr", "AciklamaEn", "AciklamaAr") FROM stdin;
3	┘çÏ»Ïğ┘èÏğ Ï¬Ï╣Ï¿┘æÏ▒ Ï╣┘å ┘ü┘äÏ│ÏÀ┘è┘å	ÏğÏ«Ï¬┘èÏğÏ▒ÏğÏ¬ ┘à┘à┘èÏ▓Ï® ┘ä┘â┘ä ┘à┘åÏğÏ│Ï¿Ï®	┘çÏ»Ïğ┘èÏğ ┘êÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬ Ï¬Ï¡┘à┘ä ┘àÏ╣┘å┘ë Ï¼┘à┘è┘ä┘ïÏğ ┘ê┘ä┘àÏ│Ï® Ï«ÏğÏÁÏ®.	/slider3.png	\N	Resim	3	f	2026-06-14 08:15:13.936783+00	Gifts That Express Palestine	┘çÏ»Ïğ┘èÏğ Ï¬Ï╣Ï¿┘æÏ▒ Ï╣┘å ┘ü┘äÏ│ÏÀ┘è┘å	Distinct Choices for Every Occasion	ÏğÏ«Ï¬┘èÏğÏ▒ÏğÏ¬ ┘à┘à┘èÏ▓Ï® ┘ä┘â┘ä ┘à┘åÏğÏ│Ï¿Ï®	Gifts and accessories that carry a beautiful meaning and a special touch.	┘çÏ»Ïğ┘èÏğ ┘êÏÑ┘âÏ│Ï│┘êÏğÏ▒ÏğÏ¬ Ï¬Ï¡┘à┘ä ┘àÏ╣┘å┘ë Ï¼┘à┘è┘ä┘ïÏğ ┘ê┘ä┘àÏ│Ï® Ï«ÏğÏÁÏ®.
1	┘à┘åÏ¬Ï¼ÏğÏ¬ ┘ü┘äÏ│ÏÀ┘è┘å┘èÏ® ÏúÏÁ┘è┘äÏ®	Ï¬Ï│┘ê┘é Ï¿Ï¬ÏÁÏğ┘à┘è┘à ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å ┘ü┘äÏ│ÏÀ┘è┘å	Ïğ┘âÏ¬Ï┤┘ü ┘àÏ¼┘à┘êÏ╣ÏğÏ¬ ┘àÏ«Ï¬ÏğÏ▒Ï® Ï¬Ï╣┘âÏ│ Ïğ┘ä┘ç┘ê┘èÏ® ┘êÏğ┘äÏ¬Ï▒ÏğÏ½ Ïğ┘ä┘ü┘äÏ│ÏÀ┘è┘å┘è.	/slider4.png	\N	Resim	1	t	2026-06-14 08:15:13.936757+00	Authentic Palestinian Products	┘à┘åÏ¬Ï¼ÏğÏ¬ ┘ü┘äÏ│ÏÀ┘è┘å┘èÏ® ÏúÏÁ┘è┘äÏ®	Shop with Palestine-Inspired Designs	Ï¬Ï│┘ê┘é Ï¿Ï¬ÏÁÏğ┘à┘è┘à ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å ┘ü┘äÏ│ÏÀ┘è┘å	Discover curated collections reflecting Palestinian identity and heritage.	Ïğ┘âÏ¬Ï┤┘ü ┘àÏ¼┘à┘êÏ╣ÏğÏ¬ ┘àÏ«Ï¬ÏğÏ▒Ï® Ï¬Ï╣┘âÏ│ Ïğ┘ä┘ç┘ê┘èÏ® ┘êÏğ┘äÏ¬Ï▒ÏğÏ½ Ïğ┘ä┘ü┘äÏ│ÏÀ┘è┘å┘è.
2	┘ä┘àÏ│ÏğÏ¬ ┘à┘åÏ▓┘ä┘èÏ® Ï¿Ï▒┘êÏ¡ Ïğ┘ä┘éÏ»Ï│	Ï»┘è┘â┘êÏ▒ Ïú┘å┘è┘é ┘ä┘à┘åÏ▓┘ä┘â	┘à┘åÏ¬Ï¼ÏğÏ¬ Ï»┘è┘â┘êÏ▒ ┘ê┘çÏ»Ïğ┘èÏğ Ï¿Ï¼┘êÏ»Ï® Ï╣Ïğ┘ä┘èÏ® ┘êÏ¬ÏÁÏğ┘à┘è┘à Ï╣ÏÁÏ▒┘èÏ®.	/slider5.png	\N	Resim	2	t	2026-06-14 08:15:13.936783+00	Home Touches with Jerusalem Spirit	┘ä┘àÏ│ÏğÏ¬ ┘à┘åÏ▓┘ä┘èÏ® Ï¿Ï▒┘êÏ¡ Ïğ┘ä┘éÏ»Ï│	Elegant Decor for Your Home	Ï»┘è┘â┘êÏ▒ Ïú┘å┘è┘é ┘ä┘à┘åÏ▓┘ä┘â	High-quality decor and gift products with modern designs.	┘à┘åÏ¬Ï¼ÏğÏ¬ Ï»┘è┘â┘êÏ▒ ┘ê┘çÏ»Ïğ┘èÏğ Ï¿Ï¼┘êÏ»Ï® Ï╣Ïğ┘ä┘èÏ® ┘êÏ¬ÏÁÏğ┘à┘è┘à Ï╣ÏÁÏ▒┘èÏ®.
\.


--
-- Data for Name: ToptanciIskontoOranlari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."ToptanciIskontoOranlari" ("Id", "ToptanciUrunGrubuId", "MinAdet", "IskontoYuzdesi", "AktifMi", "OlusturulmaTarihi", "GuncellemeTarihi", "SilindiMi") FROM stdin;
1	1	10	5	t	2026-06-25 21:44:12.868324+00	\N	f
2	1	50	10	t	2026-06-25 21:44:12.868324+00	\N	f
3	1	100	15	t	2026-06-25 21:44:12.868324+00	\N	f
4	2	5	3	t	2026-06-25 21:44:12.868324+00	\N	f
5	2	20	8	t	2026-06-25 21:44:12.868324+00	\N	f
6	3	20	7	t	2026-06-25 21:44:12.868324+00	\N	f
7	3	100	12	t	2026-06-25 21:44:12.868324+00	\N	f
\.


--
-- Data for Name: ToptanciUrunGruplari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."ToptanciUrunGruplari" ("Id", "Ad", "Aciklama", "AktifMi", "Sira", "OlusturulmaTarihi", "GuncellemeTarihi", "SilindiMi") FROM stdin;
1	Tekstil ve Giyim	K├ä┬▒yafet, kuma├à┼© ve tekstil ├â┬╝r├â┬╝nleri	t	1	2026-06-25 21:44:12.819207+00	\N	f
2	Elektronik	Elektronik cihazlar ve aksesuarlar	t	2	2026-06-25 21:44:12.819207+00	\N	f
3	G├ä┬▒da ve ├ä┬░├â┬ğecek	G├ä┬▒da ├â┬╝r├â┬╝nleri ve i├â┬ğecekler	t	3	2026-06-25 21:44:12.819207+00	\N	f
4	Kozmetik ve Bak├ä┬▒m	Kozmetik, ki├à┼©isel bak├ä┬▒m ve temizlik ├â┬╝r├â┬╝nleri	t	4	2026-06-25 21:44:12.819207+00	\N	f
\.


--
-- Data for Name: UrunOzellikDegerleri; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."UrunOzellikDegerleri" ("Id", "UrunId", "UrunOzellikTanimiId", "Deger", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: UrunOzellikTanimlari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."UrunOzellikTanimlari" ("Id", "Ad", "Kod", "UrunTipi", "AlanTipi", "YardimMetni", "Secenekler", "FiltredeGoster", "DetaySayfasindaGoster", "TeknikTablodaGoster", "AktifMi", "Sira", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
1	Renk	genel_renk	Genel	text			t	t	t	t	10	2026-06-14 08:15:13.743045+00	f
2	Stil	genel_stil	Genel	select		Modern\nMinimal\nKlasik\nRustik\nEndustriyel	t	t	t	t	20	2026-06-14 08:15:13.768774+00	f
3	Kullanim Alani	genel_kullanim_alani	Genel	select		Salon\nYatak Odasi\nOfis\nMutfak\nKoridor	t	t	t	t	30	2026-06-14 08:15:13.76939+00	f
4	Baski Teknigi	kanvas_baski_teknigi	Kanvas	text			t	t	t	t	110	2026-06-14 08:15:13.769428+00	f
5	Kanvas Kumasi	kanvas_kumas	Kanvas	text			t	t	t	t	120	2026-06-14 08:15:13.76946+00	f
6	Sase Kalinligi	kanvas_sase_kalinligi	Kanvas	text			f	t	t	t	130	2026-06-14 08:15:13.769481+00	f
7	Asma Sekli	kanvas_asma_sekli	Kanvas	select		Hazir Aparat\nDuvara Asilabilir Halka\nCercevesiz	f	t	t	t	140	2026-06-14 08:15:13.769501+00	f
8	Ayna Tipi	ayna_tipi	Ayna	select		Flotal\nBronz\nFume\nDekoratif	t	t	t	t	210	2026-06-14 08:15:13.76952+00	f
9	Cam Kalinligi	ayna_cam_kalinligi	Ayna	text			f	t	t	t	220	2026-06-14 08:15:13.769545+00	f
10	Kenar Isciligi	ayna_kenar_isciligi	Ayna	text			f	t	t	t	230	2026-06-14 08:15:13.769564+00	f
11	Montaj Yonu	ayna_montaj_yonu	Ayna	select		Dikey\nYatay\nHer Iki Yonde	f	t	t	t	240	2026-06-14 08:15:13.769582+00	f
12	Dokuma Tipi	hali_dokuma_tipi	Hali	select		Makine Dokuma\nEl Dokuma\nDijital Baski	t	t	t	t	310	2026-06-14 08:15:13.769599+00	f
13	Taban Tipi	hali_taban_tipi	Hali	text			f	t	t	t	320	2026-06-14 08:15:13.769617+00	f
14	Hav Yuksekligi	hali_hav_yuksekligi	Hali	text			f	t	t	t	330	2026-06-14 08:15:13.769642+00	f
15	Yikanabilir Mi	hali_yikanabilir	Hali	select		true\nfalse	t	t	t	t	340	2026-06-14 08:15:13.76966+00	f
16	Cam Tipi	camtablo_cam_tipi	CamTablo	select		Temperli\nParlak\nMat	t	t	t	t	410	2026-06-14 08:15:13.769678+00	f
17	Yuzey Isciligi	camtablo_yuzey	CamTablo	text			f	t	t	t	420	2026-06-14 08:15:13.769697+00	f
18	Montaj Tipi	camtablo_montaj	CamTablo	text			f	t	t	t	430	2026-06-14 08:15:13.769727+00	f
19	Kenar Formu	camtablo_kenar	CamTablo	text			f	t	t	t	440	2026-06-14 08:15:13.769747+00	f
20	Rulo Olcusu	duvarkagidi_rulo_olcusu	DuvarKagidi	text			f	t	t	t	510	2026-06-14 08:15:13.769765+00	f
21	Uygulama Tipi	duvarkagidi_uygulama	DuvarKagidi	select		Yapiskanli\nTutkalli\nKolay Soku╠êlebilir	t	t	t	t	520	2026-06-14 08:15:13.769785+00	f
22	Silinebilirlik	duvarkagidi_silinebilirlik	DuvarKagidi	select		true\nfalse	t	t	t	t	530	2026-06-14 08:15:13.769815+00	f
23	Desen Tekrari	duvarkagidi_desen_tekrari	DuvarKagidi	text			f	t	t	t	540	2026-06-14 08:15:13.769832+00	f
24	Govde Malzemesi	mobilya_govde_malzeme	SehpaMobilya	text			t	t	t	t	610	2026-06-14 08:15:13.769849+00	f
25	Ayak Malzemesi	mobilya_ayak_malzeme	SehpaMobilya	text			t	t	t	t	620	2026-06-14 08:15:13.769866+00	f
26	Tasima Kapasitesi	mobilya_tasima_kapasitesi	SehpaMobilya	text			f	t	t	t	630	2026-06-14 08:15:13.769889+00	f
27	Kurulum Durumu	mobilya_kurulum	SehpaMobilya	select		Demonte\nHazir Kurulu	t	t	t	t	640	2026-06-14 08:15:13.769907+00	f
\.


--
-- Data for Name: UrunResimleri; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."UrunResimleri" ("Id", "ResimYolu", "Baslik", "AltMetin", "MedyaTipi", "MedyaAlani", "VideoUrl", "ThumbnailYolu", "MobilResimYolu", "Etiketler", "Sira", "VarsayilanMi", "UrunSecenekId", "UrunId", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
1	/slider4.png	Filistin Motifli Duvar Tablosu	Filistin Motifli Duvar Tablosu	Gorsel	Galeri		/slider4.png		demo,palestine	1	t	\N	1	2026-06-19 12:01:10.869407+00	f
2	/slider5.png	Zeytin Dal─▒ Dekoratif Set	Zeytin Dal─▒ Dekoratif Set	Gorsel	Galeri		/slider5.png		demo,palestine	1	t	\N	2	2026-06-19 12:01:10.869407+00	f
3	/slider4.png	Filistin Hat─▒ra Hediye Kutusu	Filistin Hat─▒ra Hediye Kutusu	Gorsel	Galeri		/slider4.png		demo,palestine	1	t	\N	3	2026-06-19 12:01:10.869407+00	f
4	/slider5.png	Kud├╝s Sil├╝et Tablosu	Kud├╝s Sil├╝et Tablosu	Gorsel	Galeri		/slider5.png		demo,palestine	1	t	\N	4	2026-06-19 12:01:10.869407+00	f
5	/slider4.png	Gazze Dayan─▒┼şma Posteri	Gazze Dayan─▒┼şma Posteri	Gorsel	Galeri		/slider4.png		demo,palestine	1	t	\N	5	2026-06-19 12:01:10.869407+00	f
6	/slider5.png	Filistin Harita & Bayrak Seti	Filistin Harita & Bayrak Seti	Gorsel	Galeri		/slider5.png		demo,palestine	1	t	\N	6	2026-06-19 12:01:10.869407+00	f
7	/slider4.png	Salon ─░├ğin Kud├╝s Dekoru	Salon ─░├ğin Kud├╝s Dekoru	Gorsel	Galeri		/slider4.png		demo,palestine	1	t	\N	7	2026-06-19 12:01:10.869407+00	f
8	/slider5.png	Alt─▒n ├çer├ğeveli Duvar Dekoru	Alt─▒n ├çer├ğeveli Duvar Dekoru	Gorsel	Galeri		/slider5.png		demo,palestine	1	t	\N	8	2026-06-19 12:01:10.869407+00	f
9	/slider4.png	Filistin Sofra Sunum Seti	Filistin Sofra Sunum Seti	Gorsel	Galeri		/slider4.png		demo,palestine	1	t	\N	9	2026-06-19 12:01:10.869407+00	f
10	/slider5.png	─░sme ├ûzel Filistin Hediyesi	─░sme ├ûzel Filistin Hediyesi	Gorsel	Galeri		/slider5.png		demo,palestine	1	t	\N	10	2026-06-19 12:01:10.869407+00	f
11	/slider4.png	Kud├╝s Anahtarl─▒k & Magnet Seti	Kud├╝s Anahtarl─▒k & Magnet Seti	Gorsel	Galeri		/slider4.png		demo,palestine	1	t	\N	11	2026-06-19 12:01:10.869407+00	f
12	/slider5.png	Nak─▒┼şl─▒ Filistin Bez ├çanta	Nak─▒┼şl─▒ Filistin Bez ├çanta	Gorsel	Galeri		/slider5.png		demo,palestine	1	t	\N	12	2026-06-19 12:01:10.869407+00	f
\.


--
-- Data for Name: UrunSecenekleri; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."UrunSecenekleri" ("Id", "UrunId", "Olcu", "CerceveTipi", "CerceveRengi", "CerceveKalinligi", "MalzemeTuru", "Yon", "ParcaSayisi", "VaryantSku", "KisilestirmeMetni", "OzelTasarimNotu", "FiyatFarki", "SatisFiyati", "MaliyetFiyati", "StokAdedi", "UretimSuresiGun", "Desi", "GorselUrl", "AktifMi", "VarsayilanMi", "TukeninceGizle", "OnSipariseAcikMi", "Sira", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
1	1	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-001-STD			0	95.00	52.25	25	2	1.20	/slider4.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
2	2	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-002-STD			0	120.00	66.00	25	2	1.20	/slider5.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
3	3	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-003-STD			0	75.00	41.25	25	2	1.20	/slider4.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
4	4	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-004-STD			0	110.00	60.50	25	2	1.20	/slider5.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
5	5	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-005-STD			0	45.00	24.75	25	2	1.20	/slider4.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
6	6	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-006-STD			0	65.00	35.75	25	2	1.20	/slider5.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
7	7	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-007-STD			0	135.00	74.25	25	2	1.20	/slider4.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
8	8	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-008-STD			0	145.00	79.75	25	2	1.20	/slider5.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
9	9	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-009-STD			0	88.00	48.40	25	2	1.20	/slider4.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
10	10	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-010-STD			0	55.00	30.25	25	2	1.20	/slider5.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
11	11	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-011-STD			0	28.00	15.40	25	2	1.20	/slider4.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
12	12	Standart	Standart	Do─şal	Standart	Premium	Dikey	1	DEMO-012-STD			0	70.00	38.50	25	2	1.20	/slider5.png	t	t	f	f	1	2026-06-19 12:01:10.869407+00	f
13	13	30x40 cm						1				0	200	80	50	0	0		t	t	f	f	1	2026-06-25 21:44:12.257319+00	f
14	13	50x70 cm						1				0	350	140	30	0	0		t	f	f	f	2	2026-06-25 21:44:12.257319+00	f
15	13	70x100 cm						1				0	550	220	15	0	0		t	f	f	f	3	2026-06-25 21:44:12.257319+00	f
16	14	20x30 cm						1				0	80	30	100	0	0		t	t	f	f	1	2026-06-25 21:44:12.257319+00	f
17	14	40x60 cm						1				0	180	70	60	0	0		t	f	f	f	2	2026-06-25 21:44:12.257319+00	f
18	14	60x90 cm						1				0	320	130	25	0	0		t	f	f	f	3	2026-06-25 21:44:12.257319+00	f
19	15	50x70 cm						1				0	400	160	40	0	0		t	t	f	f	1	2026-06-25 21:44:12.257319+00	f
20	15	70x100 cm						1				0	650	260	20	0	0		t	f	f	f	2	2026-06-25 21:44:12.257319+00	f
21	15	100x140 cm						1				0	950	380	10	0	0		t	f	f	f	3	2026-06-25 21:44:12.257319+00	f
\.


--
-- Data for Name: Urunler; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."Urunler" ("Id", "Baslik", "KisaAd", "Slug", "UrlYolu", "SKU", "Barkod", "Marka", "UrunTipi", "Etiketler", "KisaAciklama", "Aciklama", "TeknikOzellikler", "MalzemeBilgisi", "BakimTalimati", "PaketlemeBilgisi", "AnaGorselUrl", "StokDurumu", "Fiyat", "IndirimliFiyat", "Maliyet", "KdvOrani", "UretimSuresiGun", "KargoyaVerilisSuresiGun", "TahminiTeslimSuresiGun", "AktifMi", "OneCikanMi", "YeniUrunMu", "KampanyaliMi", "AnaSayfadaGoster", "Sira", "GoruntulenmeSayisi", "SatisSayisi", "FavoriSayisi", "MinSiparisAdedi", "MaxSiparisAdedi", "SeoTitle", "SeoDescription", "SeoKeywords", "KategoriId", "OlusturulmaTarihi", "SilindiMi", "TopFiyat", "AciklamaAr", "AciklamaEn", "BaslikAr", "BaslikEn", "KisaAciklamaAr", "KisaAciklamaEn", "SeoDescriptionAr", "SeoDescriptionEn", "SeoTitleAr", "SeoTitleEn", "HediyePaketFiyati", "HediyePaketiVarMi", "WhatsappSiparisVarMi", "FiyatGizliMi", "ToptanciUrunGrubuId", "KampanyaBitisTarihi", "YayindaMi") FROM stdin;
2	Zeytin Dal─▒ Dekoratif Set	Zeytin Dal─▒ Dekoratif Set	olive-branch-decor-set		DEMO-002		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	Ev dekorasyonu i├ğin zarif zeytin dal─▒ temal─▒ set.	<p>Ev dekorasyonu i├ğin zarif zeytin dal─▒ temal─▒ set. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider5.png	Stokta	120.00	110.00	66.00	17	2	1	4	t	t	t	f	t	2	1	0	0	1	\N	Zeytin Dal─▒ Dekoratif Set	Ev dekorasyonu i├ğin zarif zeytin dal─▒ temal─▒ set.	palestine,7anrps48,decor,gift	2	2026-06-19 12:01:10.869407+00	f	102.00	<p>ÏÀ┘é┘à Ïú┘å┘è┘é ┘àÏ│Ï¬┘êÏ¡┘ë ┘à┘å Ï║ÏÁ┘å Ïğ┘äÏ▓┘èÏ¬┘ê┘å ┘äÏ»┘è┘â┘êÏ▒ Ïğ┘ä┘à┘åÏ▓┘ä. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Elegant olive branch themed set for home decoration. This product is for demo purposes.</p>	ÏÀ┘é┘à Ï»┘è┘â┘êÏ▒ Ï║ÏÁ┘å Ïğ┘äÏ▓┘èÏ¬┘ê┘å	Olive Branch Decor Set	ÏÀ┘é┘à Ïú┘å┘è┘é ┘àÏ│Ï¬┘êÏ¡┘ë ┘à┘å Ï║ÏÁ┘å Ïğ┘äÏ▓┘èÏ¬┘ê┘å ┘äÏ»┘è┘â┘êÏ▒ Ïğ┘ä┘à┘åÏ▓┘ä.	Elegant olive branch themed set for home decoration.	ÏÀ┘é┘à Ïú┘å┘è┘é ┘àÏ│Ï¬┘êÏ¡┘ë ┘à┘å Ï║ÏÁ┘å Ïğ┘äÏ▓┘èÏ¬┘ê┘å ┘äÏ»┘è┘â┘êÏ▒ Ïğ┘ä┘à┘åÏ▓┘ä.	Elegant olive branch themed set for home decoration.	ÏÀ┘é┘à Ï»┘è┘â┘êÏ▒ Ï║ÏÁ┘å Ïğ┘äÏ▓┘èÏ¬┘ê┘å	Olive Branch Decor Set	8.00	t	t	f	\N	\N	t
4	Kud├╝s Sil├╝et Tablosu	Kud├╝s Sil├╝et Tablosu	jerusalem-skyline-art		DEMO-004		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	Kud├╝s sil├╝etinden ilham alan premium tablo.	<p>Kud├╝s sil├╝etinden ilham alan premium tablo. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider5.png	Stokta	110.00	100.00	60.50	17	2	1	4	t	f	t	f	t	4	0	0	0	1	\N	Kud├╝s Sil├╝et Tablosu	Kud├╝s sil├╝etinden ilham alan premium tablo.	palestine,7anrps48,decor,gift	4	2026-06-19 12:01:10.869407+00	f	93.50	<p>┘ä┘êÏ¡Ï® ┘üÏğÏ«Ï▒Ï® ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å Ïú┘ü┘é ┘àÏ»┘è┘åÏ® Ïğ┘ä┘éÏ»Ï│. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Premium art inspired by the Jerusalem skyline. This product is for demo purposes.</p>	┘ä┘êÏ¡Ï® Ïú┘ü┘é Ïğ┘ä┘éÏ»Ï│	Jerusalem Skyline Art	┘ä┘êÏ¡Ï® ┘üÏğÏ«Ï▒Ï® ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å Ïú┘ü┘é ┘àÏ»┘è┘åÏ® Ïğ┘ä┘éÏ»Ï│.	Premium art inspired by the Jerusalem skyline.	┘ä┘êÏ¡Ï® ┘üÏğÏ«Ï▒Ï® ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å Ïú┘ü┘é ┘àÏ»┘è┘åÏ® Ïğ┘ä┘éÏ»Ï│.	Premium art inspired by the Jerusalem skyline.	┘ä┘êÏ¡Ï® Ïú┘ü┘é Ïğ┘ä┘éÏ»Ï│	Jerusalem Skyline Art	8.00	t	t	f	\N	\N	t
3	Filistin Hat─▒ra Hediye Kutusu	Filistin Hat─▒ra Hediye Kutusu	palestine-keepsake-gift-box		DEMO-003		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	├ûzel g├╝nler i├ğin Filistin temal─▒ hediye kutusu.	<p>├ûzel g├╝nler i├ğin Filistin temal─▒ hediye kutusu. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider4.png	Stokta	75.00	\N	41.25	17	2	1	4	t	t	t	f	t	3	5	0	0	1	\N	Filistin Hat─▒ra Hediye Kutusu	├ûzel g├╝nler i├ğin Filistin temal─▒ hediye kutusu.	palestine,7anrps48,decor,gift	3	2026-06-19 12:01:10.869407+00	f	63.75	<p>ÏÁ┘åÏ»┘ê┘é ┘çÏ»Ïğ┘èÏğ Ï¿ÏÀÏğÏ¿Ï╣ ┘ü┘äÏ│ÏÀ┘è┘å┘è ┘ä┘ä┘à┘åÏğÏ│Ï¿ÏğÏ¬ Ïğ┘äÏ«ÏğÏÁÏ®. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Palestine themed gift box for special occasions. This product is for demo purposes.</p>	ÏÁ┘åÏ»┘ê┘é ┘çÏ»Ïğ┘èÏğ Ï¬Ï░┘âÏğÏ▒┘è ┘ü┘äÏ│ÏÀ┘è┘å┘è	Palestine Keepsake Gift Box	ÏÁ┘åÏ»┘ê┘é ┘çÏ»Ïğ┘èÏğ Ï¿ÏÀÏğÏ¿Ï╣ ┘ü┘äÏ│ÏÀ┘è┘å┘è ┘ä┘ä┘à┘åÏğÏ│Ï¿ÏğÏ¬ Ïğ┘äÏ«ÏğÏÁÏ®.	Palestine themed gift box for special occasions.	ÏÁ┘åÏ»┘ê┘é ┘çÏ»Ïğ┘èÏğ Ï¿ÏÀÏğÏ¿Ï╣ ┘ü┘äÏ│ÏÀ┘è┘å┘è ┘ä┘ä┘à┘åÏğÏ│Ï¿ÏğÏ¬ Ïğ┘äÏ«ÏğÏÁÏ®.	Palestine themed gift box for special occasions.	ÏÁ┘åÏ»┘ê┘é ┘çÏ»Ïğ┘èÏğ Ï¬Ï░┘âÏğÏ▒┘è ┘ü┘äÏ│ÏÀ┘è┘å┘è	Palestine Keepsake Gift Box	8.00	t	t	f	\N	\N	t
5	Gazze Dayan─▒┼şma Posteri	Gazze Dayan─▒┼şma Posteri	gaza-solidarity-poster		DEMO-005		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	Dayan─▒┼şma mesaj─▒ ta┼ş─▒yan modern poster.	<p>Dayan─▒┼şma mesaj─▒ ta┼ş─▒yan modern poster. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider4.png	Stokta	45.00	\N	24.75	17	2	1	4	t	f	t	t	t	5	1	0	0	1	\N	Gazze Dayan─▒┼şma Posteri	Dayan─▒┼şma mesaj─▒ ta┼ş─▒yan modern poster.	palestine,7anrps48,decor,gift	5	2026-06-19 12:01:10.869407+00	f	38.25	<p>┘à┘äÏÁ┘é Ï╣ÏÁÏ▒┘è ┘èÏ¡┘à┘ä Ï▒Ï│Ïğ┘äÏ® Ï¬ÏÂÏğ┘à┘å. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Modern poster carrying a message of solidarity. This product is for demo purposes.</p>	┘à┘äÏÁ┘é Ï¬ÏÂÏğ┘à┘å Ï║Ï▓Ï®	Gaza Solidarity Poster	┘à┘äÏÁ┘é Ï╣ÏÁÏ▒┘è ┘èÏ¡┘à┘ä Ï▒Ï│Ïğ┘äÏ® Ï¬ÏÂÏğ┘à┘å.	Modern poster carrying a message of solidarity.	┘à┘äÏÁ┘é Ï╣ÏÁÏ▒┘è ┘èÏ¡┘à┘ä Ï▒Ï│Ïğ┘äÏ® Ï¬ÏÂÏğ┘à┘å.	Modern poster carrying a message of solidarity.	┘à┘äÏÁ┘é Ï¬ÏÂÏğ┘à┘å Ï║Ï▓Ï®	Gaza Solidarity Poster	8.00	t	t	f	\N	\N	t
6	Filistin Harita & Bayrak Seti	Filistin Harita & Bayrak Seti	palestine-map-a-flag-set		DEMO-006		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	Harita ve bayrak detayl─▒ dekoratif set.	<p>Harita ve bayrak detayl─▒ dekoratif set. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider5.png	Stokta	65.00	\N	35.75	17	2	1	4	t	f	t	f	t	6	0	14	0	1	\N	Filistin Harita & Bayrak Seti	Harita ve bayrak detayl─▒ dekoratif set.	palestine,7anrps48,decor,gift	6	2026-06-19 12:01:10.869407+00	f	55.25	<p>ÏÀ┘é┘à Ï»┘è┘â┘êÏ▒┘è Ï¿Ï¬┘üÏğÏÁ┘è┘ä Ïğ┘äÏ«Ï▒┘èÏÀÏ® ┘êÏğ┘äÏ╣┘ä┘à. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Decorative set with map and flag details. This product is for demo purposes.</p>	ÏÀ┘é┘à Ï«Ï▒┘èÏÀÏ® ┘êÏ╣┘ä┘à ┘ü┘äÏ│ÏÀ┘è┘å	Palestine Map & Flag Set	ÏÀ┘é┘à Ï»┘è┘â┘êÏ▒┘è Ï¿Ï¬┘üÏğÏÁ┘è┘ä Ïğ┘äÏ«Ï▒┘èÏÀÏ® ┘êÏğ┘äÏ╣┘ä┘à.	Decorative set with map and flag details.	ÏÀ┘é┘à Ï»┘è┘â┘êÏ▒┘è Ï¿Ï¬┘üÏğÏÁ┘è┘ä Ïğ┘äÏ«Ï▒┘èÏÀÏ® ┘êÏğ┘äÏ╣┘ä┘à.	Decorative set with map and flag details.	ÏÀ┘é┘à Ï«Ï▒┘èÏÀÏ® ┘êÏ╣┘ä┘à ┘ü┘äÏ│ÏÀ┘è┘å	Palestine Map & Flag Set	8.00	t	t	f	\N	\N	t
7	Salon ─░├ğin Kud├╝s Dekoru	Salon ─░├ğin Kud├╝s Dekoru	jerusalem-living-room-decor		DEMO-007		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	Salonlara s─▒cakl─▒k katan Kud├╝s temal─▒ dekor.	<p>Salonlara s─▒cakl─▒k katan Kud├╝s temal─▒ dekor. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider4.png	Stokta	135.00	\N	74.25	17	2	1	4	t	f	t	f	t	7	0	13	0	1	\N	Salon ─░├ğin Kud├╝s Dekoru	Salonlara s─▒cakl─▒k katan Kud├╝s temal─▒ dekor.	palestine,7anrps48,decor,gift	7	2026-06-19 12:01:10.869407+00	f	114.75	<p>Ï»┘è┘â┘êÏ▒ Ï¿ÏÀÏğÏ¿Ï╣ Ïğ┘ä┘éÏ»Ï│ ┘èÏÂ┘è┘ü Ï»┘üÏĞÏğ┘ï ┘äÏ║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Jerusalem themed decor that adds warmth to living rooms. This product is for demo purposes.</p>	Ï»┘è┘â┘êÏ▒ Ïğ┘ä┘éÏ»Ï│ ┘äÏ║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®	Jerusalem Living Room Decor	Ï»┘è┘â┘êÏ▒ Ï¿ÏÀÏğÏ¿Ï╣ Ïğ┘ä┘éÏ»Ï│ ┘èÏÂ┘è┘ü Ï»┘üÏĞÏğ┘ï ┘äÏ║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®.	Jerusalem themed decor that adds warmth to living rooms.	Ï»┘è┘â┘êÏ▒ Ï¿ÏÀÏğÏ¿Ï╣ Ïğ┘ä┘éÏ»Ï│ ┘èÏÂ┘è┘ü Ï»┘üÏĞÏğ┘ï ┘äÏ║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®.	Jerusalem themed decor that adds warmth to living rooms.	Ï»┘è┘â┘êÏ▒ Ïğ┘ä┘éÏ»Ï│ ┘äÏ║Ï▒┘üÏ® Ïğ┘ä┘àÏ╣┘èÏ┤Ï®	Jerusalem Living Room Decor	8.00	t	t	f	\N	\N	t
8	Alt─▒n ├çer├ğeveli Duvar Dekoru	Alt─▒n ├çer├ğeveli Duvar Dekoru	gold-framed-wall-decor		DEMO-008		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	Alt─▒n ├ğer├ğeveli sade ve ┼ş─▒k duvar dekoru.	<p>Alt─▒n ├ğer├ğeveli sade ve ┼ş─▒k duvar dekoru. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider5.png	Stokta	145.00	135.00	79.75	17	2	1	4	t	f	t	f	t	8	0	12	0	1	\N	Alt─▒n ├çer├ğeveli Duvar Dekoru	Alt─▒n ├ğer├ğeveli sade ve ┼ş─▒k duvar dekoru.	palestine,7anrps48,decor,gift	8	2026-06-19 12:01:10.869407+00	f	123.25	<p>Ï»┘è┘â┘êÏ▒ Ï¼Ï»ÏğÏ▒┘è Ï¿Ï│┘èÏÀ ┘êÏú┘å┘è┘é Ï¿ÏÑÏÀÏğÏ▒ Ï░┘çÏ¿┘è. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Simple and elegant wall decor with a gold frame. This product is for demo purposes.</p>	Ï»┘è┘â┘êÏ▒ Ï¼Ï»ÏğÏ▒┘è Ï¿ÏÑÏÀÏğÏ▒ Ï░┘çÏ¿┘è	Gold Framed Wall Decor	Ï»┘è┘â┘êÏ▒ Ï¼Ï»ÏğÏ▒┘è Ï¿Ï│┘èÏÀ ┘êÏú┘å┘è┘é Ï¿ÏÑÏÀÏğÏ▒ Ï░┘çÏ¿┘è.	Simple and elegant wall decor with a gold frame.	Ï»┘è┘â┘êÏ▒ Ï¼Ï»ÏğÏ▒┘è Ï¿Ï│┘èÏÀ ┘êÏú┘å┘è┘é Ï¿ÏÑÏÀÏğÏ▒ Ï░┘çÏ¿┘è.	Simple and elegant wall decor with a gold frame.	Ï»┘è┘â┘êÏ▒ Ï¼Ï»ÏğÏ▒┘è Ï¿ÏÑÏÀÏğÏ▒ Ï░┘çÏ¿┘è	Gold Framed Wall Decor	8.00	t	t	f	\N	\N	t
9	Filistin Sofra Sunum Seti	Filistin Sofra Sunum Seti	palestine-table-serving-set		DEMO-009		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	Mutfak ve sofra i├ğin dekoratif sunum seti.	<p>Mutfak ve sofra i├ğin dekoratif sunum seti. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider4.png	Stokta	88.00	\N	48.40	17	2	1	4	t	f	t	f	t	9	0	11	0	1	\N	Filistin Sofra Sunum Seti	Mutfak ve sofra i├ğin dekoratif sunum seti.	palestine,7anrps48,decor,gift	9	2026-06-19 12:01:10.869407+00	f	74.80	<p>ÏÀ┘é┘à Ï¬┘éÏ»┘è┘à Ï»┘è┘â┘êÏ▒┘è ┘ä┘ä┘àÏÀÏ¿Ï« ┘êÏğ┘ä┘àÏğÏĞÏ»Ï®. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Decorative serving set for kitchen and table. This product is for demo purposes.</p>	ÏÀ┘é┘à Ï¬┘éÏ»┘è┘à ┘àÏğÏĞÏ»Ï® ┘ü┘äÏ│ÏÀ┘è┘å┘è	Palestine Table Serving Set	ÏÀ┘é┘à Ï¬┘éÏ»┘è┘à Ï»┘è┘â┘êÏ▒┘è ┘ä┘ä┘àÏÀÏ¿Ï« ┘êÏğ┘ä┘àÏğÏĞÏ»Ï®.	Decorative serving set for kitchen and table.	ÏÀ┘é┘à Ï¬┘éÏ»┘è┘à Ï»┘è┘â┘êÏ▒┘è ┘ä┘ä┘àÏÀÏ¿Ï« ┘êÏğ┘ä┘àÏğÏĞÏ»Ï®.	Decorative serving set for kitchen and table.	ÏÀ┘é┘à Ï¬┘éÏ»┘è┘à ┘àÏğÏĞÏ»Ï® ┘ü┘äÏ│ÏÀ┘è┘å┘è	Palestine Table Serving Set	8.00	t	t	f	\N	\N	t
10	─░sme ├ûzel Filistin Hediyesi	─░sme ├ûzel Filistin Hediyesi	personalized-palestine-gift		DEMO-010		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	─░sim eklenebilen anlaml─▒ ki┼şiye ├Âzel hediye.	<p>─░sim eklenebilen anlaml─▒ ki┼şiye ├Âzel hediye. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider5.png	Stokta	55.00	\N	30.25	17	2	1	4	t	f	t	f	t	10	0	10	0	1	\N	─░sme ├ûzel Filistin Hediyesi	─░sim eklenebilen anlaml─▒ ki┼şiye ├Âzel hediye.	palestine,7anrps48,decor,gift	10	2026-06-19 12:01:10.869407+00	f	46.75	<p>┘çÏ»┘èÏ® ┘àÏ«ÏÁÏÁÏ® ┘ê┘àÏ╣Ï¿Ï▒Ï® ┘àÏ╣ ÏÑ┘à┘âÏğ┘å┘èÏ® ÏÑÏÂÏğ┘üÏ® Ïğ┘äÏğÏ│┘à. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Meaningful personalized gift with custom name option. This product is for demo purposes.</p>	┘çÏ»┘èÏ® ┘ü┘äÏ│ÏÀ┘è┘å┘èÏ® ┘àÏ«ÏÁÏÁÏ® Ï¿Ïğ┘äÏğÏ│┘à	Personalized Palestine Gift	┘çÏ»┘èÏ® ┘àÏ«ÏÁÏÁÏ® ┘ê┘àÏ╣Ï¿Ï▒Ï® ┘àÏ╣ ÏÑ┘à┘âÏğ┘å┘èÏ® ÏÑÏÂÏğ┘üÏ® Ïğ┘äÏğÏ│┘à.	Meaningful personalized gift with custom name option.	┘çÏ»┘èÏ® ┘àÏ«ÏÁÏÁÏ® ┘ê┘àÏ╣Ï¿Ï▒Ï® ┘àÏ╣ ÏÑ┘à┘âÏğ┘å┘èÏ® ÏÑÏÂÏğ┘üÏ® Ïğ┘äÏğÏ│┘à.	Meaningful personalized gift with custom name option.	┘çÏ»┘èÏ® ┘ü┘äÏ│ÏÀ┘è┘å┘èÏ® ┘àÏ«ÏÁÏÁÏ® Ï¿Ïğ┘äÏğÏ│┘à	Personalized Palestine Gift	8.00	t	t	f	\N	\N	t
11	Kud├╝s Anahtarl─▒k & Magnet Seti	Kud├╝s Anahtarl─▒k & Magnet Seti	jerusalem-keychain-a-magnet-set		DEMO-011		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	Kud├╝s temal─▒ anahtarl─▒k ve magnet seti.	<p>Kud├╝s temal─▒ anahtarl─▒k ve magnet seti. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider4.png	Stokta	28.00	\N	15.40	17	2	1	4	t	f	t	t	t	11	0	9	0	1	\N	Kud├╝s Anahtarl─▒k & Magnet Seti	Kud├╝s temal─▒ anahtarl─▒k ve magnet seti.	palestine,7anrps48,decor,gift	11	2026-06-19 12:01:10.869407+00	f	23.80	<p>ÏÀ┘é┘à ┘à┘èÏ»Ïğ┘ä┘èÏ® ┘ê┘àÏ║┘åÏğÏÀ┘èÏ│ Ï¿ÏÀÏğÏ¿Ï╣ Ïğ┘ä┘éÏ»Ï│. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Jerusalem themed keychain and magnet set. This product is for demo purposes.</p>	ÏÀ┘é┘à ┘à┘èÏ»Ïğ┘ä┘èÏ® ┘ê┘àÏ║┘åÏğÏÀ┘èÏ│ Ïğ┘ä┘éÏ»Ï│	Jerusalem Keychain & Magnet Set	ÏÀ┘é┘à ┘à┘èÏ»Ïğ┘ä┘èÏ® ┘ê┘àÏ║┘åÏğÏÀ┘èÏ│ Ï¿ÏÀÏğÏ¿Ï╣ Ïğ┘ä┘éÏ»Ï│.	Jerusalem themed keychain and magnet set.	ÏÀ┘é┘à ┘à┘èÏ»Ïğ┘ä┘èÏ® ┘ê┘àÏ║┘åÏğÏÀ┘èÏ│ Ï¿ÏÀÏğÏ¿Ï╣ Ïğ┘ä┘éÏ»Ï│.	Jerusalem themed keychain and magnet set.	ÏÀ┘é┘à ┘à┘èÏ»Ïğ┘ä┘èÏ® ┘ê┘àÏ║┘åÏğÏÀ┘èÏ│ Ïğ┘ä┘éÏ»Ï│	Jerusalem Keychain & Magnet Set	8.00	t	t	f	\N	\N	t
12	Nak─▒┼şl─▒ Filistin Bez ├çanta	Nak─▒┼şl─▒ Filistin Bez ├çanta	embroidered-palestine-tote-bag		DEMO-012		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	G├╝nl├╝k kullan─▒m i├ğin nak─▒┼ş detayl─▒ bez ├ğanta.	<p>G├╝nl├╝k kullan─▒m i├ğin nak─▒┼ş detayl─▒ bez ├ğanta. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider5.png	Stokta	70.00	\N	38.50	17	2	1	4	t	f	t	f	t	12	0	8	0	1	\N	Nak─▒┼şl─▒ Filistin Bez ├çanta	G├╝nl├╝k kullan─▒m i├ğin nak─▒┼ş detayl─▒ bez ├ğanta.	palestine,7anrps48,decor,gift	12	2026-06-19 12:01:10.869407+00	f	59.50	<p>Ï¡┘é┘èÏ¿Ï® ┘é┘àÏğÏ┤ ┘àÏÀÏ▒Ï▓Ï® ┘ä┘äÏğÏ│Ï¬Ï«Ï»Ïğ┘à Ïğ┘ä┘è┘ê┘à┘è. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Embroidered tote bag for daily use. This product is for demo purposes.</p>	Ï¡┘é┘èÏ¿Ï® ┘é┘àÏğÏ┤ ┘ü┘äÏ│ÏÀ┘è┘å┘èÏ® ┘àÏÀÏ▒Ï▓Ï®	Embroidered Palestine Tote Bag	Ï¡┘é┘èÏ¿Ï® ┘é┘àÏğÏ┤ ┘àÏÀÏ▒Ï▓Ï® ┘ä┘äÏğÏ│Ï¬Ï«Ï»Ïğ┘à Ïğ┘ä┘è┘ê┘à┘è.	Embroidered tote bag for daily use.	Ï¡┘é┘èÏ¿Ï® ┘é┘àÏğÏ┤ ┘àÏÀÏ▒Ï▓Ï® ┘ä┘äÏğÏ│Ï¬Ï«Ï»Ïğ┘à Ïğ┘ä┘è┘ê┘à┘è.	Embroidered tote bag for daily use.	Ï¡┘é┘èÏ¿Ï® ┘é┘àÏğÏ┤ ┘ü┘äÏ│ÏÀ┘è┘å┘èÏ® ┘àÏÀÏ▒Ï▓Ï®	Embroidered Palestine Tote Bag	8.00	t	t	f	\N	\N	t
13	Modern Soyut Kanvas Tablo	Soyut Sanat	modern-soyut-kanvas-tablo		TEST-MODERN-001	8690000000011	7ANRPS48	Genel		Modern sanat ak├ä┬▒m├ä┬▒ndan ilham alan soyut kanvas tablo.	Bu soyut kanvas tablo, modern sanat├ä┬▒n enerjisini evinize ta├à┼©├ä┬▒yor. Y├â┬╝ksek kaliteli bask├ä┬▒ ve premium kanvas malzeme.						Stokta	550	\N	200	20	3	2	5	t	t	t	f	t	1	11	0	0	1	\N	Modern Soyut Kanvas Tablo	Modern soyut kanvas tablo koleksiyonumuzu ke├à┼©fedin.		17	2026-06-25 21:44:12.257319+00	f	410	├İ┬¬├İ┬¼├ÖÔÇŞ├İ┬¿ ├ÖÔÇŞ├Ö╦å├İ┬¡├İ┬® ├İ┬ğ├ÖÔÇŞ├ÖãÆ├İ┬ğ├ÖÔÇá├Ö┬ü├İ┬ğ├İ┬│ ├İ┬ğ├ÖÔÇŞ├İ┬¬├İ┬¼├İ┬▒├Ö┼á├İ┬»├Ö┼á├İ┬® ├ÖÔÇí├İ┬░├ÖÔÇí ├İ┬À├İ┬ğ├ÖÔÇÜ├İ┬® ├İ┬ğ├ÖÔÇŞ├Ö┬ü├ÖÔÇá ├İ┬ğ├ÖÔÇŞ├İ┬¡├İ┬»├Ö┼á├İ┬½ ├İ┬Ñ├ÖÔÇŞ├ÖÔÇ░ ├ÖÔÇĞ├ÖÔÇá├İ┬▓├ÖÔÇŞ├ÖãÆ. ├İ┬À├İ┬¿├İ┬ğ├İ┬╣├İ┬® ├İ┬╣├İ┬ğ├ÖÔÇŞ├Ö┼á├İ┬® ├İ┬ğ├ÖÔÇŞ├İ┬¼├Ö╦å├İ┬»├İ┬® ├Ö╦å├ÖÔÇĞ├İ┬ğ├İ┬»├İ┬® ├ÖãÆ├İ┬ğ├ÖÔÇá├Ö┬ü├İ┬ğ├İ┬│ ├Ö┬ü├İ┬ğ├İ┬«├İ┬▒├İ┬®.	This abstract canvas brings the energy of modern art into your home. High quality print and premium canvas material.	├ÖÔÇŞ├Ö╦å├İ┬¡├İ┬® ├ÖãÆ├İ┬ğ├ÖÔÇá├Ö┬ü├İ┬ğ├İ┬│ ├İ┬¬├İ┬¼├İ┬▒├Ö┼á├İ┬»├Ö┼á├İ┬® ├İ┬¡├İ┬»├Ö┼á├İ┬½├İ┬®	Modern Abstract Canvas Painting	├ÖÔÇŞ├Ö╦å├İ┬¡├İ┬® ├ÖãÆ├İ┬ğ├ÖÔÇá├Ö┬ü├İ┬ğ├İ┬│ ├İ┬¬├İ┬¼├İ┬▒├Ö┼á├İ┬»├Ö┼á├İ┬® ├ÖÔÇĞ├İ┬│├İ┬¬├Ö╦å├İ┬¡├İ┬ğ├İ┬® ├ÖÔÇĞ├ÖÔÇá ├İ┬ğ├ÖÔÇŞ├İ┬¡├İ┬▒├ÖãÆ├İ┬ğ├İ┬¬ ├İ┬ğ├ÖÔÇŞ├Ö┬ü├ÖÔÇá├Ö┼á├İ┬® ├İ┬ğ├ÖÔÇŞ├İ┬¡├İ┬»├Ö┼á├İ┬½├İ┬®.	Abstract canvas painting inspired by modern art movements.					0	f	f	f	\N	\N	t
15	Premium ├à┬Şehir Sil├â┬╝eti Kanvas	├à┬Şehir Sil├â┬╝eti	premium-sehir-silueti-kanvas		TEST-SEHIR-003	8690000000035	7ANRPS48	Genel		Modern ├à┼©ehir sil├â┬╝etini yans├ä┬▒tan premium kanvas tablo.	├ä┬░stanbul, Londra ve Dubai sil├â┬╝etlerinden ilham alan bu premium kanvas tablo, ├à┼©ehir ya├à┼©am├ä┬▒n├ä┬▒n enerjisini yans├ä┬▒t├ä┬▒r.						Stokta	950	\N	350	20	5	2	7	t	t	f	f	f	3	0	0	0	1	\N	Premium ├à┬Şehir Sil├â┬╝eti Kanvas	Premium ├à┼©ehir sil├â┬╝eti kanvas tablo koleksiyonu.		13	2026-06-25 21:44:12.257319+00	f	710	├ÖÔÇĞ├İ┬│├İ┬¬├Ö╦å├İ┬¡├İ┬ğ├İ┬® ├ÖÔÇĞ├ÖÔÇá ├İ┬ú├Ö┬ü├ÖÔÇÜ ├İ┬Ñ├İ┬│├İ┬À├ÖÔÇá├İ┬¿├Ö╦å├ÖÔÇŞ ├Ö╦å├ÖÔÇŞ├ÖÔÇá├İ┬»├ÖÔÇá ├Ö╦å├İ┬»├İ┬¿├Ö┼á├İ┼Æ ├İ┬¬├İ┬╣├ÖãÆ├İ┬│ ├ÖÔÇŞ├Ö╦å├İ┬¡├İ┬® ├İ┬ğ├ÖÔÇŞ├ÖãÆ├İ┬ğ├ÖÔÇá├Ö┬ü├İ┬ğ├İ┬│ ├İ┬ğ├ÖÔÇŞ├Ö┬ü├İ┬ğ├İ┬«├İ┬▒├İ┬® ├ÖÔÇí├İ┬░├ÖÔÇí ├İ┬À├İ┬ğ├ÖÔÇÜ├İ┬® ├İ┬ğ├ÖÔÇŞ├İ┬¡├Ö┼á├İ┬ğ├İ┬® ├İ┬ğ├ÖÔÇŞ├İ┬¡├İ┬Â├İ┬▒├Ö┼á├İ┬®.	Inspired by the skylines of Istanbul, London and Dubai, this premium canvas reflects the energy of city life.	├ÖãÆ├İ┬ğ├ÖÔÇá├Ö┬ü├İ┬ğ├İ┬│ ├İ┬ú├Ö┬ü├ÖÔÇÜ ├İ┬ğ├ÖÔÇŞ├ÖÔÇĞ├İ┬»├Ö┼á├ÖÔÇá├İ┬® ├İ┬ğ├ÖÔÇŞ├Ö┬ü├İ┬ğ├İ┬«├İ┬▒	Premium City Skyline Canvas	├ÖÔÇŞ├Ö╦å├İ┬¡├İ┬® ├ÖãÆ├İ┬ğ├ÖÔÇá├Ö┬ü├İ┬ğ├İ┬│ ├Ö┬ü├İ┬ğ├İ┬«├İ┬▒├İ┬® ├İ┬¬├İ┬╣├ÖãÆ├İ┬│ ├İ┬ú├Ö┬ü├ÖÔÇÜ ├İ┬ğ├ÖÔÇŞ├ÖÔÇĞ├İ┬»├Ö┼á├ÖÔÇá├İ┬® ├İ┬ğ├ÖÔÇŞ├İ┬¡├İ┬»├Ö┼á├İ┬½├İ┬®.	Premium canvas painting reflecting the modern city skyline.					0	f	f	f	\N	\N	t
14	├âÔÇíi├â┬ğekli Dekoratif Poster	├âÔÇíi├â┬ğekli Poster	cicekli-dekoratif-poster		TEST-CICEK-002	8690000000028	7ANRPS48	Genel		Do├ä┼©an├ä┬▒n renklerini yans├ä┬▒tan ├â┬ği├â┬ğekli dekoratif poster.	Canl├ä┬▒ renkler ve zarif desenlerle haz├ä┬▒rlanm├ä┬▒├à┼© ├â┬ği├â┬ğekli poster. Her ortama do├ä┼©al bir hava katar.						Stokta	320	\N	100	20	2	1	4	t	f	f	f	t	2	0	0	0	1	\N	├âÔÇíi├â┬ğekli Dekoratif Poster	├âÔÇíi├â┬ğekli dekoratif poster ├â┬ğe├à┼©itleri.		15	2026-06-25 21:44:12.257319+00	f	240	├İ┬¿├Ö╦å├İ┬│├İ┬¬├İ┬▒ ├İ┬▓├ÖÔÇí├Ö╦å├İ┬▒ ├İ┬¿├İ┬ú├ÖÔÇŞ├Ö╦å├İ┬ğ├ÖÔÇá ├İ┬▓├İ┬ğ├ÖÔÇí├Ö┼á├İ┬® ├Ö╦å├İ┬ú├ÖÔÇá├ÖÔÇĞ├İ┬ğ├İ┬À ├İ┬ú├ÖÔÇá├Ö┼á├ÖÔÇÜ├İ┬®. ├Ö┼á├İ┬Â├Ö┬ü├Ö┼á ├ÖÔÇŞ├ÖÔÇĞ├İ┬│├İ┬® ├İ┬À├İ┬¿├Ö┼á├İ┬╣├Ö┼á├İ┬® ├İ┬╣├ÖÔÇŞ├ÖÔÇ░ ├İ┬ú├Ö┼á ├ÖÔÇĞ├ÖãÆ├İ┬ğ├ÖÔÇá.	Floral poster prepared with vibrant colors and elegant patterns. Adds a natural touch to any environment.	├İ┬¿├Ö╦å├İ┬│├İ┬¬├İ┬▒ ├İ┬▓├İ┬«├İ┬▒├Ö┬ü├Ö┼á ├İ┬¿├İ┬ğ├ÖÔÇŞ├İ┬ú├İ┬▓├ÖÔÇí├İ┬ğ├İ┬▒	Floral Decorative Poster	├İ┬¿├Ö╦å├İ┬│├İ┬¬├İ┬▒ ├İ┬▓├İ┬«├İ┬▒├Ö┬ü├Ö┼á ├İ┬¿├İ┬ğ├ÖÔÇŞ├İ┬ú├İ┬▓├ÖÔÇí├İ┬ğ├İ┬▒ ├Ö┼á├İ┬╣├ÖãÆ├İ┬│ ├İ┬ú├ÖÔÇŞ├Ö╦å├İ┬ğ├ÖÔÇá ├İ┬ğ├ÖÔÇŞ├İ┬À├İ┬¿├Ö┼á├İ┬╣├İ┬®.	Floral decorative poster reflecting the colors of nature.					0	f	f	f	\N	\N	t
1	Filistin Motifli Duvar Tablosu	Filistin Motifli Duvar Tablosu	palestinian-pattern-wall-art		DEMO-001		7ANRPS48	Dekoratif ├£r├╝n	palestine, gift, decor	Filistin motifleriyle haz─▒rlanm─▒┼ş dekoratif duvar tablosu.	<p>Filistin motifleriyle haz─▒rlanm─▒┼ş dekoratif duvar tablosu. ├£r├╝n demo ama├ğl─▒d─▒r.</p>	Boyut: Standart\\nTema: Filistin\\n├£retim: Demo	Kaliteli bask─▒ ve dekoratif malzeme	Kuru bezle temizleyiniz	Koruyucu paketleme	/slider4.png	Stokta	95.00	76.000	52.25	17	2	1	4	t	t	t	f	t	1	12	0	0	1	\N	Filistin Motifli Duvar Tablosu	Filistin motifleriyle haz─▒rlanm─▒┼ş dekoratif duvar tablosu.	palestine,7anrps48,decor,gift	1	2026-06-19 12:01:10.869407+00	f	80.75	<p>┘ä┘êÏ¡Ï® Ï¼Ï»ÏğÏ▒┘èÏ® ┘àÏ▓Ï«Ï▒┘üÏ® ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å Ïğ┘ä┘å┘é┘êÏ┤ Ïğ┘ä┘ü┘äÏ│ÏÀ┘è┘å┘èÏ®. ┘çÏ░Ïğ Ïğ┘ä┘à┘åÏ¬Ï¼ ┘äÏ║Ï▒ÏÂ Ïğ┘äÏ╣Ï▒ÏÂ Ïğ┘äÏ¬Ï¼Ï▒┘èÏ¿┘è.</p>	<p>Decorative wall art inspired by Palestinian patterns. This product is for demo purposes.</p>	┘ä┘êÏ¡Ï® Ï¼Ï»ÏğÏ▒┘èÏ® Ï¿┘å┘é┘êÏ┤ ┘ü┘äÏ│ÏÀ┘è┘å┘èÏ®	Palestinian Pattern Wall Art	┘ä┘êÏ¡Ï® Ï¼Ï»ÏğÏ▒┘èÏ® ┘àÏ▓Ï«Ï▒┘üÏ® ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å Ïğ┘ä┘å┘é┘êÏ┤ Ïğ┘ä┘ü┘äÏ│ÏÀ┘è┘å┘èÏ®.	Decorative wall art inspired by Palestinian patterns.	┘ä┘êÏ¡Ï® Ï¼Ï»ÏğÏ▒┘èÏ® ┘àÏ▓Ï«Ï▒┘üÏ® ┘àÏ│Ï¬┘êÏ¡ÏğÏ® ┘à┘å Ïğ┘ä┘å┘é┘êÏ┤ Ïğ┘ä┘ü┘äÏ│ÏÀ┘è┘å┘èÏ®.	Decorative wall art inspired by Palestinian patterns.	┘ä┘êÏ¡Ï® Ï¼Ï»ÏğÏ▒┘èÏ® Ï¿┘å┘é┘êÏ┤ ┘ü┘äÏ│ÏÀ┘è┘å┘èÏ®	Palestinian Pattern Wall Art	8.00	t	t	f	\N	2026-06-25 20:53:38.765956+00	t
\.


--
-- Data for Name: Yorumlar; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."Yorumlar" ("Id", "UrunId", "AppUserId", "AdSoyad", "YorumMetni", "Puan", "OnayliMi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: ZiyaretciLoglari; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."ZiyaretciLoglari" ("Id", "IpAdresi", "Url", "CihazBilgisi", "ReferansUrl", "KullaniciAdi", "Metod", "Sehir", "Ulke", "Tarayici", "CihazModeli", "IsletimSistemi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
1	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:15:20.970354+00	f
2	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:15:31.364161+00	f
3	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:15:45.469794+00	f
4	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:15:45.481645+00	f
5	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:15:54.54911+00	f
6	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:15:58.626576+00	f
7	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:15:58.639819+00	f
8	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:16:07.53806+00	f
9	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:16:08.43095+00	f
10	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:16:20.993002+00	f
11	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap?ReturnUrl=%2FFavori	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:16:23.755253+00	f
12	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:16:29.244769+00	f
13	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=10	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:17:00.605947+00	f
14	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=10	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:17:00.619341+00	f
15	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=10	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:17:11.263888+00	f
16	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:17:20.177939+00	f
17	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:17:57.431541+00	f
18	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Hakkimizda	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:18:01.370587+00	f
19	::1	/Sozlesmeler/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:18:11.866783+00	f
20	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sozlesmeler/MesafeliSatis	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:18:15.84705+00	f
21	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:18:34.540435+00	f
22	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:18:36.603147+00	f
23	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:18:53.617654+00	f
24	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:18:53.946333+00	f
25	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:19:27.877917+00	f
26	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:19:31.081951+00	f
27	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:19:33.407485+00	f
28	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap?ReturnUrl=%2FFavori	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:19:36.484941+00	f
29	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:20:06.193263+00	f
30	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:20:06.518976+00	f
31	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:20:27.388907+00	f
32	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:20:27.396886+00	f
33	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-14 08:20:27.723136+00	f
34	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:22.215538+00	f
35	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:23.520353+00	f
36	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:24.133549+00	f
37	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:24.133555+00	f
38	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:29.995477+00	f
39	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:30.014191+00	f
40	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:30.229928+00	f
41	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:30.473768+00	f
42	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:30.473768+00	f
43	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:33.294584+00	f
44	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:33.314172+00	f
45	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:33.439009+00	f
46	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:33.68203+00	f
47	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:53:33.682033+00	f
48	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:03.240582+00	f
49	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:04.469262+00	f
50	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.136095+00	f
51	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.136095+00	f
52	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.136095+00	f
53	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.136098+00	f
54	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.136095+00	f
55	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.136095+00	f
56	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.181459+00	f
57	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.184515+00	f
59	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.184495+00	f
58	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.185077+00	f
60	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.184494+00	f
61	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.186799+00	f
62	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.240201+00	f
63	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.240077+00	f
64	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:55:05.357655+00	f
65	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.345871+00	f
66	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.544379+00	f
73	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.859324+00	f
81	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:04.035312+00	f
67	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.831478+00	f
68	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.831461+00	f
84	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.835403+00	f
69	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.831461+00	f
70	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.831461+00	f
71	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.831461+00	f
72	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.831461+00	f
75	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.859316+00	f
76	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.859563+00	f
74	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.859867+00	f
77	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.859703+00	f
78	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.862109+00	f
79	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.940762+00	f
80	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:03.940254+00	f
82	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.510311+00	f
83	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.835403+00	f
85	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.835919+00	f
86	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.83593+00	f
87	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.838532+00	f
88	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.838462+00	f
91	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.861058+00	f
90	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.861032+00	f
89	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.861416+00	f
93	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.860934+00	f
92	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.860934+00	f
94	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.861001+00	f
95	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.919668+00	f
96	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:38.919668+00	f
97	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:39.005752+00	f
98	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.183614+00	f
99	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.429108+00	f
100	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.429736+00	f
101	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.428873+00	f
102	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.43043+00	f
103	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.435469+00	f
104	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.435386+00	f
105	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.456825+00	f
106	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.456359+00	f
108	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.455926+00	f
107	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.455926+00	f
109	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.457541+00	f
110	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.463547+00	f
111	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.50765+00	f
112	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.568418+00	f
113	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.568411+00	f
114	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:56:50.624341+00	f
115	127.0.0.1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:16.338593+00	f
116	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.914645+00	f
118	127.0.0.1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.91464+00	f
117	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.914648+00	f
119	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.914645+00	f
120	127.0.0.1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.914649+00	f
121	127.0.0.1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.914642+00	f
122	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.974678+00	f
123	127.0.0.1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.974687+00	f
124	127.0.0.1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.974745+00	f
125	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.974809+00	f
126	127.0.0.1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.974678+00	f
127	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:17.974678+00	f
128	127.0.0.1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:18.013876+00	f
129	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:18.02019+00	f
130	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 10:57:18.080784+00	f
131	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:36.994654+00	f
134	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.628804+00	f
133	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.6288+00	f
132	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.628801+00	f
135	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.6288+00	f
136	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.635559+00	f
137	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.635559+00	f
138	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.654871+00	f
139	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.662949+00	f
140	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.664597+00	f
141	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.666588+00	f
142	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.667072+00	f
143	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.666574+00	f
148	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:06:13.584944+00	f
145	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.716346+00	f
147	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:45.939975+00	f
144	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.71639+00	f
153	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:07:07.10149+00	f
146	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:04:37.769529+00	f
152	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:07:03.585449+00	f
149	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:06:28.030432+00	f
154	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:07:37.917374+00	f
158	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:09:02.061598+00	f
150	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:06:49.938388+00	f
151	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:06:54.441028+00	f
155	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:07:40.109723+00	f
156	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:07:40.688619+00	f
157	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:08:55.706848+00	f
159	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:09:41.075512+00	f
161	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:09:43.070178+00	f
160	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:09:43.070178+00	f
162	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:09:43.070178+00	f
163	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:09:43.143769+00	f
164	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:12:04.02837+00	f
165	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:12:04.515201+00	f
166	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:12:04.515207+00	f
167	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:12:04.5165+00	f
168	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:12:04.589374+00	f
169	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:15:00.753907+00	f
171	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:15:01.398519+00	f
170	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:15:01.398519+00	f
172	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:15:01.402752+00	f
173	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:15:01.467212+00	f
174	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:15:38.448165+00	f
175	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:15:44.433748+00	f
176	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:15:49.154072+00	f
177	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:16:39.724314+00	f
178	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:16:42.266611+00	f
179	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:16:42.615388+00	f
180	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:16:42.615222+00	f
181	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:16:42.615296+00	f
182	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:16:42.677573+00	f
183	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:18:22.388909+00	f
185	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:18:22.837551+00	f
184	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:18:22.837551+00	f
186	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:18:22.837551+00	f
187	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:18:22.896594+00	f
188	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:19:32.304573+00	f
190	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:19:32.634413+00	f
189	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:19:32.634296+00	f
191	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:19:32.634296+00	f
192	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:19:32.692608+00	f
193	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.363842+00	f
194	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.363842+00	f
195	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.366375+00	f
196	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.367363+00	f
197	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.367408+00	f
198	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.366811+00	f
199	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.412528+00	f
200	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.412738+00	f
201	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.412905+00	f
202	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.412361+00	f
203	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.412369+00	f
204	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:21:47.412361+00	f
205	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:22:13.670891+00	f
206	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:22:13.988245+00	f
207	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:22:13.988059+00	f
208	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:22:13.991813+00	f
209	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:22:14.049126+00	f
210	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:22:14.049126+00	f
211	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:22:14.097179+00	f
212	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:02.692402+00	f
213	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:03.025045+00	f
214	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:03.025045+00	f
215	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:03.796637+00	f
216	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:03.796637+00	f
217	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:34.664687+00	f
218	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:34.90212+00	f
220	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:35.00058+00	f
219	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:35.00058+00	f
221	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:35.071338+00	f
222	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:44.89481+00	f
223	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:44.908423+00	f
224	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:45.278672+00	f
227	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:45.405764+00	f
226	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:45.360003+00	f
228	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:50.545784+00	f
231	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:50.841789+00	f
234	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:52.377689+00	f
237	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:52.728086+00	f
225	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:45.360003+00	f
229	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:50.559757+00	f
244	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:54.725304+00	f
230	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:50.785781+00	f
233	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:50.894154+00	f
242	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:54.67699+00	f
245	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:54.771368+00	f
232	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:50.841789+00	f
235	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:52.388684+00	f
236	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:52.671047+00	f
239	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:52.786524+00	f
240	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:54.47127+00	f
243	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:54.725658+00	f
238	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:52.728086+00	f
241	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:23:54.481328+00	f
246	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:39:23.335228+00	f
247	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:39:23.349024+00	f
248	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:39:23.826234+00	f
249	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:39:23.826234+00	f
250	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:39:24.502986+00	f
251	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:39:24.502986+00	f
252	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:43:16.081437+00	f
253	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:43:16.105144+00	f
254	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:43:16.54411+00	f
255	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:43:16.544087+00	f
256	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:43:16.936461+00	f
257	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:43:16.936461+00	f
258	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:44:59.102903+00	f
259	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:44:59.382172+00	f
260	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:45:00.943668+00	f
261	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:45:01.015467+00	f
262	127.0.0.1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:45:01.019474+00	f
263	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:45:01.15981+00	f
264	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:45:20.156101+00	f
265	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:45:20.481763+00	f
266	127.0.0.1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:45:20.530914+00	f
267	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:45:20.53091+00	f
268	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:45:20.580551+00	f
269	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:47:26.013792+00	f
270	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:47:27.021017+00	f
271	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:47:27.044621+00	f
272	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:47:27.807115+00	f
273	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:47:28.018291+00	f
274	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:47:28.081339+00	f
275	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:47:28.081341+00	f
276	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:47:28.195628+00	f
277	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:52:21.208583+00	f
278	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:52:21.439236+00	f
279	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:20.31123+00	f
280	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:20.541815+00	f
281	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:21.850983+00	f
282	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:21.906118+00	f
283	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:21.906169+00	f
284	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:21.994567+00	f
285	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:37.091621+00	f
286	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:37.109263+00	f
287	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:37.481143+00	f
288	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:37.589389+00	f
289	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:37.589351+00	f
290	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:37.657401+00	f
291	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:48.030959+00	f
292	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:48.045785+00	f
293	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:48.372548+00	f
294	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:48.434981+00	f
295	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:48.434854+00	f
296	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:53:48.504668+00	f
297	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:54:03.580434+00	f
298	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:54:03.604685+00	f
299	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:54:04.156231+00	f
300	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:56:19.018625+00	f
301	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 11:56:19.292292+00	f
302	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:11.473897+00	f
303	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:12.747361+00	f
304	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:12.799058+00	f
305	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:13.480561+00	f
306	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:13.530553+00	f
307	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:13.532429+00	f
308	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:13.585568+00	f
309	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:57.760067+00	f
310	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:57.778901+00	f
311	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:58.172346+00	f
312	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:58.246023+00	f
313	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:58.24602+00	f
314	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:06:58.316552+00	f
315	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:07:18.641442+00	f
316	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:07:18.653839+00	f
317	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:07:18.892702+00	f
318	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:07:44.719337+00	f
319	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:07:44.734534+00	f
320	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:07:45.520277+00	f
321	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:08:01.296816+00	f
322	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:08:01.310521+00	f
323	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:08:01.597211+00	f
324	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:08:09.553156+00	f
325	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestinian-pattern-wall-art-1	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:08:10.697481+00	f
326	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:18:29.192601+00	f
327	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestinian-pattern-wall-art-1	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:18:29.321647+00	f
328	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestinian-pattern-wall-art-1	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:25:49.089313+00	f
329	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:25:49.959974+00	f
331	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:25:49.998132+00	f
330	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:25:49.99813+00	f
332	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:25:50.059996+00	f
333	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:25:57.556607+00	f
334	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:25:57.567287+00	f
335	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestinian-pattern-wall-art-1	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:25:58.679898+00	f
336	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:13.04468+00	f
337	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:13.051514+00	f
338	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestinian-pattern-wall-art-1	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:13.681412+00	f
339	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:20.527116+00	f
340	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:20.532752+00	f
341	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:20.711693+00	f
342	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:20.71252+00	f
343	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:20.714103+00	f
344	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:21.684707+00	f
345	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:47.605675+00	f
346	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:47.611722+00	f
347	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:47.794381+00	f
348	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:47.794381+00	f
349	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:47.948883+00	f
350	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:26:47.948883+00	f
351	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:34:42.793256+00	f
352	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:34:42.92349+00	f
353	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:34:43.562216+00	f
354	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:34:43.609758+00	f
355	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:34:43.609758+00	f
356	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:34:43.64136+00	f
360	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:35:07.875167+00	f
357	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:34:48.557576+00	f
358	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:34:48.57243+00	f
359	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:34:48.901503+00	f
361	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:35:07.882329+00	f
362	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:35:08.101486+00	f
363	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:44:26.572039+00	f
364	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:44:27.511335+00	f
365	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:44:27.54661+00	f
366	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:44:27.622703+00	f
367	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:44:32.824031+00	f
368	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:45:31.168525+00	f
369	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:45:31.368394+00	f
370	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:45:31.395955+00	f
371	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:45:31.422849+00	f
372	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:45:44.061487+00	f
373	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:46:21.302727+00	f
374	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:46:21.448416+00	f
375	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:46:21.51117+00	f
376	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:46:40.62757+00	f
377	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:46:41.179181+00	f
378	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:46:53.477124+00	f
379	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:46:53.484703+00	f
380	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:46:53.630128+00	f
381	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:46:53.699156+00	f
382	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:47:34.934847+00	f
383	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:47:35.063265+00	f
384	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:47:35.116203+00	f
385	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:48:51.779094+00	f
386	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:48:51.925638+00	f
387	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:48:51.992711+00	f
388	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:53:10.578671+00	f
389	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:53:10.697758+00	f
390	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:53:26.520217+00	f
391	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:53:29.07028+00	f
392	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:53:29.077461+00	f
393	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:53:34.294429+00	f
394	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:53:34.301251+00	f
395	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:54:10.68965+00	f
396	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 12:54:10.697957+00	f
397	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:20:26.619643+00	f
398	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:20:38.009782+00	f
399	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:20:38.03143+00	f
400	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:20:51.38124+00	f
401	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:20:51.404777+00	f
402	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:20:54.825631+00	f
403	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:20:54.837588+00	f
404	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:01.477929+00	f
405	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:01.495452+00	f
406	::1	/Urun/Detay/palestine-keepsake-gift-box-3	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:05.833505+00	f
407	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestine-keepsake-gift-box-3	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:36.186827+00	f
408	::1	/Urun/Detay/palestine-keepsake-gift-box-3	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestine-keepsake-gift-box-3	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:36.202016+00	f
409	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestine-keepsake-gift-box-3	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:39.450007+00	f
410	::1	/Urun/Detay/palestine-keepsake-gift-box-3	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestine-keepsake-gift-box-3	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:39.463273+00	f
411	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestine-keepsake-gift-box-3	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:42.727541+00	f
412	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:47.684501+00	f
413	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:47.701519+00	f
414	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:58.025976+00	f
415	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:21:58.035868+00	f
416	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:22:11.35403+00	f
417	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:22:18.358383+00	f
418	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:22:20.555052+00	f
419	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:22:20.571392+00	f
420	::1	/Hesap/SifremiUnuttum	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:22:31.192562+00	f
421	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/SifremiUnuttum	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:22:55.4928+00	f
422	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap?ReturnUrl=%2FFavori	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:22:58.076758+00	f
423	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:24:14.566351+00	f
424	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:29:12.619863+00	f
425	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:29:25.569848+00	f
426	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:29:47.005953+00	f
427	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:29:47.022158+00	f
428	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:30:04.430695+00	f
429	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:30:59.356099+00	f
430	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:30:59.371433+00	f
431	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:31:47.350679+00	f
432	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:33:37.909979+00	f
433	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:33:39.13084+00	f
434	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:35:29.571284+00	f
435	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:41:02.476692+00	f
436	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Home/Index	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:42:08.722138+00	f
437	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Home/Index	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:42:13.663692+00	f
438	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Home/Index	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:43:22.815377+00	f
439	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:43:31.141994+00	f
440	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:43:35.347793+00	f
441	::1	/Urun/Detay/gaza-solidarity-poster-5	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:45:36.427702+00	f
442	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/gaza-solidarity-poster-5	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:47:21.745622+00	f
443	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:48:02.40702+00	f
444	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:48:07.744823+00	f
445	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:50:33.544366+00	f
446	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:57:04.724868+00	f
447	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:57:04.977784+00	f
448	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:57:59.977504+00	f
449	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:58:00.014058+00	f
450	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 19:59:11.219962+00	f
451	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:00:32.740647+00	f
452	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:00:45.875343+00	f
453	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:00:45.91522+00	f
454	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Home/Index	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:01:49.509773+00	f
455	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:02:13.853447+00	f
456	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:02:39.258419+00	f
457	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:02:39.279852+00	f
458	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:03:29.714987+00	f
459	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:03.42963+00	f
460	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:04.82867+00	f
461	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:11.341849+00	f
462	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:15.59281+00	f
463	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:24.569455+00	f
464	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:24.593177+00	f
465	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:26.637016+00	f
466	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:26.65376+00	f
467	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:51.030662+00	f
468	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:05:51.056049+00	f
469	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:07:35.675487+00	f
470	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:07:35.697233+00	f
471	::1	/Favori	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:07:37.06037+00	f
472	::1	/Profil	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Favori	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:08:02.610531+00	f
473	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Profil	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:08:54.794723+00	f
474	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:09:20.2269+00	f
475	::1	/Urun/Detay/palestine-keepsake-gift-box-3	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:09:22.334961+00	f
476	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestine-keepsake-gift-box-3	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:09:35.2653+00	f
477	::1	/Urun/Detay/olive-branch-decor-set-2	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:09:35.9419+00	f
478	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/olive-branch-decor-set-2	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:09:41.161729+00	f
479	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/olive-branch-decor-set-2	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:16:54.029885+00	f
480	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:17:06.333445+00	f
481	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:17:06.381269+00	f
482	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:19:55.306668+00	f
483	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/olive-branch-decor-set-2	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:20:34.514204+00	f
484	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:20:36.524803+00	f
485	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-19 20:24:41.839274+00	f
486	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Home/Index	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 19:51:58.056965+00	f
527	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/CarkOdul	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:06:42.535531+00	f
487	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Home/Index	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 19:52:01.378932+00	f
488	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Home/Index	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 19:52:06.747699+00	f
489	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Home/Index	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 19:52:27.070806+00	f
490	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 19:52:36.973719+00	f
491	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Home/Index	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 19:54:16.908184+00	f
492	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Urun	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 19:55:42.438512+00	f
493	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 19:57:06.760054+00	f
494	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 19:57:06.80278+00	f
495	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 20:06:48.132802+00	f
496	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Urun	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 20:08:40.32968+00	f
497	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Urun	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 20:08:47.443725+00	f
498	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Urun	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-21 20:08:51.314013+00	f
499	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 08:37:37.234497+00	f
500	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 08:38:16.147746+00	f
501	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Siparis	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 12:50:16.314704+00	f
502	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/Siparis	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:00:05.385775+00	f
503	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap?returnUrl=%2FAdmin%2FSiparis	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:00:08.242011+00	f
504	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-23 20:00:15.104863+00	f
505	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:00:27.761485+00	f
506	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:01:18.646936+00	f
507	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:01:41.991408+00	f
508	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:02:20.700556+00	f
509	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-23 20:08:18.188342+00	f
510	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:08:33.2014+00	f
511	::1	/Urun/Detay/palestine-keepsake-gift-box-3	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:11:38.420851+00	f
512	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestine-keepsake-gift-box-3	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:12:02.696222+00	f
513	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestine-keepsake-gift-box-3	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:12:02.807553+00	f
514	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:13:13.261879+00	f
515	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:17:36.301912+00	f
516	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:17:43.941562+00	f
517	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:20:10.609799+00	f
518	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-23 20:52:58.455614+00	f
519	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:53:06.815745+00	f
520	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 20:53:53.811077+00	f
521	::1	/Hata/404	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-23 21:05:08.591471+00	f
522	::1	/Hata/404	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-23 21:05:28.897117+00	f
523	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-23 21:05:36.184281+00	f
524	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:05:55.382785+00	f
525	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/CarkOdul	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:05:56.074221+00	f
526	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:06:41.538442+00	f
528	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:07:32.408082+00	f
529	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/CarkOdul	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:07:32.839806+00	f
530	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-23 21:08:21.961125+00	f
531	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:08:38.043996+00	f
532	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:09:00.348253+00	f
533	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:12:15.234423+00	f
534	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:12:21.497003+00	f
535	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/CarkOdul	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:16:35.777987+00	f
536	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Admin/CarkOdul	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:16:39.254422+00	f
537	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:21:19.887757+00	f
538	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:22:45.737792+00	f
539	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-23 21:23:04.711628+00	f
540	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-24 15:57:05.916079+00	f
541	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-24 16:06:08.149006+00	f
542	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-24 17:08:23.495783+00	f
543	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-24 17:08:26.444544+00	f
544	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-24 17:08:27.079986+00	f
545	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Sepet	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-24 17:08:55.236727+00	f
546	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 21:29:17.764891+00	f
547	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 21:29:19.175056+00	f
548	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:29:32.066961+00	f
549	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:29:41.335014+00	f
550	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 21:44:16.330653+00	f
551	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 21:44:17.406041+00	f
552	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:44:27.438873+00	f
553	::1	/Hesap/EpostaOnayBilgilendirme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/KayitOl	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:45:06.691292+00	f
554	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:45:30.121802+00	f
555	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:46:40.333724+00	f
556	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:46:53.585342+00	f
557	::1	/Hesap/EpostaOnayBilgilendirme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/KayitOl	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:47:18.592893+00	f
558	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/EpostaOnayBilgilendirme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:47:24.462423+00	f
559	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:47:27.911275+00	f
560	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:47:27.93596+00	f
561	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:47:49.79161+00	f
562	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:48:32.935365+00	f
563	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:48:41.109326+00	f
564	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/palestinian-pattern-wall-art-1	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:48:54.377211+00	f
565	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 21:49:28.57431+00	f
566	::1	/	Mozilla/5.0 (Windows NT; Windows NT 10.0; tr-TR) WindowsPowerShell/5.1.26100.8737		Misafir	GET	-	-	Bilinmiyor	PC (Windows)	Windows	2026-06-25 21:55:24.329271+00	f
567	::1	/Urun	Mozilla/5.0 (Windows NT; Windows NT 10.0; tr-TR) WindowsPowerShell/5.1.26100.8737		Misafir	GET	-	-	Bilinmiyor	PC (Windows)	Windows	2026-06-25 21:55:27.16826+00	f
568	::1	/Urun	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 21:55:36.362615+00	f
569	::1	/Urun	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 21:55:42.430657+00	f
570	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:09:45.23538+00	f
571	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:09:59.909811+00	f
572	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:03.164523+00	f
573	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:06.168199+00	f
574	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:09.102059+00	f
575	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:11.885116+00	f
576	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:11.906272+00	f
577	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:14.692785+00	f
578	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:17.528189+00	f
579	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:20.28698+00	f
580	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:23.224564+00	f
581	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:26.021347+00	f
582	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:28.750811+00	f
583	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:32.402446+00	f
584	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:35.755945+00	f
585	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:39.139055+00	f
586	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:42.104295+00	f
587	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:42.124433+00	f
588	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:45.068153+00	f
589	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:48.11007+00	f
590	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:51.01041+00	f
591	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:53.895368+00	f
592	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:56.822346+00	f
593	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:10:59.804018+00	f
594	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:03.908582+00	f
595	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:07.248608+00	f
596	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:10.502766+00	f
597	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:13.411728+00	f
598	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:13.428484+00	f
599	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:16.346946+00	f
600	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:19.368435+00	f
601	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:22.355411+00	f
602	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:25.43963+00	f
603	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:28.448523+00	f
604	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:11:56.105334+00	f
605	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:15:28.40954+00	f
606	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:15:32.105228+00	f
607	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:15:35.686067+00	f
608	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:15:53.599486+00	f
609	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:17.046206+00	f
610	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:19.295911+00	f
611	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:21.167739+00	f
612	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:24.393615+00	f
613	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:26.128893+00	f
614	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:28.027286+00	f
615	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:30.349893+00	f
616	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:32.363779+00	f
617	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:35.932877+00	f
618	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:19:37.835258+00	f
619	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:21:48.037319+00	f
620	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:13.902572+00	f
621	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:16.211927+00	f
622	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:17.695555+00	f
623	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:19.190418+00	f
624	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:20.55688+00	f
625	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:20.578964+00	f
626	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:21.954185+00	f
627	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:23.339314+00	f
628	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:24.675512+00	f
629	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:25.97297+00	f
630	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:27.442092+00	f
631	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:29.822444+00	f
632	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:31.337316+00	f
633	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:32.809357+00	f
634	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:34.182175+00	f
635	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:34.202926+00	f
636	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:35.561597+00	f
637	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:36.962134+00	f
638	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:38.266721+00	f
640	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:41.106973+00	f
645	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:48.210246+00	f
639	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:39.568288+00	f
641	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:43.610113+00	f
642	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:45.173556+00	f
647	::1	/Hata/429	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:51.160608+00	f
649	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:54.028564+00	f
643	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:46.788424+00	f
644	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:48.192484+00	f
646	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:49.643172+00	f
648	::1	/Hata/429	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 22:22:52.656761+00	f
650	::1	/Kurumsal/Hakkimizda	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 22:28:51.11332+00	f
651	::1	/Kurumsal/Iletisim	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 22:28:51.659209+00	f
652	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 22:59:25.705112+00	f
653	::1	/Siparis/Odeme	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 22:59:26.287781+00	f
654	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 22:59:39.793954+00	f
655	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:00:07.447595+00	f
656	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:00:08.977933+00	f
657	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:00:11.660827+00	f
658	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:00:13.982031+00	f
659	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:00:44.536558+00	f
660	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:00:44.549862+00	f
661	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:01:03.425019+00	f
662	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:01:03.437543+00	f
663	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:02:31.990543+00	f
664	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:02:35.564816+00	f
665	::1	/Siparis/Odeme	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 23:02:45.609183+00	f
666	::1	/Siparis/Odeme	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 23:02:53.083+00	f
667	::1	/Urun/Detay/modern-soyut-kanvas-tablo-13	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:03:17.635667+00	f
668	::1	/Urun/Detay/palestinian-pattern-wall-art-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:03:20.165198+00	f
669	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-25 23:03:21.953611+00	f
670	::1	/Siparis/Odeme	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-25 23:03:37.254212+00	f
671	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 08:21:18.260767+00	f
672	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 08:21:18.274347+00	f
673	::1	/Siparis/KargoHesapla	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 08:21:19.0137+00	f
674	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 10:27:36.032657+00	f
675	::1	/Siparis/KargoHesapla	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 10:27:36.348104+00	f
676	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 10:27:41.148107+00	f
677	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 10:27:41.158911+00	f
678	::1	/Siparis/KargoHesapla	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 10:27:41.450797+00	f
679	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 10:27:54.43705+00	f
680	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 10:27:54.448474+00	f
681	::1	/Siparis/KargoHesapla	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 10:27:54.705974+00	f
682	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Siparis/Odeme	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 10:28:10.315383+00	f
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: kanvasuser
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20260613194853_InitialCreate	8.0.4
20260613195915_AddWholesalePrice	8.0.4
20260613202928_AddCustomerIdentityFields	8.0.4
20260613203317_AddOrderDeliveryAndPrescriptionFields	8.0.4
20260613221038_AddMultiLanguageFields	8.0.4
20260614073624_AddUserAddressField	8.0.4
20260614075244_AddLoginRequiredSetting	8.0.4
20260614080356_AddWholesaleStatus	8.0.4
20260131211352_BultenTablosu	8.0.0
20260131213049_BultenIpEklendi	8.0.0
20260504111249_AddProductSeoFields	8.0.0
20260508175141_MusteriNotuAlanlariEklendi	8.0.0
20260508204726_Fix_NullableUrunSecenekId_And_SlugIndex	8.0.0
20260523223159_AddFavoriPriceDropFields	8.0.0
20260614085324_AddSiteDegerlendirme	8.0.4
20260614092149_AddStockOutGrayDisplay	8.0.4
20260614092652_AddGiftWrapFields	8.0.4
20260614101244_AddKargoBolgeSistemi	8.0.4
20260614102323_AddSiparisKimlikFoto	8.0.4
20260614103327_AddBankaHesaplari	8.0.4
20260614222407_AddKapidaOdemeBedeli	8.0.4
20260614223525_AddKapidaOdemeLimit	8.0.4
20260614225236_AddReceteGerekliKategori	8.0.4
20260623204005_AddKampanyaBitisTarihi	8.0.0
20260623204006_AddCarkOdulleri	8.0.0
20260619122952_AddSliderMultilingual	8.0.4
20260624190456_AddPushAbonelik	8.0.4
20260615140117_AddWhatsappSiparisFields	8.0.0
\.


--
-- Name: aggregatedcounter_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: kanvasuser
--

SELECT pg_catalog.setval('hangfire.aggregatedcounter_id_seq', 1, false);


--
-- Name: counter_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: kanvasuser
--

SELECT pg_catalog.setval('hangfire.counter_id_seq', 1, false);


--
-- Name: hash_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: kanvasuser
--

SELECT pg_catalog.setval('hangfire.hash_id_seq', 1, false);


--
-- Name: job_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: kanvasuser
--

SELECT pg_catalog.setval('hangfire.job_id_seq', 1, false);


--
-- Name: jobparameter_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: kanvasuser
--

SELECT pg_catalog.setval('hangfire.jobparameter_id_seq', 1, false);


--
-- Name: jobqueue_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: kanvasuser
--

SELECT pg_catalog.setval('hangfire.jobqueue_id_seq', 1, false);


--
-- Name: list_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: kanvasuser
--

SELECT pg_catalog.setval('hangfire.list_id_seq', 1, false);


--
-- Name: set_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: kanvasuser
--

SELECT pg_catalog.setval('hangfire.set_id_seq', 1, false);


--
-- Name: state_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: kanvasuser
--

SELECT pg_catalog.setval('hangfire.state_id_seq', 1, false);


--
-- Name: Adresler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."Adresler_Id_seq"', 1, false);


--
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."AspNetRoleClaims_Id_seq"', 1, false);


--
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."AspNetUserClaims_Id_seq"', 1, false);


--
-- Name: BankaHesaplari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."BankaHesaplari_Id_seq"', 2, true);


--
-- Name: BultenAbonelikleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."BultenAbonelikleri_Id_seq"', 1, false);


--
-- Name: CarkOdulleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."CarkOdulleri_Id_seq"', 8, true);


--
-- Name: Favoriler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."Favoriler_Id_seq"', 1, false);


--
-- Name: HomePageSectionProducts_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."HomePageSectionProducts_Id_seq"', 1, false);


--
-- Name: HomePageSections_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."HomePageSections_Id_seq"', 3, true);


--
-- Name: IadeTalepleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."IadeTalepleri_Id_seq"', 1, false);


--
-- Name: IletisimMesajlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."IletisimMesajlari_Id_seq"', 1, false);


--
-- Name: KargoBolgeFiyatlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."KargoBolgeFiyatlari_Id_seq"', 20, true);


--
-- Name: KargoBolgeSehirler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."KargoBolgeSehirler_Id_seq"', 44, true);


--
-- Name: KargoBolgeler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."KargoBolgeler_Id_seq"', 10, true);


--
-- Name: KargoFirmalari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."KargoFirmalari_Id_seq"', 2, true);


--
-- Name: Kategoriler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."Kategoriler_Id_seq"', 26, true);


--
-- Name: Kuponlar_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."Kuponlar_Id_seq"', 1, false);


--
-- Name: KurumsalSayfalar_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."KurumsalSayfalar_Id_seq"', 1, false);


--
-- Name: PushAbonelikleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."PushAbonelikleri_Id_seq"', 1, false);


--
-- Name: SenTasarla_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."SenTasarla_Id_seq"', 1, false);


--
-- Name: SepetItems_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."SepetItems_Id_seq"', 11, true);


--
-- Name: Sepetler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."Sepetler_Id_seq"', 37, true);


--
-- Name: SiparisDetaylari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."SiparisDetaylari_Id_seq"', 1, false);


--
-- Name: Siparisler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."Siparisler_Id_seq"', 1, false);


--
-- Name: SiteAyarlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."SiteAyarlari_Id_seq"', 1, false);


--
-- Name: SiteDegerlendirmeleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."SiteDegerlendirmeleri_Id_seq"', 1, false);


--
-- Name: Slaytlar_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."Slaytlar_Id_seq"', 3, true);


--
-- Name: ToptanciIskontoOranlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."ToptanciIskontoOranlari_Id_seq"', 7, true);


--
-- Name: ToptanciUrunGruplari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."ToptanciUrunGruplari_Id_seq"', 4, true);


--
-- Name: UrunOzellikDegerleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."UrunOzellikDegerleri_Id_seq"', 1, false);


--
-- Name: UrunOzellikTanimlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."UrunOzellikTanimlari_Id_seq"', 27, true);


--
-- Name: UrunResimleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."UrunResimleri_Id_seq"', 12, true);


--
-- Name: UrunSecenekleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."UrunSecenekleri_Id_seq"', 21, true);


--
-- Name: Urunler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."Urunler_Id_seq"', 15, true);


--
-- Name: Yorumlar_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."Yorumlar_Id_seq"', 1, false);


--
-- Name: ZiyaretciLoglari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: kanvasuser
--

SELECT pg_catalog.setval('public."ZiyaretciLoglari_Id_seq"', 682, true);


--
-- Name: aggregatedcounter aggregatedcounter_key_key; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.aggregatedcounter
    ADD CONSTRAINT aggregatedcounter_key_key UNIQUE (key);


--
-- Name: aggregatedcounter aggregatedcounter_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.aggregatedcounter
    ADD CONSTRAINT aggregatedcounter_pkey PRIMARY KEY (id);


--
-- Name: counter counter_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.counter
    ADD CONSTRAINT counter_pkey PRIMARY KEY (id);


--
-- Name: hash hash_key_field_key; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.hash
    ADD CONSTRAINT hash_key_field_key UNIQUE (key, field);


--
-- Name: hash hash_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.hash
    ADD CONSTRAINT hash_pkey PRIMARY KEY (id);


--
-- Name: job job_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.job
    ADD CONSTRAINT job_pkey PRIMARY KEY (id);


--
-- Name: jobparameter jobparameter_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.jobparameter
    ADD CONSTRAINT jobparameter_pkey PRIMARY KEY (id);


--
-- Name: jobqueue jobqueue_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.jobqueue
    ADD CONSTRAINT jobqueue_pkey PRIMARY KEY (id);


--
-- Name: list list_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.list
    ADD CONSTRAINT list_pkey PRIMARY KEY (id);


--
-- Name: lock lock_resource_key; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.lock
    ADD CONSTRAINT lock_resource_key UNIQUE (resource);

ALTER TABLE ONLY hangfire.lock REPLICA IDENTITY USING INDEX lock_resource_key;


--
-- Name: schema schema_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.schema
    ADD CONSTRAINT schema_pkey PRIMARY KEY (version);


--
-- Name: server server_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.server
    ADD CONSTRAINT server_pkey PRIMARY KEY (id);


--
-- Name: set set_key_value_key; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.set
    ADD CONSTRAINT set_key_value_key UNIQUE (key, value);


--
-- Name: set set_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.set
    ADD CONSTRAINT set_pkey PRIMARY KEY (id);


--
-- Name: state state_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.state
    ADD CONSTRAINT state_pkey PRIMARY KEY (id);


--
-- Name: Adresler PK_Adresler; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Adresler"
    ADD CONSTRAINT "PK_Adresler" PRIMARY KEY ("Id");


--
-- Name: AspNetRoleClaims PK_AspNetRoleClaims; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetRoleClaims"
    ADD CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id");


--
-- Name: AspNetRoles PK_AspNetRoles; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetRoles"
    ADD CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id");


--
-- Name: AspNetUserClaims PK_AspNetUserClaims; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUserClaims"
    ADD CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id");


--
-- Name: AspNetUserLogins PK_AspNetUserLogins; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUserLogins"
    ADD CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey");


--
-- Name: AspNetUserRoles PK_AspNetUserRoles; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId");


--
-- Name: AspNetUserTokens PK_AspNetUserTokens; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUserTokens"
    ADD CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name");


--
-- Name: AspNetUsers PK_AspNetUsers; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUsers"
    ADD CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id");


--
-- Name: BankaHesaplari PK_BankaHesaplari; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."BankaHesaplari"
    ADD CONSTRAINT "PK_BankaHesaplari" PRIMARY KEY ("Id");


--
-- Name: BultenAbonelikleri PK_BultenAbonelikleri; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."BultenAbonelikleri"
    ADD CONSTRAINT "PK_BultenAbonelikleri" PRIMARY KEY ("Id");


--
-- Name: CarkOdulleri PK_CarkOdulleri; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."CarkOdulleri"
    ADD CONSTRAINT "PK_CarkOdulleri" PRIMARY KEY ("Id");


--
-- Name: Favoriler PK_Favoriler; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Favoriler"
    ADD CONSTRAINT "PK_Favoriler" PRIMARY KEY ("Id");


--
-- Name: HomePageSectionProducts PK_HomePageSectionProducts; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."HomePageSectionProducts"
    ADD CONSTRAINT "PK_HomePageSectionProducts" PRIMARY KEY ("Id");


--
-- Name: HomePageSections PK_HomePageSections; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."HomePageSections"
    ADD CONSTRAINT "PK_HomePageSections" PRIMARY KEY ("Id");


--
-- Name: IadeTalepleri PK_IadeTalepleri; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."IadeTalepleri"
    ADD CONSTRAINT "PK_IadeTalepleri" PRIMARY KEY ("Id");


--
-- Name: IletisimMesajlari PK_IletisimMesajlari; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."IletisimMesajlari"
    ADD CONSTRAINT "PK_IletisimMesajlari" PRIMARY KEY ("Id");


--
-- Name: KargoBolgeFiyatlari PK_KargoBolgeFiyatlari; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."KargoBolgeFiyatlari"
    ADD CONSTRAINT "PK_KargoBolgeFiyatlari" PRIMARY KEY ("Id");


--
-- Name: KargoBolgeSehirler PK_KargoBolgeSehirler; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."KargoBolgeSehirler"
    ADD CONSTRAINT "PK_KargoBolgeSehirler" PRIMARY KEY ("Id");


--
-- Name: KargoBolgeler PK_KargoBolgeler; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."KargoBolgeler"
    ADD CONSTRAINT "PK_KargoBolgeler" PRIMARY KEY ("Id");


--
-- Name: KargoFirmalari PK_KargoFirmalari; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."KargoFirmalari"
    ADD CONSTRAINT "PK_KargoFirmalari" PRIMARY KEY ("Id");


--
-- Name: Kategoriler PK_Kategoriler; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Kategoriler"
    ADD CONSTRAINT "PK_Kategoriler" PRIMARY KEY ("Id");


--
-- Name: Kuponlar PK_Kuponlar; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Kuponlar"
    ADD CONSTRAINT "PK_Kuponlar" PRIMARY KEY ("Id");


--
-- Name: KurumsalSayfalar PK_KurumsalSayfalar; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."KurumsalSayfalar"
    ADD CONSTRAINT "PK_KurumsalSayfalar" PRIMARY KEY ("Id");


--
-- Name: PushAbonelikleri PK_PushAbonelikleri; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."PushAbonelikleri"
    ADD CONSTRAINT "PK_PushAbonelikleri" PRIMARY KEY ("Id");


--
-- Name: SenTasarla PK_SenTasarla; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SenTasarla"
    ADD CONSTRAINT "PK_SenTasarla" PRIMARY KEY ("Id");


--
-- Name: SepetItems PK_SepetItems; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SepetItems"
    ADD CONSTRAINT "PK_SepetItems" PRIMARY KEY ("Id");


--
-- Name: Sepetler PK_Sepetler; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Sepetler"
    ADD CONSTRAINT "PK_Sepetler" PRIMARY KEY ("Id");


--
-- Name: SiparisDetaylari PK_SiparisDetaylari; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SiparisDetaylari"
    ADD CONSTRAINT "PK_SiparisDetaylari" PRIMARY KEY ("Id");


--
-- Name: Siparisler PK_Siparisler; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Siparisler"
    ADD CONSTRAINT "PK_Siparisler" PRIMARY KEY ("Id");


--
-- Name: SiteAyarlari PK_SiteAyarlari; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SiteAyarlari"
    ADD CONSTRAINT "PK_SiteAyarlari" PRIMARY KEY ("Id");


--
-- Name: SiteDegerlendirmeleri PK_SiteDegerlendirmeleri; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SiteDegerlendirmeleri"
    ADD CONSTRAINT "PK_SiteDegerlendirmeleri" PRIMARY KEY ("Id");


--
-- Name: Slaytlar PK_Slaytlar; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Slaytlar"
    ADD CONSTRAINT "PK_Slaytlar" PRIMARY KEY ("Id");


--
-- Name: UrunOzellikDegerleri PK_UrunOzellikDegerleri; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."UrunOzellikDegerleri"
    ADD CONSTRAINT "PK_UrunOzellikDegerleri" PRIMARY KEY ("Id");


--
-- Name: UrunOzellikTanimlari PK_UrunOzellikTanimlari; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."UrunOzellikTanimlari"
    ADD CONSTRAINT "PK_UrunOzellikTanimlari" PRIMARY KEY ("Id");


--
-- Name: UrunResimleri PK_UrunResimleri; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."UrunResimleri"
    ADD CONSTRAINT "PK_UrunResimleri" PRIMARY KEY ("Id");


--
-- Name: UrunSecenekleri PK_UrunSecenekleri; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."UrunSecenekleri"
    ADD CONSTRAINT "PK_UrunSecenekleri" PRIMARY KEY ("Id");


--
-- Name: Urunler PK_Urunler; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Urunler"
    ADD CONSTRAINT "PK_Urunler" PRIMARY KEY ("Id");


--
-- Name: Yorumlar PK_Yorumlar; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Yorumlar"
    ADD CONSTRAINT "PK_Yorumlar" PRIMARY KEY ("Id");


--
-- Name: ZiyaretciLoglari PK_ZiyaretciLoglari; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."ZiyaretciLoglari"
    ADD CONSTRAINT "PK_ZiyaretciLoglari" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: ToptanciIskontoOranlari ToptanciIskontoOranlari_pkey; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."ToptanciIskontoOranlari"
    ADD CONSTRAINT "ToptanciIskontoOranlari_pkey" PRIMARY KEY ("Id");


--
-- Name: ToptanciUrunGruplari ToptanciUrunGruplari_pkey; Type: CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."ToptanciUrunGruplari"
    ADD CONSTRAINT "ToptanciUrunGruplari_pkey" PRIMARY KEY ("Id");


--
-- Name: ix_hangfire_counter_expireat; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_counter_expireat ON hangfire.counter USING btree (expireat);


--
-- Name: ix_hangfire_counter_key; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_counter_key ON hangfire.counter USING btree (key);


--
-- Name: ix_hangfire_hash_expireat; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_hash_expireat ON hangfire.hash USING btree (expireat);


--
-- Name: ix_hangfire_job_expireat; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_job_expireat ON hangfire.job USING btree (expireat);


--
-- Name: ix_hangfire_job_statename; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_job_statename ON hangfire.job USING btree (statename);


--
-- Name: ix_hangfire_jobparameter_jobidandname; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_jobparameter_jobidandname ON hangfire.jobparameter USING btree (jobid, name);


--
-- Name: ix_hangfire_jobqueue_fetchedat_queue_jobid; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_jobqueue_fetchedat_queue_jobid ON hangfire.jobqueue USING btree (fetchedat NULLS FIRST, queue, jobid);


--
-- Name: ix_hangfire_jobqueue_jobidandqueue; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_jobqueue_jobidandqueue ON hangfire.jobqueue USING btree (jobid, queue);


--
-- Name: ix_hangfire_jobqueue_queueandfetchedat; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_jobqueue_queueandfetchedat ON hangfire.jobqueue USING btree (queue, fetchedat);


--
-- Name: ix_hangfire_list_expireat; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_list_expireat ON hangfire.list USING btree (expireat);


--
-- Name: ix_hangfire_set_expireat; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_set_expireat ON hangfire.set USING btree (expireat);


--
-- Name: ix_hangfire_set_key_score; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_set_key_score ON hangfire.set USING btree (key, score);


--
-- Name: ix_hangfire_state_jobid; Type: INDEX; Schema: hangfire; Owner: kanvasuser
--

CREATE INDEX ix_hangfire_state_jobid ON hangfire.state USING btree (jobid);


--
-- Name: EmailIndex; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "EmailIndex" ON public."AspNetUsers" USING btree ("NormalizedEmail");


--
-- Name: IX_Adresler_AppUserId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Adresler_AppUserId" ON public."Adresler" USING btree ("AppUserId");


--
-- Name: IX_AspNetRoleClaims_RoleId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON public."AspNetRoleClaims" USING btree ("RoleId");


--
-- Name: IX_AspNetUserClaims_UserId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_AspNetUserClaims_UserId" ON public."AspNetUserClaims" USING btree ("UserId");


--
-- Name: IX_AspNetUserLogins_UserId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_AspNetUserLogins_UserId" ON public."AspNetUserLogins" USING btree ("UserId");


--
-- Name: IX_AspNetUserRoles_RoleId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON public."AspNetUserRoles" USING btree ("RoleId");


--
-- Name: IX_Favoriler_AppUserId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Favoriler_AppUserId" ON public."Favoriler" USING btree ("AppUserId");


--
-- Name: IX_Favoriler_UrunId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Favoriler_UrunId" ON public."Favoriler" USING btree ("UrunId");


--
-- Name: IX_HomePageSectionProducts_SectionId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_HomePageSectionProducts_SectionId" ON public."HomePageSectionProducts" USING btree ("SectionId");


--
-- Name: IX_HomePageSectionProducts_UrunId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_HomePageSectionProducts_UrunId" ON public."HomePageSectionProducts" USING btree ("UrunId");


--
-- Name: IX_IadeTalepleri_SiparisId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_IadeTalepleri_SiparisId" ON public."IadeTalepleri" USING btree ("SiparisId");


--
-- Name: IX_KargoBolgeFiyatlari_BolgeId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_KargoBolgeFiyatlari_BolgeId" ON public."KargoBolgeFiyatlari" USING btree ("BolgeId");


--
-- Name: IX_KargoBolgeFiyatlari_KargoFirmasiId_BolgeId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE UNIQUE INDEX "IX_KargoBolgeFiyatlari_KargoFirmasiId_BolgeId" ON public."KargoBolgeFiyatlari" USING btree ("KargoFirmasiId", "BolgeId");


--
-- Name: IX_KargoBolgeSehirler_BolgeId_SehirAdi; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE UNIQUE INDEX "IX_KargoBolgeSehirler_BolgeId_SehirAdi" ON public."KargoBolgeSehirler" USING btree ("BolgeId", "SehirAdi");


--
-- Name: IX_KargoFirmalari_Kod; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE UNIQUE INDEX "IX_KargoFirmalari_Kod" ON public."KargoFirmalari" USING btree ("Kod");


--
-- Name: IX_Kategoriler_ParentKategoriId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Kategoriler_ParentKategoriId" ON public."Kategoriler" USING btree ("ParentKategoriId");


--
-- Name: IX_Kategoriler_Slug; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Kategoriler_Slug" ON public."Kategoriler" USING btree ("Slug") WHERE (("Slug" IS NOT NULL) AND ("Slug" <> ''::text));


--
-- Name: IX_PushAbonelikleri_AppUserId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_PushAbonelikleri_AppUserId" ON public."PushAbonelikleri" USING btree ("AppUserId");


--
-- Name: IX_PushAbonelikleri_Token; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_PushAbonelikleri_Token" ON public."PushAbonelikleri" USING btree ("Token");


--
-- Name: IX_SepetItems_SepetId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_SepetItems_SepetId" ON public."SepetItems" USING btree ("SepetId");


--
-- Name: IX_SepetItems_UrunId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_SepetItems_UrunId" ON public."SepetItems" USING btree ("UrunId");


--
-- Name: IX_SepetItems_UrunSecenekId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_SepetItems_UrunSecenekId" ON public."SepetItems" USING btree ("UrunSecenekId");


--
-- Name: IX_Sepetler_AppUserId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Sepetler_AppUserId" ON public."Sepetler" USING btree ("AppUserId");


--
-- Name: IX_SiparisDetaylari_SiparisId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_SiparisDetaylari_SiparisId" ON public."SiparisDetaylari" USING btree ("SiparisId");


--
-- Name: IX_SiparisDetaylari_UrunId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_SiparisDetaylari_UrunId" ON public."SiparisDetaylari" USING btree ("UrunId");


--
-- Name: IX_SiparisDetaylari_UrunSecenekId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_SiparisDetaylari_UrunSecenekId" ON public."SiparisDetaylari" USING btree ("UrunSecenekId");


--
-- Name: IX_Siparisler_AppUserId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Siparisler_AppUserId" ON public."Siparisler" USING btree ("AppUserId");


--
-- Name: IX_ToptanciIskontoOranlari_ToptanciUrunGrubuId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_ToptanciIskontoOranlari_ToptanciUrunGrubuId" ON public."ToptanciIskontoOranlari" USING btree ("ToptanciUrunGrubuId");


--
-- Name: IX_ToptanciUrunGruplari_AktifMi; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_ToptanciUrunGruplari_AktifMi" ON public."ToptanciUrunGruplari" USING btree ("AktifMi");


--
-- Name: IX_UrunOzellikDegerleri_UrunId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_UrunOzellikDegerleri_UrunId" ON public."UrunOzellikDegerleri" USING btree ("UrunId");


--
-- Name: IX_UrunOzellikDegerleri_UrunOzellikTanimiId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_UrunOzellikDegerleri_UrunOzellikTanimiId" ON public."UrunOzellikDegerleri" USING btree ("UrunOzellikTanimiId");


--
-- Name: IX_UrunOzellikTanimlari_UrunTipi_Kod; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE UNIQUE INDEX "IX_UrunOzellikTanimlari_UrunTipi_Kod" ON public."UrunOzellikTanimlari" USING btree ("UrunTipi", "Kod");


--
-- Name: IX_UrunResimleri_UrunId_Sira; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_UrunResimleri_UrunId_Sira" ON public."UrunResimleri" USING btree ("UrunId", "Sira");


--
-- Name: IX_UrunSecenekleri_UrunId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_UrunSecenekleri_UrunId" ON public."UrunSecenekleri" USING btree ("UrunId");


--
-- Name: IX_Urunler_KategoriId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Urunler_KategoriId" ON public."Urunler" USING btree ("KategoriId");


--
-- Name: IX_Urunler_SKU; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Urunler_SKU" ON public."Urunler" USING btree ("SKU");


--
-- Name: IX_Urunler_Slug; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Urunler_Slug" ON public."Urunler" USING btree ("Slug") WHERE (("Slug" IS NOT NULL) AND ("Slug" <> ''::text));


--
-- Name: IX_Urunler_ToptanciUrunGrubuId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Urunler_ToptanciUrunGrubuId" ON public."Urunler" USING btree ("ToptanciUrunGrubuId");


--
-- Name: IX_Yorumlar_UrunId; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE INDEX "IX_Yorumlar_UrunId" ON public."Yorumlar" USING btree ("UrunId");


--
-- Name: RoleNameIndex; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE UNIQUE INDEX "RoleNameIndex" ON public."AspNetRoles" USING btree ("NormalizedName");


--
-- Name: UserNameIndex; Type: INDEX; Schema: public; Owner: kanvasuser
--

CREATE UNIQUE INDEX "UserNameIndex" ON public."AspNetUsers" USING btree ("NormalizedUserName");


--
-- Name: jobparameter jobparameter_jobid_fkey; Type: FK CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.jobparameter
    ADD CONSTRAINT jobparameter_jobid_fkey FOREIGN KEY (jobid) REFERENCES hangfire.job(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: state state_jobid_fkey; Type: FK CONSTRAINT; Schema: hangfire; Owner: kanvasuser
--

ALTER TABLE ONLY hangfire.state
    ADD CONSTRAINT state_jobid_fkey FOREIGN KEY (jobid) REFERENCES hangfire.job(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: Adresler FK_Adresler_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Adresler"
    ADD CONSTRAINT "FK_Adresler_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetRoleClaims FK_AspNetRoleClaims_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetRoleClaims"
    ADD CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserClaims FK_AspNetUserClaims_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUserClaims"
    ADD CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserLogins FK_AspNetUserLogins_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUserLogins"
    ADD CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserTokens FK_AspNetUserTokens_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."AspNetUserTokens"
    ADD CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: Favoriler FK_Favoriler_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Favoriler"
    ADD CONSTRAINT "FK_Favoriler_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: Favoriler FK_Favoriler_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Favoriler"
    ADD CONSTRAINT "FK_Favoriler_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: HomePageSectionProducts FK_HomePageSectionProducts_HomePageSections_SectionId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."HomePageSectionProducts"
    ADD CONSTRAINT "FK_HomePageSectionProducts_HomePageSections_SectionId" FOREIGN KEY ("SectionId") REFERENCES public."HomePageSections"("Id") ON DELETE CASCADE;


--
-- Name: HomePageSectionProducts FK_HomePageSectionProducts_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."HomePageSectionProducts"
    ADD CONSTRAINT "FK_HomePageSectionProducts_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE RESTRICT;


--
-- Name: IadeTalepleri FK_IadeTalepleri_Siparisler_SiparisId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."IadeTalepleri"
    ADD CONSTRAINT "FK_IadeTalepleri_Siparisler_SiparisId" FOREIGN KEY ("SiparisId") REFERENCES public."Siparisler"("Id") ON DELETE CASCADE;


--
-- Name: KargoBolgeFiyatlari FK_KargoBolgeFiyatlari_KargoBolgeler_BolgeId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."KargoBolgeFiyatlari"
    ADD CONSTRAINT "FK_KargoBolgeFiyatlari_KargoBolgeler_BolgeId" FOREIGN KEY ("BolgeId") REFERENCES public."KargoBolgeler"("Id") ON DELETE CASCADE;


--
-- Name: KargoBolgeFiyatlari FK_KargoBolgeFiyatlari_KargoFirmalari_KargoFirmasiId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."KargoBolgeFiyatlari"
    ADD CONSTRAINT "FK_KargoBolgeFiyatlari_KargoFirmalari_KargoFirmasiId" FOREIGN KEY ("KargoFirmasiId") REFERENCES public."KargoFirmalari"("Id") ON DELETE CASCADE;


--
-- Name: KargoBolgeSehirler FK_KargoBolgeSehirler_KargoBolgeler_BolgeId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."KargoBolgeSehirler"
    ADD CONSTRAINT "FK_KargoBolgeSehirler_KargoBolgeler_BolgeId" FOREIGN KEY ("BolgeId") REFERENCES public."KargoBolgeler"("Id") ON DELETE CASCADE;


--
-- Name: Kategoriler FK_Kategoriler_Kategoriler_ParentKategoriId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Kategoriler"
    ADD CONSTRAINT "FK_Kategoriler_Kategoriler_ParentKategoriId" FOREIGN KEY ("ParentKategoriId") REFERENCES public."Kategoriler"("Id") ON DELETE RESTRICT;


--
-- Name: PushAbonelikleri FK_PushAbonelikleri_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."PushAbonelikleri"
    ADD CONSTRAINT "FK_PushAbonelikleri_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: SepetItems FK_SepetItems_Sepetler_SepetId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SepetItems"
    ADD CONSTRAINT "FK_SepetItems_Sepetler_SepetId" FOREIGN KEY ("SepetId") REFERENCES public."Sepetler"("Id") ON DELETE CASCADE;


--
-- Name: SepetItems FK_SepetItems_UrunSecenekleri_UrunSecenekId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SepetItems"
    ADD CONSTRAINT "FK_SepetItems_UrunSecenekleri_UrunSecenekId" FOREIGN KEY ("UrunSecenekId") REFERENCES public."UrunSecenekleri"("Id");


--
-- Name: SepetItems FK_SepetItems_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SepetItems"
    ADD CONSTRAINT "FK_SepetItems_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: Sepetler FK_Sepetler_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Sepetler"
    ADD CONSTRAINT "FK_Sepetler_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id");


--
-- Name: SiparisDetaylari FK_SiparisDetaylari_Siparisler_SiparisId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SiparisDetaylari"
    ADD CONSTRAINT "FK_SiparisDetaylari_Siparisler_SiparisId" FOREIGN KEY ("SiparisId") REFERENCES public."Siparisler"("Id") ON DELETE CASCADE;


--
-- Name: SiparisDetaylari FK_SiparisDetaylari_UrunSecenekleri_UrunSecenekId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SiparisDetaylari"
    ADD CONSTRAINT "FK_SiparisDetaylari_UrunSecenekleri_UrunSecenekId" FOREIGN KEY ("UrunSecenekId") REFERENCES public."UrunSecenekleri"("Id");


--
-- Name: SiparisDetaylari FK_SiparisDetaylari_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."SiparisDetaylari"
    ADD CONSTRAINT "FK_SiparisDetaylari_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: Siparisler FK_Siparisler_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Siparisler"
    ADD CONSTRAINT "FK_Siparisler_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id");


--
-- Name: ToptanciIskontoOranlari FK_ToptanciIskontoOranlari_ToptanciUrunGruplari; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."ToptanciIskontoOranlari"
    ADD CONSTRAINT "FK_ToptanciIskontoOranlari_ToptanciUrunGruplari" FOREIGN KEY ("ToptanciUrunGrubuId") REFERENCES public."ToptanciUrunGruplari"("Id") ON DELETE CASCADE;


--
-- Name: UrunOzellikDegerleri FK_UrunOzellikDegerleri_UrunOzellikTanimlari_UrunOzellikTanimi~; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."UrunOzellikDegerleri"
    ADD CONSTRAINT "FK_UrunOzellikDegerleri_UrunOzellikTanimlari_UrunOzellikTanimi~" FOREIGN KEY ("UrunOzellikTanimiId") REFERENCES public."UrunOzellikTanimlari"("Id") ON DELETE CASCADE;


--
-- Name: UrunOzellikDegerleri FK_UrunOzellikDegerleri_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."UrunOzellikDegerleri"
    ADD CONSTRAINT "FK_UrunOzellikDegerleri_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: UrunResimleri FK_UrunResimleri_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."UrunResimleri"
    ADD CONSTRAINT "FK_UrunResimleri_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: UrunSecenekleri FK_UrunSecenekleri_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."UrunSecenekleri"
    ADD CONSTRAINT "FK_UrunSecenekleri_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: Urunler FK_Urunler_Kategoriler_KategoriId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Urunler"
    ADD CONSTRAINT "FK_Urunler_Kategoriler_KategoriId" FOREIGN KEY ("KategoriId") REFERENCES public."Kategoriler"("Id") ON DELETE CASCADE;


--
-- Name: Urunler FK_Urunler_ToptanciUrunGruplari; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Urunler"
    ADD CONSTRAINT "FK_Urunler_ToptanciUrunGruplari" FOREIGN KEY ("ToptanciUrunGrubuId") REFERENCES public."ToptanciUrunGruplari"("Id") ON DELETE SET NULL;


--
-- Name: Yorumlar FK_Yorumlar_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: kanvasuser
--

ALTER TABLE ONLY public."Yorumlar"
    ADD CONSTRAINT "FK_Yorumlar_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: kanvasuser
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;


--
-- PostgreSQL database dump complete
--

\unrestrict sXkIw7g5IVBbvRh4RkBI1rXifAgjM68lnBeMm1M6ZApwTge3xkzgFjKQ0pPEg9B

