# reliefo-api

Reliefo Therapy App API

## DB erstellen

```bash
sudo -u postgres psql
CREATE DATABASE reliefo;
\l
```

## Benutzer erstellen und für DB berechtigen

```bash
CREATE USER reliefo WITH PASSWORD 'your_secure_password';
GRANT ALL PRIVILEGES ON DATABASE reliefo TO reliefo;
ALTER DATABASE reliefo OWNER TO reliefo;
\c reliefo
GRANT ALL ON SCHEMA public TO reliefo;
```

## Migrationen erstellen

```bash
dotnet ef migrations add InitialCreate
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## API starten zum Testem

```bash
dotnet watch
```

## Begriffsdefinitionen

* **Appointment**: Termin, an dem die Therapie stattfindet.
* **Customer**: Kunde, der die Therapie in Anspruch nimmt.
* **MedicalHistory**: Medizinische Krankengeschichte eines Kunden, die für die Therapie relevant sein könnte. Eintrags-Typen: Anamnese, Allergien, Medikamente, Vorerkrankungen, etc.
* **Receipt**: Quittung, welche nach einer Therapie ausgestellt wird.
* **Therapy**: Therapie, welche an dem Termin stattfindet.

## Nächste Schritte

* Tabellen erstellen lassen und prüfen
* Refential Constraints hinzufügen
* Testdaten einfügen
* Tests mit Postman
