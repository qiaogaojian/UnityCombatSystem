using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// buf ���ϵ���;
public class FightCalcResult
{
    public float attack;
    public float defense;

    // �����������ԣ����Զ������; ...
    public float attackR;
}

// ���Բ�ĺ���;
public class GM_Charactor : MonoBehaviour
{
    public UserInfoData userData;
    public AnimData ch;
    public FightData fightData;

    public SkillTimeLine skillTimeLine;
    public BuffTimeLine buffTimeLine;

    private bool isSelf = false;

    public void Init(int charactorId, bool isSelf)
    {
        this.userData.Init(charactorId);
        this.ch.Init(this);
        this.fightData.Init(ref this.userData);
        this.skillTimeLine.Init();
        this.buffTimeLine.Init();

        this.isSelf = isSelf;
        this.SetState(CharactorState.Idle);

        // test,������Ŀ��ôͬ����������ģ�
        EventMgr.Instance.Emit((int)GM_Event.UI, this.isSelf ? UIEvent.SyncSelfHp : UIEvent.SyncEnemyHp, this.fightData.hp);
        // end
    }

    public void SetState(CharactorState state) {
        this.ch.SetState(state);
    }

    public void StartBuff(int buffId)
    {
        if (this.buffTimeLine.StartBuff(buffId))
        {
            // test, ֪ͨUI��Buff�����ˡ�
            EventMgr.Instance.Emit((int)GM_Event.UI, UIEvent.BuffOpened);
        }
    }

    public void StartSkill(int skillId)
    {
        if (this.skillTimeLine.StartSkill(this, skillId, () =>
        {
            this.ch.SetState(CharactorState.Idle);
        }))
        {
            this.ch.SetState(CharactorState.Attack);
        }

    }

    public void OnLoseHp(int loseHp)
    {

        this.fightData.hp -= loseHp;


        if (this.fightData.hp <= 0)
        {
            this.fightData.hp = 0;
            this.SetState(CharactorState.Died);
        }

        Debug.Log($"Last HP: {this.fightData.hp}");
        EventMgr.Instance.Emit((int)GM_Event.UI, this.isSelf ? UIEvent.SyncSelfHp : UIEvent.SyncEnemyHp, this.fightData.hp);
    }

    public void CalcFightBuff(string propName, FightCalcResult ret) {
        this.buffTimeLine.CalcAllBuffsWithProp(propName, ret);
    }

    public void Update() {
        this.skillTimeLine.OnUpdate(Time.deltaTime);
        this.buffTimeLine.OnUpdate(Time.deltaTime);
    }
}
