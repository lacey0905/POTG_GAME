using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CSpawnManager : NetworkBehaviour
{
    [SyncVar]
    bool m_SpawnCheck = false;

    public void SetSpawning()
    {
        m_SpawnCheck = true;
    }

    public void ResetSpawn()
    {
        m_SpawnCheck = false;
    }

    public bool GetSpawnCheck()
    {
        return m_SpawnCheck;
    }
}
