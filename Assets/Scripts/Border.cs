using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{

    public delegate void EnemyHitBorder();
    public static event EnemyHitBorder OnEnemyHitBorder;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Random"))
            return;
        
        if (CompareTag("LeftBorder"))
        {
            SpawnerController.SetMovingRight(true);
            OnEnemyHitBorder();
        } 
        else if (CompareTag("RightBorder"))
        {
            SpawnerController.SetMovingRight(false);
            OnEnemyHitBorder();
        }
    }
}
