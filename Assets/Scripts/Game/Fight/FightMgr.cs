using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMgr : MonoBehaviour
{
    public static FightMgr Instance = null;

    // ս����Ԫ�Ĺ���, 
    private GM_Charactor player = null;
    private GM_Charactor enemy = null;
    // end

    public void Init() {
        FightMgr.Instance = this;
    }

    public void LoadAndGotoMap(int mapId) {
        // ��ͼ�ʹ���������
        GameObject mapPrefab = ResMgr.Instance.LoadAssetSync<GameObject>($"Maps/Prefabs/{mapId}.prefab");
        var map = GameObject.Instantiate(mapPrefab);
        map.name = mapPrefab.name;
        map.transform.SetParent(this.transform, false);
        // end

        // ������Ϸ��ɫ self
        GameObject selfPrefab = ResMgr.Instance.LoadAssetSync<GameObject>($"Charactors/Prefabs/Self/20001.prefab");
        var self = GameObject.Instantiate(selfPrefab);
        self.name = "B_20001";
        self.transform.SetParent(this.transform, false);
        Vector3 pos = self.transform.position;
        pos.x -= 3;
        self.transform.position = pos;
        self.transform.LookAt(new Vector3(1, 0, 0));

        this.player = self.AddComponent<GM_Charactor>();
        this.player.Init(20001, true);
        // end

        // ������Ϸ��ɫ other
        GameObject enemyPrefab = ResMgr.Instance.LoadAssetSync<GameObject>($"Charactors/Prefabs/Enemy/20001.prefab");
        var enemy = GameObject.Instantiate(enemyPrefab);
        enemy.name = "R_20001";
        enemy.transform.SetParent(this.transform, false);
        pos = enemy.transform.position;
        pos.x += 3;enemy.transform.position = pos;
        enemy.transform.position = pos;
        enemy.transform.LookAt(new Vector3(-1, 0, 0));

        this.enemy = enemy.AddComponent<GM_Charactor>();
        this.enemy.Init(20001, false);
        // end

    }

    public void OnProcessBuff(int buffId/*, int uid*/) {
        this.player.StartBuff(buffId);
    }

    public void OnProcessSkill(int skillId/*, int uid*/)
    {
        this.player.StartSkill(skillId);
    }

    public GM_Charactor[] FindTargetsInArea(GM_Charactor center, float attackR)
    {
        // ���������Χ��buff,��ô������attackR;
        // end

        // test
        if (this.player == center)
        {
            return new GM_Charactor[] { this.enemy };
        }
        else
        {
            return new GM_Charactor[] { this.player };
        }
    }
}
