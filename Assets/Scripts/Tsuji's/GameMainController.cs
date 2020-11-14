using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainController : MonoBehaviour
{
    [SerializeField] private GameObject[] playersObjList;
    [SerializeField] private List<JougiController> jConScripts;
    [SerializeField] private int reqeatedCountMax;
    [SerializeField] private int reqeatedTimeLimit;

    private GameObject mouseManagerObj;    
    private MouseController mConScript;

    private bool isReqeated;            //連打フラグ
    private bool isReturn;              //復帰可能フラグ
    private int reqeatedCount;          //連打カウント    
    private float reqeatedTime;         //時間

    // Start is called before the first frame update
    void Start()
    {
        playersObjList = GameObject.FindGameObjectsWithTag("jougi");
        mouseManagerObj = GameObject.Find("MouseManager");

       for(int i = 0; i < playersObjList.Length; i++)
        {
            jConScripts.Add(playersObjList[i].GetComponent<JougiController>());
        }
        mConScript = mouseManagerObj.GetComponent<MouseController>();

        isReqeated = false;
        isReturn = false;
    }

    // Update is called once per frame
    void Update()
    {
        reqeatedTime += Time.deltaTime;
        for(int i = 0; i < playersObjList.Length; i++)
        {
            //死んでるプレイヤーがいたら
            if (!jConScripts[i].GetLife())
            {
                //連打開始
                isReqeated = true;
                if(reqeatedTime < reqeatedTimeLimit)
                {
                    //連打!!!
                    ReqeatedHits(playersObjList[i], jConScripts[i]);
                }
                else if(!isReturn)
                { 
                    Debug.Log("失敗");
                    Destroy(playersObjList[i]);
                }
            }
        }
    }
    //連打
    private void ReqeatedHits(GameObject obj,JougiController jcon)
    {
        string pName = obj.name;        //プレイヤー名取得
        if(reqeatedCount >= reqeatedCountMax)
        {
            isReturn = true;
            Debug.Log("復帰");
            jcon.ReturnJougi();
        }
    }

    //連打
    public bool GetIsReqeated()
    {
        return isReqeated;
    }

    //復帰
    public bool GetIsReturn()
    {
        return isReturn;
    }

    public void SetReqetedCount(int count)
    {
        reqeatedCount = count;
    }
}
