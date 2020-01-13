using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    void Start()
    {
        // add isTrigger
        var boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // if enter and tag is German Ring, go to German Scene, else go to English Scene
        if (other.gameObject.tag == "Player")
        {
            RestartGame();
        }
    }


    public void RestartGame() {
        SceneManager.LoadScene("_final_scene");
    }
}
