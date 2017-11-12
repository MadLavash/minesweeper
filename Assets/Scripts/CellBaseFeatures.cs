using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CellBaseFeatures : MonoBehaviour {

    public struct cellIndex
    {
       public int x;
       public int y;

        public cellIndex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public cellIndex cellPosition { get; protected set; }

    protected GamefieldController parentScript;
    public GameObject flag { get; protected set; }


    public abstract void OnClick(int btnIndex);

    public abstract void SetParent(GamefieldController newParent);

    public abstract void SetFlag(GameObject newFlag);

    public abstract void SetPosition(int x, int y);

    public abstract void Open(int minesCount);

}
