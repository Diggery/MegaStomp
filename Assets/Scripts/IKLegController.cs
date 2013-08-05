using UnityEngine;
using System.Collections;

public class IKLegController : MonoBehaviour {
	
	float hipLength;
	float kneeLength;
	
	public Transform upperLeg;
	public Transform lowerLeg;
	public Transform foot;
	public Transform IKEnd;
	public Transform goal;
	public bool rightLeg;
	
	public Transform feetController;
	IKFeetController ikFeetController;


	void Start () {
		if (feetController) ikFeetController = feetController.GetComponent<IKFeetController>();
		hipLength = lowerLeg.localPosition.magnitude;
		kneeLength = Vector3.Distance(lowerLeg.position, IKEnd.position);
	}
	
	void FixedUpdate () {
		float hipOffset = rightLeg ? 15 : -15;

		Vector3 hipAngle = Quaternion.AngleAxis(hipOffset + feetController.eulerAngles.y, Vector3.up) * Vector3.forward;
		
		transform.LookAt(goal, hipAngle);
		IKEnd.position = goal.position;
		Vector2 targetPos = new Vector2(IKEnd.localPosition.z, -IKEnd.localPosition.y);
		
		float angle1 = 0.0f;
		float angle2 = 0.0f;

		IKSolver.CalcIK_2D(out angle1, out angle2, true, hipLength, kneeLength, targetPos.x, targetPos.y);
				
		upperLeg.localRotation = Quaternion.AngleAxis(angle1 * Mathf.Rad2Deg, Vector3.right);
		lowerLeg.localRotation = Quaternion.AngleAxis(angle2 * Mathf.Rad2Deg, Vector3.right);
		
		bool footUp = false;
		if (ikFeetController) {
			if (rightLeg) {
				footUp = ikFeetController.isRightFootUp();	
			} else {
				footUp = ikFeetController.isLeftFootUp();	
			}
		}
		float xOffset = footUp ? 90 : 90;
		foot.rotation = goal.rotation * 
			Quaternion.AngleAxis(xOffset, Vector3.right) * 
			Quaternion.AngleAxis(-hipOffset - feetController.eulerAngles.y , Vector3.forward);// *
			//Quaternion.AngleAxis(hipOffset + FeetController.eulerAngles.y, Vector3.up);
	}
}
