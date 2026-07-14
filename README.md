# TrottingData API

Et lettvektig og høytytende .NET Web API bygd for å samle, strukturere og servere startlister og resultater fra travløp. Hovedmålet med API-et er å aggregere historiske data og sanntidsdata, optimalisert for videre analyse og maskinlæring.

> **Status:** 🛠️ Work In Progress (WIP). Modeller, endepunkter, caching-strategier og generering av datasett er under kontinuerlig utvikling.

---

## 🎯 Prosjektets formål

For å kunne forutsi resultater i travløp kreves det strukturerte, rene og tette datasett. Dette API-et fungerer som det sentrale datalageret og prosesseringslaget som klargjør og serverer dataene til analyseformål.

Ved å skille datatilgangen fra selve analysemodellene oppnår vi:

* **Maksimal ytelse:** Lynraske SQL-spørringer direkte mot databasen via Dapper (Micro-ORM).
* **Smart Caching:** Et skreddersydd in-memory caching-lag sikrer at statiske oppslagsdata serveres umiddelbart uten unødvendige databaserunder.
* **Skreddersydde datasett:** API-et leverer strukturer som er ferdig vasket og tilpasset funksjonsingeniørfag (feature engineering) uten unødvendig database-overhead.

---

## 🏗️ Arkitektur og datastrategi

Applikasjonen er bygget etter strenge prinsipper for lavt minneforbruk, høy gjenbrukbarhet og separation of concerns.

### 1. Modellstrategi (Simple vs. Complex)

For å holde overføringsstørrelser nede og forenkle JSON-serialisering, skiller vi strengt mellom to modelltyper:

* **Simple Models (`Models.Entities`):** Maper 1:1 mot tabellene i MySQL-databasen. Dette er flate datastrukturer som inneholder rå ID-er, fremmednøkler (Foreign Keys) og tidsstempler. Egenskapene følger kolonnerekkefølgen i SQL-skjemaet for optimal Dapper-mapping.
* **Complex Models (`Models.Complex`):** Skreddersydd for API-responser og konsumering. Relasjoner er løst opp i faktiske objekter eller lister, og databasens interne tidsstempler og rå fremmednøkler er rensket bort.

### 2. Generisk Repository og Caching-lag

For å unngå duplisering av kode på tvers av alle oppslagstabellene, brukes en generisk `RepositoryService<TSimple, TComplex>`. Denne koordinerer forespørsler mot database og cache:

```
[Klient] ──> [ModelController] ──> [RepositoryService] ──> [CacheService] (Sjekkes først)
                                            │
                                            └──> [IDbService] (Dapper / Database)

```

* Ved forespørsel etter entiteter sjekkes det dedikerte caching-laget (`CacheService<T>`) først.
* Hvis cachen er tom, hentes dataene via `IDbService` (Dapper), caches for ettertiden, og returneres til klienten.

---

## 🚦 API Endepunkter (Generisk mønster)

Alle enkle oppslagstabeller (lookups) arver fra den generiske `ModelController<TSimple, TComplex>`. Dette betyr at de eksponerer nøyaktig det samme, forutsigbare endepunktsmønsteret:

| Metode | Endepunkt | Beskrivelse | Cache-oppførsel |
| --- | --- | --- | --- |
| **GET** | `/model/{entity}/id` | Henter alle tilgjengelige ID-er (GUIDs). | **Ingen cache** (Direkte mot DB) |
| **GET** | `/model/{entity}/entity` | Henter alle enkle (`Simple`) entiteter. | **Caches** ved første forespørsel |
| **GET** | `/model/{entity}/entity/{id}` | Henter spesifikk enkel entitet via ID. | **Caches / Hentes fra cache** |
| **GET** | `/model/{entity}/complex` | Henter alle komplekse (`Complex`) entiteter. | **Caches** ved første forespørsel |
| **GET** | `/model/{entity}/complex/{id}` | Henter spesifikk kompleks entitet via ID. | **Caches / Hentes fra cache** |

### Støttede entiteter `{entity}` akkurat nå:

* `driverlicense` (Førerkort)
* `horsesex` (Hestekjønn)
* `horsetype` (Hestetyper)
* `racecarttype` (Vogntyper)
* `racecourse` (Travbaner)
* `racegamblingtype` (Spilltyper)
* `racestarttype` (Startmetoder)

---

## 📊 Databaseskjema (Kjernestruktur)

Datamodellen fanger opp hele økosystemet rundt et travstevne:

* **Kjernedata for løp:** `Competition` & `Race` (Stevner, løpsnummer og tidspunkter).
* **Aktører:** `Driver` & `Horse` (Utøverne, inkludert sporing av fars- og morsstamme for hestene).
* **Løpsdeltakelse:** `RaceParticipant` (Koblingen: Hvilken hest som går i hvilket spor, med hvilken kusk, trener og skokonfigurasjon).
* **Resultater:** `RaceResults` (Resultatlisten: Plassering, kilometertid, odds, premie og eventuelle diskvalifikasjons- eller slettemarkeringer).

---

## 🛠️ Teknologier og verktøy

* **Rammeverk:** .NET 10 / Web API
* **Database-tilgang:** Dapper (Micro-ORM)
* **Caching:** In-Memory Caching (`CacheService<T>`)
* **Database:** MySQL
* **Testing:** `.http`-filer integrert i IDE-en for rask og reproduserbar testing av endepunkter og cache-tilstander.