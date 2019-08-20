using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.AI;
using UnityEngine.AI;

[RequireComponent(typeof (Texture))]
public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // if we have forced a reset ...
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            //... reload the scene
            Application.LoadLevelAsync(Application.loadedLevelName);
        }
    }
}
