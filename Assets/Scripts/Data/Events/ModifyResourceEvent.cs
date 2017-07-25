using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyResourceEvent : IEvent {

    private DCity city;
    private DResource resource;

    public ModifyResourceEvent(DCity city, DResource resource)
    {
        this.city = city;
        this.resource = resource;
    }

    public void Resolve()
    {
        city.AddResource(resource);
    }    
}
