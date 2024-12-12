using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReplay : MonoBehaviour {
    private bool isInReplayMode;
    private float currentReplayIndex;
    public bool isPaused;
    public bool isSlow;
    public bool isRewind;
    public float indexChangeRate;
    private Rigidbody rigidbody;
    private List<ActionReplayRecord> actionReplayRecords = new List<ActionReplayRecord> ();

    void Start () {
        rigidbody = GetComponent<Rigidbody> ();
    }
    void Update () {
        if (Input.GetKeyDown (KeyCode.R)) {
            isInReplayMode = !isInReplayMode;

            if (isInReplayMode) {
                SetTransform (0);
                rigidbody.isKinematic = true;
            } else {

                SetTransform (actionReplayRecords.Count - 1);
                rigidbody.isKinematic = false;
            }
        }

        if (isPaused) {
            indexChangeRate = 0;
        } else {
            if (isSlow) {
                if (isRewind) {
                    indexChangeRate = -1;
                    indexChangeRate *= 0.5f;
                } else {
                    indexChangeRate = 1;
                    indexChangeRate *= 0.5f;
                }
            } else if (isRewind) {
                indexChangeRate = -1;
            } else {
                indexChangeRate = 1;
            }
        }

    }
    private void FixedUpdate () {
        if (isInReplayMode == false) {
            actionReplayRecords.Add (new ActionReplayRecord { position = transform.position, rotation = transform.rotation });
        } else {
            float nextIndex = currentReplayIndex + indexChangeRate;

            if (nextIndex < actionReplayRecords.Count && nextIndex >= 0) {
                SetTransform (nextIndex);
            }
        }
    }

    private void SetTransform (float index) {
        currentReplayIndex = index;
        ActionReplayRecord actionReplayRecord = actionReplayRecords[(int) index];

        transform.position = actionReplayRecord.position;
        transform.rotation = actionReplayRecord.rotation;
    }
}