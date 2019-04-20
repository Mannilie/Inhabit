using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentData", menuName = "Scriptables/AgentData", order = 0)]
public class AgentData : ScriptableObject
{
    public float minRotationSpeed = 10f, maxRotationSpeed = 90f;

    [SerializeField]
    private float percentage;

    public float Percentage
    {
        get { return percentage; }
        set { percentage = value; }
    }
}
