using System.Collections.Generic;
using UnityEngine;

public class RubblePiece : MonoBehaviour
{
    public float minPlayOnForce = 2.7f;
    public GameObject rubbleCollisionSfx;

    public const float massDampenMod    =  100f;
    public const float minRelativePitch = -0.55f;
    public const float maxRelativePitch =  1.15f;
    public const float pitchDampenMod   =  12.5f;
    public const float volumeDampenMod  =  10f;

    public int maxVoices = 5;

    private List<GameObject> activeSfx;

    private bool splitable = false;
}
