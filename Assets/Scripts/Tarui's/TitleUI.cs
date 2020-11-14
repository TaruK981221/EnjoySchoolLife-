using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    Transform cursor;
    Animator cAnim = null;

    SceneMana scene;
    
    void Awake()
    {
        cursor = GameObject.Find("Cursor").transform;
        cAnim = cursor.GetComponent<Animator>();
        scene = GameObject.Find("SceneManager").GetComponent<SceneMana>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Cursor();

        Transition();
    }

    void Cursor()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (cAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                if (cAnim.GetBool("Cursor"))
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

    void Transition()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // ゲーム終了
            if (cAnim.GetBool("Cursor"))
            {
                scene.isEnd = true;
            }
            // ゲーム開始
            else
            {
                scene.isScene = true;
            }
        }
    }
}
