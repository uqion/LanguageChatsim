using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchManager : MonoBehaviour
{
    public bool enter = true;

    public GameObject MainMenu;
    public GameObject GermanScene;
    public GameObject EnglishScene;

    // Start is called before the first frame update
    void Start()
    {
        // add isTrigger
        var boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // if enter and tag is German Ring, go to German Scene, else go to English Scene
        if (other.gameObject.tag == "Player" && gameObject.tag == "GermanRing")
        {
            Debug.Log("entered German Ring");
            MainMenu.SetActive(false);
            GermanScene.SetActive(true);

            DialogFlow.isEnglish = false;
            DialogFlow.ACCESS_TOKEN = "a9dc57bef27d4c07bda7c26676417a9e";
            //a9dc57bef27d4c07bda7c26676417a9e
            // set menu to be false, activate German Content
        }
        else if (other.gameObject.tag == "Player" && gameObject.tag == "EnglishRing")
        {
            Debug.Log("entered English Ring");
            MainMenu.SetActive(false);
            EnglishScene.SetActive(true);

            DialogFlow.isEnglish = true;
            DialogFlow.ACCESS_TOKEN = "8b268a4934ed4e96a4c6bb32e39b92d9";
            //8b268a4934ed4e96a4c6bb32e39b92d9
        }
    }


}
