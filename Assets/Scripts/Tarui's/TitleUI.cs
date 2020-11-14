using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    Transform cursor;
    Animator cAnim = null;
    
    void Awake()
    {
        cursor = GameObject.Find("Cursor").transform;
        cAnim = cursor.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(cAnim.GetBool("Cursor"))
            {
                cAnim.SetBool("Cursor", false);
            }
            else
            {
                cAnim.SetBool("Cursor", true);
            }
        }
    }
}
