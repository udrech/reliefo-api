#!/bin/bash
set -e

# Get the directory where this script is located
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"
PARENT_DIR="$(dirname "$PROJECT_DIR")"

echo "Building reliefo-api..."
echo "Project directory: $PROJECT_DIR"

# Ensure we're on main branch and pull latest changes
cd "$PROJECT_DIR"
git checkout main
echo "Pulling latest changes..."
git pull

# Checkout or update reliefo-client if it doesn't exist
if [ ! -d "$PARENT_DIR/reliefo-client" ]; then
  echo "Cloning reliefo-client..."
  cd "$PARENT_DIR"
  git clone https://github.com/udrech/reliefo-client.git
fi

# Ensure we're on main branch and pull latest changes
cd "$PARENT_DIR/reliefo-client"
git checkout main
echo "Pulling latest changes for reliefo-client..."
git pull

# Build reliefo-client (Angular)
echo "Building reliefo-client (Angular)..."
npm ci
npx ng build --configuration production

# Copy client build into wwwroot
echo "Copying client build to wwwroot..."
cd "$PROJECT_DIR"
mkdir -p wwwroot
rm -rf wwwroot/*
cp -r ../reliefo-client/dist/reliefo-client/browser/* wwwroot/

# Build image with pack (Docker)
echo "Building Docker image with pack..."
sudo docker image rm reliefo:latest || true
sudo docker image rm ghcr.io/udrech/reliefo/reliefo:latest || true
sudo docker image prune -f
pack build reliefo:latest --builder paketobuildpacks/builder-jammy-base

echo "Build completed successfully!"
