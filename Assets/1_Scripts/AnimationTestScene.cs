using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTestScene : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _fireBtn;
    [SerializeField] private Button _reloadBtn;

    [Header("Weapons")]
    [SerializeField] private List<Weapon> _weapons;

    public void OnAnimFire()
    {
        foreach (var weapon in _weapons)
            weapon.Animator.SetTrigger("Fire");
    }

    public void OnAnimReload()
    {
        foreach (var weapon in _weapons)
            weapon.Animator.SetTrigger("Reload");
    }
}
