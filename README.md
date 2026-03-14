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
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## API starten zum Testem

```bash
dotnet watch
```

## Nächste Schritte
* Tabellen erstellen lassen und prüfen
* Refential Constraints hinzufügen
* Tests mit Postman
