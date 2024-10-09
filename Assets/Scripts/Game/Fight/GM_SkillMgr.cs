using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ParseTimeLineRet {
    public string timeLineStr = null;
    public float SkillDuration = 0.0f;
}

public class GM_SkillMgr
{
    public static GM_SkillMgr Instance = null;

    // key  mainType + subKey;  
    private Dictionary<int, Dictionary<string, MethodInfo>> skillModelSet;
    private Dictionary<int, List<SkillTimePoint>> allSkillTimeLine = new Dictionary<int, List<SkillTimePoint>>();
    private List<Type> allSkillConfigType = new List<Type>();

    public void Init() {
        GM_SkillMgr.Instance = this;
        this.skillModelSet = new Dictionary<int, Dictionary<string, MethodInfo>>();
        this.ScanAllSkillModelAndConfig();
    }

    private void ScaneOneSkillModel(Type t, SkillModel skillModel) {
        MethodInfo[] funcs = t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        for (int j = 0; j < funcs.Length; j++) {
            SkillProcesser p = funcs[j].GetCustomAttribute<SkillProcesser>();
            if (p == null) {
                continue;
            }

            int key = skillModel.mainType + p.subType; // Ĭ�ϵĴ����key, -1, 1000000 + (-1) 0999999
            Dictionary<string, MethodInfo> processFuncs = null;
            if (!this.skillModelSet.ContainsKey(key))
            {
                processFuncs = new Dictionary<string, MethodInfo>();
                this.skillModelSet.Add(key, processFuncs);
            }
            else { // key --->Init(if), Begin(else), End(else)
                processFuncs = this.skillModelSet[key];
            }

            processFuncs.Add(p.funcName, funcs[j]);
        }
    }

    private void ScaneOneSkillConfig(Type t, SkillConfig skillConfig)
    {
        this.allSkillConfigType.Add(t);
    }

    private void ScanAllSkillModelAndConfig() {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            Type[] allTypes = assembly.GetTypes();
            for (int i = 0; i < allTypes.Length; i++)
            {
                Type t = allTypes[i];
                SkillModel skillModel = t.GetCustomAttribute<SkillModel>();
                if (skillModel != null)
                {
                    this.ScaneOneSkillModel(t, skillModel);
                }

                SkillConfig skillConfig = t.GetCustomAttribute<SkillConfig>();
                if (skillConfig != null)
                {
                    this.ScaneOneSkillConfig(t, skillConfig);
                }
            }
        }
    }

    private MethodInfo GetProcesserFunc(string funcName, Dictionary<string, MethodInfo> funMap, Dictionary<string, MethodInfo> defaultFunMap)
    {
        if (funMap.ContainsKey(funcName))
        {
            return funMap[funcName];
        }
        if (defaultFunMap.ContainsKey(funcName))
        {
            return defaultFunMap[funcName];
        }

        return null;
    }

    // ÿ��Skill�����Ӧһ�� TimePointList(excel);
    // ����ÿ�ζ�ȥ���������Ի�������;
    private List<SkillTimePoint> ParserTimeLine(int skillId) {
        if (this.allSkillTimeLine.ContainsKey(skillId)) {
            return this.allSkillTimeLine[skillId];
        }

        int mainType = (int)(skillId / 1000000);
        int subType = skillId % 1000000;


        int key = mainType * 1000000;
        key = key - 1; // Ĭ��key;

        // �ȴ�funcMap�ң�û���ҵ�������default����
        Dictionary<string, MethodInfo> funMap = null;
        Dictionary<string, MethodInfo> defaultFunMap = null;

        if (this.skillModelSet.ContainsKey(key)) {
            defaultFunMap = this.skillModelSet[key];
        }

        if (this.skillModelSet.ContainsKey(skillId)) {
            funMap = this.skillModelSet[skillId];
        }
        else {
            funMap = defaultFunMap;
        }
        if (funMap == null) {
            return null;
        }

        MethodInfo getTimeLineStr = this.GetProcesserFunc("TimeLine", funMap, defaultFunMap);
        if (getTimeLineStr == null) {
            return null;
        }

        object[] paramDatas = new object[] { skillId };
        ParseTimeLineRet ret = (ParseTimeLineRet) getTimeLineStr.Invoke(null, paramDatas);
        if (ret == null || ret.timeLineStr == null) {
            return null;
        }
        

        // ������Ĭ�ϵ�ֵ
        List<SkillTimePoint> timeLine = new List<SkillTimePoint>();
        timeLine.Add(new SkillTimePoint(0, GetProcesserFunc("Init", funMap, defaultFunMap)));
        timeLine.Add(new SkillTimePoint(0, GetProcesserFunc("Begin", funMap, defaultFunMap)));
        timeLine.Add(new SkillTimePoint(ret.SkillDuration * 0.5f, GetProcesserFunc("Calc", funMap, defaultFunMap)));
        timeLine.Add(new SkillTimePoint(ret.SkillDuration, GetProcesserFunc("End", funMap, defaultFunMap)));

        string[] results = ret.timeLineStr.Split('|');
        for (int i = 0; i < results.Length; i += 2) {
            if (results[i + 0].Equals("Init"))
            {
                timeLine[0].exceTime = float.Parse(results[i + 1]);
            }
            else if (results[i + 0].Equals("Begin"))
            {
                timeLine[1].exceTime = float.Parse(results[i + 1]);
            }
            else if (results[i + 0].Equals("Calc"))
            {
                timeLine[2].exceTime = float.Parse(results[i + 1]);
            }
            else if (results[i + 0].Equals("End"))
            {
                timeLine[3].exceTime = float.Parse(results[i + 1]);
            }
            else
            {
                MethodInfo func = GetProcesserFunc(results[i + 0], funMap, defaultFunMap);
                if (func != null) {
                    timeLine.Add(new SkillTimePoint(float.Parse(results[i + 1]), func));
                }
                
            }

        }

        this.allSkillTimeLine.Add(skillId, timeLine);
        return timeLine;
    }

    public List<SkillTimeNode> GetSkillTimeNode(int skillId) {
        List<SkillTimePoint> timePoints = this.ParserTimeLine(skillId);

        List<SkillTimeNode> ret = new List<SkillTimeNode>();
        for (int i = 0; i < timePoints.Count; i++)
        {
            ret.Add(new SkillTimeNode(timePoints[i])); // �ڵ���Ż�
        }

        return ret;
    }

    // �ṩҪ���ӿڣ��������ȡ���� config,���ݼ���Id;
    public object GetSkillConfig(int skillId)
    {
        object config = null;

        for (int i = 0; i < this.allSkillConfigType.Count; i++)
        {
            config = ExcelDataMgr.Instance.GetConfigData(this.allSkillConfigType[i], skillId.ToString());
            if (config != null)
            {
                return config;
            }
        }

        Debug.Log($"Get Skill Config {skillId} null!");
        return null;
    }
}
