#!/usr/bin/env bash
# Checks markdown for GitBook-style broken links and missing internal targets.
# On failure, writes one issue per line to link-check-details.txt (file:line:content).
set -euo pipefail

ROOT="${1:-.}"
cd "$ROOT"

DETAILS_FILE="link-check-details.txt"
: > "$DETAILS_FILE"

log_detail() {
  echo "$1" >> "$DETAILS_FILE"
}

GREP_EXCLUDE=(--exclude-dir='.git' --exclude-dir='.github')

echo "==> Checking for known GitBook broken-link patterns..."

patterns=(
  '/broken/pages/'
  'broken-reference'
  '](broken-reference)'
)

for pattern in "${patterns[@]}"; do
  while IFS= read -r line; do
    [ -z "$line" ] && continue
    log_detail "$line"
  done < <(grep -RIn --include='*.md' "${GREP_EXCLUDE[@]}" -- "$pattern" . 2>/dev/null || true)
done

while IFS= read -r line; do
  [ -z "$line" ] && continue
  log_detail "$line"
done < <(grep -RIn --include='*.md' "${GREP_EXCLUDE[@]}" \
  -E '\[Broken link\]\([^)]+\)' . 2>/dev/null || true)

resolve_path() {
  local base_dir="$1"
  local target="$2"
  python3 - "$base_dir" "$target" <<'PY'
import os, sys
base, target = sys.argv[1], sys.argv[2]
print(os.path.normpath(os.path.join(os.path.abspath(base), target)))
PY
}

echo ""
echo "==> Checking internal markdown links point at existing files..."

while IFS= read -r -d '' file; do
  # Links resolve relative to the file, including each section's SUMMARY.md
  base_dir=$(dirname "$file")

  while IFS= read -r link; do
    [ -z "$link" ] && continue
    target="${link%%#*}"
    [ -z "$target" ] && continue
    [[ "$target" == http* ]] && continue
    [[ "$target" == mailto:* ]] && continue

    resolved=$(resolve_path "$base_dir" "$target")

    if [ ! -f "$resolved" ]; then
      # Find the line containing this target for Slack (file:line:content)
      match_line=$(grep -nF "](${target})" "$file" 2>/dev/null | head -1 || true)
      if [ -n "$match_line" ]; then
        line_num="${match_line%%:*}"
        line_content="${match_line#*:}"
        log_detail "./${file#./}:${line_num}:${line_content}"
      else
        log_detail "./${file#./}:?:missing file -> ${target}"
      fi
    fi
  done < <(grep -oE '\[[^]]*\]\([^)]+\)' "$file" | sed -E 's/^\[[^]]*\]\(([^)]+)\)$/\1/' | grep -E '\.md$' || true)
done < <(find . -name '*.md' -not -path './.git/*' -not -path './.github/*' -print0)

if [ -s "$DETAILS_FILE" ]; then
  count=$(wc -l < "$DETAILS_FILE" | tr -d ' ')
  echo ""
  echo "FAILED: ${count} broken link issue(s) found:"
  cat "$DETAILS_FILE"
  exit 1
fi

echo ""
echo "OK: No broken link patterns or missing internal .md targets found."
