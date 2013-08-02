using UnityEngine;
using System.Collections;

public class IKLegController : MonoBehaviour {
	
	public float hipLength;
	public float kneeLength;
	
	public Transform upperLeg;
	public Transform knee;
	public Transform lowerLeg;
	public Transform foot;
	public Transform IKEnd;
	public Transform goal;
	public bool rightFoot;


	void Start () {
		hipLength = lowerLeg.localPosition.magnitude;
		kneeLength = foot.localPosition.magnitude;	
	}
	
	void Update () {
		float hipOffset = rightFoot ? 15 : -15;

		Vector3 hipAngle = Quaternion.AngleAxis(hipOffset, Vector3.up) * Vector3.forward;
		
		transform.LookAt(goal, hipAngle);
		IKEnd.position = goal.position;
		Vector2 targetPos = new Vector2(IKEnd.localPosition.z, -IKEnd.localPosition.y);
		
		float angle1 = 0.0f;
		float angle2 = 0.0f;

		
		IKSolver.CalcIK_2D(out angle1, out angle2, true, hipLength, kneeLength, targetPos.x, targetPos.y);
		
		//angle1 -= 90;
		
		upperLeg.localRotation = Quaternion.AngleAxis(angle1 * Mathf.Rad2Deg, Vector3.right);
		knee.localRotation = Quaternion.AngleAxis(angle2 * Mathf.Rad2Deg * 0.75f, Vector3.right);
		lowerLeg.localRotation = Quaternion.AngleAxis(angle2 * Mathf.Rad2Deg, Vector3.right);
		
		foot.rotation = goal.rotation * Quaternion.AngleAxis(90, Vector3.right) * Quaternion.AngleAxis(hipOffset, Vector3.up);
	}
}
