using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ClickController : MonoBehaviour
{
    private const string GAMECELL_TAG = "gameCell";

    void Update()
    {
        if (!IsPointerOverUIObject())
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnClick(1);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                OnClick(0);
            }
        }         
    }
   
    void OnClick(int buttonIndex)
    {
        Vector3 positionInWorldPoints;
        positionInWorldPoints = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D touchedObject = null;
        touchedObject = Physics2D.OverlapPoint(positionInWorldPoints);

        if (touchedObject != null)
        {
            if (touchedObject.tag == GAMECELL_TAG)
            {
                touchedObject.SendMessage("OnClick", buttonIndex);
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}