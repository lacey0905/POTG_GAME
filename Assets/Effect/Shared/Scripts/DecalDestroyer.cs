using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroyer : MonoBehaviour {

	public float lifeTime = 5.0f;

    void Start()
    {
        StartCoroutine(End());
    }

	private IEnumerator End()
	{
		yield return new WaitForSeconds(lifeTime);
		Destroy(gameObject);
	}
}
