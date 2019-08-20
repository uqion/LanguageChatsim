using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoudnessBar : MonoBehaviour
{

    public float MicLoudness;
    public Slider slider;
    private string _device;
    public int smoothness = 10;
    private float[] pastLoudness;
    int ind = 0;
    float mic = -1;
    int micCalculatedTimes = 0;
    public Text thre;
    bool _isInitialized;

    void start()
    {
        Debug.Log("Running");
        
        MicLoudness = 0f;
    }
    //mic initialization
    void InitMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }

    int _sampleWindow = 128;
    AudioClip _clipRecord;// = AudioClip.Create("MySinusoid", 44100 * 2, 1, 44100, true);


    //get data from microphone into audioclip
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
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }



    void Update()
    {
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = LevelMax();
        ind++;
        if(pastLoudness==null)
            pastLoudness = new float[smoothness];
        pastLoudness[ind % smoothness] = MicLoudness;
        if (slider)
        {
            float sum = 0;
            for (int i = 0; i < smoothness; i++)
                sum += pastLoudness[i];
            slider.value = sum / smoothness;
        }
    }


    // start mic when scene starts
    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }


    // make sure the mic gets started & stopped when application gets focused
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

    public void setSpeechVol() {
        float sum = 0;
        for (int i = 0; i < smoothness; i++)
            sum += pastLoudness[i];
        //slider.value = sum / smoothness;
        
            mic = mic*micCalculatedTimes + sum / smoothness;
            mic /= ++micCalculatedTimes;
        
        thre.text = (mic/0.8).ToString();
    }
}