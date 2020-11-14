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

    bool isCursor = false;
    bool isCursorMove = false;

    float MoveTime = 0.0f;

    Vector3 pos;
    
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
        pos = rect.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Cursor();

        CursorMove();

        Transition();
    }

    void Cursor()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isCursorMove)
            {
                isCursorMove = true;
                isCursor = !isCursor;

                SEManager.Instance.Play(SEPath.CANCEL2);
            }
        }
    }

    void CursorMove()
    {
        if(isCursorMove)
        {
            MoveTime += Time.deltaTime;

            if (!isCursor)
            {
                rect.localPosition = Vector3.Lerp((pos - new Vector3(0, 150.0f)), pos, MoveTime / 0.5f);
            }
            else
            {
                rect.localPosition = Vector3.Lerp(pos, (pos - new Vector3(0, 150.0f)), MoveTime / 0.5f);
            }

            if (MoveTime >= 0.5f)
            {
                MoveTime = 0.0f;
                isCursorMove = false;
            }
        }
    }

    void Transition()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // ゲーム終了
            if (isCursor)
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
