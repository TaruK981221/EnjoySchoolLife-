using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JougiController : MonoBehaviour
{
    [SerializeField] private GameObject jougiObj;
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
        if (mConScript.GetClickFlg())
        {
            if (mConScript.GetHitJougiObj().name == gameObject.name)
            {
                mConScript.SetClickFlg(false);
                Vector3 force = mConScript.GetPushCompetence();
                rb.AddForce(force);
            }
        }
    }
}
