using MonsterLove.StateMachine;
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
        animator = GetComponent<Animator>();
        fsm = StateMachine<FlyingEnemyStates>.Initialize(this);
        fsm.ChangeState(FlyingEnemyStates.SearchingForObjectToShootTo);
    }

    void SearchingForObjectToShootTo_Enter()
    { 
        if(objectToAttack == null)
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

        rigidbody2D.velocity = GetDirctionTowardsEnemy() * movementSpeed;
        float distance = Vector2.Distance(objectToAttack.transform.position, transform.position);
        if(distance <= distanceToAttack)
        {
            fsm.ChangeState(FlyingEnemyStates.Atack);
        }
    }

    void Atack_Update()
    {
        if (objectToAttack == null) fsm.ChangeState(FlyingEnemyStates.SearchingForObjectToShootTo);
        if(Time.time - lastShootTime >= cooldown)
        {
            ShootInDirection(GetDirctionTowardsEnemy());
        }
    }

    public void ShootInDirection(Vector2 direction)
    {
        lastShootTime = Time.time;
        Vector2 directionWithOffset = direction * Mathf.Max(collider.bounds.extents.x, collider.bounds.extents.y);
        Vector3 spawnPosition = transform.position + new Vector3(directionWithOffset.x, directionWithOffset.y, 0);
        Bullet bullet = Instantiate(bulletPrfab, spawnPosition, Quaternion.identity);
        bullet.Move(direction);
        animator.SetTrigger("shoot");
    }

    private Vector2 GetDirctionTowardsEnemy()
    {
        return objectToAttack.transform.position - transform.position;
    }

    public Obstacle FindTile()
    {
        float distance = Vector2.Distance(transform.position, HouseManager.instance.GetHouseCenterPoint());
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
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
