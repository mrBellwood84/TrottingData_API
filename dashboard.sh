#!/bin/bash
# Gjør scriptet automatisk kjørbart for fremtiden
chmod +x "$0" 2>/dev/null

clear

# Definer farger
NC='\033[0m' 
BOLD='\033[1m'
CYAN='\033[36m'
GREEN='\033[32m'
YELLOW='\033[33m'
RED='\033[31m'
MAGENTA='\033[35m'
BLUE='\033[34m'

echo -e "${CYAN}=======================================================${NC}"
echo -e "${BOLD}⚡  CACHING & REPOSITORY INFRASTRUCTURE DASHBOARD 🗄️${NC}"
echo -e "${CYAN}=======================================================${NC}"
echo ""

echo -e "${BLUE}${BOLD}🔹 LINES OF CODE (C# Source Only)${NC}"
cloc . --exclude-dir=bin,obj --include-lang="C#"
echo ""

echo -e "${BLUE}${BOLD}🔹 ARCHITECTURE & COMPLEXITY${NC}"
interfaces=$(find . -name "*.cs" ! -path "*/obj/*" ! -path "*/bin/*" | xargs grep -h "public interface" 2>/dev/null | wc -l)
classes=$(find . -name "*.cs" ! -path "*/obj/*" ! -path "*/bin/*" | xargs grep -h "public class" 2>/dev/null | wc -l)
async_tasks=$(find . -name "*.cs" ! -path "*/obj/*" ! -path "*/bin/*" | xargs grep -h "async Task" 2>/dev/null | wc -l)
async_valuetasks=$(find . -name "*.cs" ! -path "*/obj/*" ! -path "*/bin/*" | xargs grep -h "async ValueTask" 2>/dev/null | wc -l)

# Arkitektur-metrics for cacher, repositorier og db-tjenester
cache_std=$(find . -name "*CacheService.cs" ! -name "Sourced*" ! -path "*/obj/*" ! -path "*/bin/*" | wc -l)
cache_sourced=$(find . -name "SourcedCacheService.cs" -o -name "Sourced*CacheService.cs" ! -path "*/obj/*" ! -path "*/bin/*" | wc -l)
repo_services=$(find . -name "*RepositoryService.cs" ! -path "*/obj/*" ! -path "*/bin/*" | wc -l)
db_services=$(find . -name "*DbService.cs" ! -path "*/obj/*" ! -path "*/bin/*" | wc -l)

# Finner all teknisk gjeld (filtrerer ut vår egen PersistenceNotImplementedException)
todo_count=$(grep -ri "todo" --include="*.cs" . | wc -l)
not_impl_count=$(grep -ri "NotImplementedException" --include="*.cs" . | grep -vi "PersistenceNotImplementedException" | wc -l)
log_dev_count=$(grep -riE "ConsoleLogger.LogDev|AppLogger.Dev" --include="*.cs" . | wc -l)
unsafe_cast_count=$(grep -ri "as DbService" --include="*.cs" . | wc -l)

total_debt=$((todo_count + not_impl_count + log_dev_count + unsafe_cast_count))

echo -e "  • Interfaces (Contracts):          ${GREEN}${BOLD}$interfaces${NC}"
echo -e "  • Classes (Implementations):       ${GREEN}${BOLD}$classes${NC}"
echo -e "  • Abstraction Ratio:               ${MAGENTA}${BOLD}1 interface per $(( classes / (interfaces + 1) )) class(es)${NC}"
echo -e "  • Standard Caches (PrimaryKey):    ${CYAN}${BOLD}$cache_std engines${NC}"
echo -e "  • Sourced Caches (SourceId Twins): ${CYAN}${BOLD}$cache_sourced engines${NC}"
echo -e "  • Repository Services:             ${CYAN}${BOLD}$repo_services active repos${NC}"
echo -e "  • Database Drivers / Services:     ${CYAN}${BOLD}$db_services active drivers${NC}"
echo -e "  • Asynchronous Operations:         ${CYAN}${BOLD}$((async_tasks + async_valuetasks)) total${NC} ($async_tasks Task / $async_valuetasks ValueTask)"
echo ""

# Viser status for teknisk gjeld i oppsummeringen
if [ "$total_debt" -eq 0 ]; then
    echo -e "  • Technical Debt Markers:          ${GREEN}${BOLD}0 items - Code is pristine! 🎉${NC}"
else
    echo -e "  • Technical Debt Markers:          ${YELLOW}${BOLD}$total_debt items${NC} left in code"
fi
echo ""

# Vis detaljert liste over teknisk gjeld HVIS den eksisterer med FILNAVN:LINJE
if [ "$total_debt" -gt 0 ]; then
    echo -e "${RED}${BOLD}🚨 TECHNICAL DEBT RADAR (Action Required):${NC}"
    
    if [ "$todo_count" -gt 0 ]; then
        echo -e "  ${YELLOW}• Task Tags (todo):${NC}"
        grep -rn -i "todo" --include="*.cs" . | while read -r line; do
            file=$(echo "$line" | cut -d: -f1 | sed 's/\.\///')
            num=$(echo "$line" | cut -d: -f2)
            text=$(echo "$line" | cut -d: -f3- | xargs)
            printf "    \033[33mLine %-4s\033[0m %-50s \033[90m(%s)\033[0m\n" "$num" "$file" "${text:0:45}..."
        done
    fi

    if [ "$not_impl_count" -gt 0 ]; then
        echo -e "  ${RED}• NotImplementedExceptions:${NC}"
        grep -rn "NotImplementedException" --include="*.cs" . | grep -vi "PersistenceNotImplementedException" | while read -r line; do
            file=$(echo "$line" | cut -d: -f1 | sed 's/\.\///')
            num=$(echo "$line" | cut -d: -f2)
            printf "    \033[31mLine %-4s\033[0m %s\n" "$num" "$file"
        done
    fi
    
    if [ "$log_dev_count" -gt 0 ]; then
        echo -e "  ${BLUE}• AppLogger.Dev() / LogDev() left in code:${NC}"
        grep -rnE "ConsoleLogger.LogDev|AppLogger.Dev" --include="*.cs" . | while read -r line; do
            file=$(echo "$line" | cut -d: -f1 | sed 's/\.\///')
            num=$(echo "$line" | cut -d: -f2)
            printf "    \033[34mLine %-4s\033[0m %s\n" "$num" "$file"
        done
    fi

    if [ "$unsafe_cast_count" -gt 0 ]; then
        echo -e "  ${RED}• Unsafe Type Castings (as DbService):${NC}"
        grep -rn "as DbService" --include="*.cs" . | while read -r line; do
            file=$(echo "$line" | cut -d: -f1 | sed 's/\.\///')
            num=$(echo "$line" | cut -d: -f2)
            printf "    \033[31mLine %-4s\033[0m %s (Should use proper abstraction instead)\n" "$num" "$file"
        done
    fi
    echo ""
fi

echo -e "${YELLOW}${BOLD}🔥 MONOLITH RADAR (Top 3 Largest C# Files):${NC}"
find . -type f -name "*.cs" ! -path "*/obj/*" ! -path "*/bin/*" -exec wc -l {} + | sort -rn | grep -v "total" | head -n 3 | awk '{printf "    %-10s %s\n", $1 " lines", $2}' | sed "s/\.\///"
echo ""

echo -e "${BLUE}${BOLD}🔹 INLINE SQL & DATABASE STATE${NC}"
# Skanner etter rå SQL-setninger som ligger inni C#-kodestrenger
select_queries=$(grep -riE '"\s*SELECT ' --include="*.cs" . | wc -l)
insert_commands=$(grep -riE '"\s*INSERT ' --include="*.cs" . | wc -l)
update_commands=$(grep -riE '"\s*UPDATE ' --include="*.cs" . | wc -l)
delete_commands=$(grep -riE '"\s*DELETE ' --include="*.cs" . | wc -l)
total_sql_queries=$((select_queries + insert_commands + update_commands + delete_commands))

echo -e "  • Inline SELECT Queries (Read):    ${GREEN}${BOLD}$select_queries statements${NC}"
echo -e "  • Inline INSERT Commands (Write):  ${MAGENTA}${BOLD}$insert_commands statements${NC}"
echo -e "  • Inline UPDATE Commands (Write):  ${MAGENTA}${BOLD}$update_commands statements${NC}"
echo -e "  • Inline DELETE Commands (Write):  ${RED}${BOLD}$delete_commands statements${NC}"
echo -e "  • Total SQL Statements in C#:      ${CYAN}${BOLD}$total_sql_queries statements${NC}"
echo ""

echo -e "${BLUE}${BOLD}🔹 PROJECT STATE & MOMENTUM${NC}"
if git rev-parse --is-inside-work-tree >/dev/null 2>&1; then
    
    last_commit_date=$(git log -1 --format="%cd" --date=relative)
    last_commit_msg=$(git log -1 --format="%s")
    uncommitted_count=$(git status --porcelain | wc -l)
    
    echo -e "  • Last Save Point:                 ${MAGENTA}${BOLD}$last_commit_date${NC}"
    echo -e "  • Train of Thought (Last msg):     ${CYAN}${BOLD}\"$last_commit_msg\"${NC}"
    
    if [ "$uncommitted_count" -gt 0 ]; then
        echo -e "  • Work In Progress:                ${YELLOW}${BOLD}$uncommitted_count modified/untracked files${NC}"
        echo -e "    ${YELLOW}(Files you are currently working on:)${NC}"
        git status --short | head -n 5 | sed 's/^/      /'
    else
        echo -e "  • Work In Progress:                ${GREEN}${BOLD}Working tree clean (Ready to code!)${NC}"
    fi
else
    echo -e "  • ${RED}No Git repository detected in this directory.${NC}"
fi

echo ""
echo -e "${CYAN}=======================================================${NC}"