using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FightData {
    public int hp;
    public int attack;
    public int defense;

    public void Init(ref UserInfoData udata) {
        this.hp = udata.maxHp;
        this.attack = 50;
        this.defense = 50;
    }
}