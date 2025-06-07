using Photon.VR;
using UnityEngine;
using static UnityEditor.PlayerSettings;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class BetterChangeCosmetic : MonoBehaviour
{
    [HideInInspector]
    public string Slot = "";
    [HideInInspector]
    public string CosmeticName = "";

    [HideInInspector]
    public bool ShowInfos = false;

    [Header("Trigger")]
    public string HandTag = "HandTag";


    const string SOPath = "Assets/Keos Stuff/PhotonCosmetics/SO/CosmeticListMain.asset";

    [HideInInspector]
    public CosmeticListMain SO = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(HandTag))
        {
            ChangeCosmetic();
        }
    }
    public void ChangeCosmetic()
    {
        PhotonVRManager.SetCosmetic(Slot, CosmeticName);
    }

    public void SetSO()
    {
        SO = AssetDatabase.LoadAssetAtPath<CosmeticListMain>(SOPath);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(BetterChangeCosmetic))]
public class BetterChangeCosmeticEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BetterChangeCosmetic script = (BetterChangeCosmetic)target;

        GUILayout.Space(10);
        GUILayout.Label("Info", EditorStyles.boldLabel);

        if (script.SO)
        {
            if (GUILayout.Button(script.ShowInfos ? "Hide Infos" : "Show Infos", GUILayout.Width(100)))
            {
                script.ShowInfos = !script.ShowInfos;
            }
            if (script.ShowInfos)
            {
                GUILayout.Label("Current SO:");
                GUILayout.Label(string.IsNullOrEmpty(script.SO.name) ? "None" : script.SO.name, EditorStyles.wordWrappedLabel);
                GUILayout.Label("Current SO Path:");
                GUILayout.Label(string.IsNullOrEmpty(script.SO.name) ? "None" : AssetDatabase.GetAssetPath(script.SO), EditorStyles.wordWrappedLabel);
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("Current Slot: " + (string.IsNullOrEmpty(script.Slot) ? "None" : script.Slot), EditorStyles.boldLabel);
        GUILayout.Label("Current Cosmetic: " + (string.IsNullOrEmpty(script.CosmeticName) ? "None" : script.CosmeticName), EditorStyles.boldLabel);

        GUILayout.Space(10);

        if (script.SO)
        {
            GUILayout.Label("Cosmetic Options", EditorStyles.boldLabel);
            foreach (var category in script.SO.Cosmetics)
            {
                GUILayout.Label(category.Name, EditorStyles.largeLabel);

                if (GUILayout.Button("None"))
                {
                    script.Slot = category.Name;
                    script.CosmeticName = "";
                }

                foreach (var cosmetic in category.Cosmetic)
                {
                    if (GUILayout.Button(cosmetic.Name, GUILayout.Height(25)))
                    {
                        script.Slot = category.Name;
                        script.CosmeticName = cosmetic.Name;
                    }
                }
                GUILayout.Space(5);
            }
        }
        else
        {
            GUILayout.Space(10);
            if (GUILayout.Button("Load SO", GUILayout.Height(30)))
            {
                script.SetSO();
            }
        }
    }
}

#endif