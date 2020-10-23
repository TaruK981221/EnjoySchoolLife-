using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JougiController : MonoBehaviour
{
    private enum JougiType
    {
        Default,
        Triangle,
        JougiTypeMax
    }

    [SerializeField] private JougiType jougiType;
    [SerializeField] private GameObject jougiObj;
    [SerializeField] private Vector3 force;
    [SerializeField] private Vector3 torque;

    private GameObject mConObj;
    private MouseController mConScript;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        mConObj = GameObject.Find("MouseManager");
        mConScript = mConObj.GetComponent<MouseController>();
        if(jougiObj != null)
        {
            rb = jougiObj.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (mConScript.GetClickFlg() && mConScript.GetHitJougiObj() != null)
        {
            if (mConScript.GetHitJougiObj().name == gameObject.name)
            {
                mConScript.SetClickFlg(false);
                force = mConScript.GetPushCompetence();
                DefaultMove();
                rb.AddForceAtPosition(force, torque);
            }
        }
    }

    //普通？の定規を動かす
    private void DefaultMove()
    {
        //重心を求める
        var centar = jougiObj.transform.position;
        //軸を求める(定規の正面と合わせてね)
        var axis = Vector3.left;
        //マウスの移動距離取得
        var release = mConScript.GetMouseRelease();
        var distance = centar - release;
        //二つのベクトルのなす角度を求める
        var angle = Vector3.Angle(axis, distance);
        //90°の値を0とする
        angle -= 90.0f;
        Debug.Log(angle);
        torque = Vector3.zero;
        //if (angle < 0)
        //{
        //    //クリックした地点からみて左側にスライドさせた
        //    torque.y = angle;

        //}
        //else if (angle > 0)
        //{
        //    //クリックした地点から見て右側にスライドさせた

        //    torque.y = angle;
        //}
        //else
        //{
        //    //クリックした地点からみてズレがなくスライドした。
        //    torque = Vector3.zero;
        //}
    }
}
