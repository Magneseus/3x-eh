
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeepleControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject draggingItem;

    public static Transform startParent;

    private Vector3 startPosition;
    private string realParentName;

	public void Start(){
        //inputPanel.SetActive (false);
        realParentName = "Slot";
	}

	public void Update(){
		
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
		draggingItem = gameObject;
        Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f);
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }

		/*if (buildingPanel.active == true){
			inputPanel.SetActive(true);
		}*/
    }

    public void SetRealParentName(string realParentName)
    {
        this.realParentName = realParentName;
    }

    public string GetRealParentName()
    {
        return realParentName;
    }
}