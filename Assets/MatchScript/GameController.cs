using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    UI ui;
    [SerializeField]
    GameObject matchBall;
    [SerializeField]
    GameObject inverseMatchBall;
    MatchBallController matchBallController;
    InverseMatchBallController inverseMatchBallController;

    [SerializeField]
    int gameOver = 5;
    static int _gameOverCount;
    static internal Vector3 matchBallInitPos;
    static internal Vector3 inverseMatchBallInitPos;
    internal NotificationObject<int> gameOverCount;
    internal NotificationObject<bool> clearFlag = new NotificationObject<bool>(false);


    string fileName = "savefile";
    string path;
    SavedValue savedValue;
    // const
    const int MINUS_GAMEOVER_COUNT = -1;
    const int DONT_MOVE = 0;
    const int VIBRATE = 300;

    private void Start()
    {
        _gameOverCount = gameOver;
        gameOverCount = new NotificationObject<int>(_gameOverCount);
        matchBallController = matchBall.GetComponent<MatchBallController>();
        inverseMatchBallController = inverseMatchBall.GetComponent<InverseMatchBallController>();
        gameOverCount.action += ChangeCount;
        clearFlag.action += ChangeClearFlag;
        matchBallInitPos = matchBall.transform.position;
        inverseMatchBallInitPos = inverseMatchBall.transform.position;
    }

    internal void ChangeCount(int count)
    {
        if (gameOverCount.Value <= DONT_MOVE)
        {
            ui.SetRetryButton();
        }
    }

    internal void ChangeClearFlag(bool flag)
    {
        if (clearFlag.Value == true)
        {
            ui.SetRetryButton();
        }
    }

    public void InitializeStage()
    {
        matchBall.transform.position = matchBallInitPos;
        inverseMatchBall.transform.position = inverseMatchBallInitPos;
        gameOverCount.Value = _gameOverCount;
        clearFlag.Value = false;

    }

    public void OneMoreCount()
    {
        if (gameOverCount.Value > DONT_MOVE)
        {
            gameOverCount.Value++;
        }
    }

    internal void Clear()
    {
        AutoSave();
    }
    private void Awake()
    {
        path = Application.persistentDataPath + "/" + fileName;
        savedValue = new SavedValue();
    }

    public void AutoSave()
    {
        savedValue.stageIndex = SceneManager.GetActiveScene().buildIndex;
        SaveManager.Instance.Save(savedValue, path, SaveComplete, false);
    }

    private void SaveComplete(SaveResult result, string message)
    {
        if (result == SaveResult.Error)
        {
            Debug.Log("Save Error" + message);
        }
    }
    public void MoveToNorth()
    {
        if (gameOverCount.Value > DONT_MOVE && clearFlag.Value == false)
        {
            SettingButton.Vibrate(VIBRATE);
            matchBallController.Move(MatchBallController.Direction.N);
            inverseMatchBallController.Move(InverseMatchBallController.Direction.S);
            gameOverCount.Value += MINUS_GAMEOVER_COUNT;
        }
    }

    public void MoveToSouth()
    {
        if (gameOverCount.Value > DONT_MOVE && clearFlag.Value == false)
        {
            SettingButton.Vibrate(VIBRATE);
            matchBallController.Move(MatchBallController.Direction.S);
            inverseMatchBallController.Move(InverseMatchBallController.Direction.N);
            gameOverCount.Value += MINUS_GAMEOVER_COUNT;
        }
    }
    public void MoveToEast()
    {
        if (gameOverCount.Value > DONT_MOVE && clearFlag.Value == false)
        {
            SettingButton.Vibrate(VIBRATE);
            matchBallController.Move(MatchBallController.Direction.E);
            inverseMatchBallController.Move(InverseMatchBallController.Direction.W);
            gameOverCount.Value += MINUS_GAMEOVER_COUNT;
        }
    }
    public void MoveToWest()
    {
        if (gameOverCount.Value > DONT_MOVE && clearFlag.Value == false)
        {
            SettingButton.Vibrate(VIBRATE);
            matchBallController.Move(MatchBallController.Direction.W);
            inverseMatchBallController.Move(InverseMatchBallController.Direction.E);
            gameOverCount.Value += MINUS_GAMEOVER_COUNT;
        }
    }


   
}

