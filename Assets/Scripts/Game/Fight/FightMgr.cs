using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMgr : MonoBehaviour
{
    public static FightMgr Instance = null;

    // 战斗单元的管理, 
    private GM_Charactor player = null;
    private GM_Charactor enemy = null;
    // end

    public void Init() {
        FightMgr.Instance = this;
    }

    public void LoadAndGotoMap(int mapId) {
        // 地图就创建出来了
        GameObject mapPrefab = ResMgr.Instance.LoadAssetSync<GameObject>($"Maps/Prefabs/{mapId}.prefab");
        var map = GameObject.Instantiate(mapPrefab);
        map.name = mapPrefab.name;
        map.transform.SetParent(this.transform, false);
        // end

        // 生成游戏角色 self
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

        // 生成游戏角色 other
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
        // 如果有扩大范围的buff,那么就扩大attackR;
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
