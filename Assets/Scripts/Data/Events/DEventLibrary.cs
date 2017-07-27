using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


static class DEventLibrary
{
    public static Dictionary<string, List<IEvent>> events = new Dictionary<string, List<IEvent>>();

    public static void NewEventFamily(string familyName)
    {
        if (events.ContainsKey(familyName))
            return;
        List<IEvent> newFamily = new List<IEvent>();
        events.Add(familyName, newFamily);
    }
}
