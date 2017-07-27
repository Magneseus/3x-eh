using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyResourceEvent : DEvent {

    private DCity city;
    private DResource resource;

    public ModifyResourceEvent(DCity city, DResource resource)
    {
        this.city = city;
        this.resource = resource;
    }

    override public void Resolve()
    {
        city.AddResource(resource);
    }    
}
