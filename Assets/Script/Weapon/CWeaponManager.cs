using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWeaponManager : MonoBehaviour {

    public GameObject m_Bullet;
    public GameObject m_Laser;

    void Start()
    {
        
    }

    public void SetLaser()
    {
        m_Laser.SetActive(true);
    }

    public void Attack()
    {
        GameObject _Bullet = Instantiate(m_Bullet, this.transform.position, Quaternion.identity) as GameObject;
        _Bullet.transform.rotation = this.transform.rotation;
    }
}
