using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Data file which is needed write and load variables to json files.
[System.Serializable]
public class ShakeSaveData{
    public float duration = 0;
    public float magnitude = 0;
    public float interpolationSpeed = 0;
    public float roughness = 0;
    public float rotationMagnitude = 0;
    public Vector3 originalPosition;
    public Quaternion originalRotation;
}
