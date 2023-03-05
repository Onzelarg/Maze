using System.Linq;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public EnemyScriptable enemy;
    public float health;
    float maxHealth;
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
    public GameObject healthbar;
    Camera mainCam;
    Image healthImage;
    float spawnRange;
    float despawnRange;
    Vector3 lastPosition;
    float update_time;
    float time;

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
        mainCam = Camera.main;
        update_time = 2.4f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Weapon")
        {
            int damage = collision.collider.GetComponent<Weapon>().damage;
            indicator(this.gameObject,damage);
            health -= damage;
            healthImage.fillAmount = (health / maxHealth);
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
        this.maxHealth = enemy.health;
        this.damage = enemy.damage;
        this.aggroRange = enemy.aggroRange;
        this.attackRange = enemy.attackRange;
        this.speed = enemy.speed;
        this.spawnRange = enemy.spawnRange;
        this.despawnRange = enemy.despawnRange;
        currentState = State.Wander;
        target = Vector3.zero;
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        stoppingDistance = 1.5f;
        speed = 4f;
        indicator = EnemyUI.instance.damageIndicator;
        pHealth = Player.instance.updateHealth;
         
        Vector3 boundY = gameObject.GetComponent<Collider>().bounds.size;
        Vector3 goPosition = gameObject.transform.position;
        Vector3 position = new Vector3(goPosition.x, (goPosition.y + boundY.y), goPosition.z);
        healthbar=Instantiate(healthbar, position, mainCam.transform.rotation, transform);
        healthbar.transform.name = gameObject.transform.name+" healthbar";
        Image[] images = new Image[2];
        images = healthbar.GetComponentsInChildren<Image>();
        healthImage = images[1];
        lastPosition = transform.position;
    }
     
    void Update()
    {
        time += Time.deltaTime;
        if (time >= update_time) checkPosition();
      



        if (healthbar!=null)
        {
            healthbar.transform.rotation = mainCam.transform.rotation;
        }
        
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

    void checkPosition()
    {
        time -= update_time;
        Vector3 difference = transform.position-lastPosition;
        float differenceB = Mathf.Abs(difference.x) + Mathf.Abs(difference.y) + Mathf.Abs(difference.z);
        lastPosition = transform.position;
        if (differenceB < 1.5f) getTarget();
    }

    State getState()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance>despawnRange) deSpawn();
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

    void deSpawn()
    {
        Destroy(gameObject);
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
            bool noObstacle = false;
            do
            {
                Vector3 toMove = (transform.position + (transform.forward * 4f)) + new Vector3(UnityEngine.Random.Range(-wanderDistance, wanderDistance), 0f, UnityEngine.Random.Range(-wanderDistance, wanderDistance));
                target = new Vector3(toMove.x, transform.position.y, toMove.z);               
                Vector3 direction = Vector3.Normalize(target - transform.position);
                direction = new Vector3(direction.x, 0f, direction.z);
                targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;
                noObstacle = obstacle(direction);
            } while (noObstacle);
        }
        else if(currentState==State.Aggrod)
        {
            target = player.transform.position;
            transform.LookAt(player.transform);
        }
    }

    bool obstacle(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit[] hitSomething = Physics.RaycastAll(ray, wanderDistance*1.1f, 11);
        return hitSomething.Any();
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
        Gizmos.DrawWireSphere(this.transform.position, despawnRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, aggroRange);
        Gizmos.DrawLine(transform.position, target);
        Gizmos.color = Color.yellow;
        Vector3 fw = transform.forward;
        Gizmos.DrawRay(fw, new Vector3(fw.x+5,fw.y,fw.z+5));
    }


}
