using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public readonly static string FreezeOnDialogue = "FreezeOnDialogue";
    public readonly static string Volume = "Volume";

    public static bool DevRun = true;
    public static bool DialogueEnabled = true;
    public static bool MusicEnabled = true;
    public static bool ResetPosOnStart = false;


    [SerializeField] private Slider volumeSlider;

    public void Awake()
    {
        if (DevRun)
        {
            DialogueEnabled = false;
            MusicEnabled = false;
            ResetPosOnStart = true;
        }

        SetBoolSetting(FreezeOnDialogue, false);
        if (PlayerPrefs.HasKey(Volume))
            volumeSlider.value = GetFloatSetting(Volume);
        else
            SetFloatSetting(Volume, volumeSlider.value);
        AudioListener.volume = 3;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SetFloatSetting(Volume, AudioListener.volume);
    }

    public static void SetBoolSetting(string setting, bool value)
    {
        PlayerPrefs.SetInt(setting, value ? 1 : 0);
    }

    public static bool GetBoolSetting(string setting)
    {
        return PlayerPrefs.GetInt(setting) == 1;
    }

    public static void SetFloatSetting(string setting, float value)
    {
        PlayerPrefs.SetFloat(setting, value);
    }

    public static float GetFloatSetting(string setting)
    {
        return PlayerPrefs.GetFloat(setting);
    }
}
