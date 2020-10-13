using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private GameObject hitJougiObj;
    [SerializeField] private Vector3 hitMousePos;

    [SerializeField] private int roundMaxRank = 1000;
    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Input.mousePosition;
        screenPos.x = Mathf.Clamp(screenPos.x, 0.0f, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0.0f, Screen.height);

        Ray pointRay = mainCamera.ScreenPointToRay(screenPos);
        RaycastHit hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0))
        {
            MouseRayCast(pointRay, hit);
        }
        Debug.DrawRay(pointRay.origin, pointRay.direction * 1000f);
    }
 
    void MouseRayCast(Ray ray, RaycastHit hit)
    {
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj.transform.parent != null)
            {
                if (hitObj.transform.parent.tag == "jougi")
                {
                    //定規オブジェクト取得
                    hitJougiObj = hitObj.transform.parent.gameObject;
                    //クリックした座標取得(roundMaxRankの値によって精密さが変わる) 
                    var val = hit.point * roundMaxRank;
                    hitMousePos.x = Mathf.Round(val.x);
                    hitMousePos.y = Mathf.Round(val.y);
                    hitMousePos.z = Mathf.Round(val.z);
                
                    hitMousePos = hitMousePos / roundMaxRank;
                }
            }
        }
    }

    public GameObject GetHitJougiObj()
    {
        return hitJougiObj;
    }
}


