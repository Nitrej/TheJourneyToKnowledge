using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator alpha;
    public float animationTime;

    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(LoadLevelWithAnimation(levelIndex));
    }

    public void LoadLevelAsync(int levelIndex)
    {
        StartCoroutine(LoadLevelWithAnimationAsync(levelIndex));
    }

    private IEnumerator LoadLevelWithAnimation(int levelIndex)
    {
        alpha.SetTrigger("Start");

        yield return new WaitForSeconds(animationTime);

        SceneManager.LoadScene(levelIndex);
    }
    private IEnumerator LoadLevelWithAnimationAsync(int levelIndex)
    {
        alpha.SetTrigger("Start");

        yield return new WaitForSeconds(animationTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
