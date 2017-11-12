using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBaseFeatures : MonoBehaviour {

    //private GameObject _thisObject;
    protected GameObject thisObject;
    protected bool isInitialized = false;
    public delegate void SimpleAction();

    protected virtual void Awake()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        if (isInitialized)
        {
            return;
        }
        else
        {
            isInitialized = true;
        }

        thisObject = gameObject;
    }

	public virtual void ShowMenu(bool isShow)
    {
        thisObject.SetActive(isShow);
    }
}
