using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Objects/Enemy", order = 1)]
public class EnemyScriptable : ScriptableObject
{
    public int id;
    public int health;
    public float attackRange;
    public float speed;
}