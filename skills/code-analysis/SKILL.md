---
name: code-analysis
description: Analyze codebase structure. Use this BEFORE writing code to understand existing patterns.
---

# Code Analysis Skill

## 1. The "Map Before You Move" Rule
Before writing new code, you must map the relevant existing code.

## 2. Search Tools
*   **Find Usage:** `grep -r "{function_name}" .`
*   **Find Definition:** `grep -r "class {ClassName}" .`
*   **List Structure:** `python /skills/code-analysis/scripts/ast_map.py {filepath}`

## 3. Dependency Check
*   **Node:** `cat package.json`
*   **Python:** `cat requirements.txt`
*   **Constraint:** Do not import libraries that are not listed in these files.