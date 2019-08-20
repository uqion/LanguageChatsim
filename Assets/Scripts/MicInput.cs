﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicInput : MonoBehaviour
{

    public float MicLoudness;
    public float thre;
    public DictationScript dictationScript;
    private string _device;

    void start() {
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


    private float time = 0;
    void Update()
    {
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = LevelMax();
        if (MicLoudness > thre) {
            Debug.LogFormat("Starting{0}", MicLoudness);
            dictationScript.startDitection();
            time = 0;
        }
        else
        {
            time +=Time.deltaTime;
            if (time > 2) {
                Debug.Log("Stopping");
                dictationScript.stopDitection();
            }
        }
    }

    bool _isInitialized;
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
}