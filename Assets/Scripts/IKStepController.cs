using UnityEngine;
using System.Collections;

public class IKStepController : MonoBehaviour {
	
	public Transform body;
	CrusaderControl mainController;
	public Transform pelvis;
	Vector3 lastBodyPos;
	
	public IKFootController leftFootController;
	public Transform leftFootPrint;
	public Transform leftFootGoal;

	
	public IKFootController rightFootController;
	public Transform rightFootPrint;
	public Transform rightFootGoal;
	
	public float stanceWidth = 0.5f;
	float currentStanceWidth = 0.5f;
	public float stepDistance = 1.2f;
	
	void Start () {
		mainController = body.GetComponent<CrusaderControl>();
		renderer.enabled = false;
		leftFootGoal.renderer.enabled = false;
		rightFootGoal.renderer.enabled = false;
		rightFootPrint.renderer.enabled = false;
		rightFootPrint.localPosition = new Vector3(stanceWidth, 0.0f, stepDistance * 0.75f);
		leftFootPrint.renderer.enabled = false;
		leftFootPrint.localPosition = new Vector3(-stanceWidth, 0, -stepDistance * 0.75f);
	}
	
	void FixedUpdate () {
		if (mainController.stunned) return;
		
				
		float currentStepDistance;
		float currentSpeed = body.rigidbody.velocity.magnitude;
		currentStanceWidth = Mathf.Clamp(stanceWidth - (currentSpeed * 0.1f), stanceWidth, stanceWidth);
		currentStepDistance = stepDistance;
		
		bool leftFootUp = leftFootController.footUp;
		bool rightFootUp = rightFootController.footUp;
				
		if (!leftFootUp && !rightFootUp) {
			float rightFootDistance = Vector3.Distance(rightFootPrint.position, rightFootGoal.position);
			float leftFootDistance = Vector3.Distance(leftFootPrint.position, leftFootGoal.position);
			
			if (leftFootDistance > currentStepDistance && rightFootDistance > currentStepDistance) {
				if (leftFootDistance > rightFootDistance) {
					leftFootController.liftFoot();
				} else {
					rightFootController.liftFoot();
				}
			} else {
				if (leftFootDistance > currentStepDistance) leftFootController.liftFoot(); 	
				if (rightFootDistance > currentStepDistance) rightFootController.liftFoot(); 	
			}
		}
				
		transform.rotation = body.rotation;		
		Vector3 rigPos = body.position;
		rigPos.y = 0.0f;
		transform.position = rigPos;
	}

	public float getStanceWidth() {
		return currentStanceWidth;
	}
	public Quaternion getBodyRotation() {
		return body.rotation;
	}
	
	public CrusaderControl getMainController() {
		return mainController;	
	}

}
