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
        _survivor.Weapon.WeaponData.Magazine.OnStatCurrentValueChanged += OnBulletUpdate;
        _bulletText.text = _survivor.Weapon.WeaponData.Magazine.CurrentValue.ToString();
    }

    private void OnBulletUpdate(float current, float total)
    {
        _bulletImage.fillAmount = current / total;
        _bulletText.text = current > 0 ? $"{(int)current}" : "재장전";
    }

    protected override void Dispose()
    {
        if (!gameObject.activeSelf)
            return;

        _survivor.Weapon.WeaponData.Magazine.OnStatCurrentValueChanged -= OnBulletUpdate;
        _survivor = null;
        base.Dispose();
    }
}
