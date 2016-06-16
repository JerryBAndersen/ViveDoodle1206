using UnityEngine;
using System.Collections;

public class MagInput : MonoBehaviour {

    public PickableMagazine mag;
    Transform magPosition;
    public FixedJoint fj;

    // Use this for initialization
    void Start() {
        magPosition = transform.FindChild("MagPosition").transform;
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.GetComponentInParent<PickableMagazine>() && other.GetComponent<MagTop>()) {
            if (!mag) {
                Destroy(fj);
                fj = gameObject.AddComponent<FixedJoint>();
                fj.enableCollision = true;
                mag = other.transform.GetComponentInParent<PickableMagazine>();
                mag.transform.position = magPosition.position;
                mag.transform.rotation = magPosition.rotation;
                mag.maginput = this;
                Pickable pickable = mag.GetComponent<Pickable>();
                if (pickable.picker) {
                    pickable.picker.picking = false;
                    pickable.picker.pickrigid = null;
                    pickable.picker = null;
                    pickable.GetComponent<Rigidbody>().isKinematic = false;
                }
                fj.connectedBody = mag.GetComponent<Rigidbody>();
            }
        }
    }
}