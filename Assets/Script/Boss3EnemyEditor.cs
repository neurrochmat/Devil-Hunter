#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Boss3Enemy))]
public class Boss3EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        Boss3Enemy boss3 = (Boss3Enemy)target;
        
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "Boss3 akan mengaktifkan portal jika portalController diatur, " +
            "dan juga akan memulai transisi ke scene credits sesuai dengan pengaturan di CreditSceneData.",
            MessageType.Info
        );
        
        if (boss3.creditSceneData == null)
        {
            EditorGUILayout.HelpBox(
                "CreditSceneData tidak ditemukan! Anda perlu mengatur CreditSceneData atau " +
                "scene default 'credit' akan digunakan dengan delay 2 detik.",
                MessageType.Warning
            );
            
            if (GUILayout.Button("Buat CreditSceneData Baru"))
            {
                // Buat ScriptableObject baru
                CreditSceneData data = ScriptableObject.CreateInstance<CreditSceneData>();
                
                // Simpan ke Assets/Resources folder
                string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/CreditSceneData.asset");
                AssetDatabase.CreateAsset(data, path);
                AssetDatabase.SaveAssets();
                
                // Assign ke Boss3Enemy
                boss3.creditSceneData = data;
                
                // Fokuskan ke asset baru di Project window
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = data;
                
                EditorUtility.SetDirty(boss3);
            }
        }
        
        EditorGUILayout.Space();
    }
}
#endif
