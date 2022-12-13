using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyScriptable enemy;
    int health;
    float distance;
    float attackRange;
    GameObject player;
    Vector3 target;
    Vector3 position;
    float speed;

    enum State
    {
        Moving,
        Aggrod,
        Attacking
    }
    State currentState;

    void Awake()
    {
        if (enemy!=null)
        {
            this.health = enemy.health;
            this.attackRange = enemy.attackRange;
            this.speed = enemy.speed;
            currentState = State.Moving;
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name=="Weapon")
        {
            health -= collision.collider.GetComponent<Weapon>().damage;
            Debug.Log(health);
            if (health<=0)
            {
                Destroy(this.gameObject);
            }
        }

        if (currentState==State.Attacking && collision.collider.name=="Player")
        {
            
        }
         

    }

    void Update()
    {
        distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance<attackRange)
        {
            currentState = State.Aggrod;
            Debug.Log("Attack!!!!!!!!!!!!44!");
        }
        getTarget();
    }

    void getTarget()
    {
        position = this.transform.position;
        if (currentState==State.Moving)
        {
            position.x += Random.Range(1f, 10f);
            position.z += Random.Range(1f, 10f);
            target = position;
        }
        else if(currentState==State.Aggrod)
        {
            target = player.transform.position;
        }
        Invoke(nameof(move), 1f);
    }

    void move()
    {
        this.transform.Translate(target-player.transform.position);
    }


    void attack()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }


}
