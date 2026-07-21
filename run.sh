#!/bin/bash
# Gjør scriptet automatisk kjørbart for fremtiden
chmod +x "$0" 2>/dev/null

clear

# Definer farger for herlig terminal-dashboard
NC='\033[0m' 
BOLD='\033[1m'
GREEN='\033[32m'
CYAN='\033[36m'
YELLOW='\033[33m'
MAGENTA='\033[35m'

echo -e "${CYAN}=======================================================${NC}"
echo -e "${BOLD}🌐 STARTING TROTTING DATA API IN RELEASE MODE 🏎️${NC}"
echo -e "${CYAN}=======================================================${NC}"

echo -e "${GREEN}• Prosjekt:${NC}      API"
echo -e "${GREEN}• Konfigurasjon:${NC} ${BOLD}Release${NC}"
echo -e "${GREEN}• Profil:${NC}        http"
echo ""
echo -e "${MAGENTA}⚡ Serveren gjøres klar til å levere lynraske datasett...${NC}"
echo ""

# Kjører WebAPI-et i Release-modus med http-profilen
dotnet run -c Release --project API --launch-profile http