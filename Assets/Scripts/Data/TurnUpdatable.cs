using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implement from this interface: objects that are updated within the main Turn loop
public interface ITurnUpdatable
{
    // Must update based on the number of days that have transpired
    void TurnUpdate(int numDaysPassed);
}