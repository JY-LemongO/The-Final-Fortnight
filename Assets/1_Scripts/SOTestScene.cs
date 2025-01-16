using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SOTestScene : MonoBehaviour
{
    [SerializeField] private string key_Prefabs;
    [SerializeField] private string key_Pistol;

    [SerializeField] private TMP_Text _weapon1Name;
    [SerializeField] private TMP_Text _weapon1Ammo;

    [SerializeField] private TMP_Text _weapon2Name;
    [SerializeField] private TMP_Text _weapon2Ammo;

    [SerializeField] private Button _fireWeapon1;
    [SerializeField] private Button _fireWeapon2;

    [SerializeField] private Weapon_SO _weapon1;
    [SerializeField] private Weapon_SO _weapon2;

    private void Awake()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
                ResourceManager.Instance.Dispose();
        };

        _fireWeapon1.onClick.AddListener(HandleFireWeapon1);
        _fireWeapon2.onClick.AddListener(HandleFireWeapon2);
        StartCoroutine(Co_LoadingResource());
    }

    private IEnumerator Co_LoadingResource()
    {
        var opHandle = ResourceManager.Instance.LoadAllAsyncByLabel<BaseSO>(key_Prefabs);
        while (!opHandle.IsDone)
        {
            Debug.Log("리소스 로드 중...");
            yield return new WaitForSeconds(1f);
        }

        var originWeapon = ResourceManager.Instance.Load<Weapon_SO>(key_Pistol);

        _weapon1 = originWeapon.Clone() as Weapon_SO;
        _weapon1Name.text = _weapon1.DisplayName;
        _weapon1Ammo.text = _weapon1.Magazine.CurrentValue.ToString();

        _weapon2 = originWeapon.Clone() as Weapon_SO;
        _weapon2Name.text = _weapon2.DisplayName;
        _weapon2Ammo.text = _weapon2.Magazine.CurrentValue.ToString();
    }

    private void HandleFireWeapon1()
    {
        _weapon1.Magazine.Consume(1);
        _weapon1Ammo.text = _weapon1.Magazine.CurrentValue.ToString();
    }

    private void HandleFireWeapon2()
    {
        _weapon2.Magazine.Consume(1);
        _weapon2Ammo.text = _weapon2.Magazine.CurrentValue.ToString();
    }
}

