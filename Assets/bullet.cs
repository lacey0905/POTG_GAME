using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    public float Speed;

	void Start () {
        GetComponent<Rigidbody>().AddForce(transform.forward * Speed);
        StartCoroutine("Bullet_Destroy");
    }

    void Update() {
        this.transform.localScale += new Vector3(0.0f, 0.0f, 0.3f);
    }

    void OnTriggerEnter(Collider _col) {
        if (_col.tag == "object")
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Bullet_Destroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

}
