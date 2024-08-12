using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyboardControl : MonoBehaviour
{
    public float thrust = 1.0f;
    public float torque = 1f;
    private float DeltaTime = 1f;
    private float force = 17.08f;
    private Rigidbody rb;
    private bool mode = true;
    private bool space = false;
    private float timer = 0.0f;


    private Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        anim = gameObject.GetComponentInChildren<Animator>();
    }


    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            anim.SetBool("Flip", mode);
           
            mode = !mode;
            Debug.Log("Switch mode " + mode);

            if (mode)
                rb.angularDrag = 10.0f;
            else
                rb.angularDrag = 0.0f;

        }



        if (Input.GetKey("d"))
        {
            if (mode)
            {
                rb.AddForce(transform.right * thrust, ForceMode.Force);
            }
            else
            {
                rb.AddTorque(transform.up * torque, ForceMode.Force);
            }
            //Debug.Log("Test");

        }

        if (Input.GetKey("a"))
        {
            if (mode)
            {
                rb.AddForce(-transform.right * thrust, ForceMode.Force);
            }
            else
            {
                rb.AddTorque(-transform.up * torque, ForceMode.Force);


            }
        }
        if (Input.GetKey("w"))
        {
            if (mode)
            {
                rb.AddForce(transform.forward * thrust, ForceMode.Force);

            }


            else
            {
                rb.AddTorque(-transform.right * torque, ForceMode.Force);
            }
        }
        if (Input.GetKey("s"))
        {
            if (mode)
            {
                rb.AddForce(-transform.forward * thrust, ForceMode.Force);
            }


            else
            {
                rb.AddTorque(transform.right * torque, ForceMode.Force);
            }

        }
        if (Input.GetKey("e"))
        {
            if (mode)
            {
                rb.AddForce(transform.up * thrust, ForceMode.Force);
            }
            else
            {
                rb.AddTorque(-transform.forward * torque, ForceMode.Force);
            }


        }
        if (Input.GetKey("q"))
        {
            if (mode)
            {
                rb.AddForce(-transform.up * thrust, ForceMode.Force);
            }
            else
            {
                rb.AddTorque(transform.forward * torque, ForceMode.Force);
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            timer = Time.time;
            space = true;
        }
        
        if (space == true)
        {


            if (Time.time < timer + DeltaTime)
            {

                rb.AddForce(transform.forward * force, ForceMode.Force);
            }
            if ((Time.time < timer + 2 * DeltaTime) && (Time.time >= timer + DeltaTime))
            {

                rb.AddForce(-transform.forward * force, ForceMode.Force);
            }
            if (Time.time > timer + 2 * DeltaTime)
                space = false;

        }  


    }
}