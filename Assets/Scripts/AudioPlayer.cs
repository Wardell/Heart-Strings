using UnityEngine;
using System.Collections;
using NAudio;
using NAudio.Wave;
public class AudioPlayer : MonoBehaviour {


    public string filepath;
    IWavePlayer waveOutDevice;
    WaveStream mainOutputStream;
    WaveChannel32 volumeStream;
	// Use this for initialization
	void Start () {
        waveOutDevice = new WaveOut();
        mainOutputStream = CreateInputStream(filepath);

        waveOutDevice.Init(mainOutputStream);
        waveOutDevice.Play();
 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private WaveStream CreateInputStream(string fileName)
    {
        WaveChannel32 inputStream;
        if (fileName.ToLower().EndsWith(".mp3"))
        {
            WaveStream mp3Reader = new Mp3FileReader(fileName);
            inputStream = new WaveChannel32(mp3Reader);
        }
        else
        {
            inputStream = null;
            Debug.LogError("Unsupported extension");
        }
        volumeStream = inputStream;
        return volumeStream;
    }

    void OnApplicationQuit()
    {
        CloseWave();
    }

    public void Pause()
    {
        if (waveOutDevice != null)
        {
            waveOutDevice.Pause();
        }
    }

    public void Play()
    {
        if (waveOutDevice != null)
            waveOutDevice.Play();
    }

    private void CloseWave()
    {
        if (waveOutDevice != null)
        {
            waveOutDevice.Stop();
        }

        if (mainOutputStream != null)
        {
            volumeStream.Close();
            volumeStream = null;

            mainOutputStream.Close();
            mainOutputStream = null;
        }

        if (waveOutDevice != null)
        {
            waveOutDevice.Dispose();
            waveOutDevice = null;
        }
    }

}
