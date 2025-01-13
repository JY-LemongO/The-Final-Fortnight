using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : SingletonBase<SpawnManager>
{
    private Dictionary<string, UnityEngine.Object> _spawnedObjDict = new();

    public SpawnManager()
        => _isDontDestroy = false;    

    public void Spawn()
    {
        
    }

    public void Disspawn()
    {

    }

    public override void Dispose()
    {
        
    }
}
