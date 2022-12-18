using TMPro;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyScriptable enemy;
    public int health;
    public int damage;
    public float aggroRange;
    public float attackRange;
    public float stoppingDistance;
    GameObject player;
    GameObject box;
    Vector3 target;
    public float speed;
    float wanderDistance = 5f;
    Quaternion targetRotation;
    bool toMove;
    public bool attacking;
    Rigidbody rb;
    Animator animator;
    TextMeshProUGUI healthSlide;
    TextMesh healthLoss;
    delegate void damageIndicator(GameObject gameObject,int amount);
    damageIndicator indicator;
    delegate void playerHealth(int amount);
    playerHealth pHealth;

    enum State
    {
        Wander,
        Aggrod,
        Attacking
    }
    State currentState;
     

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Weapon")
        {
            int damage = collision.collider.GetComponent<Weapon>().damage;
            indicator(this.gameObject,damage);
            health -= damage;
            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }

        if (currentState == State.Attacking && collision.collider.name == "PlayerModel")
        {
            indicator(player,damage);
            pHealth(damage);
        }


    }

    public void updateStats()
    {
        this.health = enemy.health;
        this.damage = enemy.damage;
        this.aggroRange = enemy.aggroRange;
        this.attackRange = enemy.attackRange;
        this.speed = enemy.speed;
        currentState = State.Wander;
        target = Vector3.zero;
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        stoppingDistance = 1.5f;
        speed = 4f;
        indicator = EnemyUI.instance.damageIndicator;
        pHealth = Player.instance.updateHealth;
    }

    void Update()
    {
        currentState = getState();
        switch (currentState)
        {
            case State.Attacking:
                toMove = false;
                if (!attacking)
                {
                    attack();
                }
                
                break;
            case State.Wander:
                toMove = true;
                if (NeedsDestination())
                {
                    getTarget();
                }
                break;
            case State.Aggrod:
                toMove = true;
                getTarget();
                break;
        }
        if (toMove)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        
    }

    State getState()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance < attackRange)
        {
            return State.Attacking;
        }
        if (distance < aggroRange)
        {
            return State.Aggrod;
        }
        return State.Wander;
    }

    private bool NeedsDestination()
    {
        if (target == Vector3.zero)
        {
            return true;
        }

        float targetDistance;
        targetDistance = Vector3.Distance(transform.position, target);
        if (targetDistance <= stoppingDistance || targetDistance>aggroRange)
        {
            return true;
        }

        return false;
    }

    void getTarget()
    {
        if (currentState==State.Wander)
        {
            Vector3 toMove = (transform.position + (transform.forward * 4f)) + new Vector3(UnityEngine.Random.Range(-wanderDistance, wanderDistance), 0f, UnityEngine.Random.Range(-wanderDistance, wanderDistance));
            target = new Vector3(toMove.x, transform.position.y, toMove.z);
            Vector3 direction = Vector3.Normalize(target - transform.position);
            direction = new Vector3(direction.x, 0f, direction.z);
            targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
        else if(currentState==State.Aggrod)
        {
            target = player.transform.position;
            transform.LookAt(player.transform);
        }
    }
     
    void attack()
    {
        if (!attacking)
        {
            attacking = true;
            animator.CrossFade("Attack", 0.1f);
        }
        Invoke(nameof(noAttack), 3f);
    }

    void noAttack()
    {
        attacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, aggroRange);
        Gizmos.DrawLine(transform.position, target);
        Gizmos.color = Color.yellow;
        Vector3 fw = transform.forward;
        Gizmos.DrawRay(transform.forward, new Vector3(fw.x+5,fw.y,fw.z+5));
    }


}
