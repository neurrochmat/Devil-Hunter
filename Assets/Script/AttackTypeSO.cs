using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackType", menuName = "Player/Attack Type")]
public class AttackTypeSO : ScriptableObject
{
    public string attackName;
    public AnimationClip attackAnimation;
    public float attackRange;
    public int damage;
}
