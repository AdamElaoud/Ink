using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    float speed, drag, jumpPower, startX, startY;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    Sprite neutral, blue, red;

    [SerializeField]
    AnimatorOverrideController overrideBlue, overrideNeutral, overrideRed;

    // State
    bool grounded;
    bool running;
    bool jumped;
    bool collided;
    int keysCollected;

    float horizontal;
    Vector2 movement;
    Rigidbody2D rb;
    Animator anim;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        grounded = false;
        collided = false;
        jumped = false;
        //transform.position = new Vector2(startX, startY);
        keysCollected = 0;
    }

    void Update() {
        // grounded update handled by OnCollisionEnter
        //UpdateGroundedRaycast();

        anim.SetFloat("YVelocity", rb.velocity.y);

        horizontal = Input.GetAxis("Horizontal") * 2f;

        if (horizontal != 0) {
            running = true;
            anim.SetBool("Walking", true);

            // sprite flip
            if (horizontal < 0)
                GetComponent<SpriteRenderer>().flipX = true;
            else
                GetComponent<SpriteRenderer>().flipX = false;
        } else {
            anim.SetBool("Walking", false);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && grounded) {
            jumped = true;
        }

        // set Sprite to Neutral when "L" is pressed
        if (Input.GetKeyDown(KeyCode.L) && !collided && gameObject.layer != 8) {
            GetComponent<SpriteRenderer>().sprite = neutral;
            GetComponent<Animator>().runtimeAnimatorController = overrideNeutral;
            gameObject.layer = 8;
        }

        // level restart
        if (Input.GetKeyDown(KeyCode.R))
            GameManager.instance.RestartLevel();

        // manual next level
        if (Input.GetKeyDown(KeyCode.N))
            GameManager.instance.NextLevel();

        // manual previous level
        if (Input.GetKeyDown(KeyCode.B))
            GameManager.instance.PreviousLevel();
    }

    void FixedUpdate() {
        if (running) {
            Run();
            running = false;
        }

        if (jumped) {
            Jump();
            jumped = false;
        }
    }

    void Run() {
        movement = new Vector2(horizontal * speed * drag, rb.velocity.y);
        rb.velocity = movement;
    }

    void Jump() {
        rb.velocity += new Vector2(0f, jumpPower);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        float positionY = transform.position.y;
        float collisionPosY = collision.GetContact(0).point.y;

        int layer = collision.gameObject.layer;
        bool isBarrier = collision.gameObject.CompareTag("Barrier");
        bool isLock = collision.gameObject.CompareTag("Lock");

        // Grounded Update
        if ((grounded && isBarrier) || (!isBarrier && (layer == 8 || layer == 9 || layer == 10 || isLock) && collisionPosY < positionY))
            grounded = true;
        else
            grounded = false;

        // Color Update
        if (gameObject.layer == 8) {
            if (collision.gameObject.layer == 9) {
                GetComponent<SpriteRenderer>().sprite = blue;
                GetComponent<Animator>().runtimeAnimatorController = overrideBlue;
            } else if (collision.gameObject.layer == 10) {
                GetComponent<SpriteRenderer>().sprite = red;
                GetComponent<Animator>().runtimeAnimatorController = overrideRed;
            }

            // Layer Update if Shifting Platform
            if (collision.gameObject.tag.Contains("Shifting"))
                gameObject.layer = collision.gameObject.layer;
        }

                // Moving Platform Check
        if (collision.gameObject.CompareTag("Moving Platform") || collision.gameObject.CompareTag("Shifting Moving Platform")) {
            transform.parent = collision.transform;
        }
        
        collided = true;
    }

    void OnCollisionStay2D(Collision2D collision) {          
        collided = true;
    }

    void OnCollisionExit2D(Collision2D collision) {
        int layer = collision.gameObject.layer;
        bool isBarrier = collision.gameObject.CompareTag("Barrier");

        if (!isBarrier && (layer == 8 || layer == 9 || layer == 10))
            grounded = false;

        // Color Update
        if (gameObject.layer == 8) {
            if (collision.gameObject.layer == 9)
                gameObject.layer = 9;
            else if (collision.gameObject.layer == 10)
                gameObject.layer = 10;
        }

        // Moving Platform Check
        if (collision.gameObject.CompareTag("Moving Platform")) {
            transform.parent = null;
        }

        collided = false;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // Key Count Increment
        if (collider.gameObject.CompareTag("Key"))
            keysCollected++;

        // character respawn
        if (collider.gameObject.CompareTag("Kill Zone")) {
            // hard reset
            GameManager.instance.RestartLevel();

            // soft reset
            //rb.velocity = Vector2.zero;
            //transform.position = new Vector2(startX, startY);
            //gameObject.layer = 8;
            //GetComponent<SpriteRenderer>().sprite = neutral;
        }

    }

    /* ------------------------ UNUSED ------------------------ */
    void UpdateGroundedRaycast() {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;

        RaycastHit2D ray = Physics2D.Raycast(position, direction, distance, groundLayer);

        if (ray.collider != null) {
            grounded = true;
        } else {
            grounded = false;
        }
    }
}
