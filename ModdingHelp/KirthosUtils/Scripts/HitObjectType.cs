using UnityEngine;
using System.Collections;

public class HitObjectType : MonoBehaviour
{
    public string ObjectType;
}

public interface IModulatePitchValue
{
    float PitchValue { get; }
}
