using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceEvent : DEvent
{

    public static void AddEventFromJSON(DCity currentCity, JSONNode json)
    {
        var outcomeJSON = json["outcomes"];
        var outcomes = new outcome[json["outcomes"].AsArray.Count];
        for(int i=0; i<outcomes.Length; i++)
        {
            outcomes[i] = ParseOutcome(json["outcomes"][i]);
        }

        var outcomesText = new string[json["outcomesText"].AsArray.Count];
        for(int i=0; i<outcomesText.Length; i++)
        {
            outcomesText[i] = json["outcomesText"][i];
        }

        DEventSystem.AddEvent(new ChoiceEvent(
            json["promptText"],
            currentCity,
            ParseActivationCondition(json["activationCondition"]),
            outcomes,
            outcomeJSON,
            outcomesText,
            json["turnsToActivate"],
            json["priority"],
            json["nextEvent"]            
        ));
    }

    public static outcome ParseOutcome(JSONNode outcome)
    {
        string type = outcome["type"];
        
        switch (type)
        {
            case "resource":
                outcome resource = e => e.City.AddResource(DResource.Create(outcome["value"][0], outcome["value"][1]));
                return resource;
            case "cityHealth":
                string op = outcome["value"][0];
                var amount = outcome["value"][1];
                switch (op)
                {
                    case "multiply":
                        outcome cityHealth = e => e.City.Health *= amount;
                        return cityHealth;
                }
                throw new Exception("Invalid CityHealth Operator Type");
            default:
                throw new Exception("Invalid Outcome Type");
        }
    }

    public delegate void outcome(ChoiceEvent e);
    public outcome[] outcomes = new outcome[2];
    public JSONNode outcomeJSON;
    public string[] outcomeTexts = new string[2];


    private ChoiceEvent(string promptText, DCity city, activationCondition actCondition, outcome[] outcomes, JSONNode outcomeJSON,
        string[] outcomeTexts, int turnsToActivation = 0, int priority = Constants.EVENT_PRIORITY_DEFAULT, JSONNode nextEvent = null)
    {
        this.promptText = promptText;
        this.city = city;
        this.actCondition = actCondition;
        this.outcomes = outcomes;
        this.outcomeJSON = outcomeJSON;
        this.outcomeTexts = outcomeTexts;
        this.turnsToActivation = turnsToActivation;
        this.priority = priority;
        this.nextEvent = nextEvent;
    }

    public override void Activate()
    {
        /*
         * TEMPORARY - PLUG IN UI TO REPLACE ALL LINES BELOW WITH EQUIVALENTS
        */
        Debug.Log(promptText);  // for UI - display (call DEvent.PromptText)
        Debug.Log("<player enters input via UI>");   // for UI - from UI call DGame.Instance().ResolveEvent() when player hits button to close
        int option = 1;
            dGame.ResolveEvent(option); // for UI - per above, remove this when UI call to ResolveEvent() implemented
        //Resolve(option); // TEMP - only here because DGame currently broken and cannot support singleton
    }

    override public void Resolve(int selection = Constants.NO_INPUT)
    {
        if (selection >= 0 && selection <= outcomes.Length - 1)
        {
            outcomes[selection](this);
            Debug.Log(outcomeTexts[selection]);    // for UI - replace
        }
    }

    public override JSONNode SaveToJSON()
    {
        JSONArray outcomeTextsJSON = new JSONArray();
        foreach (var o in outcomeTexts)
        {
            outcomeTextsJSON.Add(o);
        }

        return new JSONObject
        {
            { "priority", priority },
            { "turnsToActivate", turnsToActivation },
            { "promptText", promptText },
            { "activationCondition", actConditionString },
            { "outcomes", outcomeJSON.AsArray },
            { "outcomesText", outcomeTextsJSON }
        };
    }

    public override void LoadFromJSON(JSONNode json, DCity city)
    {
        AddEventFromJSON(city, json);
    }
}
