using UnityEngine;
using System.Collections;

public class IKLegController : MonoBehaviour {
	
	float upperLegLength;
	float lowerLegLength;
	
	public Transform upperLeg;
	public Transform lowerLeg;
	public Transform foot;
	public Transform IKEnd;
	public Transform goal;
	public bool rightLeg;
	
	Transform stepController;
	public IKStepController ikStepController;
	CrusaderControl mainController;


	void Start () {
		stepController = ikStepController.transform;
		
		upperLegLength = lowerLeg.localPosition.magnitude;
		lowerLegLength = Vector3.Distance(lowerLeg.position, IKEnd.position);
	}
	
	void FixedUpdate () {
		if (!mainController) {
			mainController = ikStepController.getMainController();
		}
		if (mainController.stunned) return;
				

		float hipOffset = rightLeg ? 15 : -15;

		Vector3 hipAngle = Quaternion.AngleAxis(hipOffset + stepController.eulerAngles.y, Vector3.up) * Vector3.forward;
		
		transform.LookAt(goal, hipAngle);
		IKEnd.position = goal.position;
		Vector2 targetPos = new Vector2(IKEnd.localPosition.z, -IKEnd.localPosition.y);
		
		float angle1 = 0.0f;
		float angle2 = 0.0f;

		IKSolver.CalcIK_2D(out angle1, out angle2, true, upperLegLength, lowerLegLength, targetPos.x, targetPos.y);
				
		upperLeg.localRotation = Quaternion.AngleAxis(angle1 * Mathf.Rad2Deg, Vector3.right);
		lowerLeg.localRotation = Quaternion.AngleAxis(angle2 * Mathf.Rad2Deg, Vector3.right);
		
		foot.rotation = goal.rotation;
	}

	
}
