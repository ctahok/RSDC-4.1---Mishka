---
name: file-operations
description: Create, read, update, and list files. Use this for all filesystem interactions.
---

# File Operations Skill

## 1. Reading Files
**Constraint:** Do not read files larger than 500 lines directly into context.
*   **Small Files:** Use `cat {filepath}`.
*   **Large Files:** Use `head -n 100 {filepath}` or `grep` to find specific sections.

## 2. Writing Files (Safe Write Protocol)
**Rule:** Never overwrite a file without reading it first unless it is a new file.
1.  **Check:** Does the file exist? (`ls {filepath}`)
2.  **Write:** Use the python script below to write content safely.

## 3. Tools (Scripts)
To ensure atomic writes, execute the following python script:
`python /skills/file-operations/scripts/write_safe.py --path "{filepath}" --content "{content}"`

## 4. Directory Management
*   **List:** `ls -R` (Recursive list) - *Use sparingly on large repos.*
*   **Tree:** `tree -L 2` (View structure depth 2).