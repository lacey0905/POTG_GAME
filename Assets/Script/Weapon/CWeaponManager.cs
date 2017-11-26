using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWeaponManager : MonoBehaviour {

    public GameObject m_Bullet;
    public GameObject m_Laser;

    public GameObject effect;

    public GameObject Tracer;

    public GameObject m_ShutEffect;

    public List<GameObject> m_BulletList = new List<GameObject>();

    public int b_count = 0;

    public int max = 10;


    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject _Bullet = Instantiate(Tracer, this.transform.position, Quaternion.identity) as GameObject;
            _Bullet.transform.rotation = this.transform.rotation;
            _Bullet.transform.parent = this.transform;

            m_BulletList.Add(_Bullet);
            _Bullet.SetActive(false);
        }

    }

    public void SetLaser()
    {
        m_Laser.SetActive(true);
    }

    public void NoneAttack()
    {
        m_ShutEffect.transform.localScale = new Vector3(0f, 0f, 0f);

    }
    public void Attack(bool _shut)
    {

        m_ShutEffect.transform.localScale = new Vector3(6f, 6f, 6f);

        RaycastHit _hit;

        int _Tracer = (-1) - ((1 << LayerMask.NameToLayer("Tracer")));


        Quaternion reset = transform.rotation;
        Quaternion temp = reset;

        temp.x += Random.Range(-0.02f, 0.02f);
        temp.y += Random.Range(-0.02f, 0.02f);
        temp.z += Random.Range(-0.02f, 0.02f);

        transform.rotation = temp;

        if (Physics.Raycast(transform.position, transform.forward, out _hit, 1000f, _Tracer))
        {
            if (_hit.collider)
            {
                SpawnDecal(_hit, effect);
            }
        }

        b_count++;

        if (b_count >= max)
        {
            b_count = 0;
        }
        m_BulletList[b_count].SetActive(true);
        m_BulletList[b_count].GetComponent<CAttackBullet>().SetAddForce(_hit.point);

        transform.rotation = reset;

        //GameObject _Bullet = Instantiate(Tracer, this.transform.position, Quaternion.identity) as GameObject;
        //_Bullet.transform.rotation = this.transform.rotation;

        //_Bullet.transform.parent = transform;
    }

    void SpawnDecal(RaycastHit hit, GameObject prefab)
    {
        GameObject spawnedDecal = Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
        spawnedDecal.transform.SetParent(hit.collider.transform);
    }
}
