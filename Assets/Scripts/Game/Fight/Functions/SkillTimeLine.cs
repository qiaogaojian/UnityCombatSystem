using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


// 每个配置表里面的点，对应的一个执行函数;
public class SkillTimePoint {
    public float exceTime; // 对应表格配置表里面的执行时间，每个技能都只生成一次;
    public MethodInfo exceProc; // 具体的执行的函数; 

    public SkillTimePoint(float exceTime, MethodInfo exceProc) {
        this.exceTime = exceTime;
        this.exceProc = exceProc;
    }
}

public class SkillTimeNode {
    public float runTime; // 具体的执行的时间点,  配置表--->百分比 or 具体的时间;
    public bool isExced; // 是不是已经执行过了;
    public object udata; // 用户传递的数据，保留项，目前没有用;

    public SkillTimePoint timePoint; // 具体的执行点; 

    public SkillTimeNode(SkillTimePoint pt) {
        this.timePoint = pt;
        this.runTime = timePoint.exceTime;
        this.isExced = false;
        this.udata = null;
    }

}

// 前提假设: 每次只能放一个技能;  --->你有一个技能正在放，还没有结束，你再放一个技能是放不出来的;
// 如果你的需求可以允许同时放多个技能,那么你可以参考BuffTimeLine;

public struct SkillTimeLine {
    private List<SkillTimeNode> timeNodeList; // 技能Time点执行队列;
    public GM_Charactor sender;
    public int skillId;


    private bool isRunning; // 最好搞一个State 枚举,参考Buff用的枚举;
    private Action OnComplete; // 技能结束时候的回调;

    public void Init() {
        this.timeNodeList = null;
        this.isRunning = false;
        this.sender = null;
        this.skillId = 0;
    }

    public bool StartSkill(GM_Charactor sender, int skillId, Action OnComplete)
    {
        if (this.isRunning) {
            return false;
        }
        this.timeNodeList = null;

        List<SkillTimeNode> timeNodeList = GM_SkillMgr.Instance.GetSkillTimeNode(skillId);
        if (timeNodeList == null) {
            return false;
        }

        this.timeNodeList = timeNodeList;
        this.isRunning = true;
        this.OnComplete = OnComplete;

        this.skillId = skillId;
        this.sender = sender;

        return true;
    }

    public void OnUpdate(float dt) {
        if (this.isRunning == false) {
            return;
        }

        if (this.timeNodeList == null) {
            this.isRunning = false;
            return;
        }

        bool endFlag = true;
        // 遍历所有的timeNode,来检查时间，一次执行
        for (int i = 0; i < this.timeNodeList.Count; i++) {
            if (this.timeNodeList[i].isExced) {
                continue;
            }

            this.timeNodeList[i].runTime -= dt;
            if (this.timeNodeList[i].runTime <= 0) {
                this.timeNodeList[i].isExced = true;

                object[] paramData = new object[] { this.sender, this.skillId, this.timeNodeList[i].udata };
                this.timeNodeList[i].timePoint.exceProc.Invoke(null, paramData);
            }

            endFlag = false;
        }

        // 如果你设计到我们的技能等待一段时间才能放下一个，也可以和BuffTimeLine一样，专门搞个功能组件来维护所有技能的开启等待时间;
        if (endFlag) {  // 技能释放完毕，可以开始下一个技能释放;  技能的等待时间,放到策略层;
            this.isRunning = false;
            this.timeNodeList = null;
            if (this.OnComplete != null) {
                this.OnComplete();
            }
        }
    }
}


