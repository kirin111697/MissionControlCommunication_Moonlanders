using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LanderMovement : MonoBehaviour
{
    private Rigidbody2D myRB;

    public float minForce = 0.2f;
    public float maxForce = 1.5f;

    public float moveForce = 1.0f;
    private Vector2 moveDirection = new Vector2(0.0f, 1.0f);

    public float randomForceThreshold = 800.0f;
    public float randomDirectionThreshold = 998.0f;

    public float maxHitBoundsTime = 0.25f;
    private float hitBoundsTimer = 0.0f;
    private bool hitBoundsTime = false;

    // Start is called before the first frame update
    void Start()
    {
        myRB = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hitBoundsTime && hitBoundsTimer < maxHitBoundsTime)
        {
            hitBoundsTimer += Time.deltaTime;

            //Debug.Log("Timer " + hitBoundsTimer);
        }
        else if (hitBoundsTime && hitBoundsTimer > maxHitBoundsTime)
        {
            hitBoundsTime = false;
            hitBoundsTimer = 0.0f;

            //Debug.Log("Can Change Direction");
        }
    }

    private void FixedUpdate()
    {
        float randomNum = Random.Range(0.0f, 1000.0f);

        if (randomNum >= randomForceThreshold)
        {
            float randomForce = Random.Range(minForce, maxForce);

            ChangeForce(randomForce);
        }

        randomNum = Random.Range(0.0f, 1000.0f);

        if (randomNum >= randomDirectionThreshold && !hitBoundsTime)
        {
            ChangeDirection();
        }

        myRB.AddForce(moveForce * moveDirection);
    }

    public void ChangeDirection()
    {
        myRB.velocity = new Vector2(0.0f, 0.0f);

        moveDirection *= -1.0f;
    }

    public void ChangeForce(float newForce)
    {
        moveForce = newForce;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bounds")
        {
            
            hitBoundsTime = true;
            hitBoundsTimer = 0.0f;
            
            ChangeDirection();
        }

        if(collision.gameObject.tag == "Radar")
        {
            GameManager.RadarID radarID = collision.GetComponentInParent<RadarControls>().radarID;
            GameManager.instance.UpdateRadarStatus(radarID, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Radar")
        {
            GameManager.RadarID radarID = collision.GetComponentInParent<RadarControls>().radarID;
            GameManager.instance.UpdateRadarStatus(radarID, false);
        }
    }
}
