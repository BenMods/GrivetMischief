using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CosmeticListMain", menuName = "Keos Stuff/CosmeticListMain")]
public class CosmeticListMain : ScriptableObject
{
    public List<CosmeticL> Cosmetics = new List<CosmeticL>(0);

    [System.Serializable]
    public class CosmeticL
    {
        [Header("Slot Name")]
        public string Name;
        public List<Cosmetic> Cosmetic = new List<Cosmetic>(0);
    }

    [System.Serializable]
    public class Cosmetic
    {
        [Header("Cosmetic Name")]
        public string Name;
        //public GameObject Prefab;
    }
}
