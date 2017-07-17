using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class TaskController : MonoBehaviour {

    public GameObject TaskTraySinglePrefab;
    public DTask dTask;
    public BuildingController buildingController;
    public TextMesh taskText;

    private int MouseOverCount = 0;
    private List<TaskTraySingle> listOfTraySingles;
    
	// Use this for initialization
	void Start ()
    {
        if (listOfTraySingles == null)
        {
            listOfTraySingles = new List<TaskTraySingle>();
        }
	}
	
	// Update is called once per frame
	void Update () {

	}

    #region MouseOver Functions

    public void IncreaseMouseOverCount()
    {
        MouseOverCount++;

        if (MouseOverCount == 1)
        {
            buildingController.IncreaseMouseOverCount();
        }
    }

    public void DecreaseMouseOverCount()
    {
        MouseOverCount--;

        if (MouseOverCount == 0)
        {
            buildingController.DecreaseMouseOverCount();
        }
    }

    #endregion


    // Generates the individual trays
    public void GenerateTaskTrays()
    {
        // If the list hasn't been instantiated by the Start() function
        if (listOfTraySingles == null)
        {
            listOfTraySingles = new List<TaskTraySingle>();
        }

        if (dTask == null)
        {
            throw new NullTaskException();
        }
        else
        {
            // Clear the current list of trays
            while (listOfTraySingles.Count > 0)
            {
                Destroy(listOfTraySingles[0].gameObject);
            }

            // Move the Text to the left of the boxes
            float xOffset =
                    ((float)(0) - Mathf.Floor((float)(dTask.MaxPeople) / 2.0f)) *
                    TaskTraySingle.WIDTH_CONST;

            taskText.text = dTask.Name;

            Vector3 newTaskTextPos = this.transform.position;
            newTaskTextPos.x += xOffset;
            newTaskTextPos.x -= taskText.characterSize * taskText.fontSize / 8.0f;

            taskText.transform.position = newTaskTextPos;

            // Generate the new trays
            for (int i = 0; i < dTask.MaxPeople; i++)
            {
                // Find where in the tray we will position each single
                xOffset = 
                    ((float)(i) - Mathf.Floor((float)(dTask.MaxPeople) / 2.0f)) * 
                    TaskTraySingle.WIDTH_CONST;

                GameObject go = Instantiate(TaskTraySinglePrefab, this.transform);                
                Vector3 currentPosition = go.transform.position;
                currentPosition.x += xOffset;
                go.transform.position = currentPosition;

                // Set the parent TaskController & task slot
                go.GetComponent<TaskTraySingle>().taskController = this;
                go.GetComponent<TaskTraySingle>().taskSlot = dTask.GetTaskSlot(i);
				dTask.GetTaskSlot(i).TaskTraySlot = go.GetComponent<TaskTraySingle>();
                go.GetComponent<TaskTraySingle>().UpdateSprite();

                listOfTraySingles.Add(go.GetComponent<TaskTraySingle>());
            }
        }
    }

    internal void UpdateSprite()
    {
        foreach(var entry in listOfTraySingles)
        {
            entry.UpdateSprite();
        }       
    }

    internal void ConnectToDataEngine(DTask dTask)
    {
        this.dTask = dTask;
        GenerateTaskTrays();
    }
}

#region Exceptions

public class NullTaskException : Exception
{
    public NullTaskException()
    {
    }

    public NullTaskException(string message) : base(message)
    {
    }

    public NullTaskException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NullTaskException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

#endregion
