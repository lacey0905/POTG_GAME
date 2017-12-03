using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMakeLaser : MonoBehaviour {

    int m_PassLayer;
    LineRenderer m_Laser;

	void Start () {}

    void Awake()
    {
        m_Laser = GetComponent<LineRenderer>();
        m_PassLayer = (-1) - ((1 << LayerMask.NameToLayer("CenterPoint")));
    }
	
	void Update () {
        m_Laser.SetPosition(0, transform.position);

        RaycastHit _hit;

        if (Physics.Raycast(transform.position, transform.forward, out _hit, 100f, m_PassLayer))
        {
            if (_hit.collider)
            {
                m_Laser.SetPosition(1, _hit.point);
            }
        }
        else
        {
            m_Laser.SetPosition(1, transform.position + transform.forward * 30f);
        }

    }
}
