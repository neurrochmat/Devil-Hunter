#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class CreateCreditSceneDataHelper
{
    [MenuItem("Assets/Create/Game/Credit Scene Data", false, 10)]
    public static void CreateCreditSceneData()
    {
        // Buat instance ScriptableObject baru
        CreditSceneData data = ScriptableObject.CreateInstance<CreditSceneData>();
        
        // Tentukan path tempat menyimpan asset
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/CreditSceneData.asset");
        
        // Buat asset
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
        
        // Fokuskan ke asset baru di Project window
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = data;
        
        Debug.Log("CreditSceneData berhasil dibuat di " + path);
    }
}
#endif
