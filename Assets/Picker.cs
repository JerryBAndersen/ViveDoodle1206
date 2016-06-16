using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Picker : MonoBehaviour {
    public bool picking = false;
    public Rigidbody pickrigid;
    Collider trigger;
    SteamVR_Controller.Device controller;
    public FixedJoint fj;

    // Use this for initialization
    void Start () {
        int index = (int)GetComponent<SteamVR_TrackedObject>().index;
        controller = SteamVR_Controller.Input(index);
        trigger = GetComponent<Collider>();
        if (!trigger) {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.SetParent(transform);
            sphere.transform.transform.localPosition = Vector3.zero;
            sphere.transform.transform.localRotation = Quaternion.identity;
            sphere.transform.transform.localScale = Vector3.one * .02f;
            sphere.GetComponent<Collider>().enabled = false;

            trigger = gameObject.AddComponent<SphereCollider>();
            (trigger as SphereCollider).radius = .01f;
            trigger.isTrigger = true;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu)) {
            SceneManager.LoadScene(0,LoadSceneMode.Single);
        }
    }

    void FixedUpdate() {
        if (picking && pickrigid) {
            pickrigid.MovePosition(transform.position);
            pickrigid.MoveRotation(transform.rotation);
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.GetComponent<PickableHandle>()) {
            PickableHandle pickhandle = other.GetComponent<PickableHandle>();
            Pickable pickable = pickhandle.transform.GetComponentInParent<Pickable>();

            if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger)) {
                if (pickable.GetComponent<PickableMagazine>()) {
                    PickableMagazine mag = pickable.GetComponent<PickableMagazine>();
                    if (mag.maginput) {
                        print("removing mag");
                        Vector3 forcedirection = -mag.maginput.transform.forward;
                        mag.maginput.fj.connectedBody = null;                        
                        mag.maginput.mag = null;
                        mag.maginput = null;
                        
                        forcedirection.Normalize();
                        forcedirection *= mag.ejectforce;
                        print("forcedir: " + forcedirection.x + " " + forcedirection.y + " " + forcedirection.z);
                        pickable.GetComponent<Rigidbody>().AddForce(forcedirection,ForceMode.Impulse);

                        return;
                    }
                }

                if (picking) {
                } else {
                    pickrigid = pickable.GetComponent<Rigidbody>();
                    pickrigid.isKinematic = true;
                    picking = true;
                    if (!pickable.picker) {
                        pickable.picker = this;
                    } else {
                        pickable.picker.picking = false;
                        pickable.picker.pickrigid = null;
                    }                    
                }
            }
            if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
                if (picking) {
                    pickrigid.isKinematic = false;
                    pickrigid = null;
                    picking = false;
                    pickable.picker = null;
                } else {
                }
            }

        }     
    }
}
