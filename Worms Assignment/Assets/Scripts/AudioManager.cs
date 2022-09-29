//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] soundList;

    public static AudioManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null){
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        foreach (Sound sound in soundList)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    private void Start() {
        PlaySound("bgm");
    }
    public void PlaySound(string name){
        for (int i = 0; i < soundList.Length; i++)
        {
            if(name == soundList[i].name){
                if(!soundList[i].audioSource.isPlaying){
                    Debug.Log("Playing sound: " + soundList[i].name);
                    soundList[i].audioSource.Play();
                }
                return;
            }
        }
    }

    public void StopSound(string name){
        for (int i = 0; i < soundList.Length; i++)
        {
            if(name == soundList[i].name){
                soundList[i].audioSource.Stop();
                return;
            }
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;
    public bool loop;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f,3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource audioSource;
}
