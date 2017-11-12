using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineCellBehaviour : CellBaseFeatures {

    [SerializeField]
    private Sprite mineSprite;

    public override void OnClick(int btnIndex)
    {       
        if(btnIndex == 0)
        {
            parentScript.GameOver();
        }
        else
        {
            flag.SetActive(!flag.activeInHierarchy);
        }
    }

    public override void SetParent(GamefieldController newParent)
    {
        parentScript = newParent;
    }

    public override void SetFlag(GameObject newFlag)
    {
        flag = newFlag;
    }

    public override void SetPosition(int x, int y)
    {
        cellPosition = new cellIndex(x, y);
    }

    public override void Open(int countMines)
    {
        GetComponent<SpriteRenderer>().sprite = mineSprite;
        flag.SetActive(false);
    }
}
