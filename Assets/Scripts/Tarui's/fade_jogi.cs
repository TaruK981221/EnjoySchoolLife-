using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fade_jogi : MonoBehaviour
{
    [SerializeField]
    bool FadeIn = false, FadeOut = false;
    public bool isFadeIn
    {
        set
        {
            FadeIn = value;
        }
        get
        {
            return FadeIn;
        }
    }
    public bool isFadeOut
    {
        set
        {
            FadeOut = value;
        }
        get
        {
            return FadeOut;
        }
    }

    [SerializeField,Header("フェードしている間の時間")]
    float FadeTime = 1.0f;

    float elapsedTime = 0.0f;

    float r = 0.0f;

    Vector3 startIn = Vector3.zero;
    Vector3 startOut = new Vector3(4700.0f, 0.0f);

    Vector3 endIn = new Vector3(-4700.0f, 0.0f);
    Vector3 endOut = Vector3.zero;

    bool FadeStart = false;
    bool FadeEnd = false;
    public bool isFadeEnd
    {
        get
        {
            return FadeEnd;
        }
    }

    Transform child;

    // Start is called before the first frame update
    private void Start()
    {
        child = this.transform.GetChild(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!FadeStart && (FadeIn || FadeOut))
        {
            FadeStart = !FadeStart;
            FadeEnd = false;

            if (FadeIn)
            {
                child.position = startIn;
            }
            else
            {
                child.position = startOut;
            }
        }


        if (FadeIn)
        {
            elapsedTime += Time.deltaTime;

            r = elapsedTime / FadeTime;

            child.localPosition = Vector3.Lerp(startIn, endIn, r);

            
            if (elapsedTime >= FadeTime)
            {
                FadeIn = !FadeIn;

                elapsedTime = 0.0f;

                FadeStart = !FadeStart;
                FadeEnd = true;
            }
        }

        if(FadeOut)
        {
            elapsedTime += Time.deltaTime;

            r = elapsedTime / FadeTime;

            child.localPosition = Vector3.Lerp(startOut, endOut, r);


            if (elapsedTime >= FadeTime)
            {
                FadeOut = !FadeOut;

                elapsedTime = 0.0f;

                FadeStart = !FadeStart;
                FadeEnd = true;
            }
        }
    }
}
