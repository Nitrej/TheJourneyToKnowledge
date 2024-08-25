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
    private IEnumerator LoadLevelWithAnimation(int levelIndex)
    {
        alpha.SetTrigger("Start");

        yield return new WaitForSeconds(animationTime);

        SceneManager.LoadScene(levelIndex);
    }

}
