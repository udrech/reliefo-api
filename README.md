# reliefo-api

Reliefo Therapy App API

## ToDo

* Feld birthdate in Customer hinzufügen

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
rm -rf Migrations
dotnet ef migrations add InitialCreate

dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## DB zurücksetzen

```bash
sudo -u postgres psql -d reliefo -f /mnt/c/Users/ursdr/workspace/dotnet/reliefo-api/SQL/Drop\ Tables.sql
```

## Testdaten einfügen

```bash
sudo -u postgres psql -d reliefo -f /mnt/c/Users/ursdr/workspace/dotnet/reliefo-api/SQL/Testdata.sql
```

## API starten zum Testen

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
