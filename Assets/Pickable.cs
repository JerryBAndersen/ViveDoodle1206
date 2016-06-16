using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {

    PickableHandle pickhandle;
    public Picker picker;
    Collider trigger;
    Rigidbody rigid;


	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();

        // if there is no prepared handle create one
        if (!FindObjectOfType<PickableHandle>()) {
            GameObject nu = new GameObject("Handle");            
            nu.transform.SetParent(transform);
            nu.transform.localPosition = Vector3.zero;
            nu.transform.localRotation = Quaternion.identity;
            pickhandle = nu.AddComponent<PickableHandle>();
            SphereCollider sp = nu.AddComponent<SphereCollider>();
            sp.radius = .04f;
            sp.isTrigger = true;
            trigger = sp;
        } else {
            pickhandle = FindObjectOfType<PickableHandle>();
            trigger = pickhandle.GetComponent<Collider>();
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

}
