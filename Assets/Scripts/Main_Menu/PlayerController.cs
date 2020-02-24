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
    public GameObject UserInputCanvas;
    // Start is called before the first frame update
    void Start()
    {
        rb = Player.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Physics.gravity = new Vector3(0, -200.0f, 0);
        
        // when the scene is started, set the textBox (that shows user input from mic) as false,
        // "alternate" playmode is not activated yet. 
        UserInputCanvas.SetActive(false);
    }

    void FixedUpdate()
    {
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger)
            || ViveInput.GetPress(HandRole.LeftHand, ControllerButton.Trigger))
        {
            moveCamera();
        }

        // activating "alternate" playmode, setting the textBox to active when 'A' key is pressed.
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.AKey))
        {
            UserInputCanvas.SetActive(true);
        }
    }


    //Moving the Player Object
    void moveCamera()
    {

        //Convert the rotation on y-axis into rectangular coordinates in x-axis and z-axis
        float xAxis = Mathf.Cos((transform.eulerAngles.y - 195.0f) * Mathf.PI / 180);
        float zAxis = Mathf.Sin((transform.eulerAngles.y - 195.0f) * Mathf.PI / 180);
        //Put into vector format and multiply by speed and time.deltaTime to convert in a per frame basis
        Vector3 movement = new Vector3(zAxis * -1 * speed * Time.deltaTime, 0.0f, xAxis * -1 * speed * Time.deltaTime);
        Vector3 newPos = Player.transform.position + movement;

        //rb.MovePosition(Player.transform.position + transform.forward * Time.deltaTime);
        rb.MovePosition(newPos);

    }

    private void LateUpdate()
    {
        CameraRig.transform.position = Player.transform.position;
    }
}
