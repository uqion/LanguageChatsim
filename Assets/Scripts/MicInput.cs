using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The MicInput class contains a few methods for controlling the recording of the microphone, converting in microphone data to audio data,and filtering of the microphone using the threshold variable
/// </summary>
public class MicInput : MonoBehaviour
{

    public float MicLoudness;
    public float thre;
    public int _sampleWindow = 128;
    public AudioClip _clipRecord;               // = AudioClip.Create("MySinusoid", 44100 * 2, 1, 44100, true);
    private string _device;
    private float time = 0;
    private bool _isInitialized;

    /// <summary>
    /// Unity Constructor that will set MicLoudness to zero
    /// </summary>
    void start() {
        MicLoudness = 0f;
    }
    
    /// <summary>
    /// Member Function for microphone initialization
    /// </summary>
    void InitMic()
    {
        // If no microphone device is connected to the computer or the project settings havent been setup correctly _device will be set to zero and output message will be displayed in the console
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
    }

    /// <summary>
    /// Member Function to stop microphone recording
    /// </summary>
    void StopMicrophone()
    {
        Microphone.End(_device);
    }

    /// <summary>
    /// Gets data from the microphone and stores it into an audioclip. The wavepeak of the audioclip is compared with levelMax and returns levelMax
    /// </summary>
    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            // Compares levelMax with wavePeak, if wavePeak is larger than that becomes the new levelMax
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    /// <summary>
    /// Unity Update function, If the levelMax is greater than a set value of threshold then it will call (Old Script that was removed for the new DictationScriptGeneral) dictationScript.startDitection, Else it will call dictationScript.stopDitection in 2 seconds
    /// NOTE: **This method is rendered useless since we removed the DictationScript for the new DictationScriptGeneral. The old DictationScript used a built in Speech to Text recognizer
    /// that only worked for english. Now both english and german is built into the DictationScriptGeneral using Microsoft Azure for Speech to Text. Add into the commented areas if you
    /// to use the microphone filtering aspect of the MicInput class**.
    /// </summary>
    void Update()
    {
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = LevelMax();
        if (MicLoudness > thre) {
            Debug.LogFormat("Starting{0}", MicLoudness);
            // You can place a script here to "Start Detection" of the microphone if you want to utilize the microphone filtering that has been built into the MicInput class
            time = 0;
        }
        else
        {
            time +=Time.deltaTime;
            if (time > 2) {
                // You can place a script here to "Stop Detection" of the microphone if you want to utilize the microphone filtering that has been built into the MicInput class
            }
        }
    }

    /// <summary>
    /// Starts the microphone by calling InitMic function when the scene starts up
    /// </summary>
    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    /// <summary>
    /// Stops the microphone by calling StopMicrophone function when loading a new scene
    /// </summary>
    void OnDisable()
    {
        StopMicrophone();
    }

    /// <summary>
    /// Deconstructor that stops the microphone by calling StopMicrophone function when closing the application
    /// </summary>
    void OnDestroy()
    {
        StopMicrophone();
    }

    /// <summary>
    /// Starts the microphone recording by calling InitMic function whenever the application is focused and _isInitialized is set to False
    /// </summary>
    /// <param name="focus"></param>
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Debug.Log("Focus");

            if (!_isInitialized)
            {
                Debug.Log("Init Mic");
                InitMic();
                _isInitialized = true;
            }
        }
        if (!focus)
        {
            Debug.Log("Pause");
            StopMicrophone();
            Debug.Log("Stop Mic");
            _isInitialized = false;

        }
    }
}