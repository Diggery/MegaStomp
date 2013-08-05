using UnityEngine;
using System.Collections;

public class SetUpHangingWeight : MonoBehaviour {
	
	public Vector3 axis;

	void Start () {
		gameObject.AddComponent<BoxCollider>();
		gameObject.layer = 8;
		HingeJoint joint = gameObject.AddComponent<HingeJoint>();
		joint.axis = axis;
		joint.useSpring = true;
		JointSpring springSettings = joint.spring;
		springSettings.spring = 0.25f;
		joint.spring = springSettings;
		if (!transform.parent.rigidbody) {
			Rigidbody newBody = transform.parent.gameObject.AddComponent<Rigidbody>();
			newBody.isKinematic = true;
		}
		joint.connectedBody = transform.parent.rigidbody;
		transform.parent = null;
		rigidbody.angularDrag = 30.0f;
	}

}
