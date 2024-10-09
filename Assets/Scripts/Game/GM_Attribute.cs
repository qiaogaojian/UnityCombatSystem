using System;

public class SkillModel : Attribute {
    public int mainType; // 1000000

    public SkillModel(int mainType)
    {
        this.mainType = mainType;
    }
}


public class SkillProcesser : Attribute
{
    public int subType; // -1Ϊ��ǰ�༼��Ĭ�ϵĴ���
    public string funcName;
    public SkillProcesser(string name, int subType)
    {
        this.subType = subType;
        this.funcName = name;
    }
}

public class SkillConfig : Attribute
{
}

public class BuffModel : Attribute
{
    public int mainType;

    public BuffModel(int mainType)
    {
        this.mainType = mainType;
    }
}


public class BuffProcesser : Attribute
{
    public int subType; // -1Ϊ��ǰ��bufĬ�ϵĴ���
    public string propName; // Ҫ�಻ͬ�����ԣ�����buff���㣬

    public BuffProcesser(string propName, int subType)
    {
        this.subType = subType;
        this.propName = propName;
    }
}

public class BuffConfig : Attribute { }
