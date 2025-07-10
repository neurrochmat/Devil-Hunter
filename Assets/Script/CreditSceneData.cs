using UnityEngine;

[CreateAssetMenu(fileName = "CreditSceneData", menuName = "Game/Credit Scene Data")]
public class CreditSceneData : ScriptableObject
{
    [Tooltip("Nama scene credit yang akan ditampilkan setelah mengalahkan Boss3")]
    public string creditSceneName = "credit";
    
    [Tooltip("Waktu tunggu sebelum pindah ke scene credit (dalam detik)")]
    public float delayBeforeTransition = 2.0f;
}
