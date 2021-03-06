﻿using MonsterLove.StateMachine;
using State;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float distanceToFindEnemy = 5;
    [SerializeField] private float distanceToAttack = 3f;
    [SerializeField] private float cooldown;
    [SerializeField] private float health;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private Bullet bulletPrfab;

    public new Collider2D collider { get; private set; }
    public new Rigidbody2D rigidbody2D { get; private set; }

    //public int Health => (int)health;

    public AudioManager am;
    StateMachine<FlyingEnemyStates> fsm;
    private GameObject objectToAttack;

    private float lastShootTime = int.MaxValue;
    Animator animator;

    enum FlyingEnemyStates
    {
        SearchingForObjectToShootTo,
        MoveToObject,
        Atack,
        Cooldown,
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        fsm = StateMachine<FlyingEnemyStates>.Initialize(this);
        fsm.ChangeState(FlyingEnemyStates.SearchingForObjectToShootTo);
        lastShootTime = Time.time;

        am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        am.playAudio("EnemyFlyingSpawn", 0.1f);

    }

    void SearchingForObjectToShootTo_Enter()
    {
        if (objectToAttack == null)
        {
            objectToAttack = FindEnemy();
            Obstacle obstacle = FindTile();

            if (objectToAttack == null && obstacle != null) objectToAttack = obstacle.gameObject;
            fsm.ChangeState(FlyingEnemyStates.MoveToObject);
        } 
        
    }

    void MoveToObject_Update()
    {
        if (objectToAttack == null) 
        {
            fsm.ChangeState(FlyingEnemyStates.SearchingForObjectToShootTo);
            return;
        }
        rigidbody2D.AddForce(GetDirctionTowardsEnemy() * movementSpeed);  
        //Vector2 toPosition = objectToAttack.transform.position - GetDirctionTowardsEnemy() * distanceToAttack;

        //transform.position = Vector2.Lerp(transform.position, objectToAttack.transform.position, Time.deltaTime);
        float distance = Vector2.Distance(objectToAttack.transform.position, transform.position);

        if (distance <= distanceToAttack)
        {
            fsm.ChangeState(FlyingEnemyStates.Atack);
        }
    }

    void Atack_Update()
    {
        rigidbody2D.velocity = Vector2.zero;

        if (objectToAttack == null) fsm.ChangeState(FlyingEnemyStates.SearchingForObjectToShootTo);

        if (Time.time - lastShootTime >= cooldown)
        {
            ShootInDirection(GetDirctionTowardsEnemy());
        }
        float distance = Vector2.Distance(objectToAttack.transform.position, transform.position);

        if (distance > distanceToAttack)
        {
            fsm.ChangeState(FlyingEnemyStates.MoveToObject);
        }
    }

    public void ShootInDirection(Vector2 direction)
    {
        lastShootTime = Time.time;
        Vector2 directionWithOffset = direction * Mathf.Max(collider.bounds.extents.x, collider.bounds.extents.y);
        Vector3 spawnPosition = transform.position + new Vector3(directionWithOffset.x, directionWithOffset.y, 0);
        Bullet bullet = Instantiate(bulletPrfab, spawnPosition, Quaternion.identity);
        bullet.Move(direction);
        am.playAudio("EnemyShot", 0.1f);
        animator.SetTrigger("shoot");
    }

    private Vector2 GetDirctionTowardsEnemy()
    {
        return (objectToAttack.transform.position - transform.position).normalized;
    }

    public Obstacle FindTile()
    {
        float distance = 10000f;
        Vector3 direction = HouseManager.instance.GetHouseCenterPoint() - transform.position;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, direction.normalized, distance, UnityConstants.Layers.HouseWallMask);
        Debug.DrawLine(transform.position, raycastHit.point);  
        if (raycastHit.transform!= null)
        {
            return raycastHit.transform.GetComponent<Obstacle>();
        }
        return null;
    }


    public GameObject FindEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, distanceToFindEnemy);
        Debug.DrawLine(transform.position, transform.position + new Vector3(distanceToFindEnemy,0,0));

        for (int i = 0; i < colliders.Length; i++)
        {
            WarriorMachine warrior = colliders[i].GetComponent<WarriorMachine>();
            if (warrior != null) return colliders[i].gameObject;
        }
        return null;
    }

    public void Damage(float damage)
    {
        am.playAudio("EnemyReceiveDamage", 0.1f);
        health -= damage;
        if (health <= 0)
        {
            am.playAudio("EnemyDeath", 0.1f);
            Destroy(gameObject);
        }
    }
}
