using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
{
    string fileName = "savefile";
    string path;
    SavedValue savedValue;

    private void Awake()
    {
        path = Application.persistentDataPath + "/" + fileName;
        AutoLoad();
        if (savedValue.stageIndex < SceneManager.sceneCountInBuildSettings - 1) // With the exception of the last stage
        {
            SceneManager.LoadScene(savedValue.stageIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
    public void AutoLoad()
    {
        SaveManager.Instance.Load<SavedValue>(path, LoadComplete, false);
    }

    private void LoadComplete(SavedValue data, SaveResult result, string message)
    {
        if (result == SaveResult.Success)
        {
            savedValue = data;
        }

        if (result == SaveResult.Error || result == SaveResult.EmptyData)
        {
            savedValue = new SavedValue();
            savedValue.stageIndex = 0;
        }
    }
}

