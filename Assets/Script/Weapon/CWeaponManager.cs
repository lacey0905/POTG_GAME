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
    public ParticleSystem m_ShutEffect;     // 총알 발사 이펙트

    public GameObject m_Missile;

    public GameObject m_Laser;              // 레이저
    public AudioSource m_Sound;
    public AudioSource m_Reload;

    public CPlayerManager m_Owner;

    float m_Delay = 0.1f;                   // 발사 딜레이
    bool isFireDelay = false;               // 발사 딜레이 체크

    int m_Max = 10;                         // 풀링 최대치
    int m_CurCount = 0;                     // 현재 총알 번호

    int m_TracerPassLayer;                  // 무시할 충돌 레이어
    float m_Reaction = 0.015f;              // 반동 수치

    Quaternion m_ResetAngle;                // 반동 초기화
    Quaternion m_ReactionAngle;             // 반동 각도

    RaycastHit m_Hit;

    void Start()
    {
        // 총알을 최대치 까지 미리 생성 함
        for (int i = 0; i < m_Max; i++)
        {
            GameObject _Bullet = Instantiate(m_Tracer, this.transform.position, Quaternion.identity) as GameObject;
            _Bullet.transform.rotation = this.transform.rotation;
            _Bullet.transform.parent = this.transform;
            _Bullet.SetActive(false);

            // 총알 리스트에 넣어 둠
            m_BulletList.Add(_Bullet);
        }
        // 레이캐스트가 무시할 레이어 
        m_TracerPassLayer = (-1) - ((1 << LayerMask.NameToLayer("Tracer")) | (1 << LayerMask.NameToLayer("RayFloor")) | (1 << LayerMask.NameToLayer("CenterPoint")));

        // 기본 엥글 저장
        m_ResetAngle = transform.localRotation;
        m_ReactionAngle = m_ResetAngle;
    }

    void FixedUpdate()
    {
        // 캐릭터가 공격 상태면 실행
        if (m_Owner.State.isFire)
        {
            if (m_Owner.m_CurBullet > 0)
            {
                m_Sound.enabled = true;
                // 공격 딜레이 체크
                if (!isFireDelay)
                {
                    StartCoroutine(FireDelay());                    // 딜레이 생성
                    transform.localRotation = GetReaction();        // 반동
                    m_ShutEffect.Play();                            // 이펙트 켜기
                    MakeHitTarget();                                // 레이캐스트 쏘기
                    Attack();                                       // 공격
                    transform.localRotation = m_ResetAngle;         // 반동 리셋
                    m_Owner.SetCurBullet();                         // 총알 감소
                }
            }
            else
            {
                StartCoroutine(Reload());
                m_Sound.enabled = false;
                m_ShutEffect.Clear();   // 이펙트 클리어
                m_ShutEffect.Stop();    // 이펙트 종료
            }
        }
        else
        {
            m_Sound.enabled = false;
            m_ShutEffect.Clear();   // 이펙트 클리어
            m_ShutEffect.Stop();    // 이펙트 종료
        }
    }

    IEnumerator Reload()
    {
        m_Reload.enabled = true;
        yield return new WaitForSeconds(2.0f);
        m_Reload.enabled = false;
        m_Owner.ReLoad();
    }

    // 공격 딜레이
    IEnumerator FireDelay()
    {
        isFireDelay = true;
        yield return new WaitForSeconds(0.1f);
        isFireDelay = false;
    }
    
    public void MakeHitTarget()
    {
        Physics.Raycast(transform.position, transform.forward, out m_Hit, 1000f, m_TracerPassLayer);
    }

    bool isCollTime = false;

    IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(1.0f);
        isCollTime = false;
    }

    public void Missile()
    {
        if (!isCollTime)
        {
            isCollTime = true;
            StartCoroutine(CoolTime());
            GameObject _missile = Instantiate(m_Missile, this.transform.position, Quaternion.identity) as GameObject;
            _missile.transform.rotation = this.transform.rotation;
        }
    }

    void Attack()
    {
        if (m_Hit.collider)
        {
            if (m_Hit.collider.tag == "Player")
            {
                CPlayerManager _hit = m_Hit.collider.gameObject.GetComponent<CPlayerManager>();

                if (_hit.GetMyTeam() != this.m_Owner.GetMyTeam())
                {

                    Debug.Log("적 공격");

                    m_Hit.collider.gameObject.GetComponent<CPlayerManager>().SetDecreaseHealth(10);
                }
                SpawnDecal(m_Hit, m_Mark[1]);
            }
            else
            {
                SpawnDecal(m_Hit, m_Mark[4]);
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

    public void Owner(CPlayerManager _owner)
    {
        m_Owner = _owner;
    }

    public void SetLaser(bool _set)
    {
        m_Laser.SetActive(_set);
    }

    void SpawnDecal(RaycastHit _hit, GameObject prefab)
    {
        GameObject spawnedDecal = Instantiate(prefab, _hit.point, Quaternion.LookRotation(_hit.normal)) as GameObject;
        spawnedDecal.transform.SetParent(_hit.transform);
    }
}
