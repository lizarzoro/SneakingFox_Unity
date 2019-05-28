using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol2 : MonoBehaviour
{
    public float speed, startWaitTime;
    private float waitTime;
    public Transform[] moveSpots;
    private int randomSpot;

    //public float distance;

    //private bool movingRight = true;
    //public Transform groundDetection;
    // Start is called before the first frame update

    private void Start()
    {
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        
        if(Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if(waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        //transform.Translate(Vector2.right * speed * Time.deltaTime);

        //RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);

        //if (groundInfo.collider == false)
        //{
        //    if (movingRight == true)
        //    {
        //        transform.eulerAngles = new Vector3(0, -180, 0);
        //        movingRight = false;
        //    }
        //    else
        //    {
        //        transform.eulerAngles = new Vector3(0, 0, 0);
        //        movingRight = true;
        //    }
        //}
    }
}
