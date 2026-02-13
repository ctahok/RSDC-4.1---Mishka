---
name: git-automation
description: Manage version control. Use for saving state (Saga Checkpoints) and managing features.
---

# Git Automation Skill

## 1. Saga Checkpoint Protocol
After every successful "Wave" of tasks, you must save the state.
1.  **Stage:** `git add .`
2.  **Status:** `git status` (Verify what is being committed).
3.  **Commit:** `git commit -m "feat(wave): [Wave Description]"`

## 2. Recovery (Rollback)
If verification fails 3 times, trigger the Rollback:
`git reset --hard {clean_state_hash}`

## 3. Branching Strategy
*   **Feature Work:** Always create a branch: `git checkout -b feat/[task_id]`
*   **Merging:** Only merge to main after Stage 6 (Auditor) approval.