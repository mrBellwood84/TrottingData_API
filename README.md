# TrottingData API

Et lettvektig og høytytende .NET Web API bygd for å samle, strukturere og servere startlister og resultater fra travløp. Hovedmålet med API-et er å aggregere historiske data og sanntidsdata, slik at de er optimalisert for videre analyse og maskinlæring.

> **Status:** 🛠️ Work In Progress (WIP). Modeller, datatyper, endepunkter og generering av datasett er under kontinuerlig utvikling.

---

## 🎯 Prosjektets formål

For å kunne forutsi resultater i travløp kreves det strukturerte, rene og tette datasett. Dette API-et fungerer som det sentrale datalageret og prosesseringslaget som klargjør og serverer dataene til analyseformål.

Ved å skille datatilgangen fra selve analysemodellene oppnår vi:

* **Maksimal ytelse:** Bruk av Dapper (Micro-ORM) sikrer lynraske spørringer direkte mot databasen.
* **Skreddersydde datasett:** API-et leverer strukturer som er ferdig vasket og tilpasset funksjonsingeniørfag (feature engineering) uten unødvendig database-overhead.

---

## 🏗️ Arkitektur og modellstrategi

For å holde minneforbruket lavt og sørge for en optimal struktur for både relasjonell lagring og JSON-serialisering, brukes et strengt skille mellom to modelltyper:

### 1. Simple Models (`Models.Entities`)

* **Formål:** Mappe 1:1 mot tabellene i MySQL-databasen.
* **Design:** Flate datastrukturer som inneholder rå ID-er, fremmednøkler (Foreign Keys) og operasjonelle tidsstempler (`CreatedAt`, `UpdatedAt`).
* **Rekkefølge:** Egenskapene (properties) i klassene følger nøyaktig samme rekkefølge som kolonnene i SQL-skjemaet for å sikre skuddsikker mapping i Dapper.

### 2. Complex Models (`Models.Complex`)

* **Formål:** Nestede objekter skreddersydd for API-responser og ekstern konsumering.
* **Design:** Helt rensket for databasens interne tidsstempler og rå fremmednøkkel-strenger. Relasjoner er løst opp i faktiske objekter eller lister (f.eks. har `RaceComplex` en direkte `List<RaceGamblingTypeComplex>`).
* **Mange-til-mange:** Relasjoner som går via koblingstabeller (som spilltyper på et løp) presenteres som rene, flate lister direkte på hovedobjektet for enklere serialisering.

---

## 📊 Dataseskjema (Kjernestruktur)

Datamodellen fanger opp hele økosystemet rundt et travstevne:

* **Uavhengige registre:** Førerkort (DriverLicense), hestekjønn (HorseSex), hestetyper (HorseType), spilltyper (RaceGamblingType), vogntyper (RaceCartType), travbaner (RaceCourse) og startmetoder (RaceStartType).
* **Kjernedata for løp:** * `Competition` & `Race` (Stevner, løpsnummer og tidspunkter).
* `Driver` & `Horse` (Utøverne, inkludert sporing av fars- og morsstamme for hestene).
* `RaceParticipant` (Koplingen: Hvilken hest som går i hvilket spor, med hvilken kusk, trener og skokonfigurasjon).
* `RaceResults` (Resultatlisten: Plassering, kilometertid, odds, premie og eventuelle diskvalifikasjons- eller slettemarkeringer).



---

## 🛠️ Teknologier

* **Backend:** .NET Web API
* **Micro-ORM:** Dapper
* **Database:** MySQL / MariaDB
