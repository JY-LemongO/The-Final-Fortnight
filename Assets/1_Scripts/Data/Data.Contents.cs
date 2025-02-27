using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class GenericLoader<TValue> : ILoader<int, TValue> where TValue : IConvertRowData
    {
        public List<TValue> rows = new();

        Dictionary<int, TValue> ILoader<int, TValue>.MakeDict()
        {
            Dictionary<int, TValue> dict = new();

            foreach (TValue row in rows)
            {
                int key = row.Id;
                dict[key] = row;
            }
            return dict;
        }
    }

    [Serializable]
    public abstract class CommonObjectData : IConvertRowData
    {
        public int Id => id;

        public int id;
        public string codeName;
        public string displayName;
        public string displayDesc;

        public virtual void ConvertRow(List<string> row)
        {
            id = int.Parse(row[0]);
            codeName = row[1];
            displayName = row[2];
            displayDesc = row[3];
        }
    }

    [Serializable]
    public abstract class CommonEntityData : CommonObjectData
    {
        public float hp;
        public float hpBarOffset;
        public float hpBarWidth;
        public string animControllerKey;

        public override void ConvertRow(List<string> row)
        {
            base.ConvertRow(row);
            hp = float.Parse(row[4]);
            hpBarOffset = float.Parse(row[5]);
            hpBarWidth = float.Parse(row[6]);
            animControllerKey = row[7];
        }
    }

    [Serializable]
    public class SurvivorData : CommonEntityData
    {
        public int defaultWeaponId;
        public string profileSpriteKey;

        public override void ConvertRow(List<string> row)
        {
            base.ConvertRow(row);
            defaultWeaponId = int.Parse(row[8]);
            profileSpriteKey = row[9];
        }
    }

    [Serializable]
    public class ZombieData : CommonEntityData
    {
        public int level;
        public float moveSpeed;
        public float range;
        public float attack;
        public float attackRate;

        public override void ConvertRow(List<string> row)
        {
            base.ConvertRow(row);
            level = int.Parse(row[8]);
            moveSpeed = float.Parse(row[9]);
            range = float.Parse(row[10]);
            attack = float.Parse(row[11]);
            attackRate = float.Parse(row[12]);
        }
    }

    [Serializable]
    public class WeaponData : CommonObjectData
    {
        public float damage;
        public int magazine;
        public float fireRange;
        public float fireRate;
        public string animControllerKey;
        public string profileSpriteKey;
        public Vector2 weaponPosition;
        public Vector2 bulletShellPosition;

        public override void ConvertRow(List<string> row)
        {
            float weaponPosX = float.Parse(row[10]);
            float weaponPosY = float.Parse(row[11]);
            float bulletShellPosX = float.Parse(row[12]);
            float bulletShellPosY = float.Parse(row[13]);

            base.ConvertRow(row);
            damage = float.Parse(row[4]);
            magazine = int.Parse(row[5]);
            fireRange = float.Parse(row[6]);
            fireRate = float.Parse(row[7]);
            animControllerKey = row[8];
            profileSpriteKey = row[9];
            weaponPosition = new Vector2(weaponPosX, weaponPosY);
            bulletShellPosition = new Vector2(bulletShellPosX, bulletShellPosY);
        }
    }

    [Serializable]
    public class StructureData : CommonEntityData
    {
        public Define.StructureType structureType;
        public string objectSpriteKey;
        public string previewSpriteKey;
        public int buildCost;

        public override void ConvertRow(List<string> row)
        {            
            base.ConvertRow(row);
            if (!Enum.TryParse(row[8], true, out structureType))
                DebugUtility.LogError($"[Data.COntents] Structure 타입이 올바르지 않습니다. Type::{row[8]}");
            objectSpriteKey = row[9];
            previewSpriteKey = row[10];
            buildCost = int.Parse(row[11]);
        }
    }

    [Serializable]
    public class BarricateData : StructureData
    {
        public int tier;
        public int upgradeCost;

        public override void ConvertRow(List<string> row)
        {
            base.ConvertRow(row);
            tier = int.Parse(row[12]);
            upgradeCost = int.Parse(row[13]);
        }
    }

    [Serializable]
    public class TurretData : StructureData
    {
        public int experimentCost;
        public float damage;
        public float lifeTime;
        public float fireRate;
        public float fireRange;

        public override void ConvertRow(List<string> row)
        {
            base.ConvertRow(row);
            experimentCost = int.Parse(row[13]);
            damage = float.Parse(row[14]);
            lifeTime = float.Parse(row[15]);
            fireRate = float.Parse(row[16]);
            fireRange = float.Parse(row[17]);
        }
    }

    [Serializable]
    public class WaveData : IConvertRowData
    {
        public int Id => id;

        public int id;
        public int wave;
        public int zombieId;
        public float waitTime;
        public float spawnInterval;
        public int spawnCount;

        public void ConvertRow(List<string> row)
        {
            id = int.Parse(row[0]);
            wave = int.Parse(row[1]);
            zombieId = int.Parse(row[2]);
            waitTime = float.Parse(row[3]);
            spawnInterval = float.Parse(row[4]);
            spawnCount = int.Parse(row[5]);
        }
    }

    [Serializable]
    public class GachaData : IConvertRowData
    {
        public int Id => id;

        public int id;
        public int survivorId;
        public float weight;

        public void ConvertRow(List<string> row)
        {
            id = int.Parse(row[0]);
            survivorId = int.Parse(row[1]);
            weight = float.Parse(row[2]);
        }
    }

    [Serializable]
    public class SelectableSurvivorsData : IConvertRowData
    {
        public int Id => survivorId;

        public int survivorId;        

        public void ConvertRow(List<string> row)
        {
            survivorId = int.Parse(row[0]);
        }
    }
}

