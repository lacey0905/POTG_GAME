using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerUI : MonoBehaviour {

    public TextMesh m_UIName;
    public GameObject m_UIHealth;

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetUIHealth(int _health)
    {
        m_UIHealth.transform.localScale = new Vector3((float)_health / 100f, 1f, 1f);
    }

    public void SetUIName(string _name)
    {
        m_UIName.text = _name;
    }
}
