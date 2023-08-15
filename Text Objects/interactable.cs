using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Interactable", menuName = "Interactable")]
public class interactable : ScriptableObject
{
    public string title;
    public string description;
    public string power;
    public string note;
}
