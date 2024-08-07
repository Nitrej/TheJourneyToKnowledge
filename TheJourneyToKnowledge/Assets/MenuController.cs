using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnPlayerVsPlayerButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    public void OnPlayerVsAiButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    public void OnOptionsButtonClicked()
    {

    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
