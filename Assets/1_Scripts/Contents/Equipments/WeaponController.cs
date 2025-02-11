using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Weapon _context;
    private float _currentCooldown;

    public void Setup(Weapon context)
    {
        _context = context;
    }

    public void UseWeapon()
        => HandleGunFireTimer();

    public void Fire()
        => _context.Animator.SetTrigger(_context.FireParamHash);

    public void Reload()
        => _context.Animator.SetTrigger(_context.ReloadParamHash);

    private bool CheckTargetIsDead()
    {
        if (_context.WeaponOwner.Target == null)
            return true;
        return _context.WeaponOwner.Target.IsDead;
    }

    private void SpawnBulletShell()
    {
        GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_BulletShell);
        BulletShell shell = go.GetComponent<BulletShell>();
        Vector3 shellPos = transform.position + _context.WeaponStatus.BulletShellPosition;
        shell.Setup(shellPos);
    }

    #region AnimationEventTrigger
    private void HandleAttackTarget()
    {
        if (!CheckTargetIsDead())
            _context.WeaponOwner.Target.GetDamaged(_context.WeaponStatus.Damage);
        _context.WeaponStatus.Fire();
        SpawnBulletShell();
    }

    private void HandleGunFireTimer()
    {
        if (_currentCooldown < _context.WeaponStatus.FireRate)
            _currentCooldown += Time.deltaTime;
        else
        {
            if (_context.WeaponOwner.Target == null)
                return;
            if (_context.WeaponStatus.Magazine <= 0)
                return;

            Fire();
            _currentCooldown = 0f;
        }
    }

    private void HandleReload()
    {
        _context.WeaponStatus.Reload();
    }
    #endregion
}
