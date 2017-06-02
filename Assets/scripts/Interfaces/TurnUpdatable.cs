using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use this interface for objects that are updated within the main Turn loop
public interface TurnUpdatable
{
    // Must update based on the number of days that have transpired
    void TurnUpdate(int numDaysPassed);
}