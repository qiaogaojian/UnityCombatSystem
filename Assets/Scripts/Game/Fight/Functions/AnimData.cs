using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AnimData
{
    public CharactorState state;
    public Animation anim;

    public void Init(GM_Charactor self) {
        this.anim = self.GetComponent<Animation>();
        this.state = CharactorState.Invalid;
    }

    public void SetState(CharactorState state) {
        if (this.state == state) {
            return;
        }
        var str = state.ToString();
        str = str.ToLower();
        this.anim.CrossFade(str);
    }
}


