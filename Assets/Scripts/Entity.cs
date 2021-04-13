using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int lives;
    public virtual void GetDamage()
    {
        lives--;
        if (lives < 1)
            Die();
        Debug.Log("___________" + lives);
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
}
