using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class TitleUI : MonoBehaviour
{
    Transform cursor;
    Animator cAnim = null;

    SceneMana scene;

    RectTransform rect = null;
    bool isRect = true;
    
    void Awake()
    {
        cursor = GameObject.Find("Cursor").transform;
        cAnim = cursor.GetComponent<Animator>();
        scene = GameObject.Find("SceneManager").GetComponent<SceneMana>();
        rect = cursor.GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine("Scale");
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

                SEManager.Instance.Play(SEPath.CANCEL2);
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
                SEManager.Instance.Play(SEPath.DECISION);
            }
            // ゲーム開始
            else
            {
                scene.isScene = true;
                SEManager.Instance.Play(SEPath.DECISION);
            }
        }
    }

    IEnumerator Scale()
    {
        while (true)
        {
            if (isRect)
            {
                rect.sizeDelta *= 1.2f;
                isRect = false;
            }
            else
            {
                rect.sizeDelta /= 1.2f;
                isRect = true;
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
