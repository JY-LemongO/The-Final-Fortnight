using System.Collections;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;

    private Zombie _context;    

    public void Setup(Zombie context)
    {
        _context = context;
        _context.Status.OnDead += Dead;
    }        

    public void SearchTargetByRay()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * _context.SpriteHalfSize;
        Vector2 direction = Vector2.left;
        float distance = _context.ZombieStatus.Range;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, _targetLayer);
        if (hit)
        {
            Entity target = hit.transform.GetComponent<Entity>();
            if (target.Status.IsDead)
                return;

            _context.SetTarget(target);
            Attack();
        }
    }

    public void Move()
        => StartCoroutine(Co_Move());

    public void Attack()
        => _context.Animator.SetBool(_context.AttackParamHash, true);

    public void Dead()
        => _context.Animator.SetTrigger(_context.DieParamHash);

    #region AnimationEventTrigger
    private void HandleAttackTarget()
    {
        DebugUtility.Log($"[ZombieController] 좀비가 {_context.Target.name}을 공격.");
        float damage = _context.ZombieStatus.Attack;
        _context.Target.GetDamaged(damage);
    }

    private void HandleFindTarget()
    {
        if (_context.Target.Status.IsDead)
        {
            _context.SetTarget(null);
            Move();
        }
    }

    private void HandleDie()
    {
        _context.ResetEntity();
    }
    #endregion

    private IEnumerator Co_Move()
    {
        _context.Animator.SetBool(_context.AttackParamHash, false);
        _context.Animator.SetBool(_context.WalkParamHash, true);
        while (true)
        {
            if (_context.Status.IsDead) break;
            if (_context.Target != null) break;

            transform.position += Vector3.left * _context.ZombieStatus.MoveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
