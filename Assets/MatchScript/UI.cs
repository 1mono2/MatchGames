using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
 
    [SerializeField]
    GameController gameController;
    [SerializeField]
    Text counter;
    [SerializeField]
    Button retryButton;
    [SerializeField]
    Text centerText;
    [SerializeField]
    Button nextButton;


    const string stClear = "Clear!";

    // Start is called before the first frame update
    void Start()
    {
        //Update();
    }

    // Update is called once per frame
    void Update()
    {
        counter.text = gameController.gameOverCount.Value.ToString();
    }

    internal void SetRetryButton()
    {
        retryButton.gameObject.SetActive(true);
    }

    internal void SetClearText()
    {
        centerText.text = stClear;
    }

    internal void SetNextButton()
    {
        nextButton.gameObject.SetActive(true);
    }

    public void NextStage()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if(index < SceneManager.sceneCountInBuildSettings -1)
        {
            index++;
        }
        else
        {
            index = 1;  // Return the First Stage without Preload
        }
        
        SceneManager.LoadScene(index);
        Debug.Log(index);
    }
}
