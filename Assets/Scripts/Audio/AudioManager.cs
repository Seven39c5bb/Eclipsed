using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 储存单个音频的信息
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        [Header("音频剪辑")]
        public AudioClip clip;

        [Header("音频分组")]
        public AudioMixerGroup outputGroup;

        [Header("音频音量")]
        [Range(0, 1)]
        public float volume = 1f;

        [Header("音频是否开局播放")]
        public bool playOnAwake;

        [Header("音频是否循环播放")]
        public bool loop;
    }

    /// <summary>
    /// 储存所有音频信息
    /// </summary>
    public List<Sound> sounds = new List<Sound>();
    /// <summary>
    /// 每个音频剪辑的名称对应一个音频源
    /// </summary>
    public Dictionary<string, AudioSource> audioSourceDics = new Dictionary<string, AudioSource>();

    private static AudioManager instance_AudioManager;
    public static AudioManager instance
    {
        get
        {
            if (instance_AudioManager == null)
            {
                instance_AudioManager = FindObjectOfType<AudioManager>();
            }
            return instance_AudioManager;
        }
    }

    private void Awake()
    {
        if (instance_AudioManager == null)
        {
            instance_AudioManager = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach (Sound sound in sounds)
        {
            GameObject obj = new GameObject(sound.clip.name);
            obj.transform.SetParent(transform);

            AudioSource audioSource = obj.AddComponent<AudioSource>();
            audioSource.clip = sound.clip;
            audioSource.outputAudioMixerGroup = sound.outputGroup;
            audioSource.volume = sound.volume;
            audioSource.playOnAwake = sound.playOnAwake;
            audioSource.loop = sound.loop;

            if (sound.playOnAwake)
            {
                audioSource.Play();
            }

            audioSourceDics.Add(sound.clip.name, audioSource);
        }
    }

    /// <summary>
    /// 播放某个音频
    /// </summary>
    /// <param name="name">音频名称</param>
    /// <param name="isWait">是否音频播放完等待</param>
    public static void PlayAudio(string name, bool isWait = false)
    {
        if (!instance.audioSourceDics.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}音频不存在");
            return;
        }
        if (isWait)
        {
            if (!instance.audioSourceDics[name].isPlaying)
            {
                instance.audioSourceDics[name].Play();
            }
        }
        else
        {
            instance.audioSourceDics[name].Play();
        }
    }

    /// <summary>
    /// 停止某个音频的播放
    /// </summary>
    public static void StopAudio(string name)
    {
        if (!instance.audioSourceDics.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}音频不存在");
            return;
        }
        instance.audioSourceDics[name].Stop();
    }

    /// <summary>
    /// 停止所有BGM，并切换到指定BGM
    /// </summary>
    /// <param name="name">音频名称</param>
    public static void PlayBGM(string name)
    {
        instance.StartCoroutine(instance.PlayBGMIEnumerator(name));
    }
    private IEnumerator PlayBGMIEnumerator(string name)
    {
        // 停止所有正在播放的 BGM
        foreach (var pair in instance.audioSourceDics)
        {
            if (pair.Value.outputAudioMixerGroup.name == "BGM")
            {
                yield return instance.StartCoroutine(FadeOut(pair.Key));
            }
        }

        // 播放新的 BGM
        if (instance.audioSourceDics.ContainsKey(name))
        {
            instance.audioSourceDics[name].Play();
        }
        else
        {
            Debug.LogWarning($"名为{name}的音频不存在");
        }
    }

    public static IEnumerator FadeOut(string name, float fadeTime = 0.25f)
    {
        if (!instance.audioSourceDics.ContainsKey(name))
        {
            Debug.LogWarning($"名为{name}音频不存在");
            yield return null;
        }

        var audioSource = instance.audioSourceDics[name];
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
