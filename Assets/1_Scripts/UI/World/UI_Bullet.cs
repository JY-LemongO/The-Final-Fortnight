using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bullet : UI_World
{
    [SerializeField] Image _bulletImage;
    [SerializeField] TMP_Text _bulletText;    

    private Survivor _survivor;

    public void SetSurvivor(Survivor survivor)
    {
        if (_survivor != null)
            return;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        _survivor = survivor;
        _survivor.Weapon.WeaponStatus.OnMagazineValueChanged += OnBulletUpdate;
        _bulletText.text = _survivor.Weapon.WeaponStatus.Magazine.ToString();
    }

    private void OnBulletUpdate(int current, int total)
    {
        _bulletImage.fillAmount = current / (float)total;
        _bulletText.text = current > 0 ? $"{current}" : "재장전";
    }

    protected override void Dispose()
    {
        if (!gameObject.activeSelf)
            return;

        _survivor.Weapon.WeaponStatus.OnMagazineValueChanged -= OnBulletUpdate;
        _survivor = null;
        base.Dispose();
    }
}
