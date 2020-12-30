using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStateManager : MonoBehaviour {

    [SerializeField]
    Sprite blue, red;

    [SerializeField]
    public bool shifting, moving;

    [SerializeField]
    float shiftRate, numMoves, moveX, moveY, moveRate, movePause;

    float startX, startY;
    Vector2 startPos, endPos;
    bool movingForward;
    Color redColor, blueColor;
    Animator anim;

    void Start() {
        // animations only needed for neutral platforms
        if (gameObject.layer == 8)
            anim = GetComponent<Animator>();

        redColor = new Color(1, 0.108f, 0.108f, 1);
        blueColor = new Color(0.429f, 1, 0.938f, 1);

        // tag update
        // shifting and moving
        if (shifting && moving) {
            if (gameObject.CompareTag("Barrier"))
                gameObject.tag = "Shifting Moving Barrier";
            else
                gameObject.tag = "Shifting Moving Platform";

        // shifting
        } else if (shifting) {
            if (gameObject.CompareTag("Barrier"))
                gameObject.tag = "Shifting Barrier";
            else
                gameObject.tag = "Shifting Platform";

        // moving
        } else if (moving) {

            if (gameObject.CompareTag("Barrier"))
                gameObject.tag = "Moving Barrier";
            else
                gameObject.tag = "Moving Platform";
        }

        // color shift
        if (shifting) {
            StartCoroutine(Shift());
        }

        // platform movement
        if (moving) {
            startX = transform.position.x;
            startY = transform.position.y;
            startPos = new Vector2(startX, startY);
            endPos = new Vector2(startX + moveX, startY + moveY);
            DrawLines();
            StartCoroutine(Move());
        }
    }

    IEnumerator Shift() {
        while (shifting) {
            yield return new WaitForSeconds(shiftRate);

            if (gameObject.layer == 9) {
                GetComponent<SpriteRenderer>().sprite = red;
                GetComponent<Light>().color = redColor;
                gameObject.layer = 10;

            } else if (gameObject.layer == 10) {
                GetComponent<SpriteRenderer>().sprite = blue;
                GetComponent<Light>().color = blueColor;
                gameObject.layer = 9;
            }
        }        
    }

    IEnumerator Move() {
        bool frozen = false;
        
        while (moving) {
            // at end
            if (transform.position.Equals(new Vector2(startX + moveX, startY + moveY))) {
                movingForward = false;
                frozen = true;
            }

            // at start
            if (transform.position.Equals(new Vector2(startX, startY))) {
                movingForward = true;
                frozen = true;
            }

            if (frozen) {
                yield return new WaitForSeconds(movePause);
                frozen = false;
            }

            if (movingForward) {
                transform.position = Vector2.MoveTowards(transform.position, endPos, moveRate * Time.deltaTime);
                // wait for next frame to move
                yield return null;
            } else {
                transform.position = Vector2.MoveTowards(transform.position, startPos, moveRate * Time.deltaTime);
                // wait for next frame to move
                yield return null;
            }

        }
    }

    void DrawLines() {
        LineRenderer draw = gameObject.AddComponent<LineRenderer>();
        Material material = Resources.Load("Assets/Resources/Materials/White.mat") as Material;
        // converting Vector2 to Vector3 (z set to 0)
        Vector3 start = startPos;
        Vector3 end = endPos;
        Vector3[] positions = {start, end};

        // do not cast shadows
        draw.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        draw.widthMultiplier = 0.015f;
        draw.material = material;
        draw.sortingLayerName = "Background";

        draw.positionCount = 2;
        draw.SetPositions(positions);
    }

    // draw Gizmos for level building
    void OnDrawGizmos() {
        // converting Vector2 to Vector3 (z set to 0)
        Vector3 start = startPos;
        Vector3 end = endPos;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(start, end);
        //Gizmos.DrawSphere(start, 0.5f);
        //Gizmos.DrawSphere(end, 0.5f);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // only change color if neutral
        if (gameObject.layer == 8) {
            if (collision.gameObject.layer == 9) {
                anim.SetBool("Turned Blue", true);
                GetComponent<SpriteRenderer>().sprite = blue;
                GetComponent<Light>().color = blueColor;

            } else if (collision.gameObject.layer == 10) {
                anim.SetBool("Turned Red", true);
                GetComponent<SpriteRenderer>().sprite = red;
                GetComponent<Light>().color = redColor;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        // only change color if neutral
        if (gameObject.layer == 8) {
            if (collision.gameObject.layer == 9) {
                GetComponent<SpriteRenderer>().sprite = blue;

            } else if (collision.gameObject.layer == 10) {
                GetComponent<SpriteRenderer>().sprite = red;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        // only change color if neutral
        if (gameObject.layer == 8) {
            if (collision.gameObject.layer == 9) {
                gameObject.layer = 9;

            } else if (collision.gameObject.layer == 10) {
                gameObject.layer = 10;
            }
        }
    }
}
