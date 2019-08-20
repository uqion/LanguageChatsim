using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;

public class Recording : MonoBehaviour
{
   // private List<AudioClip> audioClips = new List<AudioClip>();
    private string _device;
    // Start recording with built-in Microphone and play the recorded audio right away
    public void record()
    {
        Debug.Log("recording");
        if (_device == null) _device = Microphone.devices[0];
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(_device, true, 10, 44100);
       // audioClips.Add(audioClip);
        Debug.Log("recorded");
    }

    public void StopRecording() {
        Debug.Log("stoppingrrrS");
        if (Microphone.IsRecording(_device)) {
            Debug.Log("stop");
            Microphone.End(_device);
            //AudioClip audioClip = Combine(audioClips.ToArray());
            AudioSource audioSource = GetComponent<AudioSource>();
            byte[] wavFile = OpenWavParser.AudioClipToByteArray(audioSource.clip);
            File.WriteAllBytes(Application.persistentDataPath + "/MyFile.wav", wavFile);
            Debug.Log("Saved to" + Application.persistentDataPath + "/MyFile.wav");
            // audioSource.clip = audioClip;
            // audioSource.Play();
        }
        record();
    }

    //public static AudioClip Combine(params AudioClip[] clips)
    //{
    //    if (clips == null || clips.Length == 0)
    //        return null;

    //    int length = 0;
    //    for (int i = 0; i < clips.Length; i++)
    //    {
    //        if (clips[i] == null)
    //            continue;

    //        length += clips[i].samples * clips[i].channels;
    //    }

    //    float[] data = new float[length];
    //    length = 0;
    //    for (int i = 0; i < clips.Length; i++)
    //    {
    //        if (clips[i] == null)
    //            continue;

    //        float[] buffer = new float[clips[i].samples * clips[i].channels];
    //        clips[i].GetData(buffer, 0);
    //        //System.Buffer.BlockCopy(buffer, 0, data, length, buffer.Length);
    //        buffer.CopyTo(data, length);
    //        length += buffer.Length;
    //    }

    //    if (length == 0)
    //        return null;

    //    AudioClip result = AudioClip.Create("Combine", length / 2, 2, 44100, false, false);
    //    result.SetData(data, 0);

    //    return result;
    //}
}