using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region renderer
    [Header("AnimationData")]
    [SerializeField] private string _fireParamName;
    [SerializeField] private string _reloadParamName;

    public Animator AnimForTest => _anim;

    private SpriteRenderer _renderer;
    private Animator _anim;
    private int _fireParamHash;
    private int _reloadParamHash;
    #endregion

    public Weapon_SO WeaponData => _currentWeapon;
    private Weapon_SO _currentWeapon;
    private Survivor _context;

    private float _currentCooldown;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sortingOrder = Util.GetSortingOreder(Define.SpriteType.Weapon);

        AnimationHashInitialize();
    }

    private void Update()
    {
        if (_context == null)
            return;

        HandleGunFireTimer();
    }

    public void SetWeapon(Survivor context, Weapon_SO weapon)
    {
        _context = context;
        _currentWeapon = weapon;
        _anim.runtimeAnimatorController = weapon.AnimController;

        _currentWeapon.OnReloadWeapon += Reload;
    }

    public void ChangeWeapon(Weapon_SO newWeapon)
        => _currentWeapon = newWeapon;

    public void Fire()
        => _anim.SetTrigger(_fireParamHash);

    private void Reload()
        => _anim.SetTrigger(_reloadParamHash);

    private void SpawnBulletShell()
    {
        GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_BulletShell);
        BulletShell shell = go.GetComponent<BulletShell>();
        Vector3 shellPos = transform.position + _currentWeapon.BulletShellPosition;
        shell.Setup(shellPos);
    }

    private void HandleGunFireTimer()
    {
        if (_currentCooldown < _currentWeapon.FireRate.Value)
            _currentCooldown += Time.deltaTime;
        else
        {
            if (_context.Target == null)
                return;
            if (_currentWeapon.Magazine.CurrentValue <= 0)
                return;

            Fire();
            _currentCooldown = 0f;
        }
    }

    #region AnimationEventTrigger
    private void AnimationHashInitialize()
    {
        _fireParamHash = Animator.StringToHash(_fireParamName);
        _reloadParamHash = Animator.StringToHash(_reloadParamName);
    }

    private void HandleAttackTarget()
    {
        if (!_context.CheckTargetIsDead())
            _context.AttackTarget(_currentWeapon.Damage.Value);
        _currentWeapon.Fire();
        SpawnBulletShell();
    }

    private void HandleReload()
    {
        _currentWeapon.Reload();
    }
    #endregion
}
