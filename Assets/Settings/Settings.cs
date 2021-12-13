using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public readonly static string FreezeOnDialogue = "FreezeOnDialogue";

    public void Start()
    {
        SetBoolSetting(FreezeOnDialogue, false);
    }

    public static void SetBoolSetting(string setting, bool value)
    {
        PlayerPrefs.SetInt(setting, value ? 1 : 0);
    }

    public static bool GetBoolSetting(string setting)
    {
        return PlayerPrefs.GetInt(setting) == 1;
    }

}
