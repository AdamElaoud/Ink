using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    [SerializeField]
    float floatPower, floatSpeed;

    float initialX, initialY;

    void Start() {
        initialX = transform.position.x;
        initialY = transform.position.y;
    }

    void FixedUpdate() {
        // float animation
        float updatedY = Mathf.Sin(Time.time * floatSpeed) * floatPower;

        transform.position = new Vector3(initialX, initialY + updatedY, 0.75f);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // end level function
        if (collider.gameObject.CompareTag("Player")) {
            gameObject.SetActive(false);
            GameManager.instance.NextLevel();
        }
    }
}
