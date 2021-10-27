using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1 : MonoBehaviour
{
    public int gameOverCount = 5;
    [SceneName]
    public string stage2;

    public void NextStage()
    {
        SceneManager.LoadScene(stage2);
    }
}