using UnityEngine;
using System.Collections;

public class IKFootController : MonoBehaviour {
	
	public bool rightFoot;
	public float stepSpeed;
	public float stepHeight;
	public float tiltAmount;
	public AnimationCurve footLiftCurve;
	public AnimationCurve footTiltCurve;
	
	public Transform stepController;
	IKStepController ikStepController;
	CrusaderControl mainController;
	
	public bool footUp;
	Transform footGoal;
	Transform footPrint;
	Vector3 lastFootPos;
	Quaternion plantedFootRot;
	float stepDistance;
	float footStepTimer;
	
	void Start () {
		ikStepController = stepController.GetComponent<IKStepController>();
		stepDistance = ikStepController.stepDistance;
		if (rightFoot) {
			footGoal = ikStepController.rightFootGoal;
			footPrint = ikStepController.rightFootPrint;
		} else {
			footGoal = ikStepController.leftFootGoal;
			footPrint = ikStepController.leftFootPrint;			
		}
		plantFoot();
	}
	
	void FixedUpdate () {
		if (!mainController) {
			mainController = ikStepController.getMainController();
		}
		if (mainController.stunned) return;
		
		Quaternion footOrientation = getFootOrientation();
		Vector3 newFootPos = Vector3.zero;

		if (footUp) {
			footStepTimer += Time.deltaTime * stepSpeed;
			float stepAmount = Mathf.Clamp01(footStepTimer);
			newFootPos = Vector3.Lerp(lastFootPos, footPrint.position, stepAmount);
			newFootPos.y += footLiftCurve.Evaluate(stepAmount) * stepHeight;
			footGoal.position = newFootPos;
			
			float footTiltAmount = footTiltCurve.Evaluate(stepAmount) * tiltAmount;
			Quaternion stepRotGoal = footOrientation * Quaternion.AngleAxis(footTiltAmount, Vector3.right);
			footGoal.localRotation = Quaternion.Lerp (footGoal.localRotation, stepRotGoal, Time.deltaTime * 5);
			
			
			if (footStepTimer > 1.0f) plantFoot();
		} else {
			
			Vector3 plantedFootPos = footGoal.position;
			plantedFootPos.y = 0.0f;
			footGoal.position = plantedFootPos;
			
			footGoal.rotation = Quaternion.Lerp(footGoal.rotation, plantedFootRot, Time.deltaTime * 8);
			if (Quaternion.Angle(footGoal.rotation, footOrientation) > 70) plantedFootRot = footOrientation;
		}	
	}
	
	public void liftFoot() {
		footUp = true;
		lastFootPos = footGoal.position;
		Vector3 bodyRelative = stepController.InverseTransformPoint(lastFootPos);
		float currentOffset = Mathf.Clamp(-bodyRelative.z, -stepDistance * 0.95f, stepDistance * 0.95f);
		Vector3 localPrintPos = footPrint.localPosition;
		float stanceWidth = ikStepController.getStanceWidth();
		localPrintPos.x = !rightFoot ? -stanceWidth : stanceWidth;
		localPrintPos.z = currentOffset;
		footPrint.localPosition = localPrintPos;
	}
	
	public void plantFoot() {
		footUp = false;
		footStepTimer = 0;
		lastFootPos = footGoal.position;
		Vector3 localPrintPos = footPrint.localPosition;
		localPrintPos.z = 0.0f;
		footPrint.localPosition = localPrintPos;
		plantedFootRot = getFootOrientation();
	}
	
	Quaternion getFootOrientation() {
		float hipOffset = rightFoot ? -15 : 15;
		
		
		Quaternion orientation = ikStepController.getBodyRotation()  
			* Quaternion.AngleAxis(90.0f, Vector3.right)
			* Quaternion.AngleAxis(hipOffset, Vector3.forward);
				
		return orientation;
	}
}
