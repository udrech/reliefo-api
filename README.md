# reliefo-api

Reliefo Therapy App API

## ToDo

* Löschen mit vorgängiger Prüfung auf bestehende Verweise

## Nächste Schritte

* Container Image erstellen mit Paketo Buildpacks
* Image testen
* CI/CD Pipeline mit GitHub Actions erstellen
* Installation auf Server mit Docker Compose

## Begriffsdefinitionen

* **Appointment**: Termin, an dem die Therapie stattfindet.
* **Customer**: Kunde, der die Therapie in Anspruch nimmt.
* **MedicalHistoryRecord**: Medizinische Krankengeschichte Eintrag eines Kunden, der für die Therapie relevant sein könnte. Eintrags-Typen: Anamnese, Allergien, Medikamente, Vorerkrankungen, Verlauf, etc.
* **Bill**: Rechnung/Quittung, welche nach einer Therapie ausgestellt wird.
* **Therapy**: Therapie, welche an dem Termin stattfindet.

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

## API starten während der Entwicklung

```bash
dotnet watch
```

## Code formatieren mit StyleCop

```bash
dotnet add package StyleCop.Analyzers
dotnet format reliefo-api.csproj --verify-no-changes
```

## Container Image erstellen auf WSL

```bash
git clone https://github.com/udrech/reliefo-api.git
cd reliefo-api
sudo pack build reliefo --builder paketobuildpacks/builder-jammy-base
```

## Container Image testen mit Docker (ohne DB-Verbindung)

```bash
sudo docker run -d -p 8080:8080 -e PORT=8080 reliefo
curl http://localhost:8080/api/
```

## Container Image testen mit Docker (mit DB-Verbindung)

```bash
sudo docker run -d -p 8080:8080 -e PORT=8080 -e ConnectionStrings__DefaultConnection="Host=192.168.149.60;Database=reliefo;Username=reliefo;Password=your_secure_password" reliefo
curl http://localhost:8080/api/customers/
```

## Constainer Image in GitHub Container Registry pushen

```bash
echo YOUR_PAT | sudo docker login ghcr.io -u YOUR_GITHUB_USERNAME --password-stdin
sudo docker tag reliefo ghcr.io/udrech/reliefo:latest
sudo docker push ghcr.io/udrech/reliefo:latest
```
