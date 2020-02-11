using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using JsonData;
//using Google.Apis.Dialogflow.V2;

public class DialogFlowApiScript : MonoBehaviour
{
    //this URL is what we use to hook into the desired dialogflow (V2) functionality: detectIntent
    String CHATBOT_URL = "https://dialogflow.googleapis.com/v2/projects/englishchatbot-engdqu/agent/sessions/34563:detectIntent";

    //access token must be generated using gcloud auth (search: gcloud IAM)
    public String ACCESS_TOKEN;

    [Tooltip("Once a response is received, the response text will be displayed in this text field")]
    //Once a response is received, the response text will be displayed in this text field
    public UnityEngine.UI.Text responseText;
    //Interrupts call to direct business logic to SO_Hi tree
    public Tree_Container tree;

    // Use this for initialization
    void Start()
    {
        // AccessToken is being generated manually in terminal
        //StartCoroutine(GetAgent("ya29.c.ElpfBkjOUlTRSaNDg-i0tBjGc2WlRT9GePIqe1_j5Xq9flXHMGJWnn5sEjNHyG1VfMFqtt3WapHAVo2-RwvPNKRTHI0BkF9OVUzZJ5OWJEILr64_ge1tgcbS7AA"));

        //https://stackoverflow.com/questions/51272889/unable-to-send-post-request-to-dialogflow-404
        //first param is the dialogflow API call, second param is Json web token
        //StartCoroutine(PostRequest("https://dialogflow.googleapis.com/v2/projects/englishchatbot-engdqu/agent/sessions/34563:detectIntent",
                              //"ya29.c.Kl65By8-n8Y2sx5cr437JIEV0BeOVckLM9McvbcuwSwW8JE108WNIdnkQxlKlgAWUZuAfRktjWFbnmS6fUePDTdMAe91ZPwhlSVmOEgeqecCIWJCXCWYHat92RvNRd02"));
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This method will call into the coroutine that takes care of sending the web request to dialogflow
    /// This method exists as a way to encapsulate the actual agent call, as we might want to have a different
    /// way to call into it. (yarn?)
    /// </summary>
    /// <param name="text">The text to be sent to the agent</param>
    public void SendText(string text)
    {
        if(!string.IsNullOrEmpty(text))
        {
            StartCoroutine(PostRequestText(CHATBOT_URL, ACCESS_TOKEN, text));
        }
    }

    /// <summary>
    /// Takes care of building the post request to the dialogflow agent, and then yield until response is received
    /// once response is received, display it in \ref responseText
    /// </summary>
    /// <param name="url">The url to use to gain agent functionality \ref SendText will use \ref CHATBOT_URL and 
    /// current implementation depends on this</param>
    /// <param name="AccessToken">access token generated from gcloud auth, \ref SentText will use \ref ACCESS_TOKEN </param>
    /// <param name="param">The word to be sent to the agent</param>
    /// <returns></returns>
    IEnumerator PostRequestText(String url, String AccessToken, String param)
    {
        UnityWebRequest postRequest = new UnityWebRequest(url, "POST"); 
        RequestBody requestBody = new RequestBody();
        requestBody.queryInput = new QueryInput();
        requestBody.queryInput.text = new TextInput();
        requestBody.queryInput.text.text = param;
        requestBody.queryInput.text.languageCode = "en";

        string jsonRequestBody = JsonUtility.ToJson(requestBody, true);
        Debug.Log(jsonRequestBody);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
        //Debug.Log(bodyRaw);
        postRequest.SetRequestHeader("Authorization", "Bearer " + AccessToken);
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //postRequest.SetRequestHeader("Content-Type", "application/json");

        yield return postRequest.SendWebRequest();

        if (postRequest.isNetworkError || postRequest.isHttpError)
        {
            Debug.Log(postRequest.responseCode);
            Debug.Log(postRequest.error);
        }
        else
        {
            Debug.Log("Reached here 2");
            // Show results as text
            Debug.Log("Response: " + postRequest.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] resultbyte = postRequest.downloadHandler.data;
            string result = System.Text.Encoding.UTF8.GetString(resultbyte);
            ResponseBody content = (ResponseBody)JsonUtility.FromJson<ResponseBody>(result);
            responseText.text = content.queryResult.fulfillmentText;
            Debug.Log("CALLING TTS: " + Time.realtimeSinceStartup);
            tree.ReturnQuery(content.queryResult);
            
        }
    }

    /// <summary>
    /// Method for testing api detectIntent endpoint with "hello"
    /// </summary>
    /// <param name="url">url of chatbot to use</param>
    /// <param name="AccessToken">auth token gotten through gcp aim</param>
    /// <returns></returns>
    IEnumerator TestPostRequest(String url, String AccessToken)
    {
        UnityWebRequest postRequest = new UnityWebRequest(url, "POST");
        RequestBody requestBody = new RequestBody();
        requestBody.queryInput = new QueryInput();
        requestBody.queryInput.text = new TextInput();
        requestBody.queryInput.text.text = "hello";
        requestBody.queryInput.text.languageCode = "en";

        string jsonRequestBody = JsonUtility.ToJson(requestBody, true);
        Debug.Log(jsonRequestBody);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
        //Debug.Log(bodyRaw);
        postRequest.SetRequestHeader("Authorization", "Bearer " + AccessToken);
        postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //postRequest.SetRequestHeader("Content-Type", "application/json");

        yield return postRequest.SendWebRequest();

        if (postRequest.isNetworkError || postRequest.isHttpError)
        {
            Debug.Log(postRequest.responseCode);
            Debug.Log(postRequest.error);
        }
        else
        {
            // Show results as text
            Debug.Log("Response: " + postRequest.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] resultbyte = postRequest.downloadHandler.data;
            string result = System.Text.Encoding.UTF8.GetString(resultbyte);
            ResponseBody content = (ResponseBody)JsonUtility.FromJson<ResponseBody>(result);
            Debug.Log(content.queryResult.fulfillmentText);
        }
    }

    /// <summary>
    /// UNTESTED. Not sure if this method is necessary but the tutorial in this video has it in the code: https://www.youtube.com/watch?v=_xwpE02jAjQ&list=PLDpH9QD5z6ch708eGopPYZkNyaClSdsLl&index=5 
    /// </summary>
    /// <param name="AccessToken">Access token for gcloud app</param>
    /// <returns></returns>
    IEnumerator GetAgent(String AccessToken)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://dialogflow.googleapis.com/v2/projects/englishchatbot-engdqu/agent");

        www.SetRequestHeader("Authorization", "Bearer " + AccessToken);

        yield return www.SendWebRequest();
        //myHttpWebRequest.PreAuthenticate = true;
        //myHttpWebRequest.Headers.Add("Authorization", "Bearer " + AccessToken);
        //myHttpWebRequest.Accept = "application/json";

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}
