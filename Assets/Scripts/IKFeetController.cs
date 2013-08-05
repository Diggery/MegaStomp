using UnityEngine;
using System.Collections;

public class IKFeetController : MonoBehaviour {
	
	public Transform body;
	Vector3 lastBodyPos;
	
	public Transform leftFootPrint;
	public Transform leftFootGoal;
	public Vector3 leftFootOldPos;
	
	public Transform rightFootPrint;
	public Transform rightFootGoal;
	public Vector3 rightFootOldPos;
	
	public bool leftFootUp;
	public bool rightFootUp;
	
	public bool moving;
	
	public float stanceWidth = 0.5f;
	public float motionOffset = 1.2f;
	public float stepDistance = 1.2f;
	public float stepHeight = 1.2f;
	public float legLength = 2.3f;
	public AnimationCurve footLiftCurve;
	public AnimationCurve footTiltCurve;
	
	float leftFootStepTimer = 0.0f;
	float rightFootStepTimer = 0.0f;
	
	void Start () {
		lastBodyPos = body.position;
		leftFootGoal.renderer.enabled = false;
		rightFootGoal.renderer.enabled = false;
	}
	
	void FixedUpdate () {
				
		float currentStanceWidth;
		float currentStepDistance;

		currentStanceWidth = stanceWidth;
		currentStepDistance = stepDistance;
		
		float currentSpeed = getSpeed() * 10;
		
		//moving = currentSpeed > 0.5f ? true : false;
		currentStanceWidth = Mathf.Lerp (stanceWidth, 0.0f, currentSpeed -0.5f);
		
	//	leftFootPrint.localPosition = new Vector3(-currentStanceWidth, 0.0f, currentStanceOffset);
	//	rightFootPrint.localPosition = new Vector3(currentStanceWidth, 0.0f, -currentStanceOffset);
		
		if (!leftFootUp && !rightFootUp) {
			float rightFootDistance = Vector3.Distance(rightFootPrint.position, rightFootGoal.position);
			float leftFootDistance = Vector3.Distance(leftFootPrint.position, leftFootGoal.position);
			
			if (leftFootDistance > currentStepDistance && rightFootDistance > currentStepDistance) {
				if (leftFootDistance > rightFootDistance) {
					liftLeftFoot();
				} else {
					liftRightFoot();
				}
			} else {
				if (leftFootDistance > currentStepDistance) liftLeftFoot(); 	
				if (rightFootDistance > currentStepDistance) liftRightFoot(); 	
			}
		}
		
		//position the Left Foot
		Vector3 newLeftFootPos = Vector3.zero;
		if (leftFootUp) {
			leftFootStepTimer += Time.deltaTime * 3;
			if (leftFootStepTimer > 1.0f) plantLeftFoot();
			newLeftFootPos = Vector3.Lerp(leftFootOldPos, leftFootPrint.position, leftFootStepTimer);
			newLeftFootPos.y += footLiftCurve.Evaluate(leftFootStepTimer) * stepHeight;
			leftFootGoal.position = newLeftFootPos;
		}
		
		//position the Right Foot
		Vector3 newRightFootPos = Vector3.zero;
		if (rightFootUp) {
			rightFootStepTimer += Time.deltaTime * 3;
			if (rightFootStepTimer > 1.0f) plantRightFoot();
			newRightFootPos = Vector3.Lerp(rightFootOldPos, rightFootPrint.position, rightFootStepTimer);
			newRightFootPos.y += footLiftCurve.Evaluate(rightFootStepTimer) * stepHeight;
			rightFootGoal.position = newRightFootPos;
		}
		
		//position the pelvis
		Vector3 pelvisTarget;
		
		if (moving) {
			float pelvisHeight = legLength + ((newLeftFootPos.y + newRightFootPos.y) * 0.75f);
			pelvisTarget = transform.position + new Vector3(0.0f, pelvisHeight, 0.0f);
		} else {
			Vector3 MidFootPos = Vector3.Lerp(rightFootGoal.position, leftFootGoal.position, 0.5f);
			MidFootPos.y += legLength;
			pelvisTarget = MidFootPos;
		}
		body.position = Vector3.Lerp (body.position, pelvisTarget, Time.deltaTime * 10);
		body.rotation = Quaternion.Lerp (body.rotation, transform.rotation, Time.deltaTime * 3);
	}
	
	void liftLeftFoot() {
		leftFootUp = true;
		leftFootOldPos = leftFootGoal.position;
		Vector3 bodyRelative = transform.InverseTransformPoint(leftFootGoal.position);
		float currentOffset = Mathf.Clamp(-bodyRelative.z, -stepDistance * 0.8f, stepDistance * 0.8f);
		Vector3 localPrintPos = leftFootPrint.localPosition;
		localPrintPos.z = currentOffset;
		leftFootPrint.localPosition = localPrintPos;
	}
	
	void plantLeftFoot() {
		leftFootUp = false;
		leftFootStepTimer = 0;
		leftFootOldPos = leftFootGoal.position;
		Vector3 localPrintPos = leftFootPrint.localPosition;
		localPrintPos.z = 0.0f;
		leftFootPrint.localPosition = localPrintPos;
	}
	
	void liftRightFoot() {
		rightFootUp = true;
		rightFootOldPos = rightFootGoal.position;
		Vector3 bodyRelative = transform.InverseTransformPoint(rightFootGoal.position);
		float currentOffset = Mathf.Clamp(-bodyRelative.z, -stepDistance * 0.8f, stepDistance * 0.8f);
		Vector3 localPrintPos = rightFootPrint.localPosition;
		localPrintPos.z = currentOffset;
		rightFootPrint.localPosition = localPrintPos;
	}
	
	void plantRightFoot() {
		rightFootUp = false;
		rightFootStepTimer = 0;
		rightFootOldPos = rightFootGoal.position;
		Vector3 localPrintPos = rightFootPrint.localPosition;
		localPrintPos.z = 0.0f;
		rightFootPrint.localPosition = localPrintPos;
	}
	
	private float getSpeed() {
		Vector3 bodyDelta = lastBodyPos - body.position;
		lastBodyPos = body.position;
		return bodyDelta.magnitude;
	}
	
	public bool isRightFootUp() {
		return rightFootUp;
	}
	
	public bool isLeftFootUp() {
		return leftFootUp;
	}
}
