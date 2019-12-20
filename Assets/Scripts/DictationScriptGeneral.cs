using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

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
    public Dispatcher dispatcher;

    /// <summary>
    /// Unity Constructor that calls initSession()
    /// </summary>
    void Start()
    {
        RecognitionRoutine();
    }

    /// <summary>
    /// initSession is in charge of communication with Microsoft Azure Speech to Text. This method contains the subscription key, region, and language.
    /// Make sure you are using the correct key and region if you are running into issues with chatbot not working. Uses lock to maintain ensure that multiple
    /// threads do not access mutually exclusive variables at the same time. 
    /// </summary>
    /*
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
    */

    private async void RecognitionRoutine()
    {
        var config = SpeechConfig.FromSubscription("0cc4c3c202794959b90ad85f311d3a4d", "westus"); //Subscription Key and Rigion.
        config.SpeechRecognitionLanguage = Language; //Language.
        using (var recognizer = new SpeechRecognizer(config))
        {
            r = recognizer;
            recognizer.Recognized += Recognizer_Recognized;
            recognizer.Canceled += Recognizer_Canceled;
            var result = await recognizer.RecognizeOnceAsync().ConfigureAwait(true);
            recognizer.Dispose();
        }
    }

    private void Recognizer_Canceled(object sender, SpeechRecognitionCanceledEventArgs e)
    {
        dispatcher.AddTask(() =>
        {
            Debug.Log("Canceled recognition: " + e.Result);
            RecognitionRoutine();
        });

    }

    private void Recognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
    {
        dispatcher.AddTask(() =>
        {
            switch (e.Result.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    Debug.Log(e.Result);
                    SentButton.GetComponent<DialogFlow>().SendText(e.Result.Text);
                    break;
                default:
                    Debug.Log("Recognizing failed: " + e.Result);
                    break;
            }
            m_Recognitions.text = e.Result.Text;
            RecognitionRoutine();
        });
    }
}