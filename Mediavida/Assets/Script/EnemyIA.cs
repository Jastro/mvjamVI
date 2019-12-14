using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour {
    GameObject player;
    public bool patrol = true, guard = true, clockwise = false;
    public bool moving = true;
    public bool followingPlayer = false, goingToLastLoc = false;
    public Vector3 playerLastPos;
    public float speed = 2.0f;
    RaycastHit2D hit;
    RaycastHit2D hit2;
    public Collider target;
    float distanceDetect = 1.5f;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLastPos = this.transform.position;
    }

    void Update() {
        movement();
    }

    void movement() {
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        Vector3 dir = player.transform.position - transform.position;
        hit = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(dir.x, dir.y), dist);

        Vector3 forwardTo = this.transform.TransformDirection(Vector3.right);
        hit2 = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(forwardTo.x, forwardTo.y), 1.0f);

        if (moving) {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (patrol) {
            if (hit2.collider != null) {
                if (hit2.collider.gameObject.tag == "wall") {
                    if (clockwise) {
                        transform.Rotate(0, 0, -90);
                    } else {
                        transform.Rotate(0, 0, 90);
                    }
                }
                if(hit2.collider.gameObject.tag == "Player") {
                    followingPlayer = true;
                }
            } else {
               if (Vector3.Distance(transform.position, player.transform.position) < 0.6f) {
                    followingPlayer = true;
                }
            }
        }

        if (followingPlayer) {
            if(Vector3.Distance(transform.position, player.transform.position) > 1.5f) {
                playerLastPos = player.transform.position;
                RotateTowards(playerLastPos);
                followingPlayer = false;
                goingToLastLoc = true;
            } else {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.02f);
                RotateTowards(player.transform.position);
                playerLastPos = player.transform.position;
            }
        }

        if (goingToLastLoc) {
            RotateTowards(playerLastPos);

            if(hit2.collider.gameObject.tag == "Player" || Vector3.Distance(transform.position, player.transform.position) < 1.5f) {
                followingPlayer = true;
            }

            if(Vector3.Distance(this.transform.position, playerLastPos) < distanceDetect) {
                patrol = true;
                goingToLastLoc = false;
            }
        }
    }

    private void RotateTowards(Vector2 target) {
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
