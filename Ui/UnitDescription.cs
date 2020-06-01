using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class UnitDescription : MonoBehaviour
{
    public UnitDescript UnitDes;

    [ContextMenu("ToDateJason")]
    void SaveDateJason()
    {
        string jsonData = JsonUtility.ToJson(UnitDes);
        string path = Application.dataPath + "/Jason" + UnitDes + ".json";
        File.WriteAllText(path, jsonData);
    }
}

[System.Serializable]
public class UnitDescript
{
    public SoliderUnit soliderUnit;
    public ArrowUnit arrowUnit;
    public SpearUnit spearUnit;
    public HorseWariorUnit horseUnit;
    public CatapultUnit catapultUnit;
}


[System.Serializable]
public class ArrowUnit
{
    public string name;
    public int Hp;
    public int Attack;
    public int SpawnTime;
    public int count;
    public int price;
    public float speed;
    public string Description1;
    public string Description2;
}


[System.Serializable]
public class SoliderUnit
{
    public string name;
    public int Hp;
    public int Attack;
    public int SpawnTime;
    public int count;
    public int price;
    public float speed;
    public string Description1;
    public string Description2;
}

[System.Serializable]
public class SpearUnit
{
    public string name;
    public int Hp;
    public int Attack;
    public int SpawnTime;
    public int count;
    public int price;
    public float speed;
    public string Description1;
    public string Description2;
}

[System.Serializable]
public class HorseWariorUnit
{
    public string name;
    public int Hp;
    public int Attack;
    public int SpawnTime;
    public int count;
    public int price;
    public float speed;
    public string Description1;
    public string Description2;
}

[System.Serializable]
public class CatapultUnit
{
    public string name;
    public int Hp;
    public int Attack;
    public int SpawnTime;
    public int count;
    public int price;
    public float speed;
    public string Description1;
    public string Description2;
}