using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Microsoft.CognitiveServices.Speech;


public class DictationScriptGerman : MonoBehaviour
{
   /* [SerializeField]
    private Button SentButton;
    //private Text m_Hypotheses;

    [SerializeField]
    private InputField m_Recognitions;


    private object threadLocker = new object();
    private bool waitingForReco;
    private string message = "";3
    private bool changed = false;
    private bool micPermissionGranted = false;
    private SpeechRecognizer r;
    public int Loudness;

    void Start()
    {
        initSession();
    }

    public async void initSession()
    {
        Debug.Log("Speech Session Initiallized");
        var config = SpeechConfig.FromSubscription("01bda76b993149bba6946a9fdb1703fd", "canadacentral"); //Subscription Key and Rigion.
        config.SpeechRecognitionLanguage = "de-de"; //Language.
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
    
    void Update()
    {
#if PLATFORM_ANDROID
        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            micPermissionGranted = true;
            message = "Click button to recognize speech";
        }
#endif

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

    public void enablerec()
    {
        //    if (r != null) {
        //        initSession();
        //    }
    }

    public void disablerec()
    {
        //    if (r != null) {
        //        r.Dispose(true);
        //    }
    }*/
}
