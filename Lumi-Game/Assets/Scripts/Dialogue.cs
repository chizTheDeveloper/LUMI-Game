using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // Variable for the name of the person talking
    public string name;

    // List to hold all part of the dialogue
    public List<string> text = new List<string>();
}
