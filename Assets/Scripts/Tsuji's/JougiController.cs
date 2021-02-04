using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

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

    [SerializeField] private float jumpPow;
    [SerializeField] private float jumpMax;
    [SerializeField] private float straightPushAngle;
    [SerializeField] private float linePushAngle;

    private GameObject mConObj;
    private MouseController mConScript;
    private Rigidbody rb;
    private Vector3 playerPos;

    private bool  isJump;
    private bool  isRotate;
    private bool  isLife;
    private float posY;

    // Start is called before the first frame update
    void Start()
    {
        mConObj = GameObject.Find("MouseManager");
        mConScript = mConObj.GetComponent<MouseController>();
        if(jougiObj != null)
        {
            rb = jougiObj.GetComponent<Rigidbody>();
        }

        JougiInit();
        posY = jougiObj.transform.localPosition.y;
        playerPos = jougiObj.transform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        //マウスをクリックしたかどうか
        if (mConScript.GetClickFlg() && mConScript.GetHitJougiObj() != null)
        {
            //クリックしたオブジェクトと自身のオブジェクトの名前が同じであれば(時機判定)
            if (mConScript.GetHitJougiObj().name == gameObject.name)
            {
                SEManager.Instance.Play(SEPath.HAGIKU);
                mConScript.SetClickFlg(false);
                //力加減取得
                force = mConScript.GetPushCompetence();
                torque = Vector3.zero;

                var rmPos = mConScript.GetPushCompetence();
                var misalignment = mConScript.GetReleasePosMisalignment();

                //定規オブジェクトのY座標をposYと比較し、ジャンプしているかを判定
                if(jougiObj.gameObject.transform.localPosition.y <= posY)
                {
                    isJump = false;
                }

                //ジャンプ中でなければ
                if (!isJump)
                {
                    //クリックした座標とのズレを判定。misalignmentよりも小さい値であれば
                    //その場でクリックした判定とし、力を上に加える

                    if (Mathf.Abs(rmPos.x) < misalignment && Mathf.Abs(rmPos.z) < misalignment)
                    {
                        //縦の力を計算
                        JougiJump();
                    }
                    else
                    {
                        //横の力を計算
                        JougiMoveSide();
                    }
                    //力を加える
                    rb.AddTorque(torque, ForceMode.Impulse);
                    rb.AddForce(force, ForceMode.Impulse);
                }
            }
        }
        JougiDestroy();
    }

    //初期化
    private void JougiInit()
    {
        if (jumpPow == 0.0f) jumpPow = 5.0f;
        if (straightPushAngle == 0.0f) straightPushAngle = 80.0f;
        if (linePushAngle == 0.0f) linePushAngle = 20.0f;


        isJump = false;
        isRotate = false;
        isLife = true;

        force = Vector3.zero;
        torque = Vector3.zero;
    }

    //普通？の定規を横に動かす
    private void JougiMoveSide()
    {
        //計算前にモデルが反転していた場合、計算結果を反転させる必要がある
        if (transform.up != jougiObj.transform.up)
        {
            isRotate = true;
            Debug.Log("裏");
            Debug.Log(jougiObj.transform.right);
        }
        else
        {
            isRotate = false;
            Debug.Log("表");
            Debug.Log(jougiObj.transform.right);
        }

        //重心を求める
        var centar = jougiObj.transform.localPosition;
        //軸を求める(定規の正面と合わせてね)
        var axisForward = jougiObj.transform.forward;
        var axisRight = jougiObj.transform.right;
        //マウスの移動距離取得
        var release = mConScript.GetReleaseMousePos();
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
        if (Mathf.Abs(angle) > straightPushAngle * rotatePow ||
            Mathf.Abs(angle) < linePushAngle * rotatePow)
        {
            torque = Vector3.zero;

            force.x *= mConScript.GetPushPower();
            force.y *= mConScript.GetPushPower();
            force.z *= mConScript.GetPushPower();
        }
        //マウスを離した位置と中心点の距離ベクトル&定規の右向きベクトルとの角度が
        //＋かーかで回転方向を決める
        else if (angle2 > 0)
        {
            if (isRotate)
                torque = new Vector3(0.0f, angle, 0.0f);
            else
                torque = new Vector3(0.0f, -angle, 0.0f);
        }
        else
        {
            if (isRotate)
                torque = new Vector3(0.0f, -angle, 0.0f);
            else
                torque = new Vector3(0.0f, angle, 0.0f);
        }

    }

    //上に動かす
    private void JougiJump()
    {
        var releasePos = mConScript.GetReleaseMousePos();
        var releaseTime = mConScript.GetClickingTime();
        //上に飛ばす
        isJump = true;
        if (releaseTime > jumpMax)
            releaseTime = jumpMax;

            force  = new Vector3(0.0f, releaseTime * (jumpPow * 0.1f), 0.0f);

        //縦回転
        if (jougiObj.transform.position.z < releasePos.z)
            torque = new Vector3(releaseTime, 0.0f, 0.0f);
        else
            torque = new Vector3(-releaseTime, 0.0f, 0.0f);
          
    }

    //死亡判定
    private void JougiDestroy()
    {
        if(jougiObj.transform.position.y <= -10.0f)
        {
            isLife = false;
            Debug.Log(name + "死亡");
        }
    }

    //生存フラグを取得
    public bool GetLife()
    {
        return isLife;
    }

    public void ReturnJougi()
    {
        isLife = true;
        isJump = false;
        isRotate = false;
        JougiInit();
        jougiObj.transform.position = playerPos;
    }
}
