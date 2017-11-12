using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameDifficultBehaviour : CanvasBaseFeatures {

    private Button cancelBtn;

    public event SimpleAction cancelPressed;

    public delegate void DifficultEvent(GameHardness currentDifficult);
    public event DifficultEvent difficultChoosed;
    public enum GameHardness
    {
        easy = 0,
        normal = 1,
        hard = 2
    }

    public override void Initialize()
    {
        base.Initialize();
        cancelBtn = transform.Find("mainPanel/cancelBtn").GetComponent<Button>();
    }

    public override void ShowMenu(bool isShow)
    {
        base.ShowMenu(isShow);
    }

    public void CancelBtnPressed()
    {
        if(cancelPressed != null)
        {
            cancelPressed();
        }
    }

    public void HardnessBtnPressed(int hardnessIndex)
    {
        if(difficultChoosed != null)
        {
            difficultChoosed((GameHardness)hardnessIndex);
        }

        thisObject.SetActive(false);
    }

}
