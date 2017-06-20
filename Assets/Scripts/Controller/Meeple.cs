using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class Meeple :  MonoBehaviour
{
  public MeepleSlot home = null;
  public Vector3 origin;
  public Vector2 mousePos;
  bool dragging = false;

  void Start()
  {

  }
  void Update()
  {
    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    if(Input.GetMouseButtonDown(0) && this.GetComponent<CircleCollider2D>().bounds.Contains(mousePos))
    {


      dragging = true;
      origin = this.transform.position;

      // Debug.Log("Fuck unity interfaces");
    }
    // }else
    // {
    //   dragging = false;
    //   Debug.Log("Fuck Unity in general..");
    // }
    if(dragging)
    {
      this.GetComponent(typeof(Collider2D)).transform.position = mousePos;
      // Debug.Log("I are dragging");
    }
        if (Input.GetMouseButtonUp(0))
        {
          if(dragging)
          {
            OnEndDrag();
          }
        }
  }

   public void OnEndDrag()
   {
     UIManager[] manager = FindObjectsOfType(typeof(UIManager)) as UIManager[];

     if(manager[0].activeSlot != null)
       {
          Debug.Log("meeple added");
          if(!manager[0].activeSlot.full)
          {
            if(home != null ) home.removeMeeple(this);
              manager[0].activeSlot.addMeeple(this);
              this.home = manager[0].activeSlot;
          }
          else //didnt add to a new position/ invalid selection
          {
            this.GetComponent(typeof(Collider2D)).transform.position = origin;
          }
          dragging = false;
     }
  }


}
