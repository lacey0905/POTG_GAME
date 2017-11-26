using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    ParticleSystem pt;

	// Use this for initialization
	void Start ()
    {
        pt = GetComponent<ParticleSystem>();

        

        Debug.Log("start");

    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }

    //public void Stop()
    //{
    //    pt.Stop();
    //    if (!pt.isPlaying)
    //    {
    //        //this.gameObject.SetActive(false);
    //    }
    //}

}
