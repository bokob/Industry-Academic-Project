using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class Sound
{
    public string name; // 곡의 이름.
    public AudioClip clip; // 곡.
}

public class SoundManager : MonoBehaviour 
{

    public static SoundManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    private string currentSceneName = "";
    public AudioClip defaultBGM; // 기본 BGM

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PlayBGMBySceneName();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMBySceneName();
    }

    public void PlayBGMBySceneName()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);

        // PlayMusic(sceneName);
        PlayMusic(sceneName);
    }

    // public void PlayMusic(string name) // bgm 재생
    // {
    //     Sound s = Array.Find(musicSounds, x=>x.name == name);

    //     if(s==null)
    //     {
    //         Debug.Log("Sound Not Found");
    //     }
    //     else if (s.name == name) // 씬 이름
    //     {
    //         Debug.Log("바꿉니다.");
    //         musicSource.clip = s.clip;
    //         musicSource.Play();
    //     }
    //     else if(s.clip.name == musicSource.clip.name) // 바꿀 bgm의 이름과 현재 bgm의 이름이 같으면
    //     {
    //         Debug.Log("안바꿀거야");
    //         return;
    //     }
    // }

    public void PlayMusic(string name)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Sound s = Array.Find(musicSounds, x=>x.name == name);

        if (sceneName == "MountainPlayScene")
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            currentSceneName = sceneName;
        }
        else if (sceneName == "RiverPlayScene")
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            currentSceneName = sceneName;
        }
        else if (sceneName == "EncyclopediaScene")
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            currentSceneName = sceneName;
        }
        else if(sceneName == "QuizScene")
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            currentSceneName = sceneName;
        }
        else if(sceneName == "GraphScene")
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            currentSceneName = sceneName;
        }/* 추가로 특정 씬에 맞게 재생하게끔 여기에 추가 예정 */
        else if(sceneName == "DemoPlayScene")
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            currentSceneName = sceneName;
        }
        else 
        {
            if(musicSource.clip!=null && musicSource.clip.Equals(defaultBGM)) // 만약 다른 씬인데 같은 bgm이면 중복재생 방지
                return;

            musicSource.clip = defaultBGM;
            musicSource.Play();
            currentSceneName = "";
        }
    }


    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x=>x.name == name);

        if(s==null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute=!sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

}