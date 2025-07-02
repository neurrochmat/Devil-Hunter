using UnityEngine;

public class BossEnemy : Enemy
{
    public PortalController portalController;

    protected override void Die()
    {
        base.Die();

        if (portalController != null)
        {
            portalController.ActivatePortal();
        }
    }
}
