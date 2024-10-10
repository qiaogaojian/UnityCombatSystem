using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SkillModel(1000000)]
public class SkillAModel 
{
    /*[SkillProcesser("MyCustom", 1)]
    public static void MyCostomProcesser_1000001(GM_Charactor sender, int skillId, object udata)
    {
        Debug.Log("MyCostomProcesser_1000001!");
    }*/

    [SkillProcesser("Init", -1)] // default;
    public static void DefaultInitProcesser(GM_Charactor sender, int skillId, object udata) {
        SkillAConfig config = ExcelDataMgr.Instance.GetConfigData<SkillAConfig>(skillId.ToString());
        if (!config.SkillEffectName.Equals("default"))
        {
            GM_EffectMgr.Instance.PlayerSkillEffectAt(config.SkillEffectName, sender.transform.parent, sender.transform.position);
        }
    }

    [SkillProcesser("Begin", -1)]
    public static void DefaultBeginProcesser(GM_Charactor sender, int skillId, object udata)
    {
        // Debug.Log($"DefaultBeginProcesser Skill ID: {skillId}");
    }

    [SkillProcesser("Calc", -1)]
    public static void DefaultCalcProcesser(GM_Charactor sender, int skillId, object udata)
    {
        Debug.Log($"DefaultCalcProcesser Skill ID: {skillId}");
        SkillAConfig skillAconfig = ExcelDataMgr.Instance.GetConfigData<SkillAConfig>(skillId.ToString());
        FightCalcResult calcResult = new FightCalcResult();
        calcResult.attackR = skillAconfig.AttackR;
        sender.CalcFightBuff("AttackR", calcResult); // sender �����N��buff,ÿ��buff����attackR,��ô�Ϳ��Զ��ۼ�;

        GM_Charactor[] targets = FightMgr.Instance.FindTargetsInArea(sender, calcResult.attackR);
        int count = targets.Length;
        if (skillAconfig.TargetMax > 0) {
            count = (count > skillAconfig.TargetMax) ? skillAconfig.TargetMax : count;
        }

        float attack = (float)sender.fightData.attack;
        attack = attack * skillAconfig.DamageRate + skillAconfig.FixDamage; // A�༼�ܵ�ģ��;
        
        calcResult.attack = attack;
        // ���ӷ���������Buff��Attack���ܣ���ֻ��һ��;
        sender.CalcFightBuff("Attack", calcResult);


        // �������е�Ŀ��
        for (int i = 0; i < count; i++)
        {
            calcResult.defense = targets[i].fightData.defense;
            targets[i].CalcFightBuff("Defense", calcResult);

            if (calcResult.attack > calcResult.defense)
            {
                targets[i].OnLoseHp((int)(calcResult.attack - calcResult.defense));
            }
        }
    }

    [SkillProcesser("End", -1)]
    public static void DefaultEndProcesser(GM_Charactor sender, int skillId, object udata)
    {
        // Debug.Log($"DefaultEndProcesser Skill ID: {skillId}");
    }

    [SkillProcesser("TimeLine", -1)]
    public static ParseTimeLineRet DefaultTimeLineStr(int skillId)
    {
        ParseTimeLineRet ret = new ParseTimeLineRet();

        SkillAConfig config = ExcelDataMgr.Instance.GetConfigData<SkillAConfig>(skillId.ToString());
        if (config == null) {
            return null;
        }

        ret.SkillDuration = config.SkillDuration;
        ret.timeLineStr = config.TimeLine;

        return ret;
    }
}
