using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameApp : MonoBehaviour
{
    public static GameApp Instance = null;
    public void Init() {
        GameApp.Instance = this;
        new GM_EffectMgr().Init();
        new GM_SkillMgr().Init();
        new GM_BuffMgr().Init();

        EventMgr.Instance.AddListener((int)GM_Event.UI, this.OnUIEventProc);
    }

    private void OnUIEventProc(int eventType, object udata, object param = null)
    {
        switch ((int)udata)
        {
            case (int)UIEvent.Skill:
                FightMgr.Instance.OnProcessSkill((int)param);
                break;
            case (int)UIEvent.Buff:
                FightMgr.Instance.OnProcessBuff((int)param);
                break;
        }

    }

    public void EnterGame() {
        // 显示UI
        var canvas = GameObject.Find("Canvas");
        GameObject fightUiPrefab = ResMgr.Instance.LoadAssetSync<GameObject>("GUI/Prefabs/FightUI.prefab");
        var fightUi = GameObject.Instantiate(fightUiPrefab);
        fightUi.name = fightUiPrefab.name;
        fightUi.transform.SetParent(canvas.transform, false);
        fightUi.AddComponent<FightUICtrl>().Init();
        // end

        // 显示游戏战斗场景
        var gameObject = new GameObject();
        gameObject.name = "FightRoot";
        gameObject.AddComponent<FightMgr>().Init();
        FightMgr.Instance.LoadAndGotoMap(10001);
        // end
    }
}
