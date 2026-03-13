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
```

## Migrationen erstellen

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
