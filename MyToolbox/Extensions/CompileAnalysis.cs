﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyBox
{
    public class CompileAnalysis : MonoBehaviour
    {
        [InitializeOnLoad]
        class CompileTime : EditorWindow
        {
            static bool isTrackingTime;
            static double startTime;

            static CompileTime()
            {
                EditorApplication.update += Update;
                startTime = PlayerPrefs.GetFloat("CompileStartTime", 0);
                if (startTime > 0)
                {
                    isTrackingTime = true;
                }
            }

            static void Update()
            {
                if (EditorApplication.isCompiling && !isTrackingTime)
                {
                    startTime = EditorApplication.timeSinceStartup;
                    PlayerPrefs.SetFloat("CompileStartTime", (float)startTime);
                    isTrackingTime = true;
                }
                else if (!EditorApplication.isCompiling && isTrackingTime)
                {
                    var finishTime = EditorApplication.timeSinceStartup;
                    isTrackingTime = false;
                    var compileTime = finishTime - startTime;
                    PlayerPrefs.DeleteKey("CompileStartTime");
                    Debug.Log("Script compilation time: " + compileTime.ToString("0.000") + "s");
                }
            }
        }

    }
}

