using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boot : MonoBehaviour
{
    public void Awake() {
        DontDestroyOnLoad(this.gameObject);

        // ��ʼ�����
        new ResMgr().Init();
        new EventMgr().Init();
        new ExcelDataMgr().Init();
        this.gameObject.AddComponent<TimerMgr>().Init();
        // end

        this.gameObject.AddComponent<GameApp>().Init();
    }

    private void Start() {
        GameApp.Instance.EnterGame();
    }
}
