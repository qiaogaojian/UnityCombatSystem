using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


// ÿ�����ñ�����ĵ㣬��Ӧ��һ��ִ�к���;
public class SkillTimePoint {
    public float exceTime; // ��Ӧ������ñ������ִ��ʱ�䣬ÿ�����ܶ�ֻ����һ��;
    public MethodInfo exceProc; // �����ִ�еĺ���; 

    public SkillTimePoint(float exceTime, MethodInfo exceProc) {
        this.exceTime = exceTime;
        this.exceProc = exceProc;
    }
}

public class SkillTimeNode {
    public float runTime; // �����ִ�е�ʱ���,  ���ñ�--->�ٷֱ� or �����ʱ��;
    public bool isExced; // �ǲ����Ѿ�ִ�й���;
    public object udata; // �û����ݵ����ݣ������Ŀǰû����;

    public SkillTimePoint timePoint; // �����ִ�е�; 

    public SkillTimeNode(SkillTimePoint pt) {
        this.timePoint = pt;
        this.runTime = timePoint.exceTime;
        this.isExced = false;
        this.udata = null;
    }

}

// ǰ�����: ÿ��ֻ�ܷ�һ������;  --->����һ���������ڷţ���û�н��������ٷ�һ�������ǷŲ�������;
// �����������������ͬʱ�Ŷ������,��ô����Բο�BuffTimeLine;

public struct SkillTimeLine {
    private List<SkillTimeNode> timeNodeList; // ����Time��ִ�ж���;
    public GM_Charactor sender;
    public int skillId;


    private bool isRunning; // ��ø�һ��State ö��,�ο�Buff�õ�ö��;
    private Action OnComplete; // ���ܽ���ʱ��Ļص�;

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
        // �������е�timeNode,�����ʱ�䣬һ��ִ��
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

        // �������Ƶ����ǵļ��ܵȴ�һ��ʱ����ܷ���һ����Ҳ���Ժ�BuffTimeLineһ����ר�Ÿ�����������ά�����м��ܵĿ����ȴ�ʱ��;
        if (endFlag) {  // �����ͷ���ϣ����Կ�ʼ��һ�������ͷ�;  ���ܵĵȴ�ʱ��,�ŵ����Բ�;
            this.isRunning = false;
            this.timeNodeList = null;
            if (this.OnComplete != null) {
                this.OnComplete();
            }
        }
    }
}


