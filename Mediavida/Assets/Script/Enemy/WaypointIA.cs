using System;
using UnityEngine;

public class WaypointIA : MonoBehaviour {
    public float waitTime = 1.0f;
    
    public Transform lookAtWaiting;
    public Transform lookAtWalking;

    private bool _hasLookAtWaiting = false;
    private bool _hasLookAtWalking = false;

    public void Start() {
        _hasLookAtWaiting = lookAtWaiting != null;
        _hasLookAtWalking = lookAtWalking != null;
    }

    public bool HasLookAtWaiting() {
        return _hasLookAtWaiting;
    }

    public bool HasLookAtWalking() {
        return _hasLookAtWalking;
    }
}
