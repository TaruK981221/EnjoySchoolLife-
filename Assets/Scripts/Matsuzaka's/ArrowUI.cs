using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowUI : MonoBehaviour
{
	public Image _image;
	private Vector3 _preMousePos;
    // Start is called before the first frame update
    void Start()
    {
		_preMousePos = new Vector3(0.0f, 0.0f, 0.0f);
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			_preMousePos = Input.mousePosition;
		}
		if (!Input.GetKey(KeyCode.Mouse0))
		{
			_image.enabled = false;
		}
		else
		{
			_image.enabled = true;
			Transform trans = _image.GetComponent<Transform>();
			float rot = Mathf.Rad2Deg * (Mathf.Atan2(Input.mousePosition.x - _preMousePos.x, _preMousePos.y - Input.mousePosition.y));
			Vector3 newRot = new Vector3(0.0f, 0.0f, rot);
			Vector2 dif = new Vector2(_preMousePos.x - Input.mousePosition.x, _preMousePos.y - Input.mousePosition.y);
			Vector2 dif2 = new Vector2(_preMousePos.x - Input.mousePosition.x, _preMousePos.y - Input.mousePosition.y);
			Vector3 dif3 = new Vector3(_preMousePos.x + Input.mousePosition.x, _preMousePos.y + Input.mousePosition.y, 0.0f);
			trans.localScale = new Vector3(0.5f, Mathf.Sqrt(dif2.x * dif2.x + dif2.y * dif2.y) * 0.015f, 0.5f);
			trans.eulerAngles= newRot;

			trans.position = dif3 * 0.5f;
		}
    }
}
