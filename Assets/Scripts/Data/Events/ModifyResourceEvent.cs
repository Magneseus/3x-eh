using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyResourceEvent : DEvent {

    public static void AddEventFromJSON(DCity currentCity, JSONNode json)
    {
        var resources = new DResource[json["outcomes"].AsArray.Count];
        for (int i = 0; i < resources.Length; i++)
        {
            int amount = json["resources"][i]["value"];
            string name = json["resources"][i]["type"];
            resources[i] = DResource.Create(name, amount);
        }

        DEventSystem.AddEvent(new ModifyResourceEvent(
            json["promptText"],
            currentCity,
            resources,
            ParseActivationCondition(json["activationCondition"]),
            json["turnsToActivate"],
            json["priority"]
        ));
    }


    private DResource[] resources;

    private ModifyResourceEvent(string promptText, DCity city, DResource[] resources, 
        activationCondition actCondition,int turnsToActivation = 0, int priority = Constants.EVENT_PRIORITY_DEFAULT)
    {
        this.promptText = promptText;
        this.city = city;
        this.resources = resources;
        this.actCondition = actCondition;
        this.turnsToActivation = turnsToActivation;
        this.priority = priority;
    }

    public override void Activate()
    {
        /*
         * TEMPORARY - PLUG IN UI TO REPLACE ALL LINES BELOW WITH EQUIVALENTS
        */
        Debug.Log(promptText);  // for UI - display (call DEvent.PromptText)
        Debug.Log("<player enters confirmation via UI>");   // for UI - from UI call DGame.Instance().ResolveEvent() when player hits button to close
        dGame.ResolveEvent(); // for UI - per above, remove this when UI call to ResolveEvent() implemented
        //Resolve();  // TEMP - only here because DGame currently broken and cannot support singleton
    }

    override public void Resolve(int selection = Constants.NO_INPUT)
    {
        for(int i=0; i<resources.Length; i++)
        {
            city.AddResource(resources[i]);
        }        
    }
}
