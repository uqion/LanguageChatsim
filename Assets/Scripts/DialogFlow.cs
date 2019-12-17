using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;
using ApiAiSDK;
using ApiAiSDK.Model;
using ApiAiSDK.Unity;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using CognitiveServicesTTS;

public class DialogFlow : MonoBehaviour {

    public Text inputFieldText;
    public Text responseText;
    public Text entityText;
    private ApiAiUnity apiAiUnity;
    public Button sendButton;
    public SpeechManager speech;

    // to switch between German and English, default English
    // English token:
    public static string ACCESS_TOKEN = "501118984d85435cbc72f32560dee65c";
    // German token:
    //private static string ACCESS_TOKEN = "26d751367f9247a3adf0e6040e78b81f";
    public static bool isEnglish = true;



    private readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
    };

    // Use this for initialization
    IEnumerator Start()
    {
        // check access to the Microphone
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            throw new NotSupportedException("Microphone using not authorized");
        }

        ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) =>
        {
            return true;
        };

        // Commenting out for now since the scenario for when you ask for the wrong waterbottle doesnt seem to work. Might be an error due to us having the incorrect ACCESS_TOKEN
        AIConfiguration config;
        if (isEnglish)
        {
            config = new AIConfiguration(ACCESS_TOKEN, SupportedLanguage.English);
        } else
        {
            config = new AIConfiguration(ACCESS_TOKEN, SupportedLanguage.German);
        }
        

        apiAiUnity = new ApiAiUnity();
        apiAiUnity.Initialize(config);

        apiAiUnity.OnError += HandleOnError;
        apiAiUnity.OnResult += HandleOnResult;
    }

    void HandleOnResult(object sender, AIResponseEventArgs e)
    {
        var aiResponse = e.Response;
        if (aiResponse != null)
        {
            // get data from aiResponse
        }
        else
        {
            Debug.LogError("Response is null");
        }
    }

    public void SendText(string s) {
        AIResponse response = apiAiUnity.TextRequest(s);

        if (response != null)
        {
            Debug.Log("Resolved query: " + response.Result.ResolvedQuery);
            var outText = JsonConvert.SerializeObject(response, jsonSettings);
            Debug.Log("Result: " + outText);
            outText = response.Result.Fulfillment.Speech;

           

            responseText.text = outText;
            SpeechPlayback();
            //ResponseTextParser();
        }
        else
        {
            Debug.LogError("Response is null");
        }
    }


    public void SendText()
    {

        var text = inputFieldText.text;

        SendText(text);
    }


    //ResponseTextParser is a module that will string manipulate the global variable responseText.
    //The objective of this module is to split responseText into only the useful data from
    //the packet that we receive from DialogFlow. The useful information we are looking for are
    //intents, entities, fulfillments.
    void ResponseTextParser()
    {

        //listIndex is a Dynamic Array that will keep track of the indexes where subdata is seperated within responseText
        List<int> listIndex = new List<int>();
        
        //listClassifiers is a Dynamic Array that will store only the useful subdata of responseText
        List<string> listClassifiers = new List<string>();

        //charsToRemove is a character array that will be passed into a Trim function to help us remove some un-needed characters within our strings
        char[] charsToRemove = { '"', '}' };

        //The for loop below will store the index everytime a ',' appears within responseText
        //',' is the character where the data can be seperated within responseText
        for (int i = 0; i < responseText.text.Length; i++)
        {
            if (responseText.text[i] == ',')
            {
                listIndex.Add(i);
            }
        }

        //The for loop below will use the Dynamic Array listIndex to sort out the needed substrings
        //of subdata into our Dynamic Array listClassifiers.
        for (int i = 0; i < listIndex.Count; i++)
        {
            //The if statements below will compare if the following string in responseText is equal to the specified name of the classifier
            if (responseText.text.Substring(listIndex[i],11) == ",\"metadata\"")
            {
                listClassifiers.Add(responseText.text.Substring(listIndex[i], listIndex[i + 1] - listIndex[i]));
            }
            else if(responseText.text.Substring(listIndex[i],14) == ",\"fulfillment\"")
            {
                listClassifiers.Add(responseText.text.Substring(listIndex[i], listIndex[i + 1] - listIndex[i]));
            }
        }

        Debug.Log(listClassifiers[0]);

        //Below is just clean up for our strings within listClassifiers (we are removing un-needed characters)
        listClassifiers[0] = listClassifiers[0].Remove(0, 26);
        listClassifiers[1] = listClassifiers[1].Remove(0, 25);
        listClassifiers[1] = listClassifiers[1].Trim(charsToRemove);

        //The for loop below is just used to output our listClassifiers into the Unity Console for our sanity
        for (int i = 0; i < listClassifiers.Count; i++)
        {
            Debug.Log(listClassifiers[i]);
        }


    }

    void HandleOnError(object sender, AIErrorEventArgs e)
    {
        Debug.LogException(e.Exception);
    }

    // Update is called once per frame
    void Update () {
		
	}


    public void SpeechPlayback()
    {
        if (speech.isReady)
        {
            string msg = responseText.text;
            //speech.voiceName = VoiceName.enUSJessaRUS;
            //speech.VoicePitch = 0;
                speech.SpeakWithRESTAPI(msg);
        }
        else
        {
            Debug.Log("SpeechManager is not ready. Wait until authentication has completed.");
        }
    }
}