﻿using HarmonyLib;
using System;
using UnityEngine;
using Il2Cpp;

namespace WorkingClocks
{
    [HarmonyPatch(typeof(SaveGameSystem), "LoadSceneData", new Type[] { typeof(string), typeof(string) })]
    internal class SaveGameSystem_LoadSceneData
    {
        public static void Prefix(SaveGameSystem __instance, string name, string sceneSaveName)
        {
            WorkingClocks.GetSceneClocks();
        }
    }

    [HarmonyPatch(typeof(GameManager), "Update")]
    internal class GameManager_Update
    {
        public static void Postfix(GameManager __instance)
        {
            if (!GameManager.m_IsPaused && !InterfaceManager.IsMainMenuEnabled())
                WorkingClocks.UpdateClocks();
        }
    }
}
