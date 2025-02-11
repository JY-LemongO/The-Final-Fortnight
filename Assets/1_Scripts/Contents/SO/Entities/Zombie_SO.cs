using UnityEngine;

[CreateAssetMenu(menuName = "Entity/New Zombie", fileName = "ZOMBIE_")]
public class Zombie_SO : Entity_SO
{
    [Header("Zombie Stats")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _range;
    [SerializeField] private float _atk;
    [SerializeField] private float _atkRate;    

    public float MoveSpeed => _moveSpeed;
    public float Range => _range;
    public float Atk => _atk;
    public float AtkRate => _atkRate;    
}
