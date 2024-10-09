**Summary of the Skill System Code**

1. **Singleton Pattern Implementation**
    - The `GameApp`, `FightMgr`, and `GM_SkillMgr` classes use a static instance field to ensure a single instance throughout the game lifecycle, following the Singleton pattern.

2. **Game Initialization**
    - The `GameApp` class initializes core managers like `GM_EffectMgr`, `GM_SkillMgr`, and `GM_BuffMgr` in its `Init()` method. It also registers an event listener for UI interactions to process skill and buff events, redirecting them to `FightMgr` to handle actual gameplay.

3. **UI and Gameplay Scene Setup**
    - When the game starts, the `EnterGame` method in `GameApp` loads and displays UI elements and initializes the combat scene. It spawns player and enemy characters, assigning necessary components for gameplay.

4. **Fight Manager**
    - `FightMgr` handles the core battle mechanics. It manages player and enemy characters, spawns them on the map, and processes buffs and skills during the fight. `OnProcessSkill` and `OnProcessBuff` trigger corresponding actions (skills or buffs) on the player character.

5. **Character Management**
    - The `GM_Charactor` class manages character states, skills, and buffs. It handles health (`OnLoseHp`), skill execution (`StartSkill`), and buff application (`StartBuff`). It communicates with the event system to update UI when health changes or buffs are activated.

6. **Skill Timeline**
    - The `SkillTimeLine` struct manages skill execution over time, including delayed actions or sequences. It ensures only one skill can be active at a time and processes each skill's timed events until completion.

7. **Skill Manager**
    - `GM_SkillMgr` is responsible for loading and managing all skill configurations. It scans assemblies for skills and their associated methods (e.g., `Init`, `Calc`, `End`) and stores them for efficient retrieval during skill execution.

8. **Event-Driven Design**
    - The system heavily relies on an event-driven architecture. Skills, buffs, and UI updates are triggered via the `EventMgr`, enabling decoupled interaction between gameplay components.

**Code Example: Polymorphism in Skill Execution**

The `SkillTimeLine` executes different skills without knowing the specific skill type:

```csharp
public bool StartSkill(GM_Charactor sender, int skillId, Action OnComplete)
{
    List<SkillTimeNode> timeLine = GM_SkillMgr.Instance.GetSkillTimeNode(skillId);
    if (timeLine == null) return false;

    // Process the skill timeline irrespective of the actual skill type
    this.timelineNode = timeLine;
    this.isRunning = true;
    this.OnComplete = OnComplete;
    return true;
}
```

Here, `StartSkill` can execute any skill by retrieving its timeline, using polymorphism to process different skills with their unique logic defined elsewhere.