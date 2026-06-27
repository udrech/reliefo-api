#!/bin/bash
set -e

VERSION="${1:-}"

if [ -z "$VERSION" ]; then
  echo "Error: Version is required"
  echo "Usage: $0 <version>"
  echo "Example: $0 3"
  exit 1
fi

echo "Publishing reliefo-api container image (version: $VERSION)..."

# Read GitHub Container Registry token
TOKEN_FILE="$HOME/.secrets/ghcr-pat.token"
if [ ! -f "$TOKEN_FILE" ]; then
  echo "Error: Token file not found at $TOKEN_FILE"
  exit 1
fi

TOKEN=$(cat "$TOKEN_FILE")

# Login to GitHub Container Registry
echo "Logging in to GitHub Container Registry..."
echo "$TOKEN" | sudo docker login ghcr.io -u udrech --password-stdin

# Tag image
echo "Tagging image..."
sudo docker tag reliefo:latest ghcr.io/udrech/reliefo/reliefo:latest

if [ ! -z "$VERSION" ]; then
  sudo docker tag reliefo:latest ghcr.io/udrech/reliefo/reliefo:$VERSION
  echo "Tagged with version: $VERSION"
fi

# Push image
echo "Pushing image..."
sudo docker push ghcr.io/udrech/reliefo/reliefo:latest

if [ ! -z "$VERSION" ]; then
  sudo docker push ghcr.io/udrech/reliefo/reliefo:$VERSION
  echo "Pushed version: $VERSION"
fi

echo "Publish completed successfully!"
