using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraShake : MonoBehaviour {

    Vector3 originPos;

    public void StartShake()
    {
        originPos = transform.localPosition;
        StartCoroutine("Shake");
    }

    IEnumerator Shake()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 temp = originPos + Random.insideUnitSphere * 0.3f;
            temp.y = originPos.y;
            transform.localPosition = temp;
            yield return new WaitForSeconds(0.02f);
        }
        transform.localPosition = originPos; 
    }

}
