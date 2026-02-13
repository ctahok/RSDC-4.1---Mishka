### Rule: Processing the Task List (The Skill-Aware Coder)

#### Core Execution Loop (The "LoopAgent")
For each sub-task:
1.  **Skill Scan:** Check `/skills/skill_registry.json` and `/artifacts/00_tech_stack_rules.md`.
    *   *If Found:* Invoke `bash: read /skills/{name}/SKILL.md` [23].
    *   *If Not:* Proceed to custom coding.
2.  **Trace Thought:** Append to `/artifacts/05_dev_decision_trace.md`.
    *   *Decision:* "Using existing skill `excel-parser` instead of writing pandas logic."
3.  **Atomic Implementation:**
    *   **Skill Path:** Execute `python /skills/{name}/scripts/{action}.py` [24].
    *   **Custom Path:** Write code in atomic steps of **Max 15 lines** [25].
4.  **Verification:** Execute the test/build command.
5.  **Saga Update:** Update `/artifacts/00_project_state.md`.

#### Skill Synthesis Protocol (If a Skill is Missing)
If a task requires a reusable capability (e.g., "Scrape Website") that doesn't exist:
1.  **Scaffold:** Create `/skills/web-scraper/`.
2.  **Define:** Write `SKILL.md` with Metadata (Name, Description) using `SKILL_TEMPLATE.md` [16].
3.  **Implement:** Save the logic as `scripts/scrape.py`.
4.  **Register:** Add entry to `/skills/skill_registry.json` for future agents.

#### Error Recovery (Localized Re-Reasoning)
If verification fails >2 times:
1.  **STOP**. Do not attempt a 3rd blind fix.
2.  **Escalate:** Create `/artifacts/debug_request_[task_id].md`.
3.  **Reflexion:** If the error was due to a tool misuse, update the Skill instructions in `SKILL.md` immediately [26].