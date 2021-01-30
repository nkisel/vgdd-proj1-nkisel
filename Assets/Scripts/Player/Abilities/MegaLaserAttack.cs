using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaLaserAttack : Ability
{
    public override void Use(Vector3 spawnPos)
    {
        RaycastHit[] hit = Physics.SphereCastAll(spawnPos, 1f, transform.forward, m_Info.Range);
        float newLength = m_Info.Range;
        if (hit.Length > 0)
        {
            foreach (RaycastHit h in hit)
            {
                if (h.collider.CompareTag("Enemy"))
                {
                    h.collider.GetComponent<EnemyController>().DecreaseHealth(m_Info.Power);
                }
            }
        }
        var emitterShape = cc_PS.shape;
        emitterShape.length = newLength;
        cc_PS.Play();
    }
}
