using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Microsoft.CognitiveServices.Speech;

/// <summary>
/// DictationScriptGeneral contains methods to run the communication with Microsoft Azure Speech to Text
/// </summary>
public class DictationScriptGeneral : MonoBehaviour
{
    [SerializeField]
    private Button SentButton;

    [SerializeField]
    private InputField m_Recognitions;

    private object threadLocker = new object();
    private bool waitingForReco;
    private string message = "";
    private bool changed = false;
    private bool micPermissionGranted = false;
    private SpeechRecognizer r;
    public int Loudness;
    public string Language;

    /// <summary>
    /// Unity Constructor that calls initSession()
    /// </summary>
    void Start()
    {
        initSession();
    }
    
    /// <summary>
    /// initSession is in charge of communication with Microsoft Azure Speech to Text. This method contains the subscription key, region, and language.
    /// Make sure you are using the correct key and region if you are running into issues with chatbot not working. Uses lock to maintain ensure that multiple
    /// threads do not access mutually exclusive variables at the same time. 
    /// </summary>
    public async void initSession()
    {
        Debug.Log("Speech Session Initiallized");
        var config = SpeechConfig.FromSubscription("9bec7db1726844678698d2fe0dae2729", "westus"); //Subscription Key and Rigion.
        config.SpeechRecognitionLanguage = Language; //Language.
        // Make sure to dispose the recognizer after use!
        using (var recognizer = new SpeechRecognizer(config))
        {
            r = recognizer;
            //recognizer.AudioLevel = Loudness;
            lock (threadLocker)
            {
                waitingForReco = true;
            }

            // Starts speech recognition, and returns after a single utterance is recognized. The end of a
            // single utterance is determined by listening for silence at the end or until a maximum of 15
            // seconds of audio is processed.  The task returns the recognition text as result.
            // Note: Since RecognizeOnceAsync() returns only a single utterance, it is suitable only for single
            // shot recognition like command or query.
            // For long-running multi-utterance recognition, use StartContinuousRecognitionAsync() instead.
            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(false);

            // Checks result.
            string newMessage = "";
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                lock (threadLocker)
                {
                    newMessage = result.Text;
                    //SentButton.GetComponent<DialogFlow>().SendText(newMessage);
                    message = newMessage;
                    recognizer.Dispose();
                }
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                newMessage = "NOMATCH: Speech could not be recognized.";
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
            }

            lock (threadLocker)
            {
                changed = true;
                waitingForReco = false;
            }
        }



    }
    
    /// <summary>
    /// Unity Update function that will call initSession again if there is any audio recognition and a message that is not NULL
    /// </summary>
    void Update()
    {

        lock (threadLocker)
        {
            if (m_Recognitions != null && message!=null)
            {
                if (changed) //Only Activated When the Message has changed.
                {
                    Debug.Log(message);
                    if (message == "")
                    {
                        m_Recognitions.text = "";
                        changed = false;
                        initSession(); //might move to TTS when the Speech stops, start Recognition.
                    } else
                    {
                        m_Recognitions.text = message;

                        SentButton.GetComponent<DialogFlow>().SendText(message);

                        changed = false;
                        initSession(); //might move to TTS when the Speech stops, start Recognition.
                    }
                }
            }
        }
    }
}
