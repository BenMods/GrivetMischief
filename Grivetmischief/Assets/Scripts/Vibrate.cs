using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyInputs;

public class Vibrate : MonoBehaviour
{
    public float amplitude;
    public float duration;
    public EasyHand hand;                        
    public void VibrateController()
    {
        StartCoroutine(EasyInputs.Vibration(hand, amplitude, duration));
    }
}
