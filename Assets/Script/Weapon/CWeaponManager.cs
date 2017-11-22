using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWeaponManager : MonoBehaviour {

    public GameObject m_Bullet;
    public GameObject m_Laser;

    public GameObject effect;

    public GameObject Tracer;


    void Start()
    {
        
    }

    public void SetLaser()
    {
        m_Laser.SetActive(true);
    }

    public void Attack()
    {

        RaycastHit _hit;

        if (Physics.Raycast(transform.position, transform.forward, out _hit))
        {
            if (_hit.collider)
            {
                //Debug.Log(_hit.point);
            }
        }

        GameObject _Bullet = Instantiate(Tracer, this.transform.position, Quaternion.identity) as GameObject;
        _Bullet.transform.rotation = this.transform.rotation;
        SpawnDecal(_hit, effect);
    }

    void SpawnDecal(RaycastHit hit, GameObject prefab)
    {
        GameObject spawnedDecal = GameObject.Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal));
        //spawnedDecal.transform.SetParent(hit.collider.transform);
    }

}
