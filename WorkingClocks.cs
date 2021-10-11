using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Harmony;
using UnityEngine;
using UnityEngine.SceneManagement;

using MelonLoader;
using MelonLoader.TinyJSON;

namespace WorkingClocks
{
    class WorkingClocks : MelonMod
    {

        public static List<ClockComponent> clockList = new List<ClockComponent>();

        public override void OnApplicationStart()
        {
            UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<ClockComponent>();
        }

        internal static ClockComponent PrepareClock(GameObject clockObj)
        {
            ClockComponent clock = clockObj.AddComponent<ClockComponent>();

            //Assign hands
            List<GameObject> result = new List<GameObject>();
            GetChildrenWithName(clockObj, "smallhand", result);

            if (result.Count > 0)
            {
                clock.hourHand = result[0];
            }

            result.Clear();
            GetChildrenWithName(clockObj, "bighand", result);

            if (result.Count > 0)
            {
                clock.minuteHand = result[0];
            }

            return clock;
        }

        internal static void UpdateClocks()
        {
            int hour = GameManager.GetTimeOfDayComponent().GetHour();
            int minutes = GameManager.GetTimeOfDayComponent().GetMinutes();

            float hourDeg = (hour + (minutes / 60f)) * 30f;
            float minDeg = minutes * 6f;

            foreach (ClockComponent clock in clockList)
            {
                clock.UpdateHands(hourDeg, minDeg);
            }
        }

        internal static void GetSceneClocks()
        {
            List<GameObject> rObjs = GetRootObjects();

            List<GameObject> result = new List<GameObject>();

            //Clear object list
            clockList.Clear();

            int setupClocks = 0;

            foreach (GameObject rootObj in rObjs)
            {
                GetChildrenWithName(rootObj, "clock", result);

                if (result.Count > 0)
                {
                    foreach (GameObject child in result)
                    {
                        if (child != null && !child.GetComponent<ClockComponent>())
                        {
                            clockList.Add(PrepareClock(child));

                            setupClocks++;
                        }
                    }
                }
            }

            MelonLogger.Msg(setupClocks + " clocks found.");
        }

        internal static List<GameObject> GetRootObjects()
        {
            List<GameObject> rootObj = new List<GameObject>();

            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
            {
                Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

                GameObject[] sceneObj = scene.GetRootGameObjects();

                foreach (GameObject obj in sceneObj)
                {
                    rootObj.Add(obj);
                }
            }

            return rootObj;
        }

        internal static void GetChildrenWithName(GameObject obj, string name, List<GameObject> result)
        {
            if (obj.transform.childCount > 0)
            {

                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    GameObject child = obj.transform.GetChild(i).gameObject;

                    if (child.name.ToLower().Contains(name.ToLower()))
                    {
                        result.Add(child);

                        continue;
                    }

                    GetChildrenWithName(child, name, result);
                }
            }
        }

    }
}
