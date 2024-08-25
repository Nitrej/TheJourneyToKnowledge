using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public LevelLoader LevelLoader;
    public void OnPlayerVsPlayerButtonClicked()
    {
        LevelLoader.LoadLevel(1);
    }
    public void OnPlayerVsAiButtonClicked()
    {
        LevelLoader.LoadLevel(1);
    }
    public void OnOptionsButtonClicked()
    {

    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
