### Rule: Recursive Agentic Architecture Orchestration (v4.1)

#### Core Primitive: The Handover Configuration
At the end of every stage, generate a `NextAgentConfig` object enforcing the contract [21].

#### Stage 1: The Architect (Requirements & Skill Gap Analysis)
*   **Input:** User Query + `/skills/skill_registry.json`.
*   **Behavior:**
    1.  **Analysis:** Analyze requirements (e.g., "Needs Database", "Needs PDF parsing").
    2.  **Gap Analysis:** Compare requirements against the Registry.
        *   *Match:* Assign `postgres-manager` skill to the Tech Stack.
        *   *Gap:* Define a **"Skill Synthesis Task"** to build the missing tool (e.g., `build-new-skill: vector-db-connector`) [5].
    3.  **Output:** `/artifacts/00_tech_stack_rules.md` (Updated with "USE Skill X" directives).

#### Stage 2: The Product Manager (Specs & Vibe)
*   **Input:** `/artifacts/00_tech_stack_rules.md`.
*   **Behavior:** Define "Vibe" (Visual Personality) and "Motion" specs [10].
*   **Output:** `/artifacts/02_pm_prd.md`.

#### Stage 4: The Engineering Manager (Wave Scheduling)
*   **Input:** `/artifacts/03_tl_gherkin_specs.feature`.
*   **Behavior:** Group tasks into **Waves** based on dependencies. Ensure "Skill Synthesis" tasks are scheduled in **Wave 0** (Pre-requisites) [11].
*   **Output:** `/artifacts/04_task_waves.md`.

#### Stage 5: The Developer (Skill Consumer)
*   **Input:** `/artifacts/04_task_waves.md` and `/skills/`.
*   **Behavior:**
    1.  **Check:** Does `/artifacts/00_tech_stack_rules.md` mandate a skill?
    2.  **Load:** If yes, read the specific `/skills/{name}/SKILL.md` (Progressive Disclosure) [22].
    3.  **Execute:** Run the skill's script (e.g., `python /skills/git-manager/scripts/commit.py`).
    4.  **Code:** Only write custom application code if no Skill applies.
*   **Output:** Codebase and `/artifacts/06_verification_logs.md`.