using System.Collections.Generic;
using UnityEngine;

namespace Assets.Editor.UnitTests
{
    class Mock
    {
        private static List<GameObject> mockObjects = new List<GameObject>();

        public static void TearDown()
        {
            foreach (var entry in mockObjects)
                Object.DestroyImmediate(entry);
        }

        public static DTask CleanTask(DBuilding building, DResource resource)
        {
            var task = new DTask(building, resource);

            task.ForceClean();
            task.ForceFixed();

            return task;
        }

        public static T Component<T>() where T : Component
        {
            var mockObj = new GameObject();
            mockObjects.Add(mockObj);
            return mockObj.AddComponent<T>().GetComponent<T>();
        }

    }
}
