using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoPilot : MonoBehaviour
{
    private float mu;
    private float d_mu;
    private float F = 500f;
    private float M;
    private float manual_F;
    public float timeRot = 10f;
    public float timeFlight= 10f;

    [SerializeField] public GameObject Target;
    public GameObject Camera;

    private int switcher = 1;
    private float distance = 0f;
    private float angle = 0f;
    private Vector3 tensor = new Vector3(38.8f,38.8f,38.8f);
    private Rigidbody rb;
    private float MM = 2f;

    private float start_time = 0f;
    private bool flag = false;
    private float t_min = 0f;
    private float t_max = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.inertiaTensor = tensor;
        M = rb.mass;
    }


    private void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            distance = Raycast().y;
            angle = Mathf.Deg2Rad * Raycast().z;
            if (distance != 0)
            {
                start_time = Time.time;
                switcher = 2;
                Debug.Log("Switched 2 (Autorot)");
            }
            else
                Debug.Log("Try again");
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {


        switch (switcher)
        {
            case 1:
                break;

            case 2:
                AutoRot(angle, start_time);
                break;

            case 3:
                Mover(distance);
                break;
        }
    }

    Vector3 Raycast()
    {
        RaycastHit hit;
        float q_y = 0;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.TransformDirection(Vector3.forward), out hit))
        {
            if (hit.collider.gameObject == Target)
            {
                Debug.Log("Found an object - distance: " + hit.distance);
                Quaternion q_rot = Camera.transform.localRotation;
               // Debug.Log("Found an object - rotation: " + q_rot.eulerAngles);
                q_y = q_rot.eulerAngles.y;
                return new Vector3(0, hit.distance, q_y);

            }

        }
        return new Vector3(0, 0, 0);
    }

    void AutoRot(float ang, float start_time)
    {

        float tkrot = timeRot;
        int koef = 1;
        if (ang > Mathf.PI)
            ang = -(2 * Mathf.PI - ang);
        if (ang < -Mathf.PI)
            ang = (2 * Mathf.PI + ang);
 
        if (flag == false)
        {
            t_min = tkrot / 2 - Mathf.Sqrt(tkrot * tkrot / 4 + ang * tensor.x / MM);
            //Debug.Log(t_min);
            t_max = tkrot / 2 + Mathf.Sqrt(tkrot * tkrot / 4 + ang * tensor.x / MM);
           // Debug.Log(t_max);
            flag = true;
        }

        if ((ang > 0) && (Time.time - start_time < t_min))
        {//Debug.Log("One side");
            rb.AddTorque(transform.up * MM, ForceMode.Force);
        }
        if ((ang > 0) && (Time.time - start_time > t_max) && (Time.time - start_time < tkrot))
        {//Debug.Log("Second side");
            rb.AddTorque(-transform.up * MM, ForceMode.Force);
        }

        if ((ang < 0) && (Time.time - start_time < t_min))
        {//Debug.Log("One side ang -");
            rb.AddTorque(-transform.up * MM, ForceMode.Force);
        }
        if ((ang < 0) && (Time.time - start_time > t_max) && (Time.time - start_time < tkrot))
        {//Debug.Log("Second side ang -");
            rb.AddTorque(transform.up * MM, ForceMode.Force);
        }



        if (Time.time - start_time > tkrot)
        {
            switcher = 3;
            flag = false;
            Debug.Log("Switched 3 (Move)");
        }

    }

    void Mover(float distance)
    {
        float tkflight = timeFlight;
        if (flag == false)
        {
            t_min = tkflight / 2 - Mathf.Sqrt(tkflight * tkflight / 4 - distance * rb.mass / F);
            //Debug.Log(t_min);
            t_max = tkflight / 2 + Mathf.Sqrt(tkflight * tkflight / 4 - distance * rb.mass / F);
           // Debug.Log(t_max);
            start_time = Time.time;
            flag = true;

            //Debug.Log(start_time);
            //Debug.Log(Time.time);
        }

        if (Time.time - start_time < t_min)
        {
            rb.AddForce(transform.forward * F, ForceMode.Force);
        }

        if ((Time.time - start_time > t_max) && (Time.time - start_time < tkflight))
        {
            rb.AddForce(-transform.forward * F, ForceMode.Force);
        }

        if (Time.time - start_time > tkflight)
        {
            switcher = 1;
            flag = false;
            Debug.Log("Switched 1 (Wait for Input)");
        }

    }

}
