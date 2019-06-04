using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_circle : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;
    public bool letsChase;

    public Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            letsChase = true;
        }

        if(letsChase == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        }
        //else if(Vector2.Distance(transform.position, player.position) > stoppingDistance)
        //{
        //    transform.position = this.transform.position;
        //}
    }
}
