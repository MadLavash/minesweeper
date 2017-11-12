using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultCellBehaviour : CellBaseFeatures {

    private TextMesh currentText;
    private int minesCount;
    private GameObject hiderSprite;
    public bool isOpened = false;

    void Awake()
    {
        currentText = transform.Find("minesCount").GetComponent<TextMesh>();
        hiderSprite = transform.Find("cellSprite").gameObject;
    }

    public override void OnClick(int btnIndex)
    {
        if(btnIndex == 0)
        {
           parentScript.OpenDefaultCell(cellPosition.x, cellPosition.y);
        }
        else
        {
            if (!isOpened)
            {
                flag.SetActive(!flag.activeInHierarchy);
            }
  
        }
    }

    public override void SetPosition(int x, int y)
    {
        cellPosition = new cellIndex(x, y);
    }

    public override void SetParent(GamefieldController newParent)
    {
        parentScript = newParent;
    }

    public override void SetFlag(GameObject newFlag)
    {
        flag = newFlag;
    }

    public override void Open(int newMinesCount)
    {
        if (isOpened)
        {
            return;
        }
        else
        {
            isOpened = true;
            parentScript.CellOpened();
        }

        minesCount = newMinesCount;
        flag.SetActive(false);

        if (minesCount > 0)
        {
            currentText.text = minesCount.ToString();
            currentText.GetComponent<MeshRenderer>().sortingOrder = 10;
         
        }
        else
        {
            currentText.gameObject.SetActive(false);
        }

        hiderSprite.SetActive(false);
    }
}
