---
name: test-verification
description: Run tests and validate outputs. Use this to generate "Proof of Work" artifacts.
---

# Test Verification Skill

## 1. The "Proof of Work" Protocol
You cannot mark a task as complete without a verification log.

## 2. Test Execution
*   **Unit Tests:** `npm test` or `pytest`.
*   **UI Tests:** `npm run cy:run` (Cypress) or `python manage.py test`.

## 3. Visual Verification (For "Vibe" Checks)
If the task involves UI:
1.  Run the local server.
2.  Take a screenshot (if enabled) or curl the localhost endpoint.
3.  Log the output to `/artifacts/06_verification_logs.md`.

## 4. Failure Handling
*   **1st Fail:** Analyze error log -> Fix.
*   **2nd Fail:** Analyze error log -> Fix.
*   **3rd Fail:** STOP. Mark Task as FAILED in `00_project_state.md`.