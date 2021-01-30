using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : Ability
{
    public override void Use(Vector3 spawnPos)
    {
        RaycastHit[] hit = Physics.SphereCastAll(spawnPos, 0.5f, transform.forward, m_Info.Range);
        float newLength = m_Info.Range;
        if (hit.Length > 0)
        {
            /*for (int i = 0; i < hit.Length; i++)
            {
                newLength = (hit[i].point - spawnPos).magnitude;
                if (hit[i].collider.CompareTag("Enemy"))
                {
                    hit[i].collider.GetComponent<EnemyController>().DecreaseHealth(m_Info.Power);
                }
            }*/
            foreach (RaycastHit h in hit)
            {
                newLength = (h.point - spawnPos).magnitude;
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
