using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{

    public int hitsAllowed = 4;
    private int _hitsTaken;
    
    // Start is called before the first frame update
    void Start()
    {
        _hitsTaken = 0;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _hitsTaken++;
        Destroy(col.gameObject);
        
        if (_hitsTaken >= hitsAllowed)
            Destroy(gameObject);
    }
}
