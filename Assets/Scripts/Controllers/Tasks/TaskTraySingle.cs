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
    private BoxCollider2D boxCollider;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/task-storage-damaged");
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        spriteRenderer.gameObject.SetActive(taskSlot.Enabled);
        boxCollider.enabled = taskSlot.Enabled;
        

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
            if (taskSlot.Task.Output.Name.Contains("Food"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-food-infected");
            if (taskSlot.Task.Output.Name.Contains("Fuel"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-fuel-infected");
            if (taskSlot.Task.Output.Name.Contains("Materials"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-materials-infected");
            if (taskSlot.Task.Output.Name.Contains("Shelter"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-shelter-infected");
            if (taskSlot.Task.Output.Name.Contains("Storage"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-storage-infected");
        }
        else if (taskSlot.Damaged)
        {
            if (taskSlot.Task.Output.Name.Contains("Food"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-food-damaged");
            if (taskSlot.Task.Output.Name.Contains("Fuel"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-fuel-damaged");
            if (taskSlot.Task.Output.Name.Contains("Materials"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-materials-damaged");
            if (taskSlot.Task.Output.Name.Contains("Shelter"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-shelter-damaged");
            if (taskSlot.Task.Output.Name.Contains("Storage"))
                spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-storage-damaged");
        }
        else
        {
            if (taskSlot.Task.Output != null)
            {
                if (taskSlot.Task.Output.Name.Contains("Food"))
                    spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-food-clean");
                if (taskSlot.Task.Output.Name.Contains("Fuel"))
                    spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-fuel-clean");
                if (taskSlot.Task.Output.Name.Contains("Materials"))
                    spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-materials-clean");
                if (taskSlot.Task.Output.Name.Contains("Shelter"))
                    spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-shelter-clean");
                if (taskSlot.Task.Output.Name.Contains("Storage"))
                    spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-storage-clean");
            }
            else
            {
                if (taskSlot.Task.Name.Contains("Assess"))
                    spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-assess-clean");
                if (taskSlot.Task == taskSlot.Task.Building.City.townHall.getExploreTask())
                
                    spriteRenderer.sprite = Resources.Load<Sprite>(@"Sprites/UI/task-explore-clean");
            }
            
        }
    }
    #endregion
}
