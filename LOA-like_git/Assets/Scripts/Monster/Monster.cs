using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float initHealth = 1000000f;
    public float curHealth = 1000000f;

    private void Update()
    {
        if (curHealth < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void GetDamage(float _damage)
    {
        curHealth -= _damage;
    }
}
