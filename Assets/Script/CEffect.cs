using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEffect : MonoBehaviour {

	void Start ()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }

}
