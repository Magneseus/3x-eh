using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TaskTraySingle : MonoBehaviour {

    public static readonly float WIDTH_CONST = 3.25f / 4.0f;
    public TaskController taskController;
    public DTaskSlot taskSlot;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    #region MouseOver Functions
    public void OnMouseEnter()
    {
        taskController.IncreaseMouseOverCount();
    }

    public void OnMouseExit()
    {
        taskController.DecreaseMouseOverCount();
    }

    internal void UpdateSprite()
    {
        if (taskSlot.Infected)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/food-infected");
        }
        else if (taskSlot.Damaged)
        {
            spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/food-damaged");
        }
        else
        {
            spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/food-clean");
        }
    }
    #endregion
}
