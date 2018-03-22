using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Version : MonoBehaviour
{
    public byte majorBuild = 0;
    public byte minorBuild = 0;
    public byte revision = 0;
    public int build = 0;
    public static string version;

    private void Awake()
    {
        version = majorBuild + "." + minorBuild + "." + revision + ":" + build;
    }
}
