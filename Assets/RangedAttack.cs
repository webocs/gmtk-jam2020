using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public GameObject[] projectiles;
    public EnemyController enemyController;
    [Min(1)]
    public float attackRate;

    private float attackTimer;

    private void Awake()
    {
        attackTimer = attackRate;
        enemyController = GetComponent<EnemyController>();
    }
    private void Update()
    {
        if (!enemyController.isMoving && attackTimer < 0)
        {
            List<Vector2> directions = new List<Vector2>();
            if (!enemyController.leftBoundaryChecker.IsColliding) directions.Add(Vector2.left);
            if (!enemyController.rightBoundaryChecker.IsColliding) directions.Add(Vector2.right);
            if (!enemyController.topBoundaryChecker.IsColliding) directions.Add(Vector2.up);
            if (!enemyController.bottomBoundaryChecker.IsColliding) directions.Add(Vector2.down);
            if (directions.Count > 0)
            {
                attack(directions[UnityEngine.Random.Range(0, directions.Count)]);
                attackTimer = attackRate;
            }            
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void attack(Vector2 direction)
    {
        GameObject projectile = null;
        if (direction == Vector2.left) projectile = projectiles[0];
        if (direction == Vector2.up) projectile = projectiles[1];
        if (direction == Vector2.right) projectile = projectiles[2];
        if (direction == Vector2.down) projectile = projectiles[3];
        Instantiate(projectile, (Vector2)transform.position + direction, Quaternion.identity);
    }
}
