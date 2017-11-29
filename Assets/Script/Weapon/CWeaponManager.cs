using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWeaponManager : MonoBehaviour {
    /// <summary>
    /// 0 - 피1
    /// 1 - 피2
    /// 2 - 메탈
    /// 3 - 모래
    /// 4 - 스톤
    /// </summary>
    public List<GameObject> m_Mark = new List<GameObject>();            // 총알 자국 이펙트 리스트
    public List<GameObject> m_BulletList = new List<GameObject>();      // 총알 풀링 리스트

    public GameObject m_Tracer;             // 총알
    public GameObject m_ShutEffect;         // 총알 발사 이펙트
    public GameObject m_Laser;              // 레이저

    public CPlayerManager m_Manager;

    float m_Delay = 0.1f;

    int m_Max = 100;                         // 풀링 최대치
    int m_CurCount = 0;                     // 현재 총알 번호

    int m_TracerPassLayer;                  // 무시할 충돌 레이어
    float m_Reaction = 0.015f;

    Quaternion m_ResetAngle;
    Quaternion m_ReactionAngle;

    bool isFireDelay = false;

    void Start()
    {
        // 총알을 최대치 까지 미리 생성 함
        for (int i = 0; i < m_Max; i++)
        {
            GameObject _Bullet = Instantiate(m_Tracer, this.transform.position, Quaternion.identity) as GameObject;
            _Bullet.transform.rotation = this.transform.rotation;
            _Bullet.transform.parent = this.transform;
            _Bullet.SetActive(false);

            m_BulletList.Add(_Bullet);
        }
        m_TracerPassLayer = (-1) - ((1 << LayerMask.NameToLayer("Tracer")) | (1 << LayerMask.NameToLayer("RayFloor")));

        // 기본 엥글
        m_ResetAngle = transform.localRotation;
        m_ReactionAngle = m_ResetAngle;
    }

    void FixedUpdate()
    {
        // 캐릭터가 공격 상태면 실행
        if (m_Manager.State.isFire)
        {
            if (!isFireDelay)
            {
                StartCoroutine(FireDelay());
                m_ShutEffect.transform.localScale = new Vector3(6f, 6f, 6f);

                transform.localRotation = m_ReactionAngle;

                MakeHitTarget();
                Attack();

                transform.localRotation = m_ResetAngle;
            }
        }
        else
        {
            m_ShutEffect.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    IEnumerator FireDelay()
    {
        isFireDelay = true;
        yield return new WaitForSeconds(0.1f);
        isFireDelay = false;
    }

    RaycastHit m_Hit;
    public void MakeHitTarget()
    {
        Physics.Raycast(transform.position, transform.forward, out m_Hit, 1000f, m_TracerPassLayer);
    }


    int _damage;

    public int _Damage()
    {
        return _damage;
    }

    public void SetReset() {
        _damage = 0;
    }

    void Attack()
    {
        if (m_Hit.collider)
        {
            if (m_Hit.collider.tag == "Player")
            {
                
                //m_Hit.collider.gameObject.GetComponent<CPlayerManager>().SetDecreaseHealth(10);
                //Debug.Log(m_Hit.collider.gameObject.GetComponent<CPlayerManager>().Data.Health);

                Debug.Log(m_Hit.collider.gameObject.GetComponent<CPlayerManager>().netId);

                m_Hit.collider.gameObject.GetComponent<CPlayerManager>().TakeDamage(10, m_Hit.point);

                if (m_Manager.active > 0)
                {
                    SpawnDecal(m_Hit, m_Mark[1], m_Manager.hit);
                }

            }
            else
            {
                SpawnDecal(m_Hit, m_Mark[4], m_Manager.hit);
            }
        }

        m_CurCount++;                                       // 현재 총알 번호

        // 총알 번호 초기화
        if (m_CurCount >= m_Max)
        {
            m_CurCount = 0;
        }
        m_BulletList [m_CurCount].SetActive(true);
        m_BulletList [m_CurCount].GetComponent<CAttackBullet>().SetTracerTarget(m_Hit.point);
    }

    public Quaternion GetReaction()
    {
        Quaternion _reaction = m_ResetAngle;

        _reaction.x += Random.Range(-m_Reaction, m_Reaction);
        _reaction.y += Random.Range(-m_Reaction, m_Reaction);
        _reaction.z += Random.Range(-m_Reaction, m_Reaction);

        return _reaction;
    }

    public void SetReaction(Quaternion _angle)
    {
        m_ReactionAngle = _angle;
    }

    public void Owner(CPlayerManager _manager)
    {
        m_Manager = _manager;
    }

    public void SetLaser()
    {
        m_Laser.SetActive(true);
    }

    void SpawnDecal(RaycastHit _hit, GameObject prefab, Vector3 _point)
    {
        GameObject spawnedDecal = Instantiate(prefab, _point, Quaternion.LookRotation(_hit.normal)) as GameObject;
        spawnedDecal.transform.SetParent(_hit.transform);
    }
}
