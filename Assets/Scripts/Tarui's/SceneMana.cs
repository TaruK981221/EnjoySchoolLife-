using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMana : MonoBehaviour
{
    [SerializeField,Header("遷移先のシーン名を入力")]
    string SceneName = "TitleScene";

    bool IsScene = false;
    public bool isScene
    {
        set
        {
            IsScene = value;
        }
        get
        {
            return IsScene;
        }
    }

    bool IsEnd;
    public bool isEnd
    {
        set
        {
            IsEnd = value;
        }
        get
        {
            return IsEnd;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transition();

        GameEnd();
    }

    void Transition()
    {
        if (isScene)
        {
            SceneManager.LoadScene(SceneName);
        }
    }

    void GameEnd()
    {
        if(IsEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
