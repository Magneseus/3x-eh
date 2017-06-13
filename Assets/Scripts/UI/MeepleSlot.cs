using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleSlot : MonoBehaviour {
  private Color startcolor;
  public bool full = false;
  private bool active = false;
  public Meeple meeple;
  SpriteRenderer renderer1;

	// Use this for initialization
	void Start () {
    renderer1 = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {
    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    // NOTE: handles mouseEnter and mouseLeave events instead of unities broken ass crappy ones
    if(!active && this.GetComponent<BoxCollider2D>().bounds.Contains(mousePos))
    OnActive();
    if(active && !this.GetComponent<BoxCollider2D>().bounds.Contains(mousePos))
      OnInactive();

    if(meeple != null)
  this.GetComponent(typeof(Collider2D)).transform.position = this.transform.position;
	}

  // NOTE: the default unity mouse event methods are broken when dragging a sprite (unless we can draw a sprite above the mouse cursor)
  void OnActive()

  {
    // Debug.Log("working");
    active = true;
    startcolor = renderer1.color;
    renderer1.color = Color.yellow;
    UIManager[] manager = FindObjectsOfType(typeof(UIManager)) as UIManager[];

    manager[0].activeSlot = this;
  }
  // void OnMouseOver()
  // {
  //
  //
  // }
  void OnInactive()
  {
    UIManager[] manager = FindObjectsOfType(typeof(UIManager)) as UIManager[];
    manager[0].activeSlot = null;
    // Debug.Log("working better");
    renderer1.color = startcolor;
    active = false;

  }
  public void addMeeple(Meeple m)
  {
    Debug.Log("Adding meeple to tray.");
    this.meeple = m;
    m.GetComponent(typeof(Collider2D)).transform.position =
  this.transform.position;
    // Count++;
    full = true;
  }
  public void removeMeeple(Meeple m)
  {
    Debug.Log("removing meeple to tray.");
    this.meeple = null;
    // m.GetComponent(typeof(Collider2D)).transform.position =
  // this.transform.position;
    // Count++;
    full = false;
  }
}
