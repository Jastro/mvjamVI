using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 2.0f;
    bool haveRedKey = false;
    bool haveBlueKey = false;
    bool alarmIsRunning = false;

    private void Update() {
        movement();
        rotateToMouse();
    }

    void movement() {
        if(Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.World);
        }

        if(Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        }

        if(Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }

        if(Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
    }

    void rotateToMouse() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.tag != "wall") {
            switch(collision.collider.gameObject.tag) {
                case "redKey":
                    Destroy(collision.collider.gameObject);
                    haveRedKey = true;
                    break;
                case "blueKey":
                    Destroy(collision.collider.gameObject);
                    haveBlueKey = true;
                    break;
                case "redLock":
                    if (haveRedKey) {
                        Destroy(collision.collider.gameObject);
                    }
                    break;
                case "blueLock":
                    if(haveBlueKey) {
                        Destroy(collision.collider.gameObject);
                    }
                    break;
                case "unlocker":
                    if(haveBlueKey) {
                        if(Input.GetKey(KeyCode.E)) {
                           //code for deactive alarm
                        }
                    }
                    break;
            }
        }
    }
}
