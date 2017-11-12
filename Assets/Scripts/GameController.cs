using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance { get; private set; }
    private CanvasController canvasScript;
    public enum GameState
    {
        gameStart,
        gamePlay,
        gameOver
    }
    public GameState currentGameState;
    private GamefieldController fieldScript;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);            
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentGameState = GameState.gameStart;
        canvasScript = GameObject.Find("Canvas").GetComponent<CanvasController>();
        fieldScript = GameObject.Find("gameField").GetComponent<GamefieldController>();
        SubscribeToEvent();
    }

    void Start()
    {
        canvasScript.ShowMenu(CanvasController.MenuNames.gameActions);
    }

    void SubscribeToEvent()
    {
        canvasScript.gameActionsUI.gameActionsMenuEvent += GameEventHandling;
        canvasScript.gameDifficultUI.difficultChoosed += NewGameHandling;
        fieldScript.gameOver += GameOverHandling;
    }

    private void NewGameHandling(gameDifficultBehaviour.GameHardness currentDifficult)
    {
        currentGameState = GameState.gamePlay;
        canvasScript.ShowMenu(CanvasController.MenuNames.inGame);
        fieldScript.CreateField(currentDifficult);
      
    }

    private void GameEventHandling(gameActionsBehaviour.ActionType currentType)
    {
        switch (currentType)
        {
            case gameActionsBehaviour.ActionType.newGame:
                canvasScript.ShowMenu(CanvasController.MenuNames.gameDifficult);
                break;
            case gameActionsBehaviour.ActionType.saveGame:
                fieldScript.SaveGamefield();
                break;
            case gameActionsBehaviour.ActionType.loadGame:
                currentGameState = GameState.gamePlay;
                fieldScript.RestoreGamefield();
                canvasScript.ShowMenu(CanvasController.MenuNames.inGame);
                break;
        }
    }

    private void GameOverHandling(bool isWinner)
    {
        currentGameState = GameState.gameOver;
        canvasScript.ShowMenu(CanvasController.MenuNames.gameActions);
        canvasScript.ShowFinalUI(isWinner);
       
    }

}
