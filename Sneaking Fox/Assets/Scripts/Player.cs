using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // State
    public int curHealth;
    public int maxHealth = 100;

    void Start()
    {
        curHealth = maxHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
