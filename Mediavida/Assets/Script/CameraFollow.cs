using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    GameObject player;
    public bool followPlayer = true;
    Camera cam;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
    }

    private void LateUpdate() {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        this.transform.position = newPos;

        if (Input.GetKey(KeyCode.LeftShift)) {
            followPlayer = false;
        } else {
            followPlayer = true;
        }

        if (followPlayer) {
            canFollowPlayer();
        } else {
            lookAhead();
        }
    }

    void canFollowPlayer() {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        this.transform.position = newPos;
    }

    void lookAhead() {
        Vector3 camPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y));
        if (player.GetComponent<SpriteRenderer>().isVisible) {
            if (Mathf.Abs(camPos.y) > 0.3f && Mathf.Abs(camPos.y) < 5.0f) {
                if (Mathf.Abs(camPos.x) > 0.0f && Mathf.Abs(camPos.x) < 5.0f) {
                    transform.position = camPos;
                }
            }
        }
    }
}
