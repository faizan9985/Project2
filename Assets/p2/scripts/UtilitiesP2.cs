using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using System;
using Image = UnityEngine.UI.Image;
using System.Threading;

public class UtilitiesP2 : MonoBehaviour
{

   
    void OnEnable()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void ExitApp()
    {
#if UNITY_EDITOR
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        {
            Application.Quit();
        }
#endif
    }

}
