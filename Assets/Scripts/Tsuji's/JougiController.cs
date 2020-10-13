using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JougiController : MonoBehaviour
{
    [SerializeField] private GameObject jougiObj;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if(jougiObj != null)
        {
            rb = jougiObj.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 force = new Vector3(100.0f, 0.0f, 0.0f);
            rb.AddForce(force);
        }
    }
}
