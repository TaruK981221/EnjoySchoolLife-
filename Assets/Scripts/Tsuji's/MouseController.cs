using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private GameObject hitJougiObj;        //操作する定規のオブジェクト
    [SerializeField] private Vector3 clickMousePos;         //クリックした瞬間の座標
    [SerializeField] private Vector3 releaseMousePos;       //離した瞬間の座標
    [SerializeField] private Vector3 pushCompetence;        //オブジェクトを押す力

    [SerializeField] private float releasePosMisalignment;  //クリックした座標のズレ
    [SerializeField] private int   roundMaxRank = 1000;     //四捨五入の範囲
    [SerializeField] private int   pushPower;               //力加減
    [SerializeField] private bool  isClickFlg;              //クリックフラグ

    private Camera mainCamera;
    private Vector3 mouseMoveDistance;                      //マウスの移動距離
    private float clickingTime;
    private float tempTime;

    private GameObject gameMainManager;
    private GameMainController gmConScript;

    private int reqeatedCount;
    // Start is called before the first frame update
    void Start()
    {
        gameMainManager = GameObject.Find("GameMainManager");
        gmConScript = gameMainManager.GetComponent<GameMainController>();

        mainCamera = Camera.main;
        isClickFlg = false;
        clickingTime = 0.0f;
        tempTime = 0.0f;
        reqeatedCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Input.mousePosition;
        screenPos.x = Mathf.Clamp(screenPos.x, 0.0f, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0.0f, Screen.height);

        Ray pointRay = mainCamera.ScreenPointToRay(screenPos);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            MouseDownRayCast(pointRay, hit);
            //連打可能ならば
            if (gmConScript.GetIsReqeated())
            {
                reqeatedCount++;
                gmConScript.SetReqetedCount(reqeatedCount);
            }
        }

        if (Input.GetMouseButton(0))
        {
            tempTime += (pushPower * 0.1f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit = new RaycastHit();
            MouseUpRayCast(pointRay, hit);
            isClickFlg = true;
            clickingTime = tempTime;
            tempTime = 0.0f;
        }
        Debug.DrawRay(pointRay.origin, pointRay.direction * 1000f);
    }
 
    //マウスの右ボタンをクリックしたら座標とクリックしたオブジェクトを取得
    void MouseDownRayCast(Ray ray, RaycastHit hit)
    {
        if(hitJougiObj != null)
        {
            hitJougiObj = null;
        }

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj.transform.parent != null)
            {
                if (hitObj.transform.parent.tag == "jougi")
                {
                    //定規オブジェクト取得
                    hitJougiObj = hitObj.transform.parent.gameObject;
                    //クリックした座標取得(roundMaxRankの値によって精密さが変わる) 
                    var val = hit.point * roundMaxRank;
                    clickMousePos.x = Mathf.Round(val.x);
                    clickMousePos.y = Mathf.Round(val.y);
                    clickMousePos.z = Mathf.Round(val.z);

                    clickMousePos = clickMousePos / roundMaxRank;
                }
            }
        }
    }

    //マウスの右ボタンを離したら、座標を取得し、クリックした時の座標と計算して差を求める
    void MouseUpRayCast(Ray ray, RaycastHit hit)
    {
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj != null && hitJougiObj != null)
            {
                //クリックして離した座標取得(roundMaxRankの値によって精密さが変わる) 
                var val = hit.point * roundMaxRank;

                releaseMousePos.x = Mathf.Round(val.x);
                releaseMousePos.y = Mathf.Round(val.y);
                releaseMousePos.z = Mathf.Round(val.z);

                releaseMousePos = releaseMousePos / roundMaxRank;
                //移動量を計算する。
                pushCompetence = clickMousePos - releaseMousePos;
                mouseMoveDistance = pushCompetence;
                pushCompetence *= pushPower;
            }
        }
    }


    //Getter
    public GameObject GetHitJougiObj()
    {
        if (hitJougiObj != null) return hitJougiObj;
        else return null;
    }

    public Vector3 GetPushCompetence()
    {
        return pushCompetence;
    }

    public Vector3 GetReleaseMousePos()
    {
        return releaseMousePos;
    }

    public bool GetClickFlg()
    {
        return isClickFlg;
    }

    public float GetReleasePosMisalignment()
    {
        return releasePosMisalignment;
    }

    public float GetPushPower()
    {
        return pushPower;
    }

    public float GetClickingTime()
    {
        return clickingTime;
    }
    //マウスの移動距離
    public Vector3 GetMouseMoveDistance()
    {
        return mouseMoveDistance;
    }
    //Setter
    public void SetClickFlg(bool flag)
    {
        isClickFlg = flag;
    }
}


