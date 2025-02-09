using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IAnimatedObject
{
    #region Events
    public event Action<int, int> OnMagazineValueChanged;
    #endregion

    #region Renderer
    [Header("AnimationData")]
    [SerializeField] private string _fireParamName;
    [SerializeField] private string _reloadParamName;

    private int _fireParamHash;
    private int _reloadParamHash;
    #endregion

    public Animator Animator { get; private set; }
    public WeaponStatus WeaponStatus { get; private set; }

    private SpriteRenderer _renderer;
    private Survivor _context;

    private float _currentCooldown;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (_context == null)
            return;

        HandleGunFireTimer();
    }

    public void InitWeapon(Survivor context, Weapon_SO weapon)
    {
        _context = context;
        ChangeWeapon(weapon);
    }

    public void ChangeWeapon(Weapon_SO newWeapon)
    {
        transform.position = newWeapon.WeaponPosition;
        Animator.runtimeAnimatorController = newWeapon.AnimController;
        WeaponStatus.Setup(newWeapon);
    }        

    public void Fire()
        => Animator.SetTrigger(_fireParamHash);

    private void Reload()
        => Animator.SetTrigger(_reloadParamHash);

    private void Init()
    {
        ComponentsSetting();
        AnimationHashInitialize();
    }

    private void ComponentsSetting()
    {
        Animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sortingOrder = Util.GetSortingOreder(Define.SpriteType.Weapon);
    }

    private void SpawnBulletShell()
    {
        GameObject go = ResourceManager.Instance.Instantiate(Constants.Key_BulletShell);
        BulletShell shell = go.GetComponent<BulletShell>();
        Vector3 shellPos = transform.position + WeaponStatus.BulletShellPosition;
        shell.Setup(shellPos);
    }

    private void HandleGunFireTimer()
    {
        if (_currentCooldown < WeaponStatus.FireRate)
            _currentCooldown += Time.deltaTime;
        else
        {
            if (_context.Target == null)
                return;
            if (WeaponStatus.Magazine <= 0)
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
        if (!_context.Controller.CheckTargetIsDead())
            _context.Controller.AttackTarget(WeaponStatus.Damage);
        WeaponStatus.Fire();
        SpawnBulletShell();
    }

    private void HandleReload()
    {
        WeaponStatus.Reload();
    }

    public void SetAnimatorController(RuntimeAnimatorController controller)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
