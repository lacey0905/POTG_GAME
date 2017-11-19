using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMakeLaser : MonoBehaviour {


    LineRenderer m_Laser;

	void Start () {}

    void Awake()
    {
        m_Laser = GetComponent<LineRenderer>();
    }
	
	void Update () {
        m_Laser.SetPosition(0, transform.position);

        RaycastHit _hit;

        if (Physics.Raycast(transform.position, transform.forward, out _hit))
        {
            if (_hit.collider)
            {
                m_Laser.SetPosition(1, _hit.point);
            }
        }
        else
        {
            m_Laser.SetPosition(1, transform.position + transform.forward * 50f);
        }

    }
}
