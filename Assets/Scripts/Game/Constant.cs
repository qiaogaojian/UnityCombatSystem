using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharactorState
{
    Invalid = -1,
    Idle,
    Run,
    Attack,
    Win,
    Died
}

public enum GM_Event
{
    UI = 0,
    NET,
    AI,
}

public enum UIEvent
{
    Skill = 0,
    Buff = 1,
    SyncEnemyHp = 2,
    SyncSelfHp = 3,
    BuffOpened,
    BuffFreezed,
    BuffReady,
}
