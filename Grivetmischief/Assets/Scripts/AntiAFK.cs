using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR;
using System.Collections.Generic;

public class AntiAFK : MonoBehaviourPunCallbacks
{
    [Header("AFK Settings")]
    public float afkTimeLimit = 120f; 
    private float afkTimer = 0f;

    private Vector3 lastHeadPos;
    private Vector3 lastLeftHandPos;
    private Vector3 lastRightHandPos;

    private InputDevice headDevice;
    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;

    private void Start()
    {
        InitializeDevices();

        lastHeadPos = GetDevicePosition(headDevice);
        lastLeftHandPos = GetDevicePosition(leftHandDevice);
        lastRightHandPos = GetDevicePosition(rightHandDevice);
    }

    private void InitializeDevices()
    {
        var headDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.Head, headDevices);
        if (headDevices.Count > 0) headDevice = headDevices[0];

        var leftHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
        if (leftHandDevices.Count > 0) leftHandDevice = leftHandDevices[0];

        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count > 0) rightHandDevice = rightHandDevices[0];
    }

    private void Update()
    {
        Vector3 currentHeadPos = GetDevicePosition(headDevice);
        Vector3 currentLeftHandPos = GetDevicePosition(leftHandDevice);
        Vector3 currentRightHandPos = GetDevicePosition(rightHandDevice);

        if (HasMoved(currentHeadPos, lastHeadPos) || HasMoved(currentLeftHandPos, lastLeftHandPos) || HasMoved(currentRightHandPos, lastRightHandPos))
        {
            afkTimer = 0f; 
            lastHeadPos = currentHeadPos;
            lastLeftHandPos = currentLeftHandPos;
            lastRightHandPos = currentRightHandPos;
        }
        else
        {
            afkTimer += Time.deltaTime;
            if (afkTimer >= afkTimeLimit)
            {
                KickPlayer();
            }
        }
    }

    private Vector3 GetDevicePosition(InputDevice device)
    {
        if (device.isValid && device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
        {
            return position;
        }
        return Vector3.zero;
    }

    private bool HasMoved(Vector3 current, Vector3 last, float threshold = 0.01f)
    {
        return Vector3.Distance(current, last) > threshold;
    }

    private void KickPlayer()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Player kicked for AFK");
            PhotonNetwork.LeaveRoom();
        }
        afkTimer = 0f; 
    }
}
