# Høyytelses API for Travdata og Analyse

Dette er et lynraskt, strukturert API utviklet i .NET for å hente, mappe og tilrettelegge store mengder travdata. Systemet er designet for å fungere som det solide fundamentet for videre dataanalyse og maskinlæring.

Ved å fokusere på rene arkitekturmønstre og robuste databasestrukturer, henter API-et komplekse hierarkier på tvers av 16 SQL-tabeller på rundt **66 ms**, mens cachen serverer forespørsler på ekstremt lave **2–4 ms**.

---

## 🚀 Kom i gang lokalt

Applikasjonen er satt opp med en ferdig HTTP-profil som kjører på port `5007`.

1. Klon repositoriet.
2. Kjør applikasjonen fra rotmappen:
```bash
dotnet run --project API --launch-profile http

```


3. Åpne nettleseren på den lokale Scalar-dokumentasjonen for å teste endepunktene interaktivt:
   👉 **[http://localhost:5007/scalar/v1](http://localhost:5007/scalar/v1)**

---

## 🛠️ Arkitektur og Designfilosofi

### Generell og Språkuavhengig Kode

Dette prosjektet unngår komplekse, språkspesifikke snarveier eller obskure .NET-funksjoner. Koden er skrevet med fokus på generelle, anerkjente designmønstre som enkelt kan forstås og overføres til andre språk (som f.eks. Python, Kotlin eller Go). Dette gjør kodebasen ekstremt vedlikeholdsvennlig og ryddig.

### Kontroller-struktur

Kontrollerne er delt inn etter oppgave og kompleksitet for å holde ansvarsområdene rene:

* **`ReadSingleModelController`**: Grunnmuren for lesing av enkeltstående kildemodeller.
* **`ReadAllModelController`**: Brukes for enklere oppslagstabeller og mindre komplekse datasett.
* **`ReadSourceModelController`**: Håndterer kildeentiteter som hester (`Horse`), kusker (`Driver`) og trenere (`Trainer`). *Merk: Trener-data gjenbruker døråpningen via kusk-strukturen i stedet for å ha en overflødig, egen oppslagstabell.*
* **Spesialiserte endepunkter**: Komplekse modeller som krever dypere logikk (for eksempel `Competition`) arver fra basisklassen, men utvides med egne skreddersydde funksjoner.

### Fail-Fast på oppstart (DI-validering)

For å unngå kjedelige feil i produksjon, validerer applikasjonen hele avhengighetsgrafen (Dependency Injection) i det sekundet appen starter:

```csharp
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

```

Dersom en tjeneste eller et repository mangler registrering, krasjer applikasjonen umiddelbart under bygging i stedet for å feile under kjøring.

---

## ⚡ Ytelse og Caching

### Intelligent Lazy Loading

Cachen i systemet baserer seg utelukkende på en **lazy loading**-strategi.

* Første gang en ressurs etterspørres, hentes den fra databasen, mappes og lagres i minnet.
* Neste forespørsel henter dataen direkte fra minnet, noe som kutter responstiden ned til bare **2–4 ms**.
* Dette sparer databasen for unødvendig last, spesielt på de tyngste tabellene som inneholder millioner av rader (f.eks. `Race`, `Participant` og `Result`).

---

## 📊 Prosjektverktøy og Veien Videre

* **`dashboard.sh`**: Et eget CLI-skript i rotmappen som brukes aktivt i det daglige arbeidet for å holde oversikt over feil (bugtracking), gjøremål (TODOs) og den generelle progresjonen i prosjektet.
* **Dataset Builder (Under utvikling)**: Neste store steg i prosjektet er en dedikert modul som skal sy sammen rådataene fra API-et til ferdige, flate datasett tilpasset maskinlæringsmodeller i Python.
* **Dokumentasjon**: Vi bruker OpenAPI i kombinasjon med **Scalar** for et moderne, raskt og interaktivt API-grensesnitt, fremfor tunge, statiske dokumenter i Markdown.

---

## ⚙️ Teknologier og verktøy

* **Rammeverk:** .NET 10 / ASP.NET Core Web API.
* **Database-tilgang:** Dapper (Micro-ORM) for direkte, optimaliserte og rålesende SQL-spørringer uten overhead.
* **Database:** MySQL.
* **Caching:** Skreddersydd in-memory caching-lag (`CacheService<T>`) for lynraske oppslag.
* **Dokumentasjon og testing:** OpenAPI (Swagger/Scalar) for visuell utforskning, og integrerte `.http`-filer i IDE-en for raske, reproduserbare manuelle tester av API- og cache-tilstander.

---

> 🤖 **Om dokumentasjonen:** Utarbeidet i samarbeid med en AI-partner. Datastrukturen og arkitekturen er 100 % menneskeskapt.