public static class Constants
{
    #region Timer
    public const float SearchingDelay = 0.1f;
    public const float HPBarFadeOutWaitTime = 1f;
    public const float HitFlashTime = 0.1f;
    #endregion

    #region Key_Addressables
    // Labels
    public const string Key_Init = "Init";
    public const string Key_Prefabs = "Prefabs";
    public const string Key_Entities = "Entities";
    public const string Key_Equipments = "Equipments";

    // Prefabs
    public const string Key_Survivor = "Survivor.prefab";
    public const string Key_Zombie = "Zombie.prefab";
    public const string Key_Weapon = "Weapon.prefab";
    public const string Key_Barricate = "Barricate.prefab";
    public const string Key_BulletShell = "BulletShell.prefab";
    public const string Key_PreviewObject = "PreviewObject.prefab";

    // Effects
    public const string Key_Z_HitParticle = "FleshImpact_E.prefab";
    public const string Key_Z_HitAnimation = "E_Hit_Blood.prefab";
    public const string Key_DamageText = "DamageText.prefab";

    // UI
    public const string Key_HPBar = "HP_Bar.prefab";

    // Entities - Define으로 교체예정
    public const string Key_S_Soldier_01 = "SURVIVOR_SOLDIER_01";
    public const string Key_S_Soldier_02 = "SURVIVOR_SOLDIER_02";
    public const string Key_Z_Normal_01 = "ZOMBIE_NORMAL_01";
    public const string Key_Z_Normal_02 = "ZOMBIE_NORMAL_02";

    // Equipments
    public const string Key_W_Pistol_02 = "WEAPON_PISTOL_01.asset";
    #endregion

    #region Values
    public const string HitFlashAmountShaderKey = "_FlashAmount";

    public const string DamageColorHEX_L = "#FF9700";
    public const string DamageColorHEX_M = "#FF4F00";
    public const string DamageColorHEX_H = "#FF0092";

    public const float DamageTextFontSize_L = 4f;
    public const float DamageTextFontSize_M = 6f;
    public const float DamageTextFontSize_H = 10f;

    public const string TARGET_TAG = "HumanSideEntity";

    public const int Barricate_Buildable = 3;

    public const float Min_Build_PosX = -3f;
    public const float Max_Build_PosX = 5f;

    public const float Min_Build_PosY = -2.5f;
    public const float Max_Build_PosY = 0f;

    public const int InventorySlotCount = 28;
    public const int SurvivorSlotCount = 5;
    #endregion
}
