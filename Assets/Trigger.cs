using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    Transform fullyPressed;
    Transform triggerModel;
    [Range( 0f, 1f)]
    public float f = 0f;

    Quaternion original;

	// Use this for initialization
	void Start () {
        f = 0f;
        fullyPressed = transform.Find("FullyPressed");
        triggerModel = transform.Find("WPN_AKM_trigger");
        original = triggerModel.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        Quaternion target = Quaternion.Lerp(original, fullyPressed.rotation, f);
        triggerModel.rotation = target;
	}
}
