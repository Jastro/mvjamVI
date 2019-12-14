using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDetection : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D collision) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement PM = player.GetComponent<PlayerMovement>();
        PM.target = collision;
    }

    void OnCollisionExit2D(Collision2D collision) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement PM = player.GetComponent<PlayerMovement>();
        PM.target = null;
    }
}
