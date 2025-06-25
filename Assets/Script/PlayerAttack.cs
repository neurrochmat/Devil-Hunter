using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private List<AttackTypeSO> attackTypes;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private int currentAttackIndex = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldownTimer > 0.2f && playerMovement.canAttack())
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        var currentAttack = attackTypes[currentAttackIndex];
        anim.Play(currentAttack.attackAnimation.name);
        cooldownTimer = 0;

        // Detect enemies in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, currentAttack.attackRange, enemyLayers);
        foreach (var enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(currentAttack.damage);
        }

        // Ganti ke tipe serangan berikutnya
        currentAttackIndex = (currentAttackIndex + 1) % attackTypes.Count;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null && attackTypes.Count > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackTypes[currentAttackIndex].attackRange);
        }
    }
}
