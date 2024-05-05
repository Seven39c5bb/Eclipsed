using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 设置管理器
/// </summary>
/// <remarks>
/// 暂时没用
/// </remarks>
public class SettingManager : MonoBehaviour
{
    [Header("音频混合器")]
    public AudioMixer audioMixer;

    /// <summary>
    /// 设置BGM音量
    /// </summary>
    /// <param name="volume">传入值应为-80到0</param>
    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", volume);
    }

    /// <summary>
    /// 设置SFX音量
    /// </summary>
    /// <param name="volume">传入值应为-80到0</param>
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }

    /// <summary>
    /// 设置总音量
    /// </summary>
    /// <param name="volume">传入值应为-80到0</param>
    public void SetTotalVolume(float volume)
    {
        audioMixer.SetFloat("Total", volume);
    }
}
