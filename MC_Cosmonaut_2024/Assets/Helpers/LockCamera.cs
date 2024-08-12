using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LockCamera : MonoBehaviour
{
    // Start is called before the first frame update

    private Transform target;

    private Vector3 offset;// = new Vector3(0, 2, -5);
    private Quaternion offsetRot;

    public float smooth = 5.0f;


    private float time;

    private Quaternion tmp;
    void Awake()
    {
        offset = transform.localPosition;
        offsetRot = transform.localRotation;
    }


    void Start()
    {
        time = Time.time;
        tmp = offsetRot;
        target = transform.parent;

    }
    public float x = 1f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)  )
         {
            var camera = GetComponent<CameraLock>();
            camera.enabled = !camera.enabled;
        }
    }

    void LateUpdate()
    {    
       

        //tmp = transform.rotation;
        transform.position = Vector3.Lerp(transform.position, target.position + tmp * offset,  smooth);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, offsetRot, smooth);


        if (Time.time - time > x)
        {
            time = Time.time;
            tmp = target.rotation;

            Debug.Log("");
        }



    }
}
