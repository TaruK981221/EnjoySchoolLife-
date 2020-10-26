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
    [SerializeField] private int rotatePow;

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
                rb.AddTorque(torque);
                rb.AddForce(force);
            }
        }
    }

    //普通？の定規を動かす
    private void DefaultMove()
    {
        //重心を求める
        var centar = jougiObj.transform.position;
        //軸を求める(定規の正面と合わせてね)
        var axisForward = jougiObj.transform.forward;
        var axisRight = jougiObj.transform.right;
        //マウスの移動距離取得
        var release = mConScript.GetMouseRelease();
        var distance = centar - release;
        //二つのベクトルのなす角度を求める
        var angle = Vector3.Angle(axisForward, distance);
        var angle2 = Vector3.Angle(axisRight, distance);
        //90°の値を0とする
        angle  -= 90.0f;
        angle2 -= 90.0f;
        //回転量を計算
        angle *= rotatePow;
        //一定の角度以上になると真っ直ぐ飛ばすようにする
        if (Mathf.Abs(angle) > 80 * rotatePow)
        {
            torque = Vector3.zero;
        }
        //マウスを離した位置と中心点の距離ベクトル&定規の右向きベクトルとの角度が
        //＋かーかで回転方向を決める
        else if (angle2 > 0)
        {
            Debug.Log(angle);

            torque = new Vector3(0.0f, angle, 0.0f);
        }
        else
        {
            Debug.Log(-angle);

            torque = new Vector3(0.0f, -angle, 0.0f);
        }
    }
}
