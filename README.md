# reliefo-api

Reliefo Therapy App API

## Ideen, Wünsche,ToDo

* Doku: Wartung, Angular Update, etc.
* CI/CD Pipeline für Container Image Erstellung mit GitHub Actions erstellen

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
dotnet format reliefo-api.csproj --exclude **/Migrations/** --verify-no-changes
```

## Container Image erstellen auf WSL

1. Client clonen + Build

```bash
git clone https://github.com/udrech/reliefo-client.git
cd reliefo-client
ng build --configuration production
```

1. API clonen + Client in wwwroot kopieren

```bash
git clone https://github.com/udrech/reliefo-api.git
cd reliefo-api
mkdir -p wwwroot
rm -rf ./wwwroot/*
cp -r ../reliefo-client/dist/reliefo-client/browser/* wwwroot/
```

1. Container Image erstellen mit Paketo Buildpack

```bash
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

## Container starten mit Docker Compose

```bash
sudo docker-compose up -d
curl http://localhost:8080/api/customers/
```

## Container stoppen mit Docker Compose

```bash
sudo docker-compose down
```

## Open Bash Shell in Container

```bash
sudo docker exec -it <container_id> bash
```

## Container Image in GitHub Container Registry pushen

```bash
echo YOUR_PAT | sudo docker login ghcr.io -u udrech --password-stdin
sudo docker tag reliefo ghcr.io/udrech/reliefo/reliefo:latest
sudo docker tag reliefo ghcr.io/udrech/reliefo/reliefo:1
sudo docker push ghcr.io/udrech/reliefo/reliefo:latest
sudo docker push ghcr.io/udrech/reliefo/reliefo:1
```

## Container Images von GitHub Container Registry auflisten

```bash
curl -H "Accept: application/vnd.github.v3+json" -H "Authorization: Bearer YOUR_PAT" https://api.github.com/orgs/udrech/packages/container/reliefo/versions
```

## Datenstruktur für carbone.io Studio

```json
{
  "appointments": [],
  "CreatedAt": "2024-06-01T12:00:00Z",
  "customer": {},
  "Number": 999,
  "TotalPrice": 999
}
```

## carbone.io Test mit curl

```bash
curl --location --request POST "https://api.carbone.io/render/1412795548263211167" \
     --header "carbone-version: 5" \
     --header "Content-Type: application/json" \
     --header "Authorization: Bearer ..." \
     --data @request.json
```

```bash
curl "https://api.carbone.io/render/MTAuMjAuMjEuNTIgICAgKGMtlbG4HoVY5y9KPZO49AcmVwb3J0.pdf" \
     --header "Authorization: Bearer ..." \
     --output bills/MTAuMjAuMjEuNTIgICAgKGMtlbG4HoVY5y9KPZO49AcmVwb3J0.pdf
```
