using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GamefieldController : MonoBehaviour
{

    [SerializeField]
    private GameObject bgCell;
    [SerializeField]
    private GameObject defaultCell;
    [SerializeField]
    private GameObject mineCell;
    [SerializeField]
    private GameObject flag;

    private GameObject[,] fieldBg;
    private CellBaseFeatures[,] fieldCells;

    private byte[,] minesField;

    private const int EASY_FIELD_SIZE = 10;
    private const int EASY_MINES_COUNT = 10;
    private const int NORMAL_FIELD_SIZE = 20;
    private const int NORMAL_MINES_COUNT = 40;
    private const int HARD_FIELD_SIZE = 30;
    private const int HARD_MINES_COUNT = 90;

    private const string FIELD_SIZE_SAVE = "fieldSize";
    private const string MINES_COUNT_SAVE = "minesCount";
    private const string MINEFIELD_SAVE = "minefield";
    private const string GAMEFIELD_SAVE = "gamefield";

    private float sizeOfCell = 64f / 200f;

    private int fieldSize;
    private int minesCount;
    private int cellsToOpen;

    public delegate void BoolAction(bool isWinner);
    public event BoolAction gameOver;


    public void CreateField(gameDifficultBehaviour.GameHardness currentHardness)
    {
        if(fieldBg != null)
        {
            DestroyField();
        }

        switch (currentHardness)
        {
            case gameDifficultBehaviour.GameHardness.easy:
                fieldSize = EASY_FIELD_SIZE;
                minesCount = EASY_MINES_COUNT;
                break;
            case gameDifficultBehaviour.GameHardness.normal:
                fieldSize = NORMAL_FIELD_SIZE;
                minesCount = NORMAL_MINES_COUNT;
                break;
            case gameDifficultBehaviour.GameHardness.hard:
                fieldSize = HARD_FIELD_SIZE;
                minesCount = HARD_MINES_COUNT;
                break;
        }

        CreateMines(fieldSize, minesCount);

        CreateMainField();

    }

    private void CreateMainField()
    {
        cellsToOpen = (fieldSize * fieldSize) - minesCount;

        Vector2 startPosition;
        startPosition.x = -((fieldSize * sizeOfCell) / 2f - sizeOfCell / 2f);
        startPosition.y = -startPosition.x;

        GameObject currentFlag;

        fieldBg = new GameObject[fieldSize, fieldSize];
        fieldCells = new CellBaseFeatures[fieldSize, fieldSize];

        for (int column = 0; column < fieldSize; column++)
        {
            for (int row = 0; row < fieldSize; row++)
            {
                fieldBg[row, column] = Instantiate(bgCell, startPosition, Quaternion.identity, transform);

                if (minesField[row, column] != 1)
                {
                    fieldCells[row, column] = Instantiate(defaultCell, startPosition, Quaternion.identity, transform).GetComponent<CellBaseFeatures>();
                }
                else
                {
                    fieldCells[row, column] = Instantiate(mineCell, startPosition, Quaternion.identity, transform).GetComponent<CellBaseFeatures>();
                }

                fieldCells[row, column].SetPosition(row, column);
                fieldCells[row, column].SetParent(this);
                currentFlag = Instantiate(flag, startPosition, Quaternion.identity, transform);
                currentFlag.SetActive(false);
                fieldCells[row, column].SetFlag(currentFlag);

                if (row == fieldSize - 1)
                {
                    startPosition.x = -((fieldSize * sizeOfCell) / 2f - sizeOfCell / 2f);
                    startPosition.y -= sizeOfCell;
                }
                else
                {
                    startPosition.x += sizeOfCell;
                }
            }
        }
    }

    private void CreateMines(int fieldSize, int minesCount)
    {
        minesField = new byte[fieldSize, fieldSize];

        int currentMinesCount = minesCount;
        int x = 0;
        int y = 0;

        while (currentMinesCount > 0)
        {
            x = UnityEngine.Random.Range(0, fieldSize);
            y = UnityEngine.Random.Range(0, fieldSize);

            if (minesField[x, y] == 0)
            {
                minesField[x, y] = 1;
                currentMinesCount--;
            }
        }
    }

    public void OpenDefaultCell(int cellX, int cellY, bool continueCycle = true)
    {
        int minesCount = 0;

        for (int row = cellX - 1; row <= cellX + 1; row++)
        {
            for (int column = cellY - 1; column <= cellY + 1; column++)
            {
                if (row >= 0 && row < fieldSize && column >= 0 && column < fieldSize)
                {
                    if(minesField[row, column] == 1)
                    {
                        minesCount++;
                    }
                }
            }
        }

        fieldCells[cellX, cellY].Open(minesCount);
        //CellOpened();

        if (minesCount == 0 && continueCycle)
        {
            OpenNeighbor(cellX, cellY);
        }
    }

    private void OpenNeighbor(int cellX, int cellY)
    {
        for (int row = cellX - 1; row <= cellX + 1; row++)
        {
            for (int column = cellY - 1; column <= cellY + 1; column++)
            {
                if (row >= 0 && row < fieldSize && column >= 0 && column < fieldSize)
                {
                    if (row != cellX || column != cellY)
                    {
                        if(minesField[row, column] != 1 && !(fieldCells[row, column] as DefaultCellBehaviour).isOpened)
                        {
                            OpenDefaultCell(row, column);
                        }
        
                    }
                }
            }
        }

    }

    public void GameOver()
    {
        for (int column = 0; column < fieldSize; column++)
        {
            for (int row = 0; row < fieldSize; row++)
            {
                if(minesField[row, column] == 1)
                {
                    fieldCells[row, column].Open(0);
                    fieldBg[row, column].GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    OpenDefaultCell(row, column, false);
                }
            }
        }

        if(gameOver != null)
        {
            gameOver(false);
        }
    }

    public void SaveGamefield()
    {
        Dictionary<string, object> saveData = new Dictionary<string, object>();

        saveData.Add(FIELD_SIZE_SAVE, fieldSize);
        saveData.Add(MINES_COUNT_SAVE, minesCount);
        saveData.Add(MINEFIELD_SAVE, CreateMinesData());
        saveData.Add(GAMEFIELD_SAVE, CreateFieldData());

        PlayerPrefs.SetString(gameActionsBehaviour.GAME_SAVE_NAME, MiniJSON.Json.Serialize(saveData));
        
    }

    private string CreateMinesData()
    {
        string minesData = "";

        for (int column = 0; column < fieldSize; column++)
        {
            for (int row = 0; row < fieldSize; row++)
            {
                minesData += minesField[row, column];
            }
        }


        return minesData;
    }

    private string CreateFieldData()
    {
        //0 - cell closed
        //1 - cell opened
        //2 - cell flagged

       string fieldData = "";

        for (int column = 0; column < fieldSize; column++)
        {
            for (int row = 0; row < fieldSize; row++)
            {
                if (fieldCells[row, column].flag.activeInHierarchy)
                {
                    fieldData += 2;
                }
                else if (fieldCells[row, column] is DefaultCellBehaviour)
                {                  
                    if ((fieldCells[row, column] as DefaultCellBehaviour).isOpened)
                    {
                        fieldData += 1;
                    }
                    else
                    {
                        fieldData += 0;
                    }
                }
                else
                {
                    fieldData += 0;
                }
            }
        }

        return fieldData;
    }

    public void RestoreGamefield()
    {
        if (PlayerPrefs.GetString(gameActionsBehaviour.GAME_SAVE_NAME, gameActionsBehaviour.UNUSED) == gameActionsBehaviour.UNUSED)
        {
            return;
        }

        if (fieldBg != null)
        {
            DestroyField();
        }


        Dictionary<string, object> saveData = MiniJSON.Json.Deserialize(PlayerPrefs.GetString(gameActionsBehaviour.GAME_SAVE_NAME, 
            gameActionsBehaviour.UNUSED) as string) as Dictionary<string, object>;

        fieldSize = Convert.ToInt32(saveData[FIELD_SIZE_SAVE]);
        minesCount = Convert.ToInt32(saveData[MINES_COUNT_SAVE]);
        minesField = new byte[fieldSize, fieldSize];

        string minesData = Convert.ToString(saveData[MINEFIELD_SAVE]);

        int x = 0;
        int y = 0;

        for (int index = 0; index < minesData.Length; index++)
        {
            minesField[x, y] = (byte)char.GetNumericValue(minesData[index]);
            //print(minesField[x, y]);
            x++;

            if (x == fieldSize)
            {
                x = 0;
                y++;
            }

        }

        CreateMainField();

        string fieldData = Convert.ToString(saveData[GAMEFIELD_SAVE]);
        //print(fieldData);
         x = 0;
         y = 0;

        for (int index = 0; index < fieldData.Length; index++)
        {
            if (minesField[x, y] != 1 && ((byte)char.GetNumericValue(fieldData[index]) == 1))
            {
                if (!(fieldCells[x, y] as DefaultCellBehaviour).isOpened)
                {

                    OpenDefaultCell(x, y, false);
                }
            }

            if ((byte)char.GetNumericValue(fieldData[index]) == 2)
            {
                fieldCells[x, y].flag.SetActive(true);
            }            

            x++;

            if (x == fieldSize)
            {
                x = 0;
                y++;
            }

        }


        /*for(int index = 0; index < test.Length; index++)
        {
            print(test[index]);
        }*/

        /*IEnumerable restoringNum = saveData[GAMEFIELD_SAVE] as IEnumerable;

        x = 0;
        y = 0;

        foreach (object element in restoringNum)
        {
            if (minesField[x, y] != 1 && (Convert.ToByte(element) == 1))
            {
                if (!(fieldCells[x, y] as DefaultCellBehaviour).isOpened)
                {
                    OpenDefaultCell(x, y, false);
                }
            }

            if (Convert.ToByte(element) == 2)
            {
                fieldCells[x, y].flag.SetActive(true);
            }

            x++;
            if (x == fieldSize)
            {
                x = 0;
                y++;
            }
        }*/
    }

    private void DestroyField()
    {
        for (int column = 0; column < fieldSize; column++)
        {
            for (int row = 0; row < fieldSize; row++)
            {
                Destroy(fieldBg[row, column]);
                Destroy(fieldCells[row, column].flag);
                Destroy(fieldCells[row, column].gameObject);
               
            }
        }

        fieldBg = null;
        fieldCells = null;
    }

    public void CellOpened()
    {
        cellsToOpen--;

        if (cellsToOpen <= 0)
        {
            if(gameOver != null)
            {
                gameOver(true);
            }
        }
    }



}
