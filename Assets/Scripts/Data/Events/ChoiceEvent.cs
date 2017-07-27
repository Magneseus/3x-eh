using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceEvent : DEvent
{

    public delegate void outcome(ChoiceEvent e);
    public outcome[] outcomes = new outcome[2];
    public string[] outcomeTexts = new string[2];


    public ChoiceEvent(string promptText, DCity city, activationCondition actCondition, outcome[] outcomes, 
        string[] outcomeTexts, int turnsToActivation = 0, int priority = Constants.EVENT_PRIORITY_DEFAULT)
    {
        this.promptText = promptText;
        this.city = city;
        this.actCondition = actCondition;
        this.outcomes = outcomes;
        this.outcomeTexts = outcomeTexts;
        this.turnsToActivation = turnsToActivation;
        this.priority = priority;
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
}
