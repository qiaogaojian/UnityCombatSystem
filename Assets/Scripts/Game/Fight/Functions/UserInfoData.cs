using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct UserInfoData
{
    public int charactorId;
    public int ulevel;
    public string unick;
    public int maxHp;

    public void Init(int charactorId) {
        this.charactorId = charactorId;
        // ��ȡ������ص�����;
        this.ulevel = 10;
        this.unick = "blake";
        this.maxHp = 500;
    }
}
