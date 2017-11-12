using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameActionsBehaviour : CanvasBaseFeatures {

    private Button loadBtn;
    private Button saveBtn;
    private Button cancelBtn;
    private Text gameOverText;

    public const string GAME_SAVE_NAME = "gameSave";
    public const string UNUSED = "unused";

    private const string GAME_WIN_TEXT = "Победа!";
    private const string GAME_LOSE_TEXT = "Поражение!";

    public enum ActionType
    {
        newGame,
        loadGame,
        saveGame
    }

    public delegate void EventAction(ActionType currentType);
    public event EventAction gameActionsMenuEvent;

    protected override void Awake()
    {
        base.Awake();

    }

    public override void Initialize()
    {
        base.Initialize();
        loadBtn = transform.Find("mainPanel/loadBtn").GetComponent<Button>();
        saveBtn = transform.Find("mainPanel/saveBtn").GetComponent<Button>();
        cancelBtn = transform.Find("mainPanel/cancelBtn").GetComponent<Button>();
        gameOverText = transform.Find("gameOverText").GetComponent<Text>();
    }

    void OnEnable()
    {

        if (PlayerPrefs.GetString(GAME_SAVE_NAME, UNUSED) == UNUSED)
        {
            loadBtn.interactable = false;
        }
        else
        {
            loadBtn.interactable = true;
        }

    }

    private void SetUpState(bool isStartState)
    {
        saveBtn.gameObject.SetActive(!isStartState);
        cancelBtn.gameObject.SetActive(!isStartState);
    }

    public void NewGamePressed()
    {
        if(gameActionsMenuEvent != null)
        {
            gameActionsMenuEvent(ActionType.newGame);
        }
    }

    public override void ShowMenu(bool isShow)
    {
        base.ShowMenu(isShow);

        if (GameController.instance.currentGameState != GameController.GameState.gamePlay)
        {
            SetUpState(true);
        }
        else
        {
            SetUpState(false);
        }

        if(GameController.instance.currentGameState != GameController.GameState.gameOver)
        {
            gameOverText.gameObject.SetActive(false);
        }

    }

    public void SaveGamePressed()
    {
        if (gameActionsMenuEvent != null)
        {
            gameActionsMenuEvent(ActionType.saveGame);
        }

        ShowMenu(false);
    }

    public void CancelBtnPressed()
    {
        ShowMenu(false);
    }

    public void LoadGamePressed()
    {
        if (gameActionsMenuEvent != null)
        {
            gameActionsMenuEvent(ActionType.loadGame);
        }

        ShowMenu(false);
    }

    public void ShowGameOverText(bool isWinner)
    {
        gameOverText.gameObject.SetActive(true);

        if (isWinner)
        {
            gameOverText.text = GAME_WIN_TEXT;
        }
        else
        {
            gameOverText.text = GAME_LOSE_TEXT;
        }
    }


}
