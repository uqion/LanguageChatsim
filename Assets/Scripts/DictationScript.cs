using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class DictationScript : MonoBehaviour
{
    [SerializeField]
    private Button SentButton;
    //private Text m_Hypotheses;

    [SerializeField]
    private InputField m_Recognitions;

    [SerializeField]
    private Text m_condidence;

    [SerializeField]
    private MicInput mic;

    private DictationRecognizer m_DictationRecognizer;

    void Start()
    {
        m_DictationRecognizer = new DictationRecognizer();
        // m_DictationRecognizer.AutoSilenceTimeoutSeconds = 50;

        Debug.Log("Dictation Recognizer is being initialized");

        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            // m_Recognitions.text = "";
            //SentButton.GetComponent<DialogFlow>().inputFieldText.text = text;
            // m_Hypotheses.text = "Selection:";
            m_Recognitions.text = text;
            SentButton.GetComponent<DialogFlow>().SendText(text);
            m_condidence.text = confidence.ToString();
            //;
        };

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            Debug.LogFormat("Dictation hypothesis: {0}", text);
            // m_Hypotheses.text += text + "\n";
        };

        /*m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
            m_DictationRecognizer.Start();
        };*/

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };
        m_DictationRecognizer.DictationComplete += SessionEnd;
        m_DictationRecognizer.Start();
    }

    public void startDitection() {
        if (m_DictationRecognizer.Status != SpeechSystemStatus.Running) {
            m_DictationRecognizer.Start();
        }
        else
        {
            Debug.Log("SpeechSystemStatus is Running");
        }
    }

    public void stopDitection()
    {
        if (m_DictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            m_DictationRecognizer.Stop();
        }
    }

    public void SessionEnd(DictationCompletionCause cause)
    {
        //m_Hypotheses.text = "Selection:";
        //m_Recognitions.text += "\n";
    }

}
