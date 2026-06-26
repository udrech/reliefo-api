# reliefo-api

Reliefo Therapy App API

## Ideen, Wünsche,ToDo

* Checks:
  * Termin nicht ändern, wenn für diesen bereits eine Quittung erstellt wurde.
  * Evtl. Therapie nur teilweise änderbar, wenn für diese bereits eine Quittung erstellt wurde.
  * Evtl. Kunde nur teilweise änderbar, wenn für diesen bereits eine Quittung erstellt wurde.
  * Prüfen ob Quittungsdatei vorhanden ist und andernfalls Quittung markieren.
* Doku: Wartung, .NET Update, etc.
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
sudo docker image rm reliefo:latest
sudo docker image prune
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
sudo docker tag reliefo:latest ghcr.io/udrech/reliefo/reliefo:latest
sudo docker tag reliefo:latest ghcr.io/udrech/reliefo/reliefo:2
sudo docker push ghcr.io/udrech/reliefo/reliefo:latest
sudo docker push ghcr.io/udrech/reliefo/reliefo:2
```

## Self-hosted GitHub Actions Runner installieren

Die Pipeline `.github/workflows/build-and-publish.yml` läuft auf einem selbst gehosteten Runner (z.B. WSL), da dort Docker und `pack` bereits verfügbar sind.

1. Auf GitHub: **Settings → Actions → Runners → New self-hosted runner** öffnen und der Anleitung folgen (Download + Konfiguration mit dem angezeigten Token), z.B.:

```bash
cd ~
mkdir actions-runner && cd actions-runner
curl -o actions-runner-linux-x64-2.335.1.tar.gz -L https://github.com/actions/runner/releases/download/v2.335.1/actions-runner-linux-x64-2.335.1.tar.gz
tar xzf ./actions-runner-linux-x64-2.335.1.tar.gz
./config.sh --url https://github.com/udrech/reliefo-api --token <TOKEN_AUS_GITHUB>
```

(Runner in deinem Home-Verzeichnis `~/actions-runner` erstellen, **nicht** im Projekt-Root.)

1. Voraussetzungen auf dem Runner installieren: `git`, `node`/`npm` + `@angular/cli` (für `ng build`), `docker`, [`pack` CLI](https://buildpacks.io/docs/tools/pack/) (Paketo Buildpacks).

1. GitHub Personal Access Token (PAT) mit `write:packages` Berechtigung lokal auf dem Runner ablegen (wird beim Push nach ghcr.io verwendet, nicht als GitHub Secret):

```bash
mkdir -p ~/.secrets
echo "<PAT>" > ~/.secrets/ghcr-pat.token
chmod 600 ~/.secrets/ghcr-pat.token
```

1. Runner als Dienst starten, damit er dauerhaft läuft:

```bash
sudo ./svc.sh install
sudo ./svc.sh start
```

## Container Image Pipeline manuell auslösen

Die Pipeline wird nur manuell gestartet (kein automatischer Trigger):

* Auf GitHub: **Actions → Build and Publish Container Image → Run workflow**, dabei die `version` angeben (z.B. `2`).
* Oder mit der GitHub CLI:

```bash
gh workflow run build-and-publish.yml -f version=2
```

## Container Images von GitHub Container Registry auflisten

<https://github.com/users/udrech/packages/container/package/reliefo%2Freliefo>

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
