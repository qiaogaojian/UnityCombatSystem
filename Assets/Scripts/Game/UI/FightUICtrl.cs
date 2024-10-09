using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUICtrl : MonoBehaviour
{
    private Text selfHpLabel = null;
    private Text enemyHpLabel = null;
    private Text buffTipLabel = null;

    public void Init() {
        var bt = this.transform.Find("SkillOptRoot/SkillA").GetComponent<Button>();
        bt.onClick.AddListener(this.OnSkillAClick);

        bt = this.transform.Find("SkillOptRoot/Buff1").GetComponent<Button>();
        bt.onClick.AddListener(this.OnBuff1Click);

        this.selfHpLabel = this.transform.Find("top/self/num").GetComponent<Text>();
        this.enemyHpLabel = this.transform.Find("top/enemy/num").GetComponent<Text>();
        this.buffTipLabel = this.transform.Find("top/SkillTip").GetComponent<Text>();

        EventMgr.Instance.AddListener((int)GM_Event.UI, this.OnUIEvent);
    }

    private void OnUIEvent(int eventName, object udata, object param = null)
    {
        switch ((int)udata)
        {
            case (int)UIEvent.SyncSelfHp:
                this.selfHpLabel.text = ((int)param).ToString();
                break;
            case (int)UIEvent.SyncEnemyHp:
                this.enemyHpLabel.text = ((int)param).ToString();
                break;
            case (int)UIEvent.BuffOpened:
                this.buffTipLabel.text = "Buff Opened";
                break;
            case (int)UIEvent.BuffFreezed:
                this.buffTipLabel.text = "Buff Freezed";
                break;
            case (int)UIEvent.BuffReady:
                this.buffTipLabel.text = "Buff Ready";
                break;
        }
    }

    private void OnSkillAClick()
    {
        EventMgr.Instance.Emit((int)GM_Event.UI, UIEvent.Skill, 1000001);
    }

    private void OnBuff1Click()
    {
        EventMgr.Instance.Emit((int)GM_Event.UI, UIEvent.Buff, 100001);
    }
}
