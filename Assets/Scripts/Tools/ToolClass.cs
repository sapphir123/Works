using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolClass
{
    public static ToolClass Instance
    {
        get
        {
            if (instance == null)
                instance = new ToolClass();
            return instance;
        }
    }
    private static ToolClass instance;

    public enum KeyFunction
    {

    }
}
