using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

internal class Util
{
    // CREDIT: https://docs.unity3d.com/ScriptReference/XR.XRDevice-isPresent.html
    internal static bool IsVRPresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
        foreach (var xrDisplay in xrDisplaySubsystems)
        {
            if (xrDisplay.running)
            {
                return true;
            }
        }
        return false;
    }
}