using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class PlayerController : MonoBehaviour
{
    public float speed;         //Move Speed from Camera Tilt

    private Rigidbody rb;

    private Vector3 offset = new Vector3(1.79f, -3.926f, 10.371f);
    
    public GameObject CameraRig;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        rb = Player.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Physics.gravity = new Vector3(0, -200.0f, 0);
    }

    void FixedUpdate()
    {
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger)
            || ViveInput.GetPress(HandRole.LeftHand, ControllerButton.Trigger))
        {
            Debug.Log("Moving");
            moveCamera();
        }
    }


    //Moving the Player Object
    void moveCamera()
    {
        //Input on x (Horizontal)
        float hAxis = Input.GetAxis("Horizontal");
        //Input on z (Vertical)
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = Quaternion.Euler(0.0f, transform.eulerAngles.y - 90.0f, 0.0f) * new Vector3(hAxis * speed * Time.deltaTime, 0.0f, vAxis * speed * Time.deltaTime);
        Vector3 newPos = Player.transform.position + movement;

        //rb.MovePosition(Player.transform.position + transform.forward * Time.deltaTime);
        rb.MovePosition(newPos);

        //rb.velocity = new Vector3(Input.GetAxis("Horizontal") * 3f,
        //                                 0.0f,
        //                                 Input.GetAxis("Vertical") * 3f);
    }

    private void LateUpdate()
    {
        CameraRig.transform.position = Player.transform.position;
    }
}
