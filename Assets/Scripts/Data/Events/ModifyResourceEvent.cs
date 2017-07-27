using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyResourceEvent : DEvent {

    private DCity city;
    private DResource resource;

    public ModifyResourceEvent(string promptText, DCity city, DResource resource)
    {
        this.promptText = promptText;
        this.city = city;
        this.resource = resource;
    }

    public override void Activate()
    {
        DGame.Instance().GameState = DGame._gameState.EVENT;
        /*
         * TEMPORARY - PLUG IN UI TO REPLACE ALL LINES BELOW WITH EQUIVALENTS
        */
        Debug.Log(promptText);  // for UI - display (call DEvent.PromptText)
        Debug.Log("<player enters confirmation via UI>");   // for UI - from UI call DGame.ResolveEvent() when player hits button to close
        Resolve(); // for UI - per above, remove this when UI call to ResolveEvent() implemented
    }

    override public void Resolve(string selection = Constants.NO_INPUT)
    {
        city.AddResource(resource);
    }
}
