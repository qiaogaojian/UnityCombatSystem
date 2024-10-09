using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SkillModel(2000000)]
public class SkillBModel 
{
    [SkillProcesser("Init", -1)] // default;
    public static void DefaultInitProcesser(GM_Charactor sender, int skillId, object udata) {
        Debug.Log($"B  DefaultInitProcesser Skill ID: {skillId}");
    }

    [SkillProcesser("Begin", -1)]
    public static void DefaultBeginProcesser(GM_Charactor sender, int skillId, object udata)
    {
        Debug.Log($"B: DefaultBeginProcesser Skill ID: {skillId}");
    }

    [SkillProcesser("Calc", -1)]
    public static void DefaultCalcProcesser(GM_Charactor sender, int skillId, object udata)
    {
        Debug.Log($"B: DefaultCalcProcesser Skill ID: {skillId}");
    }

    [SkillProcesser("End", -1)]
    public static void DefaultEndProcesser(GM_Charactor sender, int skillId, object udata)
    {
        Debug.Log($"B: DefaultEndProcesser Skill ID: {skillId}");
    }

    [SkillProcesser("TimeLine", -1)]
    public static ParseTimeLineRet DefaultTimeLineStr(int skillId) {
        ParseTimeLineRet ret = new ParseTimeLineRet();

        SkillBConfig config = ExcelDataMgr.Instance.GetConfigData<SkillBConfig>(skillId.ToString());
        if (config == null)
        {
            return null;
        }

        ret.SkillDuration = config.SkillDuration;
        ret.timeLineStr = config.TimeLine;

        return ret;
    }
}
