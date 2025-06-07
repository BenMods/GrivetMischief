#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using Photon.VR.Player;

public class AutoCosmeticSO : MonoBehaviour
{
    private const string SOPath = "Assets/Keos Stuff/PhotonCosmetics/SO/CosmeticListMain.asset";
    [HideInInspector]
    public CosmeticListMain SO;
#if UNITY_EDITOR
    public void SetSO()
    {
        SO = AssetDatabase.LoadAssetAtPath<CosmeticListMain>(SOPath);
    }

    public void SetUpSO(PhotonVRPlayer player)
    {
        SO.Cosmetics.Clear();


        for (int i = 0; i < player.CosmeticSlots.Count; i++)
        {
            CosmeticListMain.CosmeticL item = new CosmeticListMain.CosmeticL();
            item.Name = player.CosmeticSlots[i].SlotName;
            SO.Cosmetics.Add(item);

            for (int I = player.CosmeticSlots[i].Object.childCount - 1; I >= 0; I--)
            {
                CosmeticListMain.Cosmetic o = new CosmeticListMain.Cosmetic();
                o.Name = player.CosmeticSlots[i].Object.GetChild(I).name;
                SO.Cosmetics[i].Cosmetic.Add(o);
            }
        }

        //foreach (var item in SO.Cosmetics)
        //{
        //    item.Cosmetic.Add()
        //}
    }
#endif
}
#if UNITY_EDITOR
[CustomEditor(typeof(AutoCosmeticSO))]
public class AutoCosmeticSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AutoCosmeticSO s = (AutoCosmeticSO)target;

        if (!s.SO)
        {
            if (GUILayout.Button("Get SO"))
            {
                s.SetSO();
            }
        }
        else
        {
            GUILayout.Label("Current SO: " + s.SO.name);
            GUILayout.Label("SO Path: " + AssetDatabase.GetAssetPath(s.SO));

            if (s.TryGetComponent<PhotonVRPlayer>(out PhotonVRPlayer p))
            {
                if (GUILayout.Button("Set Up / Refresh SO"))
                {
                    s.SetUpSO(p);
                }
            }
        }
    }
}
#endif