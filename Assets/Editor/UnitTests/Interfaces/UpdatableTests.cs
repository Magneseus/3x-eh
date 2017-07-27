using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class UpdatableTests {

	[Test]
	public void UpdatableTestsSimplePasses() {
        // Use the Assert class to test conditions.
        Assert.Pass();
	}

	[Test]
    public void CanBeInstantiated()
    {
        int startNumber = 0;
        int numDaysPassed = 7;

        StubUpdatable myStub = new StubUpdatable(startNumber);
        Assert.AreEqual(startNumber, myStub._number);

        myStub.TurnUpdate(numDaysPassed);
        Assert.AreEqual(numDaysPassed, myStub._number);
    }

    class StubUpdatable : ITurnUpdatable
    {
        public int _number;

        public StubUpdatable(int number)
        {
            _number = number;
        }

        public void TurnUpdate(int numDaysPassed)
        {
            _number += numDaysPassed;
        }
    }


}
