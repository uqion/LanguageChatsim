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
    public GameObject AlertCanvas;
    public GameObject tintSphere;
    public GameObject waterBottleLabel;
    public GameObject bottleOfWaterLabel;

    private List<Tuple<string, float>> confidences;
    private bool hasWordBelowThreshold;
    public float confidenceThreshold = 0.99f;


    // Start is called before the first frame update
    void Start()
    {
        hasWordBelowThreshold = false;
        rb = Player.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        Physics.gravity = new Vector3(0, -200.0f, 0);
        UserInputCanvas.SetActive(false);
        AlertCanvas.SetActive(false);
        waterBottleLabel.SetActive(false);
        bottleOfWaterLabel.SetActive(false);


    }

    void FixedUpdate()
    {
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger)
            || ViveInput.GetPress(HandRole.LeftHand, ControllerButton.Trigger))
        {
            moveCamera();
        }

        // activating "alternate" playmode, setting the textBox to active when 'A' key is pressed.
        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.AKey) && hasWordBelowThreshold)
        {
            AlertCanvas.SetActive(false);
            UserInputCanvas.SetActive(true);
            tintSphere.SetActive(true);
            waterBottleLabel.SetActive(true);
            bottleOfWaterLabel.SetActive(true);
        }

        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.BKey))
        {
            UserInputCanvas.SetActive(false);
            waterBottleLabel.SetActive(false);
            bottleOfWaterLabel.SetActive(false);
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

    public void SetConfidences(List<Tuple<string, float>> val)
    {
        confidences = val;
        foreach (Tuple<string, float> con in confidences)
        {
            if (con.Item2 <= confidenceThreshold)
            {
                Debug.Log("[CONFIDENCE] " + con.Item2);
                hasWordBelowThreshold = true;
                AlertCanvas.SetActive(true);
            }
        }
    }

    private void SetCanvasActive()
    {
        Debug.Log("PLAYER button press");
        if (confidences != null)
        {
            Debug.Log("PLAYER: got confidence");
        }
        confidences = null;
    }

    private void LateUpdate()
    {
        CameraRig.transform.position = Player.transform.position;
    }
}
