--
-- PostgreSQL database dump
--

\restrict 6zJlJNe9nKLSfogMJN3r6oEqel7EDYoWaUuXUv5qUb5J6Amj8NtsqzgUY9aTf2a

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
-- Name: hangfire; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA hangfire;


--
-- Name: public; Type: SCHEMA; Schema: -; Owner: -
--

-- *not* creating schema, since initdb creates it


--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA public IS '';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: aggregatedcounter; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.aggregatedcounter (
    id bigint NOT NULL,
    key text NOT NULL,
    value bigint NOT NULL,
    expireat timestamp with time zone
);


--
-- Name: aggregatedcounter_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: -
--

CREATE SEQUENCE hangfire.aggregatedcounter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: aggregatedcounter_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: -
--

ALTER SEQUENCE hangfire.aggregatedcounter_id_seq OWNED BY hangfire.aggregatedcounter.id;


--
-- Name: counter; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.counter (
    id bigint NOT NULL,
    key text NOT NULL,
    value bigint NOT NULL,
    expireat timestamp with time zone
);


--
-- Name: counter_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: -
--

CREATE SEQUENCE hangfire.counter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: counter_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: -
--

ALTER SEQUENCE hangfire.counter_id_seq OWNED BY hangfire.counter.id;


--
-- Name: hash; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.hash (
    id bigint NOT NULL,
    key text NOT NULL,
    field text NOT NULL,
    value text,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


--
-- Name: hash_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: -
--

CREATE SEQUENCE hangfire.hash_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: hash_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: -
--

ALTER SEQUENCE hangfire.hash_id_seq OWNED BY hangfire.hash.id;


--
-- Name: job; Type: TABLE; Schema: hangfire; Owner: -
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


--
-- Name: job_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: -
--

CREATE SEQUENCE hangfire.job_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: job_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: -
--

ALTER SEQUENCE hangfire.job_id_seq OWNED BY hangfire.job.id;


--
-- Name: jobparameter; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.jobparameter (
    id bigint NOT NULL,
    jobid bigint NOT NULL,
    name text NOT NULL,
    value text,
    updatecount integer DEFAULT 0 NOT NULL
);


--
-- Name: jobparameter_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: -
--

CREATE SEQUENCE hangfire.jobparameter_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: jobparameter_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: -
--

ALTER SEQUENCE hangfire.jobparameter_id_seq OWNED BY hangfire.jobparameter.id;


--
-- Name: jobqueue; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.jobqueue (
    id bigint NOT NULL,
    jobid bigint NOT NULL,
    queue text NOT NULL,
    fetchedat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


--
-- Name: jobqueue_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: -
--

CREATE SEQUENCE hangfire.jobqueue_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: jobqueue_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: -
--

ALTER SEQUENCE hangfire.jobqueue_id_seq OWNED BY hangfire.jobqueue.id;


--
-- Name: list; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.list (
    id bigint NOT NULL,
    key text NOT NULL,
    value text,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


--
-- Name: list_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: -
--

CREATE SEQUENCE hangfire.list_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: list_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: -
--

ALTER SEQUENCE hangfire.list_id_seq OWNED BY hangfire.list.id;


--
-- Name: lock; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.lock (
    resource text NOT NULL,
    updatecount integer DEFAULT 0 NOT NULL,
    acquired timestamp with time zone
);


--
-- Name: schema; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.schema (
    version integer NOT NULL
);


--
-- Name: server; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.server (
    id text NOT NULL,
    data jsonb,
    lastheartbeat timestamp with time zone NOT NULL,
    updatecount integer DEFAULT 0 NOT NULL
);


--
-- Name: set; Type: TABLE; Schema: hangfire; Owner: -
--

CREATE TABLE hangfire.set (
    id bigint NOT NULL,
    key text NOT NULL,
    score double precision NOT NULL,
    value text NOT NULL,
    expireat timestamp with time zone,
    updatecount integer DEFAULT 0 NOT NULL
);


--
-- Name: set_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: -
--

CREATE SEQUENCE hangfire.set_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: set_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: -
--

ALTER SEQUENCE hangfire.set_id_seq OWNED BY hangfire.set.id;


--
-- Name: state; Type: TABLE; Schema: hangfire; Owner: -
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


--
-- Name: state_id_seq; Type: SEQUENCE; Schema: hangfire; Owner: -
--

CREATE SEQUENCE hangfire.state_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: state_id_seq; Type: SEQUENCE OWNED BY; Schema: hangfire; Owner: -
--

ALTER SEQUENCE hangfire.state_id_seq OWNED BY hangfire.state.id;


--
-- Name: Adresler; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: Adresler_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: AspNetRoleClaims; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AspNetRoleClaims" (
    "Id" integer NOT NULL,
    "RoleId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


--
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: AspNetRoles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text
);


--
-- Name: AspNetUserClaims; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AspNetUserClaims" (
    "Id" integer NOT NULL,
    "UserId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);


--
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: AspNetUserLogins; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" text NOT NULL
);


--
-- Name: AspNetUserRoles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL
);


--
-- Name: AspNetUserTokens; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text
);


--
-- Name: AspNetUsers; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: BankaHesaplari; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: BankaHesaplari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: BultenAbonelikleri; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."BultenAbonelikleri" (
    "Id" integer NOT NULL,
    "Email" text NOT NULL,
    "KayitTarihi" timestamp with time zone NOT NULL,
    "AktifMi" boolean NOT NULL,
    "IpAdresi" text
);


--
-- Name: BultenAbonelikleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: CarkOdulleri; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: CarkOdulleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: Favoriler; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: Favoriler_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: HomePageSectionProducts; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."HomePageSectionProducts" (
    "Id" integer NOT NULL,
    "SectionId" integer NOT NULL,
    "UrunId" integer NOT NULL,
    "SortOrder" integer NOT NULL
);


--
-- Name: HomePageSectionProducts_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: HomePageSections; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: HomePageSections_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: IadeTalepleri; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: IadeTalepleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: IletisimMesajlari; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: IletisimMesajlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: KargoBolgeFiyatlari; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."KargoBolgeFiyatlari" (
    "Id" integer NOT NULL,
    "KargoFirmasiId" integer NOT NULL,
    "BolgeId" integer NOT NULL,
    "Fiyat" numeric NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


--
-- Name: KargoBolgeFiyatlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: KargoBolgeSehirler; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."KargoBolgeSehirler" (
    "Id" integer NOT NULL,
    "BolgeId" integer NOT NULL,
    "SehirAdi" text NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


--
-- Name: KargoBolgeSehirler_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: KargoBolgeler; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: KargoBolgeler_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: KargoFirmalari; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: KargoFirmalari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: Kategoriler; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: Kategoriler_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: Kuponlar; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: Kuponlar_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: KurumsalSayfalar; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: KurumsalSayfalar_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: PushAbonelikleri; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: PushAbonelikleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: SenTasarla; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: SenTasarla_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: SepetItems; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: SepetItems_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: Sepetler; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: Sepetler_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: SiparisDetaylari; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: SiparisDetaylari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: Siparisler; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: Siparisler_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: SiteAyarlari; Type: TABLE; Schema: public; Owner: -
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
    "IptalSuresiSaat" integer DEFAULT 3 NOT NULL,
    "FooterAciklamasiEn" text DEFAULT 'A Palestinian e-commerce site offering varied products at competitive prices with fast delivery to all cities.'::text NOT NULL,
    "FooterAciklamasiAr" text DEFAULT 'متجر إلكتروني فلسطيني يقدم منتجات متنوعة بأسعار منافسة وتوصيل سريع لجميع المدن'::text NOT NULL,
    "FooterAciklamasiTr" text DEFAULT 'Rekabetçi fiyatlarla çeşitli ürünler sunan ve tüm şehirlere hızlı teslimat yapan bir Filistin e-ticaret sitesi.'::text NOT NULL
);


--
-- Name: SiteAyarlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: SiteDegerlendirmeleri; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: SiteDegerlendirmeleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: Slaytlar; Type: TABLE; Schema: public; Owner: -
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
    "AciklamaAr" text DEFAULT ''::text NOT NULL,
    "ButonMetni" text DEFAULT ''::text NOT NULL,
    "ButonMetniEn" text DEFAULT ''::text NOT NULL,
    "ButonMetniAr" text DEFAULT ''::text NOT NULL,
    "BaglantiUrl" text
);


--
-- Name: Slaytlar_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: ToptanciIskontoOranlari; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: ToptanciIskontoOranlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: ToptanciUrunGruplari; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: ToptanciUrunGruplari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: UrunOzellikDegerleri; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."UrunOzellikDegerleri" (
    "Id" integer NOT NULL,
    "UrunId" integer NOT NULL,
    "UrunOzellikTanimiId" integer NOT NULL,
    "Deger" text NOT NULL,
    "OlusturulmaTarihi" timestamp with time zone NOT NULL,
    "SilindiMi" boolean NOT NULL
);


--
-- Name: UrunOzellikDegerleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: UrunOzellikTanimlari; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: UrunOzellikTanimlari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: UrunResimleri; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: UrunResimleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: UrunSecenekleri; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: UrunSecenekleri_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: Urunler; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: Urunler_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: Yorumlar; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: Yorumlar_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: ZiyaretciLoglari; Type: TABLE; Schema: public; Owner: -
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


--
-- Name: ZiyaretciLoglari_Id_seq; Type: SEQUENCE; Schema: public; Owner: -
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
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


--
-- Name: aggregatedcounter id; Type: DEFAULT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.aggregatedcounter ALTER COLUMN id SET DEFAULT nextval('hangfire.aggregatedcounter_id_seq'::regclass);


--
-- Name: counter id; Type: DEFAULT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.counter ALTER COLUMN id SET DEFAULT nextval('hangfire.counter_id_seq'::regclass);


--
-- Name: hash id; Type: DEFAULT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.hash ALTER COLUMN id SET DEFAULT nextval('hangfire.hash_id_seq'::regclass);


--
-- Name: job id; Type: DEFAULT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.job ALTER COLUMN id SET DEFAULT nextval('hangfire.job_id_seq'::regclass);


--
-- Name: jobparameter id; Type: DEFAULT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.jobparameter ALTER COLUMN id SET DEFAULT nextval('hangfire.jobparameter_id_seq'::regclass);


--
-- Name: jobqueue id; Type: DEFAULT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.jobqueue ALTER COLUMN id SET DEFAULT nextval('hangfire.jobqueue_id_seq'::regclass);


--
-- Name: list id; Type: DEFAULT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.list ALTER COLUMN id SET DEFAULT nextval('hangfire.list_id_seq'::regclass);


--
-- Name: set id; Type: DEFAULT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.set ALTER COLUMN id SET DEFAULT nextval('hangfire.set_id_seq'::regclass);


--
-- Name: state id; Type: DEFAULT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.state ALTER COLUMN id SET DEFAULT nextval('hangfire.state_id_seq'::regclass);


--
-- Data for Name: aggregatedcounter; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.aggregatedcounter (id, key, value, expireat) FROM stdin;
\.


--
-- Data for Name: counter; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.counter (id, key, value, expireat) FROM stdin;
\.


--
-- Data for Name: hash; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.hash (id, key, field, value, expireat, updatecount) FROM stdin;
\.


--
-- Data for Name: job; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.job (id, stateid, statename, invocationdata, arguments, createdat, expireat, updatecount) FROM stdin;
\.


--
-- Data for Name: jobparameter; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.jobparameter (id, jobid, name, value, updatecount) FROM stdin;
\.


--
-- Data for Name: jobqueue; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.jobqueue (id, jobid, queue, fetchedat, updatecount) FROM stdin;
\.


--
-- Data for Name: list; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.list (id, key, value, expireat, updatecount) FROM stdin;
\.


--
-- Data for Name: lock; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.lock (resource, updatecount, acquired) FROM stdin;
\.


--
-- Data for Name: schema; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.schema (version) FROM stdin;
22
\.


--
-- Data for Name: server; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.server (id, data, lastheartbeat, updatecount) FROM stdin;
abdulmuin:29632:dd9e427c-9253-4132-8940-32f8fc0e5812	{"Queues": ["default"], "StartedAt": "2026-06-27T15:40:18.2010183Z", "WorkerCount": 20}	2026-06-27 15:45:18.770277+00	0
abdulmuin:12272:b0e21d61-b031-4c54-bd2f-df29a230712b	{"Queues": ["default"], "StartedAt": "2026-06-27T15:46:05.5818051Z", "WorkerCount": 20}	2026-06-27 15:47:35.736902+00	0
\.


--
-- Data for Name: set; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.set (id, key, score, value, expireat, updatecount) FROM stdin;
\.


--
-- Data for Name: state; Type: TABLE DATA; Schema: hangfire; Owner: -
--

COPY hangfire.state (id, jobid, name, reason, createdat, data, updatecount) FROM stdin;
\.


--
-- Data for Name: Adresler; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Adresler" ("Id", "Baslik", "AdSoyad", "Telefon", "Sehir", "Ilce", "AcikAdres", "AppUserId", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: AspNetRoleClaims; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AspNetRoleClaims" ("Id", "RoleId", "ClaimType", "ClaimValue") FROM stdin;
\.


--
-- Data for Name: AspNetRoles; Type: TABLE DATA; Schema: public; Owner: -
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
-- Data for Name: AspNetUserClaims; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AspNetUserClaims" ("Id", "UserId", "ClaimType", "ClaimValue") FROM stdin;
\.


--
-- Data for Name: AspNetUserLogins; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AspNetUserLogins" ("LoginProvider", "ProviderKey", "ProviderDisplayName", "UserId") FROM stdin;
\.


--
-- Data for Name: AspNetUserRoles; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AspNetUserRoles" ("UserId", "RoleId") FROM stdin;
57039d3f-f6b2-498e-9a3b-0e985154adad	fe42d608-671b-46c9-add1-ee9711fda800
fed886bd-cd51-4cb1-9a15-c5447b1c63f4	e5b7cdf7-fb08-48f8-a0b4-76b5db17d04c
\.


--
-- Data for Name: AspNetUserTokens; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AspNetUserTokens" ("UserId", "LoginProvider", "Name", "Value") FROM stdin;
\.


--
-- Data for Name: AspNetUsers; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."AspNetUsers" ("Id", "AdSoyad", "Sehir", "SifreSifirlamaToken", "SifreSifirlamaGecerlilik", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "DogumTarihi", "KimlikFotografYolu", "KimlikNo", "Adres", "WholesaleStatus", "BasvuruTarihi") FROM stdin;
57039d3f-f6b2-498e-9a3b-0e985154adad	7ANRPS48 Admin	Ramallah	\N	\N	admin@7anrps48.com	ADMIN@7ANRPS48.COM	admin@7anrps48.com	ADMIN@7ANRPS48.COM	t	AQAAAAIAAYagAAAAEF8daTB4ytBcgFB+xxdkYXvabwhIW1ycnDohe7CA3yK9uqGH5UrFbkDnMV7Bn2lSUA==	ZA3GTOI7WYGZ4PTIUN3SY6SCWWJQFCBG	d9da7a54-ffe2-4384-a977-6ef1efbc39f2	\N	f	f	\N	t	0	\N				0	\N
fed886bd-cd51-4cb1-9a15-c5447b1c63f4	Test Kullanıcı	Gaza	\N	\N	testuser2026@example.com	TESTUSER2026@EXAMPLE.COM	testuser2026@example.com	TESTUSER2026@EXAMPLE.COM	f	AQAAAAIAAYagAAAAEOGAZMSuwPG1w4GrdCTWgqN513JQJOPUhE4ZZXCTA1tSUNHsTTMsLTOflL3Hkab2aw==	YMUQQYAAX7OMGSOK5BHOG2BYMZS6J5NS	8ad46df0-1762-4555-aea3-9f57fc872f08	+970599123456	f	f	\N	t	0	1990-01-15 00:00:00+00	/uploads/kimlikler/f91e1730c21844be86099bce352d6829.png	12345678901	Test Adres, Gaza, Filistin	0	\N
\.


--
-- Data for Name: BankaHesaplari; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."BankaHesaplari" ("Id", "BankaAdi", "HesapSahibi", "IBAN", "SubeKodu", "HesapNo", "AktifMi", "Sira", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: BultenAbonelikleri; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."BultenAbonelikleri" ("Id", "Email", "KayitTarihi", "AktifMi", "IpAdresi") FROM stdin;
\.


--
-- Data for Name: CarkOdulleri; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."CarkOdulleri" ("Id", "LabelTr", "LabelEn", "LabelAr", "Tip", "Deger", "Renk", "MesajTr", "MesajEn", "MesajAr", "Sira", "AktifMi", "OlusturulmaTarihi", "GuncellenmeTarihi", "SilindiMi") FROM stdin;
1	%5 İndirim	5% Off	خصم 5%	discount	5	#313511	Harika! Sepetinde %5 indirim kazandın!	\N	رائع! لقد ربحت خصم 5%!	1	t	2026-06-23 20:59:42.878242+00	\N	f
2	%10 İndirim	10% Off	خصم 10%	discount	10	#b58735	Harika! Sepetinde %10 indirim kazandın!	\N	رائع! لقد ربحت خصم 10%!	2	t	2026-06-23 20:59:42.878242+00	\N	f
3	%15 İndirim	15% Off	خصم 15%	discount	15	#4a4a2e	Süper! Sepetinde %15 indirim kazandın!	\N	ممتاز! لقد ربحت خصم 15%!	3	t	2026-06-23 20:59:42.878242+00	\N	f
4	%20 İndirim	20% Off	خصم 20%	discount	20	#c49a3c	Müthiş! Sepetinde %20 indirim kazandın!	\N	رائع! لقد ربحت خصم 20%!	4	t	2026-06-23 20:59:42.878242+00	\N	f
5	%25 İndirim	25% Off	خصم 25%	discount	25	#313511	Harika! Sepetinde %25 indirim kazandın!	\N	مذهل! لقد ربحت خصم 25%!	5	t	2026-06-23 20:59:42.878242+00	\N	f
6	Ücretsiz Kargo	Free Shipping	شحن مجاني	freeship	0	#b58735	Tebrikler! Ücretsiz kargo kazandın!	\N	تهانينا! لقد ربحت شحنًا مجانيًا!	6	t	2026-06-23 20:59:42.878242+00	\N	f
7	Tekrar Dene	Try Again	حاول مرة أخرى	none	0	#6f6a5e	\N	\N	حظ أوفر في المرة القادمة!	7	t	2026-06-23 20:59:42.878242+00	\N	f
8	%10 İndirim	10% Off	خصم 10%	discount	10	#d4a84b	Harika! Sepetinde %10 indirim kazandın!	\N	رائع! لقد ربحت خصم 10%!	8	t	2026-06-23 20:59:42.878242+00	\N	f
\.


--
-- Data for Name: Favoriler; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Favoriler" ("Id", "AppUserId", "UrunId", "FiyatDustugundaBildir", "EskiFiyat", "SonBildirimTarihi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: HomePageSectionProducts; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."HomePageSectionProducts" ("Id", "SectionId", "UrunId", "SortOrder") FROM stdin;
\.


--
-- Data for Name: HomePageSections; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."HomePageSections" ("Id", "SectionType", "Enabled", "SortOrder", "Title", "Subtitle", "ViewAllText", "ViewAllUrl", "ImageUrl", "Description", "ButtonText", "ButtonUrl") FROM stdin;
1	4	t	10				/Urun	\N	\N	\N	\N
2	5	t	20				/Urun?sort=popularity	\N	\N	\N	\N
3	6	t	30				/Urun?sort=price_asc	\N	\N	\N	\N
\.


--
-- Data for Name: IadeTalepleri; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."IadeTalepleri" ("Id", "SiparisId", "MusteriId", "Neden", "Aciklama", "IBAN", "Durum", "AdminNotu", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: IletisimMesajlari; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."IletisimMesajlari" ("Id", "AdSoyad", "Email", "Konu", "Mesaj", "Tarih", "IpAdresi", "OkunduMu") FROM stdin;
\.


--
-- Data for Name: KargoBolgeFiyatlari; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."KargoBolgeFiyatlari" ("Id", "KargoFirmasiId", "BolgeId", "Fiyat", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: KargoBolgeSehirler; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."KargoBolgeSehirler" ("Id", "BolgeId", "SehirAdi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: KargoBolgeler; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."KargoBolgeler" ("Id", "Ad", "Sira", "OlusturulmaTarihi", "SilindiMi", "Ulke", "Aciklama", "Fiyat") FROM stdin;
\.


--
-- Data for Name: KargoFirmalari; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."KargoFirmalari" ("Id", "Ad", "Kod", "LogoUrl", "Telefon", "TakipUrl", "GondericiUnvan", "GondericiAdres", "GondericiTelefon", "AktifMi", "VarsayilanMi", "Fiyat", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: Kategoriler; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Kategoriler" ("Id", "Ad", "KisaAciklama", "Aciklama", "Slug", "GorselUrl", "BannerUrl", "SeoTitle", "SeoDescription", "UstMetin", "AltMetin", "KampanyaEtiketi", "UrunSiralamaTipi", "Sira", "AktifMi", "ParentKategoriId", "OlusturulmaTarihi", "SilindiMi", "AciklamaAr", "AciklamaEn", "AdAr", "AdEn", "AltMetinAr", "AltMetinEn", "KampanyaEtiketiAr", "KampanyaEtiketiEn", "KisaAciklamaAr", "KisaAciklamaEn", "SeoDescriptionAr", "SeoDescriptionEn", "SeoTitleAr", "SeoTitleEn", "UstMetinAr", "UstMetinEn", "ReceteGerekliMi") FROM stdin;
64	Kedi ve Köpek Bakımı	Evcil hayvanlar için vitamin, hijyen ve bakım ürünleri	Kedi ve köpeklerin günlük bakımını destekleyen veteriner ürünleri.	kedi-ve-kopek-bakimi	/img/products/vet-real-pet-vitamin.jpg	/slider2.png	Kedi ve Köpek Bakımı - 7ANRPS48	Evcil hayvan bakım ve destek ürünleri			Evcil Bakım	manual	1	t	\N	2026-06-26 19:12:59.964939+00	f	منتجات بيطرية تدعم العناية اليومية بالقطط والكلاب.	Veterinary products supporting daily cat and dog care.	رعاية القطط والكلاب	Cat & Dog Care			رعاية الحيوانات	Pet Care	فيتامينات ومنتجات نظافة وعناية للحيوانات الأليفة	Vitamins, hygiene and care products for pets	منتجات رعاية ودعم للحيوانات الأليفة	Pet care and support products	رعاية القطط والكلاب - 7ANRPS48	Cat & Dog Care - 7ANRPS48			f
65	Klinik Sarf ve Hijyen	Veteriner klinikleri için pratik sarf ürünleri	Muayene, bakım ve hijyen süreçlerinde kullanılan tamamlayıcı ürünler.	klinik-sarf-ve-hijyen	/img/products/vet-real-wound-spray.jpg	/slider4.png	Klinik Sarf ve Hijyen - 7ANRPS48	Veteriner klinik sarf ve hijyen ürünleri			Klinik	manual	2	t	\N	2026-06-26 19:12:59.964939+00	f	منتجات مساعدة للفحص والعناية والنظافة في العيادات.	Complementary products for examination, care and hygiene in clinics.	مستلزمات ونظافة العيادة	Clinic Consumables & Hygiene			عيادة	Clinic	مستلزمات عملية للعيادات البيطرية	Practical consumables for veterinary clinics	مستلزمات ونظافة للعيادات البيطرية	Veterinary clinic consumables and hygiene products	مستلزمات ونظافة العيادة - 7ANRPS48	Clinic Consumables & Hygiene - 7ANRPS48			f
66	Çiftlik Hayvanı Destek	Büyükbaş, küçükbaş ve kanatlılar için destek ürünleri	Çiftlik hayvanlarının bakımını destekleyen saha kullanımına uygun ürünler.	ciftlik-hayvani-destek	/img/products/vet-real-mineral-block.jpg	/slider3.png	Çiftlik Hayvanı Destek - 7ANRPS48	Çiftlik hayvanları için destek ürünleri			Çiftlik	manual	3	t	\N	2026-06-26 19:12:59.964939+00	f	منتجات مناسبة للاستخدام الميداني لدعم رعاية حيوانات المزرعة.	Field-use products supporting farm animal care.	دعم حيوانات المزرعة	Farm Animal Support			مزرعة	Farm	منتجات دعم للأبقار والأغنام والدواجن	Support products for cattle, sheep, goats and poultry	منتجات دعم لحيوانات المزرعة	Support products for farm animals	دعم حيوانات المزرعة - 7ANRPS48	Farm Animal Support - 7ANRPS48			f
72	Reçeteli Ürünler	Reçeteli Ürünler kategorisindeki ürünleri keşfedin.	Reçeteli Ürünler kategorisindeki ürünleri keşfedin.	receteli-urunler	\N	\N	Reçeteli Ürünler	Reçeteli Ürünler veteriner ürünleri.				manual	3	t	65	2026-06-26 19:50:28.942043+00	f	استكشف منتجات منتجات بوصفة طبية.	Explore Prescription Products products.	منتجات بوصفة طبية	Prescription Products					استكشف منتجات منتجات بوصفة طبية.	Explore Prescription Products products.	منتجات بوصفة طبية منتجات بيطرية.	Prescription Products veterinary products.	منتجات بوصفة طبية	Prescription Products			t
68	Şampuan ve Tüy Bakımı	Şampuan ve Tüy Bakımı kategorisindeki ürünleri keşfedin.	Şampuan ve Tüy Bakımı kategorisindeki ürünleri keşfedin.	sampuan-ve-tuy-bakimi	\N	\N	Şampuan ve Tüy Bakımı	Şampuan ve Tüy Bakımı veteriner ürünleri.				manual	2	t	64	2026-06-26 19:50:28.942043+00	f	استكشف منتجات الشامبو والعناية بالفرو.	Explore Shampoo & Coat Care products.	الشامبو والعناية بالفرو	Shampoo & Coat Care					استكشف منتجات الشامبو والعناية بالفرو.	Explore Shampoo & Coat Care products.	الشامبو والعناية بالفرو منتجات بيطرية.	Shampoo & Coat Care veterinary products.	الشامبو والعناية بالفرو	Shampoo & Coat Care			f
71	Dezenfektan ve Hijyen	Dezenfektan ve Hijyen kategorisindeki ürünleri keşfedin.	Dezenfektan ve Hijyen kategorisindeki ürünleri keşfedin.	dezenfektan-ve-hijyen	\N	\N	Dezenfektan ve Hijyen	Dezenfektan ve Hijyen veteriner ürünleri.				manual	2	t	65	2026-06-26 19:50:28.942043+00	f	استكشف منتجات المطهرات والنظافة.	Explore Disinfectant & Hygiene products.	المطهرات والنظافة	Disinfectant & Hygiene					استكشف منتجات المطهرات والنظافة.	Explore Disinfectant & Hygiene products.	المطهرات والنظافة منتجات بيطرية.	Disinfectant & Hygiene veterinary products.	المطهرات والنظافة	Disinfectant & Hygiene			f
70	Klinik Sarf Malzemeleri	Klinik Sarf Malzemeleri kategorisindeki ürünleri keşfedin.	Klinik Sarf Malzemeleri kategorisindeki ürünleri keşfedin.	klinik-sarf-malzemeleri	\N	\N	Klinik Sarf Malzemeleri	Klinik Sarf Malzemeleri veteriner ürünleri.				manual	1	t	65	2026-06-26 19:50:28.942043+00	f	استكشف منتجات مستهلكات العيادة.	Explore Clinic Consumables products.	مستهلكات العيادة	Clinic Consumables					استكشف منتجات مستهلكات العيادة.	Explore Clinic Consumables products.	مستهلكات العيادة منتجات بيطرية.	Clinic Consumables veterinary products.	مستهلكات العيادة	Clinic Consumables			f
67	Vitamin ve Takviyeler	Vitamin ve Takviyeler kategorisindeki ürünleri keşfedin.	Vitamin ve Takviyeler kategorisindeki ürünleri keşfedin.	vitamin-ve-takviyeler	\N	\N	Vitamin ve Takviyeler	Vitamin ve Takviyeler veteriner ürünleri.				manual	1	t	64	2026-06-26 19:50:28.942043+00	f	استكشف منتجات الفيتامينات والمكملات.	Explore Vitamins & Supplements products.	الفيتامينات والمكملات	Vitamins & Supplements					استكشف منتجات الفيتامينات والمكملات.	Explore Vitamins & Supplements products.	الفيتامينات والمكملات منتجات بيطرية.	Vitamins & Supplements veterinary products.	الفيتامينات والمكملات	Vitamins & Supplements			f
75	Mineral Bloklar	Mineral Bloklar kategorisindeki ürünleri keşfedin.	Mineral Bloklar kategorisindeki ürünleri keşfedin.	mineral-bloklar	\N	\N	Mineral Bloklar	Mineral Bloklar veteriner ürünleri.				manual	3	t	66	2026-06-26 19:50:28.942043+00	f	استكشف منتجات كتل المعادن.	Explore Mineral Blocks products.	كتل المعادن	Mineral Blocks					استكشف منتجات كتل المعادن.	Explore Mineral Blocks products.	كتل المعادن منتجات بيطرية.	Mineral Blocks veterinary products.	كتل المعادن	Mineral Blocks			f
73	Buzağı ve Kuzu Destek	Buzağı ve Kuzu Destek kategorisindeki ürünleri keşfedin.	Buzağı ve Kuzu Destek kategorisindeki ürünleri keşfedin.	buzagi-ve-kuzu-destek	\N	\N	Buzağı ve Kuzu Destek	Buzağı ve Kuzu Destek veteriner ürünleri.				manual	1	t	66	2026-06-26 19:50:28.942043+00	f	استكشف منتجات دعم العجول والحملان.	Explore Calf & Lamb Support products.	دعم العجول والحملان	Calf & Lamb Support					استكشف منتجات دعم العجول والحملان.	Explore Calf & Lamb Support products.	دعم العجول والحملان منتجات بيطرية.	Calf & Lamb Support veterinary products.	دعم العجول والحملان	Calf & Lamb Support			f
74	Kanatlı Premiksleri	Kanatlı Premiksleri kategorisindeki ürünleri keşfedin.	Kanatlı Premiksleri kategorisindeki ürünleri keşfedin.	kanatli-premiksleri	\N	\N	Kanatlı Premiksleri	Kanatlı Premiksleri veteriner ürünleri.				manual	2	t	66	2026-06-26 19:50:28.942043+00	f	استكشف منتجات خلطات الدواجن.	Explore Poultry Premixes products.	خلطات الدواجن	Poultry Premixes					استكشف منتجات خلطات الدواجن.	Explore Poultry Premixes products.	خلطات الدواجن منتجات بيطرية.	Poultry Premixes veterinary products.	خلطات الدواجن	Poultry Premixes			f
69	Kulak ve Göz Bakımı	Kulak ve Göz Bakımı kategorisindeki ürünleri keşfedin.	Kulak ve Göz Bakımı kategorisindeki ürünleri keşfedin.	kulak-ve-goz-bakimi	\N	\N	Kulak ve Göz Bakımı	Kulak ve Göz Bakımı veteriner ürünleri.				manual	3	t	64	2026-06-26 19:50:28.942043+00	f	استكشف منتجات العناية بالأذن والعين.	Explore Ear & Eye Care products.	العناية بالأذن والعين	Ear & Eye Care					استكشف منتجات العناية بالأذن والعين.	Explore Ear & Eye Care products.	العناية بالأذن والعين منتجات بيطرية.	Ear & Eye Care veterinary products.	العناية بالأذن والعين	Ear & Eye Care			f
\.


--
-- Data for Name: Kuponlar; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Kuponlar" ("Id", "Kod", "Tip", "Deger", "MinSepetTutari", "SonKullanmaTarihi", "KullanimLimiti", "KullanilanMiktar", "AktifMi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: KurumsalSayfalar; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."KurumsalSayfalar" ("Id", "Baslik", "Icerik", "UrlSlug", "Sira", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: PushAbonelikleri; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."PushAbonelikleri" ("Id", "Token", "AppUserId", "Tarayici", "Platform", "AktifMi", "SonGorulmeTarihi", "Tercihler", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: SenTasarla; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."SenTasarla" ("Id", "KullaniciId", "DosyaYolu", "Olcu", "Fiyat", "Efekt", "CercveliMi", "ParcaSayisi", "OlusturmaTarihi", "SepeteEklendi", "SessionId") FROM stdin;
\.


--
-- Data for Name: SepetItems; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."SepetItems" ("Id", "SepetId", "UrunId", "UrunSecenekId", "Adet", "Fiyat", "UrunBaslik", "UrunResimUrl", "CerceveModeli", "SecenekAdi", "MusteriNotu", "OlusturulmaTarihi", "SilindiMi", "HediyePaketFiyati", "HediyePaketi") FROM stdin;
\.


--
-- Data for Name: Sepetler; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Sepetler" ("Id", "AppUserId", "SessionId", "SonGuncellemeTarihi", "TerkEdildi", "TerkEdilmeTarihi", "HatirlatmaGonderildi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
47	\N	07791cbf-ccd9-d5c2-cc2d-5c5d8fe84e2f	2026-06-26 19:30:23.038233+00	f	\N	f	2026-06-26 19:30:23.038232+00	f
48	\N	4b8c279a-aa31-830f-bc95-2638b66daa97	2026-06-26 19:30:27.317568+00	f	\N	f	2026-06-26 19:30:27.317567+00	f
49	\N	0cb859a3-5cf4-a605-29b6-78360361354f	2026-06-26 19:30:27.342543+00	f	\N	f	2026-06-26 19:30:27.342543+00	f
50	\N	6ade3115-66c7-a5e2-5bd5-1ff48cc5dab3	2026-06-26 19:32:30.974008+00	f	\N	f	2026-06-26 19:32:30.974008+00	f
51	\N	332e72d1-6e3b-b917-e5df-ca01488a30ee	2026-06-26 19:32:35.15442+00	f	\N	f	2026-06-26 19:32:35.15442+00	f
52	\N	2b35ab5f-000a-af7b-7b1f-87c363ac9815	2026-06-26 19:32:35.182579+00	f	\N	f	2026-06-26 19:32:35.182579+00	f
53	\N	45fe4e9e-e685-8c01-7f92-df924e98f97e	2026-06-26 19:49:00.092507+00	f	\N	f	2026-06-26 19:49:00.092506+00	f
54	\N	8c869a81-edc4-7953-5bac-ecbe7b6011d5	2026-06-26 19:49:04.298194+00	f	\N	f	2026-06-26 19:49:04.298194+00	f
55	\N	67489935-fa82-8bf5-1838-3b210b37debd	2026-06-26 19:49:04.322187+00	f	\N	f	2026-06-26 19:49:04.322187+00	f
56	\N	f1b14a6d-ae14-96e8-f1be-2b17ec5c8dab	2026-06-26 19:51:08.602481+00	f	\N	f	2026-06-26 19:51:08.602481+00	f
57	\N	4ede2e5c-a615-7ddb-690d-935f8c658d3a	2026-06-26 19:51:11.352759+00	f	\N	f	2026-06-26 19:51:11.352759+00	f
58	\N	14062463-710e-a5ea-49ba-68d1756e454a	2026-06-26 19:51:11.363186+00	f	\N	f	2026-06-26 19:51:11.363186+00	f
59	57039d3f-f6b2-498e-9a3b-0e985154adad	\N	2026-06-26 21:48:58.331681+00	f	\N	f	2026-06-26 21:48:58.33168+00	f
\.


--
-- Data for Name: SiparisDetaylari; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."SiparisDetaylari" ("Id", "SiparisId", "UrunSecenekId", "Adet", "BirimFiyat", "UrunId", "CerceveModeli", "MusteriNotu", "OlusturulmaTarihi", "SilindiMi", "HediyePaketFiyati", "HediyePaketi") FROM stdin;
27	7	41	1	180	37	Standart	Klinik icin acil	2026-06-27 07:41:02.490991+00	f	0	f
28	7	42	1	280	38	Standart	Cerrahi set temizligi kontrol edilsin	2026-06-27 07:41:02.490991+00	f	0	f
29	8	43	3	55	39	Standart	Kuzular icin acil ihtiyac	2026-06-27 07:41:02.490991+00	f	0	f
30	9	41	1	180	37	Standart		2026-06-27 07:41:02.490991+00	f	0	f
31	9	42	1	280	38	Standart	Hediye paket yapilsin	2026-06-27 07:41:02.490991+00	f	5	t
32	9	43	2	55	39	Standart	Sigirlar icin	2026-06-27 07:41:02.490991+00	f	0	f
33	10	43	3	55	39	Standart	Koyun surusu icin	2026-06-27 07:41:02.490991+00	f	0	f
34	11	41	2	180	37	Standart	Magazadan teslim alacak	2026-06-27 07:41:02.490991+00	f	0	f
35	11	42	1	280	38	Standart	Kontrol edip alacak	2026-06-27 07:41:02.490991+00	f	0	f
\.


--
-- Data for Name: Siparisler; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Siparisler" ("Id", "AppUserId", "SiparisNo", "KuponKodu", "IndirimTutari", "MusteriAdSoyad", "Telefon", "Eposta", "Sehir", "Ilce", "AcikAdres", "ToplamTutar", "Durum", "KargoTakipNo", "KargoFirmasiId", "KargoFirmasi", "KargoUcreti", "Aciklama", "EmailHashKodu", "FaturaDosyaYolu", "FaturaDosyaAdi", "FaturaYuklendiMi", "FaturaYuklenmeTarihi", "FaturaMailGonderildiMi", "FaturaMailGonderimTarihi", "OlusturulmaTarihi", "SilindiMi", "ReceteDosyaYolu", "TeslimatTipi", "KimlikFotoYolu", "KapidaOdemeHizmetBedeli", "OdemeYontemi", "ReceteOnayDurumu", "ReceteRedSebebi") FROM stdin;
7	\N	VET-202606-1006	\N	0	Ahmet El-Filistini	+970599111111	ahmet@example.com	Ramallah	Al-Bireh	Ramallah, Filistin	450	3	\N	\N	\N	15	Steteskop ve Dikis Seti siparisi	\N	\N	\N	f	\N	f	\N	2026-06-27 07:40:32.747877+00	f	\N	AdreseTeslim	\N	0	BankaHavalesi	0	\N
8	\N	VET-202606-1007	\N	0	Fatima Hassan	+970599222222	fatima@example.com	Hebron	Halhul	Hebron, Filistin	280	1	\N	\N	\N	15	B12 enjeksiyon siparisi	\N	\N	\N	f	\N	f	\N	2026-06-27 07:40:32.747877+00	f	\N	AdreseTeslim	\N	12	KapidaOdeme	0	\N
9	\N	VET-202606-1008	WELCOME10	10	Mahmoud Abbas	+970599333333	mahmoud@example.com	Nablus	Rafidia	Nablus, Filistin	620	2	\N	\N	\N	0	Birkac urun toplu siparis	\N	\N	\N	f	\N	f	\N	2026-06-27 07:40:32.747877+00	f	\N	AdreseTeslim	\N	0	BankaHavalesi	0	\N
10	\N	VET-202606-1009	\N	0	Layla Khoury	+970599444444	layla@example.com	Gaza	Al-Rimal	Gaza, Filistin	195	4	\N	\N	\N	15	B12 enjeksiyon - kucukbas	\N	\N	\N	f	\N	f	\N	2026-06-27 07:40:32.747877+00	f	\N	AdreseTeslim	\N	0	BankaHavalesi	0	\N
11	\N	VET-202606-1010	KURBAN25	25	Yusuf Ibrahim	+970599555555	yusuf@example.com	Jenin	Jenin Camp	Jenin, Filistin	540	5	\N	\N	\N	0	Magazadan teslim alindi	\N	\N	\N	f	\N	f	\N	2026-06-27 07:40:32.747877+00	f	\N	MagazadanTeslim	\N	18	KapidaOdeme	0	\N
\.


--
-- Data for Name: SiteAyarlari; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."SiteAyarlari" ("Id", "SiteAdi", "MarkaAdi", "SiteBasligi", "SiteAciklamasi", "SiteLogoUrl", "FaviconUrl", "BaseUrl", "TemaRengi", "UstBarMesaji", "KampanyaMesaji", "UstBarEtkin", "UstBarHizi", "FooterAciklamasi", "Telefon", "Email", "Adres", "WhatsappNumarasi", "CalismaSaatleri", "FacebookUrl", "InstagramUrl", "TwitterUrl", "YoutubeUrl", "TiktokUrl", "PinterestUrl", "ParaBirimi", "KargoBedeli", "UcretsizKargoLimiti", "StokUyariLimiti", "StoktaYokSatisIzni", "PaytrAktifMi", "PaytrTestModu", "PaytrMerchantId", "PaytrMerchantKeyProtected", "PaytrMerchantSaltProtected", "PaytrCallbackUrl", "PaytrBasariliDonusUrl", "PaytrBasarisizDonusUrl", "KargoFirmasi", "KargoTakipUrl", "SiparisTeslimSuresiGun", "IadeHakkiGun", "MetaTitle", "MetaDescription", "MetaKeywords", "GoogleAnalyticsId", "FacebookPixelId", "VarsayilanSosyalPaylasimGorseliUrl", "CookieMetni", "YeniSiparisMailBildirimi", "StokUyarisiMailBildirimi", "IadeTalebiMailBildirimi", "BildirimAliciEmail", "BakimModuAktif", "BakimModuMesaji", "GirisZorunluMu", "StokBiteniGriGoster", "KapidaOdemeAktifMi", "KapidaOdemeHizmetBedeli", "KapidaOdemeLimiti", "ToptanciMinSiparisTutari", "IptalSuresiSaat", "FooterAciklamasiEn", "FooterAciklamasiAr", "FooterAciklamasiTr") FROM stdin;
1	7ANRPS48	7ANRPS48	7ANRPS48 - متجرك الإلكتروني	منتجات متنوعة بأفضل الأسعار مع خدمة توصيل سريعة	/74anrps48logo2.svg	/74anrps48logo2.svg	https://filistin.kastamonuesnaf.com.tr	#1a5632	توصيل مجاني للطلبات فوق 200 ₪	الدفع عند الاستلام متاح	t	34	متجر إلكتروني فلسطيني يقدم منتجات متنوعة بأسعار منافسة وتوصيل سريع لجميع المدن	+970-599-000-000	info@7anrps48.com	فلسطين	+970599000000	السبت - الخميس: 9:00 - 21:00							₪	15	200	5	f	f	f							توصيل محلي		3	7	7ANRPS48 - متجر إلكتروني فلسطيني | تسوق أونلاين	تسوق أونلاين من 7ANRPS48. منتجات متنوعة بأفضل الأسعار مع توصيل سريع لجميع مدن فلسطين. الدفع عند الاستلام أو تحويل بنكي.	تسوق أونلاين, فلسطين, متجر إلكتروني, توصيل, 7ANRPS48			/74anrps48logo2.svg	نستخدم ملفات تعريف الارتباط لتحسين تجربتك وتحليل حركة الموقع.	t	t	t	info@7anrps48.com	f	نحن نعمل على تحسين الموقع لتقديم تجربة تسوق أفضل. سنعود قريباً!	f	t	f	0.0	2000	500	3	A Palestinian e-commerce site offering varied products at competitive prices with fast delivery to all cities.	متجر إلكتروني فلسطيني يقدم منتجات متنوعة بأسعار منافسة وتوصيل سريع لجميع المدن	Rekabetçi fiyatlarla çeşitli ürünler sunan ve tüm şehirlere hızlı teslimat yapan bir Filistin e-ticaret sitesi.
\.


--
-- Data for Name: SiteDegerlendirmeleri; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."SiteDegerlendirmeleri" ("Id", "AdSoyad", "Eposta", "Puan", "Baslik", "YorumMetni", "OnayliMi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: Slaytlar; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Slaytlar" ("Id", "Baslik", "AltBaslik", "Aciklama", "ResimUrl", "VideoUrl", "Tur", "Sira", "AktifMi", "OlusturmaTarihi", "BaslikEn", "BaslikAr", "AltBaslikEn", "AltBaslikAr", "AciklamaEn", "AciklamaAr", "ButonMetni", "ButonMetniEn", "ButonMetniAr", "BaglantiUrl") FROM stdin;
11	Evcil Hayvan Sağlığı ve Bakımı	Vitamin, bakım ve hijyen ürünleri	Kedi ve köpeklerin günlük bakımını destekleyen pratik ürünleri keşfedin.	/slider2.png	\N	Görsel	2	t	2026-06-26 19:12:59.964939+00	Pet Health & Care	صحة ورعاية الحيوانات الأليفة	Vitamins, care and hygiene products	فيتامينات ومنتجات عناية ونظافة	Discover practical products supporting daily care for cats and dogs.	اكتشف منتجات عملية تدعم العناية اليومية بالقطط والكلاب.				\N
12	Çiftlik Hayvanları İçin Destek Ürünleri	Mineral, bakım ve saha kullanımına uygun çözümler	Büyükbaş, küçükbaş ve kanatlı işletmeleri için seçilmiş destek ürünleri.	/slider3.png	\N	Görsel	3	t	2026-06-26 19:12:59.964939+00	Support Products for Farm Animals	منتجات دعم لحيوانات المزرعة	Minerals, care and field-use solutions	معادن وعناية وحلول مناسبة للاستخدام الميداني	Selected support products for cattle, sheep, goats and poultry.	منتجات دعم مختارة للأبقار والأغنام والدواجن.				\N
13	Klinik Sarf ve Bakım Setleri	Günlük kullanım için hazır çözümler	Muayene, bakım ve hijyen süreçlerinde kullanılabilecek tamamlayıcı ürünler.	/slider4.png	\N	Görsel	4	t	2026-06-26 19:12:59.964939+00	Clinic Consumables & Care Sets	مستلزمات العيادة ومجموعات العناية	Ready solutions for daily use	حلول جاهزة للاستخدام اليومي	Complementary products for examination, care and hygiene workflows.	منتجات مساعدة للفحص والعناية والنظافة.				\N
14	Hızlı Teslim Veteriner Ürünleri	Filistin genelinde güvenli teslimat	Stoklu ürünlerde hızlı hazırlık ve güvenli paketleme.	/slider5.png	\N	Görsel	5	t	2026-06-26 19:12:59.964939+00	Fast Delivery Veterinary Products	منتجات بيطرية بتوصيل سريع	Secure delivery across Palestine	توصيل آمن في جميع مناطق فلسطين	Fast preparation and secure packaging for in-stock products.	تحضير سريع وتغليف آمن للمنتجات المتوفرة في المخزون.				\N
10	Veteriner Klinik Ürünleri	Klinik, çiftlik ve evcil hayvan bakımında güvenilir ürünler	Muayenehane, pet shop ve hayvan sahipleri için seçilmiş veteriner ürünleri.	/slider1.png	\N	Görsel	1	f	2026-06-26 19:12:59.964939+00	Veterinary Clinic Products	منتجات بيطرية موثوقة	Reliable products for clinics, farms and pets	منتجات موثوقة للعيادات والمزارع والحيوانات الأليفة	Selected veterinary products for clinics, pet shops and animal owners.	منتجات بيطرية مختارة للعيادات ومتاجر الحيوانات وأصحاب الحيوانات الأليفة.				\N
\.


--
-- Data for Name: ToptanciIskontoOranlari; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."ToptanciIskontoOranlari" ("Id", "ToptanciUrunGrubuId", "MinAdet", "IskontoYuzdesi", "AktifMi", "OlusturulmaTarihi", "GuncellemeTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: ToptanciUrunGruplari; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."ToptanciUrunGruplari" ("Id", "Ad", "Aciklama", "AktifMi", "Sira", "OlusturulmaTarihi", "GuncellemeTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: UrunOzellikDegerleri; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."UrunOzellikDegerleri" ("Id", "UrunId", "UrunOzellikTanimiId", "Deger", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: UrunOzellikTanimlari; Type: TABLE DATA; Schema: public; Owner: -
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
21	Uygulama Tipi	duvarkagidi_uygulama	DuvarKagidi	select		Yapiskanli\nTutkalli\nKolay Sokülebilir	t	t	t	t	520	2026-06-14 08:15:13.769785+00	f
22	Silinebilirlik	duvarkagidi_silinebilirlik	DuvarKagidi	select		true\nfalse	t	t	t	t	530	2026-06-14 08:15:13.769815+00	f
23	Desen Tekrari	duvarkagidi_desen_tekrari	DuvarKagidi	text			f	t	t	t	540	2026-06-14 08:15:13.769832+00	f
24	Govde Malzemesi	mobilya_govde_malzeme	SehpaMobilya	text			t	t	t	t	610	2026-06-14 08:15:13.769849+00	f
25	Ayak Malzemesi	mobilya_ayak_malzeme	SehpaMobilya	text			t	t	t	t	620	2026-06-14 08:15:13.769866+00	f
26	Tasima Kapasitesi	mobilya_tasima_kapasitesi	SehpaMobilya	text			f	t	t	t	630	2026-06-14 08:15:13.769889+00	f
27	Kurulum Durumu	mobilya_kurulum	SehpaMobilya	select		Demonte\nHazir Kurulu	t	t	t	t	640	2026-06-14 08:15:13.769907+00	f
\.


--
-- Data for Name: UrunResimleri; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."UrunResimleri" ("Id", "ResimYolu", "Baslik", "AltMetin", "MedyaTipi", "MedyaAlani", "VideoUrl", "ThumbnailYolu", "MobilResimYolu", "Etiketler", "Sira", "VarsayilanMi", "UrunSecenekId", "UrunId", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
23	/img/products/vet-real-pet-vitamin.jpg	VetPlus Multivitamin Kedi & Köpek Damlası 50 ml	VetPlus Multivitamin Kedi & Köpek Damlası 50 ml	image	gallery		/img/products/vet-real-pet-vitamin.jpg	/img/products/vet-real-pet-vitamin.jpg	veteriner,demo	1	t	\N	26	2026-06-26 19:12:59.964939+00	f
24	/img/products/vet-real-pet-shampoo.jpg	Dermacare Hassas Deri Pet Şampuanı 250 ml	Dermacare Hassas Deri Pet Şampuanı 250 ml	image	gallery		/img/products/vet-real-pet-shampoo.jpg	/img/products/vet-real-pet-shampoo.jpg	veteriner,demo	1	t	\N	27	2026-06-26 19:12:59.964939+00	f
25	/img/products/vet-real-probiotic.jpg	ProBioVet Sindirim Destek Tozu 100 g	ProBioVet Sindirim Destek Tozu 100 g	image	gallery		/img/products/vet-real-probiotic.jpg	/img/products/vet-real-probiotic.jpg	veteriner,demo	1	t	\N	28	2026-06-26 19:12:59.964939+00	f
26	/img/products/vet-real-wound-spray.jpg	Klinik Yara Bakım Spreyi 100 ml	Klinik Yara Bakım Spreyi 100 ml	image	gallery		/img/products/vet-real-wound-spray.jpg	/img/products/vet-real-wound-spray.jpg	veteriner,demo	1	t	\N	29	2026-06-26 19:12:59.964939+00	f
27	/img/products/vet-real-ear-cleaner.jpg	OtoClean Kulak Temizleme Solüsyonu 120 ml	OtoClean Kulak Temizleme Solüsyonu 120 ml	image	gallery		/img/products/vet-real-ear-cleaner.jpg	/img/products/vet-real-ear-cleaner.jpg	veteriner,demo	1	t	\N	30	2026-06-26 19:12:59.964939+00	f
28	/img/products/vet-real-mineral-block.jpg	MineralBlock Büyükbaş & Küçükbaş Mineral Bloğu 2 kg	MineralBlock Büyükbaş & Küçükbaş Mineral Bloğu 2 kg	image	gallery		/img/products/vet-real-mineral-block.jpg	/img/products/vet-real-mineral-block.jpg	veteriner,demo	1	t	\N	31	2026-06-26 19:12:59.964939+00	f
29	/img/products/vet-real-poultry-premix.jpg	PoultryCare Kanatlı Vitamin Premiksi 500 g	PoultryCare Kanatlı Vitamin Premiksi 500 g	image	gallery		/img/products/vet-real-poultry-premix.jpg	/img/products/vet-real-poultry-premix.jpg	veteriner,demo	1	t	\N	32	2026-06-26 19:12:59.964939+00	f
30	/img/products/vet-real-calf-start.jpg	CalfStart Buzağı Başlangıç Destek Paketi	CalfStart Buzağı Başlangıç Destek Paketi	image	gallery		/img/products/vet-real-calf-start.jpg	/img/products/vet-real-calf-start.jpg	veteriner,demo	1	t	\N	33	2026-06-26 19:12:59.964939+00	f
\.


--
-- Data for Name: UrunSecenekleri; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."UrunSecenekleri" ("Id", "UrunId", "Olcu", "CerceveTipi", "CerceveRengi", "CerceveKalinligi", "MalzemeTuru", "Yon", "ParcaSayisi", "VaryantSku", "KisilestirmeMetni", "OzelTasarimNotu", "FiyatFarki", "SatisFiyati", "MaliyetFiyati", "StokAdedi", "UretimSuresiGun", "Desi", "GorselUrl", "AktifMi", "VarsayilanMi", "TukeninceGizle", "OnSipariseAcikMi", "Sira", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
32	26	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-26			0	149	0	50	2	1	/img/products/vet-real-pet-vitamin.jpg	t	t	f	t	1	2026-06-26 19:12:59.964939+00	f
33	27	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-27			0	99	0	50	2	1	/img/products/vet-real-pet-shampoo.jpg	t	t	f	t	1	2026-06-26 19:12:59.964939+00	f
34	28	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-28			0	179	0	50	2	1	/img/products/vet-real-probiotic.jpg	t	t	f	t	1	2026-06-26 19:12:59.964939+00	f
35	29	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-29			0	135	0	50	2	1	/img/products/vet-real-wound-spray.jpg	t	t	f	t	1	2026-06-26 19:12:59.964939+00	f
36	30	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-30			0	119	0	50	2	1	/img/products/vet-real-ear-cleaner.jpg	t	t	f	t	1	2026-06-26 19:12:59.964939+00	f
37	31	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-31			0	219	0	50	2	1	/img/products/vet-real-mineral-block.jpg	t	t	f	t	1	2026-06-26 19:12:59.964939+00	f
38	32	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-32			0	159	0	50	2	1	/img/products/vet-real-poultry-premix.jpg	t	t	f	t	1	2026-06-26 19:12:59.964939+00	f
39	33	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-33			0	289	0	50	2	1	/img/products/vet-real-calf-start.jpg	t	t	f	t	1	2026-06-26 19:12:59.964939+00	f
41	37	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-37			0	180	75	20	1	1	/img/products/vet-stethoscope.jpg	t	t	f	t	1	2026-06-27 07:40:06.211304+00	f
42	38	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-38			0	280	115	15	1	2	/img/products/vet-suture-kit.jpg	t	t	f	t	1	2026-06-27 07:40:06.211304+00	f
43	39	Standart	Standart	Standart	Standart	Standart	Standart	1	VET-VAR-39			0	55	20	40	1	1	/img/products/vet-copper-b12-injection.jpg	t	t	f	t	1	2026-06-27 07:40:06.211304+00	f
\.


--
-- Data for Name: Urunler; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Urunler" ("Id", "Baslik", "KisaAd", "Slug", "UrlYolu", "SKU", "Barkod", "Marka", "UrunTipi", "Etiketler", "KisaAciklama", "Aciklama", "TeknikOzellikler", "MalzemeBilgisi", "BakimTalimati", "PaketlemeBilgisi", "AnaGorselUrl", "StokDurumu", "Fiyat", "IndirimliFiyat", "Maliyet", "KdvOrani", "UretimSuresiGun", "KargoyaVerilisSuresiGun", "TahminiTeslimSuresiGun", "AktifMi", "OneCikanMi", "YeniUrunMu", "KampanyaliMi", "AnaSayfadaGoster", "Sira", "GoruntulenmeSayisi", "SatisSayisi", "FavoriSayisi", "MinSiparisAdedi", "MaxSiparisAdedi", "SeoTitle", "SeoDescription", "SeoKeywords", "KategoriId", "OlusturulmaTarihi", "SilindiMi", "TopFiyat", "AciklamaAr", "AciklamaEn", "BaslikAr", "BaslikEn", "KisaAciklamaAr", "KisaAciklamaEn", "SeoDescriptionAr", "SeoDescriptionEn", "SeoTitleAr", "SeoTitleEn", "HediyePaketFiyati", "HediyePaketiVarMi", "WhatsappSiparisVarMi", "FiyatGizliMi", "ToptanciUrunGrubuId", "KampanyaBitisTarihi", "YayindaMi") FROM stdin;
30	OtoClean Kulak Temizleme Solüsyonu 120 ml	Kulak Temizleme	otoclean-kulak-temizleme-solusyonu-120ml	/Urun/Detay/otoclean-kulak-temizleme-solusyonu-120ml	VET-CLN-002	8692000000056	VetCare Palestine	Klinik Hijyen	kulak,temizlik,kedi,köpek	Kedi ve köpeklerde kulak çevresi hijyeni için temizleme solüsyonu.	Kulak çevresindeki kirlerin yumuşatılmasına yardımcı olur. Kulak hastalıklarında veteriner hekime danışılmalıdır.	120 ml, damlalıklı şişe, kedi ve köpek için	Temizleme solüsyonu	Kulak kanalına derin uygulamayınız.	Damlalıklı şişe	/img/products/vet-real-ear-cleaner.jpg	Stokta	145	119	52	20	2	1	4	t	f	f	t	t	5	0	0	0	1	\N	OtoClean Kulak Temizleme Solüsyonu	Kedi ve köpekler için kulak temizleme solüsyonu.	kulak,temizlik,solüsyon	69	2026-06-26 19:12:59.964939+00	f	119	يساعد على تليين الأوساخ حول الأذن. يجب استشارة الطبيب البيطري في حالات أمراض الأذن.	Helps soften dirt around the ear area. Consult a veterinarian for ear diseases.	محلول تنظيف الأذن أوتوكلين 120 مل	OtoClean Ear Cleaning Solution 120 ml	محلول تنظيف لنظافة منطقة الأذن لدى القطط والكلاب.	A cleaning solution for ear area hygiene in cats and dogs.	محلول تنظيف الأذن للقطط والكلاب.	Ear cleaning solution for cats and dogs.	محلول تنظيف الأذن أوتوكلين	OtoClean Ear Cleaning Solution	15	t	t	f	\N	\N	t
26	VetPlus Multivitamin Kedi & Köpek Damlası 50 ml	Multivitamin Damla	vetplus-multivitamin-kedi-kopek-damlasi-50ml	/Urun/Detay/vetplus-multivitamin-kedi-kopek-damlasi-50ml	VET-PET-001	8692000000018	VetCare Palestine	Pet Destek	vitamin,kedi,köpek,damla	Kedi ve köpeklerin günlük vitamin desteği için formüle edilmiş tamamlayıcı damla.	Günlük beslenmeyi desteklemek amacıyla geliştirilmiş, mama ile kolayca karıştırılabilen multivitamin damla. Hastalık tedavisi yerine geçmez; veteriner hekimin önerisiyle kullanılması tavsiye edilir.	50 ml, damlalıklı şişe, kedi ve köpek kullanımına uygun	Likit destek ürünü	Serin ve kuru yerde saklayınız. Çocuklardan uzak tutunuz.	Emniyet kapaklı şişe ve koruyucu kutu	/img/products/vet-real-pet-vitamin.jpg	Stokta	185	149	70	20	2	1	4	t	t	t	t	t	1	1	0	0	1	\N	VetPlus Multivitamin Kedi & Köpek Damlası	Kedi ve köpekler için günlük multivitamin desteği.	kedi,köpek,vitamin,damla	67	2026-06-26 19:12:59.964939+00	f	149	قطرات فيتامينات لدعم التغذية اليومية ويمكن خلطها بسهولة مع الطعام. لا تغني عن العلاج وينصح باستخدامها بتوجيه الطبيب البيطري.	A multivitamin drop developed to support daily nutrition and easily mixed with food. It does not replace treatment; use with veterinary advice is recommended.	قطرات فيتامين متعددة للقطط والكلاب 50 مل	VetPlus Multivitamin Drops for Cats & Dogs 50 ml	قطرات مكملة متعددة الفيتامينات لدعم القطط والكلاب يومياً.	A complementary multivitamin drop formulated for daily cat and dog support.	دعم يومي متعدد الفيتامينات للقطط والكلاب.	Daily multivitamin support for cats and dogs.	قطرات فيتامين متعددة للقطط والكلاب	VetPlus Multivitamin Drops for Cats & Dogs	15	t	t	f	\N	\N	t
38	Cerrahi Dikis Seti 7 Parca Paslanmaz Celik	Dikis Seti 7 Parca	cerrahi-dikis-seti-7-parca	/Urun/Detay/cerrahi-dikis-seti-7-parca	VET-CL-005	8692000000056	VetCerrahi	Cerrahi Malzeme	dikis seti,cerrahi,veteriner,portegu,penset	Veteriner hekimler icin 7 parca paslanmaz celik cerrahi dikis seti.	Profesyonel veteriner hekimler icin 7 parca cerrahi dikis seti. Paslanmaz celik, ozel fermuARli tasima cantasi icinde.	7 parca, paslanmaz celik, 18x14x3 cm, 250 gr	Paslanmaz celik cerrahi aletler	Kullanim sonrasi dezenfekte ediniz.	FermuARli tasima cantasi icerisinde	/img/products/vet-suture-kit.jpg	Stokta	350	280	140	20	1	1	3	t	t	t	f	t	10	0	0	0	1	100	Cerrahi Dikis Seti 7 Parca - 7ANRPS48	Veteriner hekimler icin 7 parca cerrahi dikis seti	cerrahi,dikis seti,veteriner,portegu	70	2026-06-27 07:39:49.479609+00	f	\N	طقم خياطة جراحي احترافي مكون من 7 قطع	Professional 7-piece surgical suture kit	طقم خياطة جراحي 7 قطع ستانلس ستيل	Surgical Suture Kit 7-Piece Stainless Steel	طقم خياطة جراحي مكون من 7 قطع	Surgical suture kit 7 pieces of stainless steel	طقم خياطة جراحي مكون من 7 قطع	Surgical suture kit 7 pieces	طقم خياطة جراحي 7 قطع	Surgical Suture Kit 7-Piece	0	f	t	f	\N	\N	t
39	Bakir + Vitamin B12 Enjeksiyon 20 ml - Buyukbas & Kucukbas	Bakir B12 Enjeksiyon	bakir-vitamin-b12-enjeksiyon-20ml	/Urun/Detay/bakir-vitamin-b12-enjeksiyon-20ml	VET-FRM-001	8692000000063	ShowVet	Ciftlik Ilac	bakir,b12,enjeksiyon,buyukbas,kucukbas,anemi	Buyukbas ve kucukbas hayvanlarda bakir eksikligi ve anemide kullanilan enjeksiyon. 20 ml.	Sigir, dana, koyun, kuzuda bakir eksikligi (hipokuroz) ve anemi tedavisinde kullanilir. Her ml 20 mg bakir + 1 mg B12 icerir.	20 ml, 20 mg/ml bakir + 1 mg/ml B12	Bakir metiyonat + Vitamin B12	25C altinda, isiktan koruyunuz.	Renkli cam sise, karton kutu	/img/products/vet-copper-b12-injection.jpg	Stokta	65	55	25	8	1	1	3	t	t	t	f	t	11	0	0	0	1	100	Bakir + Vitamin B12 Enjeksiyon 20 ml - 7ANRPS48	Buyukbas ve kucukbas hayvanlar icin bakir + B12 enjeksiyonu	bakir,b12,enjeksiyon,buyukbas,kucukbas	73	2026-06-27 07:39:49.479609+00	f	\N	محلول قابل للحقن لعلاج نقص النحاس وفقر الدم	Injectable solution for copper deficiency and anemia	حقنة نحاس + فيتامين ب12 20 مل	Copper + Vitamin B12 Injection 20 ml	حقنة لعلاج نقص النحاس وفقر الدم	Injection for copper deficiency and anemia	حقنة لعلاج نقص النحاس وفقر الدم	Injection for copper deficiency and anemia	حقنة نحاس + فيتامين ب12 20 مل	Copper + Vitamin B12 Injection 20 ml	0	f	t	f	\N	\N	t
37	Veteriner Steteskop Cift Tambur 60 cm	Steteskop 60 cm	veteriner-steteskop-cift-tambur-60cm	/Urun/Detay/veteriner-steteskop-cift-tambur-60cm	VET-CL-004	8692000000049	Panascope	Klinik Ekipman	steteskop,veteriner,muayene,oskultasyon	Veteriner kullanimi icin ozel tasarlanmis cift tambur ve cift borulu steteskop.	Veteriner hekimler icin ozel olarak dizayn edilmis profesyonel steteskop. Cift tambur ve cift boru yapisi sayesinde net oskultasyon saglar. Yedek set ile birlikte gelir.	Cift tambur, cift boru, 60 cm	Paslanmaz celik ve medikal PVC	Serin ve kuru yerde saklayiniz.	Orijinal kutusu ve yedek set ile birlikte	/img/products/vet-stethoscope.jpg	Stokta	220	180	90	20	1	1	3	t	t	t	f	t	9	1	0	0	1	100	Veteriner Steteskop Cift Tambur 60 cm	Veteriner kullanimi icin cift tambur steteskop	steteskop,veteriner,muayene	70	2026-06-27 07:39:49.479609+00	f	\N	سماعة بيطرية احترافية بنظام القناتين والصدر المزدوج.	Professional veterinary stethoscope with dual chest piece.	سماعة طبية بيطرية مزدوجة 60 سم	Veterinary Stethoscope Dual Chest Piece 60 cm	سماعة بيطرية مزدوجة مصممة خصيصًا للاستخدام البيطري	Veterinary stethoscope specially designed for veterinary use	سماعة بيطرية مزدوجة	Veterinary stethoscope dual chest piece	سماعة طبية بيطرية مزدوجة 60 سم	Veterinary Stethoscope Dual Chest Piece 60 cm	0	f	t	f	\N	\N	t
27	Dermacare Hassas Deri Pet Şampuanı 250 ml	Pet Şampuanı	dermacare-hassas-deri-pet-sampuani-250ml	/Urun/Detay/dermacare-hassas-deri-pet-sampuani-250ml	VET-PET-002	8692000000025	VetCare Palestine	Pet Bakım	şampuan,kedi,köpek,hijyen	Hassas derili kedi ve köpekler için nazik temizleme şampuanı.	Düzenli bakımda tüylerin kolay taranmasına yardımcı olur. Paraben içermez, ferah ve hafif kokuludur.	250 ml, hassas deri, kedi ve köpek kullanımına uygun	Şampuan	Gözle temasından kaçınınız. İyice durulayınız.	Sızdırmaz kapaklı şişe	/img/products/vet-real-pet-shampoo.jpg	Stokta	125	99	45	20	2	1	4	t	t	f	t	t	2	0	0	0	1	\N	Dermacare Hassas Deri Pet Şampuanı	Hassas derili kedi ve köpekler için pet şampuanı.	pet,şampuan,kedi,köpek	68	2026-06-26 19:12:59.964939+00	f	99	يساعد على تمشيط الفراء بسهولة أثناء العناية المنتظمة. خالٍ من البارابين وبرائحة خفيفة.	Helps make the coat easier to comb during regular care. Paraben-free with a light fresh scent.	شامبو للحيوانات الأليفة للبشرة الحساسة 250 مل	Dermacare Sensitive Skin Pet Shampoo 250 ml	شامبو تنظيف لطيف للقطط والكلاب ذات البشرة الحساسة.	A gentle cleansing shampoo for cats and dogs with sensitive skin.	شامبو للقطط والكلاب ذات البشرة الحساسة.	Pet shampoo for cats and dogs with sensitive skin.	شامبو للحيوانات الأليفة للبشرة الحساسة	Dermacare Sensitive Skin Pet Shampoo	15	t	t	f	\N	\N	t
28	ProBioVet Sindirim Destek Tozu 100 g	Sindirim Destek	probiovet-sindirim-destek-tozu-100g	/Urun/Detay/probiovet-sindirim-destek-tozu-100g	VET-PET-003	8692000000032	VetCare Palestine	Pet Destek	probiyotik,sindirim,kedi,köpek	Kedi ve köpeklerde günlük sindirim desteği için tamamlayıcı toz.	Mama üzerine serpilerek kullanılabilen pratik sindirim destek ürünüdür. Veteriner hekimin önerdiği kullanım miktarına uyunuz.	100 g, ölçü kaşıklı, mama üstüne kullanım	Toz destek ürünü	Kuru ve serin yerde saklayınız.	Kilitli kapaklı kutu	/img/products/vet-real-probiotic.jpg	Stokta	210	179	82	20	2	1	4	t	f	t	t	t	3	0	0	0	1	\N	ProBioVet Sindirim Destek Tozu	Kedi ve köpekler için sindirim destek tozu.	probiyotik,sindirim,toz	67	2026-06-26 19:12:59.964939+00	f	179	منتج عملي لدعم الهضم يمكن رشه فوق الطعام. يرجى اتباع الكمية التي يوصي بها الطبيب البيطري.	A practical digestive support product that can be sprinkled over food. Follow the amount recommended by your veterinarian.	مسحوق دعم الهضم بروبيوفيت 100 غ	ProBioVet Digestive Support Powder 100 g	مسحوق مكمل لدعم الهضم اليومي لدى القطط والكلاب.	A complementary powder for daily digestive support in cats and dogs.	مسحوق دعم الهضم للقطط والكلاب.	Digestive support powder for cats and dogs.	مسحوق دعم الهضم بروبيوفيت	ProBioVet Digestive Support Powder	15	t	t	f	\N	\N	t
29	Klinik Yara Bakım Spreyi 100 ml	Yara Bakım Spreyi	klinik-yara-bakim-spreyi-100ml	/Urun/Detay/klinik-yara-bakim-spreyi-100ml	VET-CLN-001	8692000000049	VetCare Palestine	Klinik Sarf	yara,bakım,sprey,klinik	Veteriner kliniklerinde bakım süreçlerini desteklemek için pratik sprey.	Temizlik ve bakım süreçlerinde yardımcı amaçla kullanılan, kolay uygulanabilir sprey formunda klinik sarf ürünüdür. Tedavi ürünü değildir.	100 ml, sprey başlık, saha ve klinik kullanımına uygun	Bakım spreyi	Harici kullanım içindir. Gözle temas ettirmeyiniz.	Sprey şişe	/img/products/vet-real-wound-spray.jpg	Stokta	160	135	60	20	2	1	4	t	t	f	t	t	4	0	0	0	1	\N	Klinik Yara Bakım Spreyi	Veteriner klinikleri için bakım spreyi.	yara,bakım,sprey,klinik	70	2026-06-26 19:12:59.964939+00	f	135	منتج عيادي بشكل بخاخ سهل الاستخدام للمساعدة في التنظيف والعناية. ليس منتجاً علاجياً.	An easy-to-apply spray form clinic consumable used as an aid in cleaning and care processes. Not a treatment product.	بخاخ عناية بالجروح للعيادة 100 مل	Clinic Wound Care Spray 100 ml	بخاخ عملي يدعم إجراءات العناية في العيادات البيطرية.	A practical spray supporting care workflows in veterinary clinics.	بخاخ عناية للعيادات البيطرية.	Care spray for veterinary clinics.	بخاخ عناية بالجروح للعيادة	Clinic Wound Care Spray	15	t	t	f	\N	\N	t
31	MineralBlock Büyükbaş & Küçükbaş Mineral Bloğu 2 kg	Mineral Blok	mineralblock-buyukbas-kucukbas-mineral-blogu-2kg	/Urun/Detay/mineralblock-buyukbas-kucukbas-mineral-blogu-2kg	VET-FRM-001	8692000000063	VetCare Palestine	Çiftlik Destek	mineral,büyükbaş,küçükbaş,çiftlik	Büyükbaş ve küçükbaş hayvanlar için mineral destek bloğu.	Ahır ve mera kullanımına uygun, günlük mineral alımını desteklemek amacıyla kullanılan pratik blok formu.	2 kg, asma delikli, saha kullanımına uygun	Mineral blok	Kuru zeminde muhafaza ediniz.	Dayanıklı paketleme	/img/products/vet-real-mineral-block.jpg	Stokta	260	219	105	20	2	1	4	t	t	t	t	t	6	0	0	0	1	\N	MineralBlock Büyükbaş & Küçükbaş Mineral Bloğu	Çiftlik hayvanları için mineral destek bloğu.	mineral,büyükbaş,küçükbaş	75	2026-06-26 19:12:59.964939+00	f	219	قالب عملي مناسب للاستخدام في الحظائر والمراعي لدعم المدخول اليومي من المعادن.	A practical block form suitable for barn and pasture use to support daily mineral intake.	قالب معادن للأبقار والأغنام 2 كغ	MineralBlock Cattle & Small Ruminant Mineral Block 2 kg	قالب دعم معدني للأبقار والأغنام والماعز.	A mineral support block for cattle, sheep and goats.	قالب دعم معدني لحيوانات المزرعة.	Mineral support block for farm animals.	قالب معادن للأبقار والأغنام	MineralBlock Cattle & Small Ruminant Mineral Block	15	t	t	f	\N	\N	t
32	PoultryCare Kanatlı Vitamin Premiksi 500 g	Kanatlı Premiks	poultrycare-kanatli-vitamin-premiksi-500g	/Urun/Detay/poultrycare-kanatli-vitamin-premiksi-500g	VET-FRM-002	8692000000070	VetCare Palestine	Kanatlı Destek	kanatlı,vitamin,premiks	Kanatlı işletmeleri için vitamin destek premiksi.	Yem karışımlarında destek amacıyla kullanılan, saha kullanımına uygun premiks formu. Kullanım oranı için uzman önerisi alınmalıdır.	500 g, toz premiks, kanatlı yem desteği	Toz premiks	Nemden uzak tutunuz.	Kilitli ambalaj	/img/products/vet-real-poultry-premix.jpg	Stokta	190	159	75	20	2	1	4	t	t	f	t	t	7	0	0	0	1	\N	PoultryCare Kanatlı Vitamin Premiksi	Kanatlılar için vitamin destek premiksi.	kanatlı,vitamin,premiks	74	2026-06-26 19:12:59.964939+00	f	159	بريمكس مناسب للاستخدام الميداني في خلطات العلف. يجب أخذ رأي المختص لتحديد نسبة الاستخدام.	A field-use premix form used as support in feed mixtures. Expert advice should be obtained for usage rate.	بريمكس فيتامين للدواجن 500 غ	PoultryCare Poultry Vitamin Premix 500 g	بريمكس دعم فيتاميني لمزارع الدواجن.	A vitamin support premix for poultry operations.	بريمكس دعم فيتاميني للدواجن.	Vitamin support premix for poultry.	بريمكس فيتامين للدواجن	PoultryCare Poultry Vitamin Premix	15	t	t	f	\N	\N	t
33	CalfStart Buzağı Başlangıç Destek Paketi	Buzağı Destek	calfstart-buzagi-baslangic-destek-paketi	/Urun/Detay/calfstart-buzagi-baslangic-destek-paketi	VET-FRM-003	8692000000087	VetCare Palestine	Buzağı Destek	buzağı,destek,çiftlik	Buzağı bakım döneminde kullanılan başlangıç destek paketi.	Yeni doğan ve genç buzağıların bakım programlarını desteklemek amacıyla hazırlanmış pratik paket.	3 parçalı destek seti, saha kullanımına uygun	Destek seti	Veteriner önerisiyle kullanınız.	Kutulu set	/img/products/vet-real-calf-start.jpg	Stokta	340	289	145	20	2	1	4	t	f	t	t	t	8	1	0	0	1	\N	CalfStart Buzağı Başlangıç Destek Paketi	Buzağı bakımına destek paketi.	buzağı,destek,çiftlik	73	2026-06-26 19:12:59.964939+00	f	289	حزمة عملية لدعم برامج رعاية العجول حديثة الولادة والصغيرة.	A practical pack prepared to support care programs for newborn and young calves.	حزمة دعم بداية للعجول	CalfStart Calf Starter Support Pack	حزمة دعم تستخدم خلال فترة رعاية العجول.	A starter support pack used during calf care periods.	حزمة دعم لرعاية العجول.	Support pack for calf care.	حزمة دعم بداية للعجول	CalfStart Calf Starter Support Pack	15	t	t	f	\N	\N	t
\.


--
-- Data for Name: Yorumlar; Type: TABLE DATA; Schema: public; Owner: -
--

COPY public."Yorumlar" ("Id", "UrunId", "AppUserId", "AdSoyad", "YorumMetni", "Puan", "OnayliMi", "OlusturulmaTarihi", "SilindiMi") FROM stdin;
\.


--
-- Data for Name: ZiyaretciLoglari; Type: TABLE DATA; Schema: public; Owner: -
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
1095	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-27 15:46:06.805781+00	f
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
683	::1	/	Mozilla/5.0 (Windows NT; Windows NT 10.0; tr-TR) WindowsPowerShell/5.1.26100.8737		Misafir	GET	-	-	Bilinmiyor	PC (Windows)	Windows	2026-06-26 14:59:38.140772+00	f
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
822	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 16:18:48.68736+00	f
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
823	::1	/Urun	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 16:18:48.833979+00	f
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
684	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 14:59:52.512678+00	f
685	::1	/	Mozilla/5.0 (Windows NT; Windows NT 10.0; tr-TR) WindowsPowerShell/5.1.26100.8737		Misafir	GET	-	-	Bilinmiyor	PC (Windows)	Windows	2026-06-26 15:00:42.34207+00	f
686	::1	/	Mozilla/5.0 (Windows NT; Windows NT 10.0; tr-TR) WindowsPowerShell/5.1.26100.8737		Misafir	GET	-	-	Bilinmiyor	PC (Windows)	Windows	2026-06-26 15:01:02.219903+00	f
687	::1	/	Mozilla/5.0 (Windows NT; Windows NT 10.0; tr-TR) WindowsPowerShell/5.1.26100.8737		Misafir	GET	-	-	Bilinmiyor	PC (Windows)	Windows	2026-06-26 15:01:10.868147+00	f
688	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:01:33.042414+00	f
689	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:01:35.235987+00	f
690	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:01:36.603991+00	f
691	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:01:37.481414+00	f
692	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap?returnUrl=%2FAdmin%2FHome%2FIndex	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:02:21.563824+00	f
693	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:02:23.614609+00	f
694	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:02:23.641066+00	f
695	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:08:00.750583+00	f
696	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:18:44.831323+00	f
697	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:18:47.984067+00	f
698	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:18:49.465701+00	f
699	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:18:50.662539+00	f
700	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 15:19:47.90067+00	f
701	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 15:19:48.106479+00	f
702	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 15:19:48.323789+00	f
703	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 15:20:49.864343+00	f
704	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 15:20:53.133017+00	f
705	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:21:12.76494+00	f
706	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:21:30.028073+00	f
707	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:26:37.492052+00	f
708	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:26:41.311688+00	f
709	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:31:38.510051+00	f
710	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:31:40.824559+00	f
711	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:31:42.251369+00	f
712	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:31:55.84953+00	f
713	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:31:58.336758+00	f
714	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:00.955903+00	f
715	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:17.97869+00	f
716	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:19.978329+00	f
717	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:21.652022+00	f
718	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:22.621249+00	f
719	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:23.670842+00	f
720	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:24.559089+00	f
721	::1	/Kurumsal/SSS	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:25.483151+00	f
722	::1	/Kurumsal/Gizlilik	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:26.343377+00	f
723	::1	/Kurumsal/KullaniciSozlesmesi	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:27.222043+00	f
724	::1	/Kurumsal/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:28.115154+00	f
725	::1	/Sozlesmeler/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:28.131557+00	f
726	::1	/Kurumsal/IadeKosullari	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:29.036366+00	f
727	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:29.935789+00	f
728	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:32:30.780421+00	f
729	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:33:22.845648+00	f
730	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Hesap/GirisYap?returnUrl=%2FAdmin%2FHome%2FIndex	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:33:29.069384+00	f
731	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:33:29.811742+00	f
732	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:46.223196+00	f
733	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:49.063163+00	f
734	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:50.261949+00	f
735	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:51.253997+00	f
736	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:52.123618+00	f
737	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:53.039509+00	f
738	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:53.987931+00	f
739	::1	/Kurumsal/SSS	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:55.095898+00	f
740	::1	/Kurumsal/Gizlilik	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:55.903368+00	f
741	::1	/Kurumsal/KullaniciSozlesmesi	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:56.718789+00	f
742	::1	/Kurumsal/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:57.569665+00	f
743	::1	/Sozlesmeler/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:57.581015+00	f
744	::1	/Kurumsal/IadeKosullari	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:58.385363+00	f
745	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:58:59.229737+00	f
746	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:00.063626+00	f
747	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:01.255981+00	f
748	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:02.86488+00	f
749	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:03.744974+00	f
750	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:04.560758+00	f
751	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:05.347064+00	f
752	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:06.233657+00	f
753	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:07.13699+00	f
754	::1	/Kurumsal/SSS	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:07.956174+00	f
755	::1	/Kurumsal/Gizlilik	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:08.765635+00	f
756	::1	/Kurumsal/KullaniciSozlesmesi	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:09.583597+00	f
757	::1	/Kurumsal/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:10.386507+00	f
758	::1	/Sozlesmeler/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:10.398377+00	f
759	::1	/Kurumsal/IadeKosullari	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:11.224553+00	f
760	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:12.131006+00	f
761	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:12.999103+00	f
762	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:14.126082+00	f
763	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:15.772307+00	f
772	::1	/Kurumsal/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:23.256011+00	f
764	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:16.63649+00	f
765	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:17.409963+00	f
766	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:18.232405+00	f
767	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:19.098215+00	f
768	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:19.927046+00	f
769	::1	/Kurumsal/SSS	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:20.767129+00	f
770	::1	/Kurumsal/Gizlilik	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:21.587387+00	f
771	::1	/Kurumsal/KullaniciSozlesmesi	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:22.397311+00	f
773	::1	/Sozlesmeler/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:23.26425+00	f
774	::1	/Kurumsal/IadeKosullari	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:24.088932+00	f
775	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:24.985196+00	f
776	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 15:59:25.823479+00	f
777	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:17:53.183811+00	f
778	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:17:55.978239+00	f
779	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:17:57.242491+00	f
780	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:17:58.265194+00	f
781	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:17:59.065785+00	f
782	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:17:59.991766+00	f
783	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:00.841058+00	f
784	::1	/Kurumsal/SSS	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:01.698499+00	f
785	::1	/Kurumsal/Gizlilik	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:02.517607+00	f
786	::1	/Kurumsal/KullaniciSozlesmesi	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:03.279281+00	f
787	::1	/Kurumsal/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:04.06924+00	f
788	::1	/Sozlesmeler/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:04.083839+00	f
789	::1	/Kurumsal/IadeKosullari	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:04.862444+00	f
790	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:05.82961+00	f
791	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:06.607362+00	f
792	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:07.679544+00	f
793	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:09.293148+00	f
794	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:10.207431+00	f
795	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:11.037363+00	f
796	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:11.834523+00	f
797	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:12.668961+00	f
798	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:13.522011+00	f
799	::1	/Kurumsal/SSS	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:14.50249+00	f
800	::1	/Kurumsal/Gizlilik	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:15.307661+00	f
801	::1	/Kurumsal/KullaniciSozlesmesi	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:16.086124+00	f
802	::1	/Kurumsal/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:16.890811+00	f
803	::1	/Sozlesmeler/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:16.903716+00	f
804	::1	/Kurumsal/IadeKosullari	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:17.756569+00	f
805	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:18.598588+00	f
806	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:19.405734+00	f
807	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:20.497223+00	f
808	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:22.484045+00	f
809	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:23.486836+00	f
810	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:24.316378+00	f
811	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:25.175505+00	f
812	::1	/Kurumsal/Hakkimizda	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:26.054437+00	f
816	::1	/Kurumsal/KullaniciSozlesmesi	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:29.300643+00	f
817	::1	/Kurumsal/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:30.129072+00	f
813	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:26.869695+00	f
814	::1	/Kurumsal/SSS	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:27.712637+00	f
815	::1	/Kurumsal/Gizlilik	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:28.485931+00	f
818	::1	/Sozlesmeler/MesafeliSatis	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:30.137766+00	f
819	::1	/Kurumsal/IadeKosullari	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:30.946785+00	f
820	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:31.738051+00	f
821	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:18:32.585929+00	f
824	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:20:59.218116+00	f
825	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:21:01.275674+00	f
826	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:22:36.094054+00	f
827	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:22:53.147673+00	f
828	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:22:53.15853+00	f
829	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:22:56.555671+00	f
830	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:22:56.568066+00	f
831	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:22:58.678372+00	f
832	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:22:58.694177+00	f
833	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:26:40.773588+00	f
834	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:27:09.865138+00	f
835	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 16:27:14.564704+00	f
836	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:15:54.979111+00	f
837	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:16:08.483308+00	f
838	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:16:27.642028+00	f
839	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:16:31.622314+00	f
840	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:16:34.325244+00	f
841	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:16:37.123376+00	f
842	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:50:52.630137+00	f
843	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:50:57.446248+00	f
844	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:51:00.011952+00	f
845	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 18:51:02.473642+00	f
846	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:13:36.023023+00	f
847	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:13:40.374434+00	f
848	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:13:42.85939+00	f
849	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/148.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:13:45.179088+00	f
850	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:13:59.083317+00	f
851	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:14:45.543654+00	f
852	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:14:47.65613+00	f
853	::1	/Urun/Detay/calfstart-buzagi-baslangic-destek-paketi-33	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:15:31.412969+00	f
854	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/calfstart-buzagi-baslangic-destek-paketi-33	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:15:38.634541+00	f
855	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:35.787167+00	f
856	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:35.805011+00	f
857	::1	/Urun/CanliAra	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:41.00285+00	f
858	::1	/Urun/CanliAra	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:41.412726+00	f
906	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:49:08.009572+00	f
859	::1	/Urun/CanliAra	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:42.227034+00	f
860	::1	/Urun/CanliAra	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:42.865507+00	f
861	::1	/Urun/CanliAra	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:45.237526+00	f
866	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:56.41434+00	f
867	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:17:02.325238+00	f
870	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:18:38.168293+00	f
862	::1	/Urun/CanliAra	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:46.244512+00	f
863	::1	/Urun/CanliAra	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:47.065718+00	f
864	::1	/Urun/CanliAra	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:48.643187+00	f
868	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:18:29.082344+00	f
869	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:18:33.94117+00	f
865	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:16:51.330134+00	f
871	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 19:29:06.761962+00	f
872	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:16.053552+00	f
873	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:19.658375+00	f
874	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:21.234753+00	f
875	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:22.987644+00	f
876	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:24.644413+00	f
877	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:25.900207+00	f
878	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:27.30191+00	f
879	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:27.3295+00	f
880	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:28.600741+00	f
881	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:29.918082+00	f
882	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:30:31.151749+00	f
883	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 19:32:08.175526+00	f
884	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:24.368962+00	f
885	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:27.650566+00	f
886	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:29.210039+00	f
887	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:30.918538+00	f
888	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:32.539115+00	f
889	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:33.801748+00	f
890	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:35.139036+00	f
891	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:35.167574+00	f
892	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:36.5028+00	f
893	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:37.705828+00	f
894	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:32:39.046083+00	f
895	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 19:42:22.983134+00	f
896	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:48:53.365797+00	f
897	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:48:56.743127+00	f
898	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:48:58.29484+00	f
899	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:49:00.042951+00	f
900	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:49:01.652247+00	f
901	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:49:02.918397+00	f
902	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:49:04.281912+00	f
903	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:49:04.308706+00	f
904	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:49:05.542851+00	f
905	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:49:06.726664+00	f
907	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:04.520205+00	f
908	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:06.650969+00	f
909	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:07.645964+00	f
910	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:08.59635+00	f
911	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:09.493501+00	f
912	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:10.412416+00	f
913	::1	/Siparis/Odeme	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:11.347081+00	f
914	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:11.357904+00	f
915	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:12.234464+00	f
916	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:13.092482+00	f
917	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:51:13.974312+00	f
918	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:54:45.606933+00	f
919	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 19:54:52.119323+00	f
920	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 20:03:11.802346+00	f
921	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 20:05:06.272547+00	f
922	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:05:18.144161+00	f
923	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:05:20.148596+00	f
924	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) HeadlessChrome/148.0.7778.96 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:05:21.61137+00	f
925	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:05:51.16414+00	f
926	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:05:55.988834+00	f
927	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:05:55.996011+00	f
928	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:06:04.827337+00	f
929	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:07:45.329657+00	f
930	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:18:27.340576+00	f
931	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:22:36.849312+00	f
932	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:22:47.304807+00	f
933	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:22:51.75374+00	f
934	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:22:51.762103+00	f
935	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:22:54.381+00	f
936	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:22:54.38812+00	f
937	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:23:06.531838+00	f
938	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 20:30:31.810126+00	f
939	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:30:48.200682+00	f
940	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:30:48.435076+00	f
941	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:37:59.258657+00	f
942	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:39:06.039642+00	f
943	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:39:23.884028+00	f
944	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:39:33.712799+00	f
945	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:39:46.363515+00	f
946	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:39:48.980614+00	f
947	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:39:53.628897+00	f
948	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:40:35.653583+00	f
949	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:42:29.375936+00	f
950	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:43:01.25847+00	f
951	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:44:19.758536+00	f
952	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:46:33.925195+00	f
953	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:46:59.23821+00	f
954	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:46:59.247533+00	f
955	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:47:05.104579+00	f
956	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:47:05.112466+00	f
957	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:48:49.940832+00	f
958	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:49:21.607431+00	f
959	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:50:56.63916+00	f
960	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:51:16.282562+00	f
961	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:54:02.848487+00	f
962	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 20:54:42.723508+00	f
963	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:55:59.198022+00	f
964	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:56:02.274929+00	f
965	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:56:23.323498+00	f
966	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:56:26.797902+00	f
967	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:56:27.378433+00	f
968	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:56:30.271855+00	f
969	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:56:47.219167+00	f
970	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:56:51.542883+00	f
971	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:56:54.529847+00	f
972	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 20:58:33.519696+00	f
973	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 20:58:49.279834+00	f
974	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:00:18.573111+00	f
975	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:04:30.09742+00	f
976	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:05:29.682673+00	f
977	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:07:08.245043+00	f
978	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:07:43.522913+00	f
979	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:07:48.062346+00	f
980	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:09:16.195888+00	f
981	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:10:21.384311+00	f
982	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:12:04.79233+00	f
983	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:12:16.362191+00	f
984	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=66	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:14:51.182083+00	f
985	::1	/Kurumsal/Iletisim	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:14:57.810944+00	f
986	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:14:59.839721+00	f
987	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:22:54.264891+00	f
988	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:22:59.888996+00	f
989	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:24:30.567169+00	f
990	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:24:47.170423+00	f
991	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:25:20.752126+00	f
992	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:26:48.118331+00	f
993	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:26:55.734319+00	f
994	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:28:24.499607+00	f
995	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:28:31.744209+00	f
996	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:29:07.986142+00	f
997	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:29:08.309185+00	f
998	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:29:13.495861+00	f
999	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:30:38.650433+00	f
1000	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:30:39.288823+00	f
1001	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:30:43.484512+00	f
1002	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:32:03.822748+00	f
1003	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=64	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:32:06.098711+00	f
1004	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:35:57.202078+00	f
1005	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:36:01.754777+00	f
1006	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:37:14.894191+00	f
1007	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:37:14.887365+00	f
1008	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:37:15.511656+00	f
1009	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:37:15.568654+00	f
1010	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:37:19.716439+00	f
1011	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:37:24.71303+00	f
1012	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:37:58.715513+00	f
1013	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:39:04.175058+00	f
1014	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:40:13.994414+00	f
1015	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:40:17.671608+00	f
1016	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:41:02.970371+00	f
1017	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=65	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:41:05.899476+00	f
1018	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=65	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:41:21.235199+00	f
1019	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=65	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:41:44.794002+00	f
1020	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun?k=65	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:41:50.074894+00	f
1021	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:41:54.381306+00	f
1022	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:41:54.508296+00	f
1023	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:41:58.572808+00	f
1024	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:42:44.568725+00	f
1025	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Kurumsal/Iletisim	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:42:52.876495+00	f
1026	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-26 21:46:58.398044+00	f
1027	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:47:05.044604+00	f
1028	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:47:54.762209+00	f
1029	::1	/Urun/Detay/vetplus-multivitamin-drops-for-cats-dogs-50-ml-1	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:48:17.973851+00	f
1030	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:48:18.076809+00	f
1031	::1	/Urun/Detay	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:48:21.660534+00	f
1032	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:48:21.664953+00	f
1033	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:48:25.21382+00	f
1034	::1	/Sepet	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:48:58.307763+00	f
1035	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:48:58.997495+00	f
1036	::1	/Hesap/KayitOl	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:49:02.930194+00	f
1037	::1	/Urun/Detay/vetplus-multivitamin-kedi-kopek-damlasi-50ml-26	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:49:17.221723+00	f
1038	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-26 21:51:04.107032+00	f
1039	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 07:14:42.371838+00	f
1040	::1	/Hesap/GirisYap	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 07:35:12.841482+00	f
1041	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 07:35:18.144761+00	f
1042	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-27 07:42:19.051588+00	f
1043	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-27 15:24:50.188684+00	f
1044	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:25:36.225377+00	f
1045	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:25:37.296567+00	f
1046	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:25:37.296567+00	f
1047	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:25:57.109597+00	f
1048	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:26:15.067128+00	f
1049	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:26:15.507346+00	f
1050	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:26:15.507346+00	f
1051	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:26:46.228713+00	f
1052	::1	/Urun/Detay/veteriner-steteskop-cift-tambur-60cm-37	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:26:57.972366+00	f
1053	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/veteriner-steteskop-cift-tambur-60cm-37	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:26:58.738212+00	f
1054	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/veteriner-steteskop-cift-tambur-60cm-37	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:26:59.039003+00	f
1055	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/veteriner-steteskop-cift-tambur-60cm-37	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:26:59.127123+00	f
1060	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:27:01.801974+00	f
1056	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun/Detay/veteriner-steteskop-cift-tambur-60cm-37	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:26:59.127123+00	f
1058	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:27:01.554727+00	f
1057	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:27:00.967818+00	f
1059	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	Misafir	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:27:01.556478+00	f
1061	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-27 15:27:25.935404+00	f
1062	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:27:34.459858+00	f
1064	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:27:35.98322+00	f
1063	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:27:35.98322+00	f
1065	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:28:24.955475+00	f
1066	::1	/Urun	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:28:36.112433+00	f
1067	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Urun	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:29:06.619118+00	f
1068	::1	/Hata/404	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:29:11.456072+00	f
1069	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:29:47.967684+00	f
1070	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:29:49.789993+00	f
1071	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:29:49.810298+00	f
1072	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:30:02.001123+00	f
1073	::1	/Favori	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:30:19.76514+00	f
1074	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Favori	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:30:25.75149+00	f
1075	::1	/Favori	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Favori	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:30:25.78203+00	f
1076	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/Favori	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:30:59.639133+00	f
1077	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36	http://localhost:5002/	admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:31:06.770523+00	f
1078	127.0.0.1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-27 15:35:31.677161+00	f
1079	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:35:38.969435+00	f
1080	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:35:48.019233+00	f
1081	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:35:48.045289+00	f
1082	127.0.0.1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-27 15:36:33.224322+00	f
1083	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:36:41.97277+00	f
1084	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:36:42.027468+00	f
1085	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:36:59.371974+00	f
1086	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:37:06.402746+00	f
1087	::1	/	curl/8.18.0		Misafir	GET	-	-	Bilinmiyor	PC / Bilinmiyor	Bilinmiyor	2026-06-27 15:40:18.728801+00	f
1088	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:40:26.137785+00	f
1089	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:40:35.758279+00	f
1090	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:40:35.78455+00	f
1091	::1	/Dil/Degistir	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:40:48.90846+00	f
1092	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:40:48.931395+00	f
1093	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:41:03.651736+00	f
1094	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:41:55.527902+00	f
1096	::1	/	Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/149.0.0.0 Safari/537.36		admin@7anrps48.com	GET	-	-	Chrome	PC (Windows)	Windows	2026-06-27 15:46:13.94458+00	f
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: -
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
20260615212141_AddToptanciMinSiparisTutari	8.0.4
20260619122952_ToptanciUrunGrubuId	8.0.0
20260615221301_AddToptanciUrunGrubu	8.0.0
20260616122634_AddBasvuruTarihi	8.0.0
20260616124117_AddFilistinKargoBolgeleri	8.0.0
20260624204621_AddReceteOnayDurumu	8.0.0
20260624201857_AddYayindaMiToUrunler	8.0.0
20260624195919_AddIptalSuresiSaat	8.0.0
20260624200430_AddStokBildirimLog	8.0.0
20260623205857_AddCarkOdulleri	8.0.0
\.


--
-- Name: aggregatedcounter_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: -
--

SELECT pg_catalog.setval('hangfire.aggregatedcounter_id_seq', 1, false);


--
-- Name: counter_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: -
--

SELECT pg_catalog.setval('hangfire.counter_id_seq', 1, false);


--
-- Name: hash_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: -
--

SELECT pg_catalog.setval('hangfire.hash_id_seq', 1, false);


--
-- Name: job_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: -
--

SELECT pg_catalog.setval('hangfire.job_id_seq', 1, false);


--
-- Name: jobparameter_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: -
--

SELECT pg_catalog.setval('hangfire.jobparameter_id_seq', 1, false);


--
-- Name: jobqueue_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: -
--

SELECT pg_catalog.setval('hangfire.jobqueue_id_seq', 1, false);


--
-- Name: list_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: -
--

SELECT pg_catalog.setval('hangfire.list_id_seq', 1, false);


--
-- Name: set_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: -
--

SELECT pg_catalog.setval('hangfire.set_id_seq', 1, false);


--
-- Name: state_id_seq; Type: SEQUENCE SET; Schema: hangfire; Owner: -
--

SELECT pg_catalog.setval('hangfire.state_id_seq', 1, false);


--
-- Name: Adresler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Adresler_Id_seq"', 1, false);


--
-- Name: AspNetRoleClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."AspNetRoleClaims_Id_seq"', 1, false);


--
-- Name: AspNetUserClaims_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."AspNetUserClaims_Id_seq"', 1, false);


--
-- Name: BankaHesaplari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."BankaHesaplari_Id_seq"', 2, true);


--
-- Name: BultenAbonelikleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."BultenAbonelikleri_Id_seq"', 1, false);


--
-- Name: CarkOdulleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."CarkOdulleri_Id_seq"', 8, true);


--
-- Name: Favoriler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Favoriler_Id_seq"', 1, false);


--
-- Name: HomePageSectionProducts_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."HomePageSectionProducts_Id_seq"', 1, false);


--
-- Name: HomePageSections_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."HomePageSections_Id_seq"', 3, true);


--
-- Name: IadeTalepleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."IadeTalepleri_Id_seq"', 1, true);


--
-- Name: IletisimMesajlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."IletisimMesajlari_Id_seq"', 1, false);


--
-- Name: KargoBolgeFiyatlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."KargoBolgeFiyatlari_Id_seq"', 20, true);


--
-- Name: KargoBolgeSehirler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."KargoBolgeSehirler_Id_seq"', 44, true);


--
-- Name: KargoBolgeler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."KargoBolgeler_Id_seq"', 10, true);


--
-- Name: KargoFirmalari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."KargoFirmalari_Id_seq"', 2, true);


--
-- Name: Kategoriler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Kategoriler_Id_seq"', 66, true);


--
-- Name: Kuponlar_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Kuponlar_Id_seq"', 1, false);


--
-- Name: KurumsalSayfalar_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."KurumsalSayfalar_Id_seq"', 1, false);


--
-- Name: PushAbonelikleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."PushAbonelikleri_Id_seq"', 1, false);


--
-- Name: SenTasarla_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."SenTasarla_Id_seq"', 1, false);


--
-- Name: SepetItems_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."SepetItems_Id_seq"', 11, true);


--
-- Name: Sepetler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Sepetler_Id_seq"', 59, true);


--
-- Name: SiparisDetaylari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."SiparisDetaylari_Id_seq"', 35, true);


--
-- Name: Siparisler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Siparisler_Id_seq"', 11, true);


--
-- Name: SiteAyarlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."SiteAyarlari_Id_seq"', 1, false);


--
-- Name: SiteDegerlendirmeleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."SiteDegerlendirmeleri_Id_seq"', 1, false);


--
-- Name: Slaytlar_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Slaytlar_Id_seq"', 14, true);


--
-- Name: ToptanciIskontoOranlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."ToptanciIskontoOranlari_Id_seq"', 7, true);


--
-- Name: ToptanciUrunGruplari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."ToptanciUrunGruplari_Id_seq"', 4, true);


--
-- Name: UrunOzellikDegerleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."UrunOzellikDegerleri_Id_seq"', 1, false);


--
-- Name: UrunOzellikTanimlari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."UrunOzellikTanimlari_Id_seq"', 27, true);


--
-- Name: UrunResimleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."UrunResimleri_Id_seq"', 30, true);


--
-- Name: UrunSecenekleri_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."UrunSecenekleri_Id_seq"', 43, true);


--
-- Name: Urunler_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Urunler_Id_seq"', 39, true);


--
-- Name: Yorumlar_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Yorumlar_Id_seq"', 1, false);


--
-- Name: ZiyaretciLoglari_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."ZiyaretciLoglari_Id_seq"', 1096, true);


--
-- Name: aggregatedcounter aggregatedcounter_key_key; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.aggregatedcounter
    ADD CONSTRAINT aggregatedcounter_key_key UNIQUE (key);


--
-- Name: aggregatedcounter aggregatedcounter_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.aggregatedcounter
    ADD CONSTRAINT aggregatedcounter_pkey PRIMARY KEY (id);


--
-- Name: counter counter_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.counter
    ADD CONSTRAINT counter_pkey PRIMARY KEY (id);


--
-- Name: hash hash_key_field_key; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.hash
    ADD CONSTRAINT hash_key_field_key UNIQUE (key, field);


--
-- Name: hash hash_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.hash
    ADD CONSTRAINT hash_pkey PRIMARY KEY (id);


--
-- Name: job job_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.job
    ADD CONSTRAINT job_pkey PRIMARY KEY (id);


--
-- Name: jobparameter jobparameter_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.jobparameter
    ADD CONSTRAINT jobparameter_pkey PRIMARY KEY (id);


--
-- Name: jobqueue jobqueue_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.jobqueue
    ADD CONSTRAINT jobqueue_pkey PRIMARY KEY (id);


--
-- Name: list list_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.list
    ADD CONSTRAINT list_pkey PRIMARY KEY (id);


--
-- Name: lock lock_resource_key; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.lock
    ADD CONSTRAINT lock_resource_key UNIQUE (resource);

ALTER TABLE ONLY hangfire.lock REPLICA IDENTITY USING INDEX lock_resource_key;


--
-- Name: schema schema_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.schema
    ADD CONSTRAINT schema_pkey PRIMARY KEY (version);


--
-- Name: server server_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.server
    ADD CONSTRAINT server_pkey PRIMARY KEY (id);


--
-- Name: set set_key_value_key; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.set
    ADD CONSTRAINT set_key_value_key UNIQUE (key, value);


--
-- Name: set set_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.set
    ADD CONSTRAINT set_pkey PRIMARY KEY (id);


--
-- Name: state state_pkey; Type: CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.state
    ADD CONSTRAINT state_pkey PRIMARY KEY (id);


--
-- Name: Adresler PK_Adresler; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Adresler"
    ADD CONSTRAINT "PK_Adresler" PRIMARY KEY ("Id");


--
-- Name: AspNetRoleClaims PK_AspNetRoleClaims; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetRoleClaims"
    ADD CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id");


--
-- Name: AspNetRoles PK_AspNetRoles; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetRoles"
    ADD CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id");


--
-- Name: AspNetUserClaims PK_AspNetUserClaims; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUserClaims"
    ADD CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id");


--
-- Name: AspNetUserLogins PK_AspNetUserLogins; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUserLogins"
    ADD CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey");


--
-- Name: AspNetUserRoles PK_AspNetUserRoles; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId");


--
-- Name: AspNetUserTokens PK_AspNetUserTokens; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUserTokens"
    ADD CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name");


--
-- Name: AspNetUsers PK_AspNetUsers; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUsers"
    ADD CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id");


--
-- Name: BankaHesaplari PK_BankaHesaplari; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."BankaHesaplari"
    ADD CONSTRAINT "PK_BankaHesaplari" PRIMARY KEY ("Id");


--
-- Name: BultenAbonelikleri PK_BultenAbonelikleri; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."BultenAbonelikleri"
    ADD CONSTRAINT "PK_BultenAbonelikleri" PRIMARY KEY ("Id");


--
-- Name: CarkOdulleri PK_CarkOdulleri; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."CarkOdulleri"
    ADD CONSTRAINT "PK_CarkOdulleri" PRIMARY KEY ("Id");


--
-- Name: Favoriler PK_Favoriler; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Favoriler"
    ADD CONSTRAINT "PK_Favoriler" PRIMARY KEY ("Id");


--
-- Name: HomePageSectionProducts PK_HomePageSectionProducts; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HomePageSectionProducts"
    ADD CONSTRAINT "PK_HomePageSectionProducts" PRIMARY KEY ("Id");


--
-- Name: HomePageSections PK_HomePageSections; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HomePageSections"
    ADD CONSTRAINT "PK_HomePageSections" PRIMARY KEY ("Id");


--
-- Name: IadeTalepleri PK_IadeTalepleri; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."IadeTalepleri"
    ADD CONSTRAINT "PK_IadeTalepleri" PRIMARY KEY ("Id");


--
-- Name: IletisimMesajlari PK_IletisimMesajlari; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."IletisimMesajlari"
    ADD CONSTRAINT "PK_IletisimMesajlari" PRIMARY KEY ("Id");


--
-- Name: KargoBolgeFiyatlari PK_KargoBolgeFiyatlari; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."KargoBolgeFiyatlari"
    ADD CONSTRAINT "PK_KargoBolgeFiyatlari" PRIMARY KEY ("Id");


--
-- Name: KargoBolgeSehirler PK_KargoBolgeSehirler; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."KargoBolgeSehirler"
    ADD CONSTRAINT "PK_KargoBolgeSehirler" PRIMARY KEY ("Id");


--
-- Name: KargoBolgeler PK_KargoBolgeler; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."KargoBolgeler"
    ADD CONSTRAINT "PK_KargoBolgeler" PRIMARY KEY ("Id");


--
-- Name: KargoFirmalari PK_KargoFirmalari; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."KargoFirmalari"
    ADD CONSTRAINT "PK_KargoFirmalari" PRIMARY KEY ("Id");


--
-- Name: Kategoriler PK_Kategoriler; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Kategoriler"
    ADD CONSTRAINT "PK_Kategoriler" PRIMARY KEY ("Id");


--
-- Name: Kuponlar PK_Kuponlar; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Kuponlar"
    ADD CONSTRAINT "PK_Kuponlar" PRIMARY KEY ("Id");


--
-- Name: KurumsalSayfalar PK_KurumsalSayfalar; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."KurumsalSayfalar"
    ADD CONSTRAINT "PK_KurumsalSayfalar" PRIMARY KEY ("Id");


--
-- Name: PushAbonelikleri PK_PushAbonelikleri; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PushAbonelikleri"
    ADD CONSTRAINT "PK_PushAbonelikleri" PRIMARY KEY ("Id");


--
-- Name: SenTasarla PK_SenTasarla; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SenTasarla"
    ADD CONSTRAINT "PK_SenTasarla" PRIMARY KEY ("Id");


--
-- Name: SepetItems PK_SepetItems; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SepetItems"
    ADD CONSTRAINT "PK_SepetItems" PRIMARY KEY ("Id");


--
-- Name: Sepetler PK_Sepetler; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Sepetler"
    ADD CONSTRAINT "PK_Sepetler" PRIMARY KEY ("Id");


--
-- Name: SiparisDetaylari PK_SiparisDetaylari; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SiparisDetaylari"
    ADD CONSTRAINT "PK_SiparisDetaylari" PRIMARY KEY ("Id");


--
-- Name: Siparisler PK_Siparisler; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Siparisler"
    ADD CONSTRAINT "PK_Siparisler" PRIMARY KEY ("Id");


--
-- Name: SiteAyarlari PK_SiteAyarlari; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SiteAyarlari"
    ADD CONSTRAINT "PK_SiteAyarlari" PRIMARY KEY ("Id");


--
-- Name: SiteDegerlendirmeleri PK_SiteDegerlendirmeleri; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SiteDegerlendirmeleri"
    ADD CONSTRAINT "PK_SiteDegerlendirmeleri" PRIMARY KEY ("Id");


--
-- Name: Slaytlar PK_Slaytlar; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Slaytlar"
    ADD CONSTRAINT "PK_Slaytlar" PRIMARY KEY ("Id");


--
-- Name: UrunOzellikDegerleri PK_UrunOzellikDegerleri; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UrunOzellikDegerleri"
    ADD CONSTRAINT "PK_UrunOzellikDegerleri" PRIMARY KEY ("Id");


--
-- Name: UrunOzellikTanimlari PK_UrunOzellikTanimlari; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UrunOzellikTanimlari"
    ADD CONSTRAINT "PK_UrunOzellikTanimlari" PRIMARY KEY ("Id");


--
-- Name: UrunResimleri PK_UrunResimleri; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UrunResimleri"
    ADD CONSTRAINT "PK_UrunResimleri" PRIMARY KEY ("Id");


--
-- Name: UrunSecenekleri PK_UrunSecenekleri; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UrunSecenekleri"
    ADD CONSTRAINT "PK_UrunSecenekleri" PRIMARY KEY ("Id");


--
-- Name: Urunler PK_Urunler; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Urunler"
    ADD CONSTRAINT "PK_Urunler" PRIMARY KEY ("Id");


--
-- Name: Yorumlar PK_Yorumlar; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Yorumlar"
    ADD CONSTRAINT "PK_Yorumlar" PRIMARY KEY ("Id");


--
-- Name: ZiyaretciLoglari PK_ZiyaretciLoglari; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ZiyaretciLoglari"
    ADD CONSTRAINT "PK_ZiyaretciLoglari" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: ToptanciIskontoOranlari ToptanciIskontoOranlari_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ToptanciIskontoOranlari"
    ADD CONSTRAINT "ToptanciIskontoOranlari_pkey" PRIMARY KEY ("Id");


--
-- Name: ToptanciUrunGruplari ToptanciUrunGruplari_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ToptanciUrunGruplari"
    ADD CONSTRAINT "ToptanciUrunGruplari_pkey" PRIMARY KEY ("Id");


--
-- Name: ix_hangfire_counter_expireat; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_counter_expireat ON hangfire.counter USING btree (expireat);


--
-- Name: ix_hangfire_counter_key; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_counter_key ON hangfire.counter USING btree (key);


--
-- Name: ix_hangfire_hash_expireat; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_hash_expireat ON hangfire.hash USING btree (expireat);


--
-- Name: ix_hangfire_job_expireat; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_job_expireat ON hangfire.job USING btree (expireat);


--
-- Name: ix_hangfire_job_statename; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_job_statename ON hangfire.job USING btree (statename);


--
-- Name: ix_hangfire_jobparameter_jobidandname; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_jobparameter_jobidandname ON hangfire.jobparameter USING btree (jobid, name);


--
-- Name: ix_hangfire_jobqueue_fetchedat_queue_jobid; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_jobqueue_fetchedat_queue_jobid ON hangfire.jobqueue USING btree (fetchedat NULLS FIRST, queue, jobid);


--
-- Name: ix_hangfire_jobqueue_jobidandqueue; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_jobqueue_jobidandqueue ON hangfire.jobqueue USING btree (jobid, queue);


--
-- Name: ix_hangfire_jobqueue_queueandfetchedat; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_jobqueue_queueandfetchedat ON hangfire.jobqueue USING btree (queue, fetchedat);


--
-- Name: ix_hangfire_list_expireat; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_list_expireat ON hangfire.list USING btree (expireat);


--
-- Name: ix_hangfire_set_expireat; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_set_expireat ON hangfire.set USING btree (expireat);


--
-- Name: ix_hangfire_set_key_score; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_set_key_score ON hangfire.set USING btree (key, score);


--
-- Name: ix_hangfire_state_jobid; Type: INDEX; Schema: hangfire; Owner: -
--

CREATE INDEX ix_hangfire_state_jobid ON hangfire.state USING btree (jobid);


--
-- Name: EmailIndex; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "EmailIndex" ON public."AspNetUsers" USING btree ("NormalizedEmail");


--
-- Name: IX_Adresler_AppUserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Adresler_AppUserId" ON public."Adresler" USING btree ("AppUserId");


--
-- Name: IX_AspNetRoleClaims_RoleId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON public."AspNetRoleClaims" USING btree ("RoleId");


--
-- Name: IX_AspNetUserClaims_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AspNetUserClaims_UserId" ON public."AspNetUserClaims" USING btree ("UserId");


--
-- Name: IX_AspNetUserLogins_UserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AspNetUserLogins_UserId" ON public."AspNetUserLogins" USING btree ("UserId");


--
-- Name: IX_AspNetUserRoles_RoleId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON public."AspNetUserRoles" USING btree ("RoleId");


--
-- Name: IX_Favoriler_AppUserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Favoriler_AppUserId" ON public."Favoriler" USING btree ("AppUserId");


--
-- Name: IX_Favoriler_UrunId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Favoriler_UrunId" ON public."Favoriler" USING btree ("UrunId");


--
-- Name: IX_HomePageSectionProducts_SectionId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_HomePageSectionProducts_SectionId" ON public."HomePageSectionProducts" USING btree ("SectionId");


--
-- Name: IX_HomePageSectionProducts_UrunId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_HomePageSectionProducts_UrunId" ON public."HomePageSectionProducts" USING btree ("UrunId");


--
-- Name: IX_IadeTalepleri_SiparisId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_IadeTalepleri_SiparisId" ON public."IadeTalepleri" USING btree ("SiparisId");


--
-- Name: IX_KargoBolgeFiyatlari_BolgeId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_KargoBolgeFiyatlari_BolgeId" ON public."KargoBolgeFiyatlari" USING btree ("BolgeId");


--
-- Name: IX_KargoBolgeFiyatlari_KargoFirmasiId_BolgeId; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_KargoBolgeFiyatlari_KargoFirmasiId_BolgeId" ON public."KargoBolgeFiyatlari" USING btree ("KargoFirmasiId", "BolgeId");


--
-- Name: IX_KargoBolgeSehirler_BolgeId_SehirAdi; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_KargoBolgeSehirler_BolgeId_SehirAdi" ON public."KargoBolgeSehirler" USING btree ("BolgeId", "SehirAdi");


--
-- Name: IX_KargoFirmalari_Kod; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_KargoFirmalari_Kod" ON public."KargoFirmalari" USING btree ("Kod");


--
-- Name: IX_Kategoriler_ParentKategoriId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Kategoriler_ParentKategoriId" ON public."Kategoriler" USING btree ("ParentKategoriId");


--
-- Name: IX_Kategoriler_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Kategoriler_Slug" ON public."Kategoriler" USING btree ("Slug") WHERE (("Slug" IS NOT NULL) AND ("Slug" <> ''::text));


--
-- Name: IX_PushAbonelikleri_AppUserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_PushAbonelikleri_AppUserId" ON public."PushAbonelikleri" USING btree ("AppUserId");


--
-- Name: IX_PushAbonelikleri_Token; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_PushAbonelikleri_Token" ON public."PushAbonelikleri" USING btree ("Token");


--
-- Name: IX_SepetItems_SepetId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_SepetItems_SepetId" ON public."SepetItems" USING btree ("SepetId");


--
-- Name: IX_SepetItems_UrunId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_SepetItems_UrunId" ON public."SepetItems" USING btree ("UrunId");


--
-- Name: IX_SepetItems_UrunSecenekId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_SepetItems_UrunSecenekId" ON public."SepetItems" USING btree ("UrunSecenekId");


--
-- Name: IX_Sepetler_AppUserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Sepetler_AppUserId" ON public."Sepetler" USING btree ("AppUserId");


--
-- Name: IX_SiparisDetaylari_SiparisId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_SiparisDetaylari_SiparisId" ON public."SiparisDetaylari" USING btree ("SiparisId");


--
-- Name: IX_SiparisDetaylari_UrunId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_SiparisDetaylari_UrunId" ON public."SiparisDetaylari" USING btree ("UrunId");


--
-- Name: IX_SiparisDetaylari_UrunSecenekId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_SiparisDetaylari_UrunSecenekId" ON public."SiparisDetaylari" USING btree ("UrunSecenekId");


--
-- Name: IX_Siparisler_AppUserId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Siparisler_AppUserId" ON public."Siparisler" USING btree ("AppUserId");


--
-- Name: IX_ToptanciIskontoOranlari_ToptanciUrunGrubuId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ToptanciIskontoOranlari_ToptanciUrunGrubuId" ON public."ToptanciIskontoOranlari" USING btree ("ToptanciUrunGrubuId");


--
-- Name: IX_ToptanciUrunGruplari_AktifMi; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_ToptanciUrunGruplari_AktifMi" ON public."ToptanciUrunGruplari" USING btree ("AktifMi");


--
-- Name: IX_UrunOzellikDegerleri_UrunId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_UrunOzellikDegerleri_UrunId" ON public."UrunOzellikDegerleri" USING btree ("UrunId");


--
-- Name: IX_UrunOzellikDegerleri_UrunOzellikTanimiId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_UrunOzellikDegerleri_UrunOzellikTanimiId" ON public."UrunOzellikDegerleri" USING btree ("UrunOzellikTanimiId");


--
-- Name: IX_UrunOzellikTanimlari_UrunTipi_Kod; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "IX_UrunOzellikTanimlari_UrunTipi_Kod" ON public."UrunOzellikTanimlari" USING btree ("UrunTipi", "Kod");


--
-- Name: IX_UrunResimleri_UrunId_Sira; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_UrunResimleri_UrunId_Sira" ON public."UrunResimleri" USING btree ("UrunId", "Sira");


--
-- Name: IX_UrunSecenekleri_UrunId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_UrunSecenekleri_UrunId" ON public."UrunSecenekleri" USING btree ("UrunId");


--
-- Name: IX_Urunler_KategoriId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Urunler_KategoriId" ON public."Urunler" USING btree ("KategoriId");


--
-- Name: IX_Urunler_SKU; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Urunler_SKU" ON public."Urunler" USING btree ("SKU");


--
-- Name: IX_Urunler_Slug; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Urunler_Slug" ON public."Urunler" USING btree ("Slug") WHERE (("Slug" IS NOT NULL) AND ("Slug" <> ''::text));


--
-- Name: IX_Urunler_ToptanciUrunGrubuId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Urunler_ToptanciUrunGrubuId" ON public."Urunler" USING btree ("ToptanciUrunGrubuId");


--
-- Name: IX_Yorumlar_UrunId; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX "IX_Yorumlar_UrunId" ON public."Yorumlar" USING btree ("UrunId");


--
-- Name: RoleNameIndex; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "RoleNameIndex" ON public."AspNetRoles" USING btree ("NormalizedName");


--
-- Name: UserNameIndex; Type: INDEX; Schema: public; Owner: -
--

CREATE UNIQUE INDEX "UserNameIndex" ON public."AspNetUsers" USING btree ("NormalizedUserName");


--
-- Name: jobparameter jobparameter_jobid_fkey; Type: FK CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.jobparameter
    ADD CONSTRAINT jobparameter_jobid_fkey FOREIGN KEY (jobid) REFERENCES hangfire.job(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: state state_jobid_fkey; Type: FK CONSTRAINT; Schema: hangfire; Owner: -
--

ALTER TABLE ONLY hangfire.state
    ADD CONSTRAINT state_jobid_fkey FOREIGN KEY (jobid) REFERENCES hangfire.job(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: Adresler FK_Adresler_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Adresler"
    ADD CONSTRAINT "FK_Adresler_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetRoleClaims FK_AspNetRoleClaims_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetRoleClaims"
    ADD CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserClaims FK_AspNetUserClaims_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUserClaims"
    ADD CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserLogins FK_AspNetUserLogins_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUserLogins"
    ADD CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetRoles_RoleId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserRoles FK_AspNetUserRoles_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: AspNetUserTokens FK_AspNetUserTokens_AspNetUsers_UserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."AspNetUserTokens"
    ADD CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: Favoriler FK_Favoriler_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Favoriler"
    ADD CONSTRAINT "FK_Favoriler_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: Favoriler FK_Favoriler_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Favoriler"
    ADD CONSTRAINT "FK_Favoriler_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: HomePageSectionProducts FK_HomePageSectionProducts_HomePageSections_SectionId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HomePageSectionProducts"
    ADD CONSTRAINT "FK_HomePageSectionProducts_HomePageSections_SectionId" FOREIGN KEY ("SectionId") REFERENCES public."HomePageSections"("Id") ON DELETE CASCADE;


--
-- Name: HomePageSectionProducts FK_HomePageSectionProducts_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."HomePageSectionProducts"
    ADD CONSTRAINT "FK_HomePageSectionProducts_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE RESTRICT;


--
-- Name: IadeTalepleri FK_IadeTalepleri_Siparisler_SiparisId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."IadeTalepleri"
    ADD CONSTRAINT "FK_IadeTalepleri_Siparisler_SiparisId" FOREIGN KEY ("SiparisId") REFERENCES public."Siparisler"("Id") ON DELETE CASCADE;


--
-- Name: KargoBolgeFiyatlari FK_KargoBolgeFiyatlari_KargoBolgeler_BolgeId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."KargoBolgeFiyatlari"
    ADD CONSTRAINT "FK_KargoBolgeFiyatlari_KargoBolgeler_BolgeId" FOREIGN KEY ("BolgeId") REFERENCES public."KargoBolgeler"("Id") ON DELETE CASCADE;


--
-- Name: KargoBolgeFiyatlari FK_KargoBolgeFiyatlari_KargoFirmalari_KargoFirmasiId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."KargoBolgeFiyatlari"
    ADD CONSTRAINT "FK_KargoBolgeFiyatlari_KargoFirmalari_KargoFirmasiId" FOREIGN KEY ("KargoFirmasiId") REFERENCES public."KargoFirmalari"("Id") ON DELETE CASCADE;


--
-- Name: KargoBolgeSehirler FK_KargoBolgeSehirler_KargoBolgeler_BolgeId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."KargoBolgeSehirler"
    ADD CONSTRAINT "FK_KargoBolgeSehirler_KargoBolgeler_BolgeId" FOREIGN KEY ("BolgeId") REFERENCES public."KargoBolgeler"("Id") ON DELETE CASCADE;


--
-- Name: Kategoriler FK_Kategoriler_Kategoriler_ParentKategoriId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Kategoriler"
    ADD CONSTRAINT "FK_Kategoriler_Kategoriler_ParentKategoriId" FOREIGN KEY ("ParentKategoriId") REFERENCES public."Kategoriler"("Id") ON DELETE RESTRICT;


--
-- Name: PushAbonelikleri FK_PushAbonelikleri_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."PushAbonelikleri"
    ADD CONSTRAINT "FK_PushAbonelikleri_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;


--
-- Name: SepetItems FK_SepetItems_Sepetler_SepetId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SepetItems"
    ADD CONSTRAINT "FK_SepetItems_Sepetler_SepetId" FOREIGN KEY ("SepetId") REFERENCES public."Sepetler"("Id") ON DELETE CASCADE;


--
-- Name: SepetItems FK_SepetItems_UrunSecenekleri_UrunSecenekId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SepetItems"
    ADD CONSTRAINT "FK_SepetItems_UrunSecenekleri_UrunSecenekId" FOREIGN KEY ("UrunSecenekId") REFERENCES public."UrunSecenekleri"("Id");


--
-- Name: SepetItems FK_SepetItems_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SepetItems"
    ADD CONSTRAINT "FK_SepetItems_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: Sepetler FK_Sepetler_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Sepetler"
    ADD CONSTRAINT "FK_Sepetler_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id");


--
-- Name: SiparisDetaylari FK_SiparisDetaylari_Siparisler_SiparisId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SiparisDetaylari"
    ADD CONSTRAINT "FK_SiparisDetaylari_Siparisler_SiparisId" FOREIGN KEY ("SiparisId") REFERENCES public."Siparisler"("Id") ON DELETE CASCADE;


--
-- Name: SiparisDetaylari FK_SiparisDetaylari_UrunSecenekleri_UrunSecenekId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SiparisDetaylari"
    ADD CONSTRAINT "FK_SiparisDetaylari_UrunSecenekleri_UrunSecenekId" FOREIGN KEY ("UrunSecenekId") REFERENCES public."UrunSecenekleri"("Id");


--
-- Name: SiparisDetaylari FK_SiparisDetaylari_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."SiparisDetaylari"
    ADD CONSTRAINT "FK_SiparisDetaylari_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: Siparisler FK_Siparisler_AspNetUsers_AppUserId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Siparisler"
    ADD CONSTRAINT "FK_Siparisler_AspNetUsers_AppUserId" FOREIGN KEY ("AppUserId") REFERENCES public."AspNetUsers"("Id");


--
-- Name: ToptanciIskontoOranlari FK_ToptanciIskontoOranlari_ToptanciUrunGruplari; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."ToptanciIskontoOranlari"
    ADD CONSTRAINT "FK_ToptanciIskontoOranlari_ToptanciUrunGruplari" FOREIGN KEY ("ToptanciUrunGrubuId") REFERENCES public."ToptanciUrunGruplari"("Id") ON DELETE CASCADE;


--
-- Name: UrunOzellikDegerleri FK_UrunOzellikDegerleri_UrunOzellikTanimlari_UrunOzellikTanimi~; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UrunOzellikDegerleri"
    ADD CONSTRAINT "FK_UrunOzellikDegerleri_UrunOzellikTanimlari_UrunOzellikTanimi~" FOREIGN KEY ("UrunOzellikTanimiId") REFERENCES public."UrunOzellikTanimlari"("Id") ON DELETE CASCADE;


--
-- Name: UrunOzellikDegerleri FK_UrunOzellikDegerleri_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UrunOzellikDegerleri"
    ADD CONSTRAINT "FK_UrunOzellikDegerleri_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: UrunResimleri FK_UrunResimleri_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UrunResimleri"
    ADD CONSTRAINT "FK_UrunResimleri_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: UrunSecenekleri FK_UrunSecenekleri_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."UrunSecenekleri"
    ADD CONSTRAINT "FK_UrunSecenekleri_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- Name: Urunler FK_Urunler_Kategoriler_KategoriId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Urunler"
    ADD CONSTRAINT "FK_Urunler_Kategoriler_KategoriId" FOREIGN KEY ("KategoriId") REFERENCES public."Kategoriler"("Id") ON DELETE CASCADE;


--
-- Name: Urunler FK_Urunler_ToptanciUrunGruplari; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Urunler"
    ADD CONSTRAINT "FK_Urunler_ToptanciUrunGruplari" FOREIGN KEY ("ToptanciUrunGrubuId") REFERENCES public."ToptanciUrunGruplari"("Id") ON DELETE SET NULL;


--
-- Name: Yorumlar FK_Yorumlar_Urunler_UrunId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Yorumlar"
    ADD CONSTRAINT "FK_Yorumlar_Urunler_UrunId" FOREIGN KEY ("UrunId") REFERENCES public."Urunler"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

\unrestrict 6zJlJNe9nKLSfogMJN3r6oEqel7EDYoWaUuXUv5qUb5J6Amj8NtsqzgUY9aTf2a

