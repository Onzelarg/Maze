using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Objects/Enemy", order = 1)]
public class EnemyScriptable : ScriptableObject
{
    public int id;
    public int health;
    public float aggroRange;
    public float attackRange;
    public float spawnRange;
    public float despawnRange;
    public float speed;
    public int damage;
    
}
