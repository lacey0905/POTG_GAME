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

    bool isFireDelay = false;


    public RaycastHit GetAttackRay()
    {
        RaycastHit _hit;
        Physics.Raycast(transform.position, transform.forward, out _hit, 1000f, m_TracerPassLayer);
        return _hit;
    }

        //        
        //    }

        //    if (_hit.collider.tag == "Player")
        //                {
        //                    SpawnDecal(_hit, m_Mark [1]);            // 충돌 한 좌표에 마크 표시

        //    _hit.collider.gameObject.GetComponent<CPlayerManager>().SetDecreaseHealth(1);

        //}
        //                else
        //                {
        //                    SpawnDecal(_hit, m_Mark [4]);            // 충돌 한 좌표에 마크 표시
        //                }





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
    }

    void FixedUpdate()
    {
        if (m_Manager.State.isFire)
        {
            //Attack();
        }
        else
        {
            m_ShutEffect.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    public void Owner(CPlayerManager _manager)
    {
        m_Manager = _manager;
    }

    public void SetLaser()
    {
        m_Laser.SetActive(true);
    }

    IEnumerator FireDelay()
    {
        isFireDelay = true;
        yield return new WaitForSeconds(m_Delay);
        isFireDelay = false;
    }

    public void Attack(Vector3 _point)
    {
        m_ShutEffect.transform.localScale = new Vector3(6f, 6f, 6f);
        if (!isFireDelay)
        {
            StartCoroutine(FireDelay());
            
            Quaternion _Reset = transform.localRotation;                // 원래 회전 값 저장
            Quaternion _Reaction = _Reset;                              // 반동 회전 값

            _Reaction.x += Random.Range(-m_Reaction, m_Reaction);
            _Reaction.y += Random.Range(-m_Reaction, m_Reaction);
            _Reaction.z += Random.Range(-m_Reaction, m_Reaction);

            //transform.localRotation = _Reaction;                        // 반동 회전 값 적용
            

            m_CurCount++;                                       // 현재 총알 번호

            // 총알 번호 초기화
            if (m_CurCount >= m_Max)
            {
                m_CurCount = 0;
            }
            m_BulletList[m_CurCount].SetActive(true);
            m_BulletList [m_CurCount].GetComponent<CAttackBullet>().SetTracerTarget(_point);

            transform.localRotation = _Reset;
        }
    }

    void SpawnDecal(RaycastHit hit, GameObject prefab)
    {
        GameObject spawnedDecal = Instantiate(prefab, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
        spawnedDecal.transform.SetParent(hit.collider.transform);
    }
}
