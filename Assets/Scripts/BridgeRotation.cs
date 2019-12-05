using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRotation : MonoBehaviour
{

    float x;
    public double Offset { get; set; }

    private Vector3 target;
    // Use this for initialization
    void Start()
    {
        //transform.rotation = Quaternion.Euler(90, 0, 0);
        //transform.rotation = Quaternion.AngleAxis(30, Vector3.left);
        target = new Vector3(274.033f, 59.02f, 213.2148f);
        Debug.Log(transform.position.x.ToString() + " " + transform.position.y.ToString() + " " + transform.position.z.ToString());
    }
    void Update()
    {
        //transform.rotation = Quaternion.Euler(0, 0, 45);
        transform.eulerAngles = new Vector3(10f, 0, 0);
        //x += Time.deltaTime * 10;
        //transform.rotation = Quaternion.Euler(x, 0, 0);
        //transform.RotateAround(transform.position, Vector3.right, 10 * Time.deltaTime);
        //transform.RotateAround(target, Vector3.right, 10 * Time.deltaTime);
    }
}
