using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MeepleController : MonoBehaviour {

    public DPerson dPerson;

    public Collider2D boxCollider;

    private TaskTraySingle parentTray;
    private Transform returnParent;
    private bool dragging;
    private List<Collider2D> collisions;

	// Use this for initialization
	void Start () {
        dragging = false;
        collisions = new List<Collider2D>();
        boxCollider = GetComponent<Collider2D>();
        parentTray = null;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    internal void ConnectToDataEngine(DGame dGame, string cityName)
    {
        dPerson= new DPerson(dGame.Cities[cityName], this);
    }

    #region MouseOver Functions

    public void OnMouseEnter()
    {
        if (parentTray != null)
        {
            parentTray.taskController.IncreaseMouseOverCount();
        }
    }

    public void OnMouseExit()
    {
        if (parentTray != null)
        {
            parentTray.taskController.DecreaseMouseOverCount();
        }
    }

    #endregion

    #region Drag Functions
    public void OnMouseDown()
    {
        // Store where we originated
        returnParent = this.transform.parent;
        dragging = true;
        this.transform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        if (parentTray != null)
            parentTray.taskController.IncreaseMouseOverCount();
    }

    public void OnMouseDrag()
    {
        if (dragging)
        {
            // Move to where the mouse cursor is
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 1;
            this.transform.position = pos;
        }
    }

    public void OnMouseUp()
    {
        TaskTraySingle oldParentTray = parentTray;

        if (collisions.Count == 0)
        {
            // Reset position
            this.transform.parent = returnParent;
            this.transform.localPosition = new Vector3(0, 0, -3);
        }
        else
        {
            // Find the closest collider and make that Tray the new parent
            float minDist = float.MaxValue;
            TaskTraySingle closestTray = null;
            foreach (Collider2D col in collisions)
            {
                ColliderDistance2D colDist = boxCollider.Distance(col);
                if (colDist.isValid && colDist.distance < minDist)
                {
                    minDist = colDist.distance;
                    closestTray = col.GetComponent<TaskTraySingle>();
                }
            }

            // If the closest tray is full, reset
            if (closestTray.taskSlot.Person != null)
            {
                // Reset position
                this.transform.parent = returnParent;
                this.transform.localPosition = new Vector3(0, 0, -3);
            }
            else
            {
                // Set the new task
                dPerson.SetTaskSlot(closestTray.taskSlot);

                // Set the new parent transform
                this.transform.parent = closestTray.transform;
                this.transform.localPosition = new Vector3(0, 0, -3);
                this.parentTray = closestTray;
            }
        }


        dragging = false;
        this.transform.gameObject.layer = LayerMask.NameToLayer("Default");

        if (oldParentTray != null)
            oldParentTray.taskController.DecreaseMouseOverCount();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TaskTray")
        {
            collisions.Add(collision);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TaskTray")
        {
            collisions.Remove(collision);
        }
    }
    #endregion
}
