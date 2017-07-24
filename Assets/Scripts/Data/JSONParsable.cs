using System.Collections.Generic;
using SimpleJSON;

// Implement from this interface: objects that are saved/loaded for a game session
public interface JSONParsable<T>
{
    // Must be able to be saved to a JSONNode (which can then be written to a file)
    JSONNode SaveToJSON();

    // Must be able to be loaded from a JSONNode (which can be read from a file)
    T LoadFromJSON(JSONNode jsonNode);
}
