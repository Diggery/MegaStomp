using UnityEngine;
using System.Collections;

public class IKFeetController : MonoBehaviour {
	
	public Transform body;
	
	public Transform leftFootPrint;
	public Transform leftFootGoal;
	public Vector3 leftFootOldPos;
	
	public Transform rightFootPrint;
	public Transform rightFootGoal;
	public Vector3 rightFootOldPos;
	
	public bool leftFootUp;
	public bool rightFootUp;
	
	public float stanceWidth = 0.5f;
	public float stanceOffset = 0.5f;
	public float motionOffset = 1.2f;
	public float stepDistance = 1.2f;
	public float stepHeight = 1.2f;
	public float legLength = 2.3f;
	public AnimationCurve footLiftCurve;
	
	float leftFootStepTimer = 0.0f;
	float rightFootStepTimer = 0.0f;
	
	void Start () {
	
	}
	
	void Update () {
		
		float bodyDelta = getBodyDelta();
		float stepThreshold = 0.0f;
		
		float currentStanceWidth;
		float currentStanceOffset;
		
//		if (Mathf.Abs(bodyDelta) > 0.1) {
//			currentStanceWidth = stanceWidth * 0.5f ;
//			currentStanceOffset = 0.0f;
//			stepThreshold = stepDistance;
//		} else {
			currentStanceWidth = stanceWidth;
			currentStanceOffset = stanceOffset;			
			stepThreshold = stepDistance;// * 0.5f;
		//}
		
		leftFootPrint.localPosition = new Vector3(-currentStanceWidth, 0.0f, currentStanceOffset);
		rightFootPrint.localPosition = new Vector3(currentStanceWidth, 0.0f, -currentStanceOffset);
		
		if (!leftFootUp && !rightFootUp) {
			float rightFootDistance = Vector3.Distance(rightFootPrint.position, rightFootGoal.position);
			float leftFootDistance = Vector3.Distance(leftFootPrint.position, leftFootGoal.position);
			
			if (leftFootDistance > stepThreshold && rightFootDistance > stepThreshold) {
				if (leftFootDistance > rightFootDistance) {
					liftLeftFoot();
				} else {
					liftRightFoot();
				}
			} else {
				if (leftFootDistance > stepThreshold) liftLeftFoot(); 	
				if (rightFootDistance > stepThreshold) liftRightFoot(); 	
			}
		}
		
		float momentum = 0.0f;
		
		if (Mathf.Abs(bodyDelta) > 0.1f) {
			momentum = bodyDelta * 2;
		}
		Vector3 stepOffset = new Vector3(0.0f, 0.0f, momentum );
		
		Vector3 newRightFootPos = Vector3.zero;
		if (rightFootUp) {
			rightFootStepTimer += Time.deltaTime * 3;
			if (rightFootStepTimer > 1.0f) plantRightFoot();
			newRightFootPos = Vector3.Lerp(rightFootOldPos, rightFootPrint.position + stepOffset, rightFootStepTimer);
			newRightFootPos.y += footLiftCurve.Evaluate(rightFootStepTimer) * stepHeight;
			rightFootGoal.position = newRightFootPos;
		}
		
		Vector3 newLeftFootPos = Vector3.zero;
		if (leftFootUp) {
			leftFootStepTimer += Time.deltaTime * 3;
			if (leftFootStepTimer > 1.0f) plantLeftFoot();
			newLeftFootPos = Vector3.Lerp(leftFootOldPos, leftFootPrint.position + stepOffset, leftFootStepTimer);
			newLeftFootPos.y += footLiftCurve.Evaluate(leftFootStepTimer) * stepHeight;
			leftFootGoal.position = newLeftFootPos;
		}
		
		float pelvisHeight = legLength + ((newLeftFootPos.y + newRightFootPos.y) * 0.75f) ;
		body.position = Vector3.Lerp (body.position, transform.position + new Vector3(0.0f, pelvisHeight, 0.0f), Time.deltaTime * 8);
	}
	
	void liftLeftFoot() {
		leftFootUp = true;
		leftFootOldPos = leftFootGoal.position;
	}
	
	void plantLeftFoot() {
		leftFootUp = false;
		leftFootStepTimer = 0;
		leftFootOldPos = leftFootGoal.position;
	}
	
	void liftRightFoot() {
		rightFootUp = true;
		rightFootOldPos = rightFootGoal.position;
	}
	
	void plantRightFoot() {
		rightFootUp = false;
		rightFootStepTimer = 0;
		rightFootOldPos = rightFootGoal.position;
	}
	
	float getBodyDelta() {
		Vector3 bodyRelative = body.InverseTransformPoint(transform.position);

		return Mathf.Clamp(bodyRelative.z * 0.3f, -motionOffset, motionOffset);
	}
}
