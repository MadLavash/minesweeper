using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    public CanvasBaseFeatures inGameUI { get; private set; }
    public gameActionsBehaviour gameActionsUI { get; private set; }
    public gameDifficultBehaviour gameDifficultUI { get; private set; }

    public enum MenuNames
    {
        gameActions,
        gameDifficult,
        inGame
    }

    private void Awake()
    {
        inGameUI = transform.Find("inGame").GetComponent<CanvasBaseFeatures>();
        inGameUI.Initialize();

        gameActionsUI = transform.Find("gameActions").GetComponent<gameActionsBehaviour>();
        gameActionsUI.Initialize();

        gameDifficultUI = transform.Find("gameDifficult").GetComponent<gameDifficultBehaviour>();
        gameDifficultUI.cancelPressed += CancelDifficultHandling;
        gameDifficultUI.Initialize();      
    }

    public void ShowMenu(MenuNames currentName)
    {
        switch (currentName)
        {
            case MenuNames.gameActions:
                gameActionsUI.ShowMenu(true);
                //inGameUI.ShowMenu(false);
                gameDifficultUI.ShowMenu(false);
                break;
            case MenuNames.gameDifficult:
                gameActionsUI.ShowMenu(false);
                //inGameUI.ShowMenu(false);
                gameDifficultUI.ShowMenu(true);
                break;
            case MenuNames.inGame:
                //gameActionsUI.ShowMenu(false);
                inGameUI.ShowMenu(true);
                //gameDifficultUI.ShowMenu(false);
                break;
        }

        if(GameController.instance.currentGameState != GameController.GameState.gamePlay)
        {
            inGameUI.ShowMenu(false);
        }
        else
        {
            inGameUI.ShowMenu(true);
        }

    }

    public void CancelDifficultHandling()
    {
        ShowMenu(MenuNames.gameActions);
    }

    public void ShowFinalUI(bool isWinner)
    {
        gameActionsUI.ShowGameOverText(isWinner);
    }
}
