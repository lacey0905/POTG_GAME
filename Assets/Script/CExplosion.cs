using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CExplosion : MonoBehaviour {

    int m_Damage = 10;

    bool isHit = false;

    float radius = 10.0F;
    float power = 1000.0F;


    void Start()
    {
        StartCoroutine(Destroy());

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider col in colliders)
        {
            Debug.Log("a");
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(this.gameObject);
    }

}
