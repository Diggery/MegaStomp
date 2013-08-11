using UnityEngine;
using System.Collections;

public class CrusaderControl : MonoBehaviour {
	
	public float spring;
	public float legLength;
	public float moveSpeed;
	public float turnSpeed;

	public float stability = 0.3f;
    public float speed = 2.0f;
	
	public bool inputOn;
	public bool stunned;
	
	public Transform leftFoot;
	public Transform rightFoot;

	Vector3 currentInput;

	void Start () {
	
	}
	
	void FixedUpdate () {
		
		if (!stunned) {
			float heightDelta = legLength - transform.position.y;
			heightDelta = Mathf.Clamp(heightDelta * spring * spring, 0.0f, 300.0f);
			rigidbody.AddForce (Vector3.up * heightDelta);
			
			if (inputOn) {
				rigidbody.AddRelativeForce (Vector3.forward * currentInput.x * moveSpeed);	
				rigidbody.AddRelativeTorque (Vector3.right * currentInput.x * turnSpeed * 0.5f);	
				rigidbody.AddRelativeTorque (Vector3.up * currentInput.y * turnSpeed);	
			} else {
				Vector3 midFoot = Vector3.Lerp(leftFoot.position, rightFoot.position, 0.5f);
				Vector3 targetOffset = midFoot - transform.position;
				targetOffset.y = 0.0f;
				rigidbody.AddForce(targetOffset * moveSpeed * 2.0f);
			}
		
			Vector3 predictedUp = Quaternion.AngleAxis(
            	rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
            	rigidbody.angularVelocity
       			) * transform.up;
 
        	Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        	rigidbody.AddTorque(torqueVector * speed * speed);
			
			
		} else {
			
		}
		

	
	}
	
	public void setInputOn() {
		inputOn = true;
	}
	
	public void setInputOff() {
		inputOn = false;
	}
	
	public void input(Vector3 newInput) {
		currentInput = newInput;
	}
	
		
	public void enableRagDoll(Vector3 newForce) {
		stunned = true;
		Rigidbody[] rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody currentRigidbody in rigidbodies) {
			if (currentRigidbody.gameObject.layer == 9) {
				currentRigidbody.useGravity = true;
				currentRigidbody.isKinematic = false;	
			}
		}
		//root.rigidbody.AddForce(newForce, ForceMode.Impulse);
	}
		
	public void disableRagDoll(Vector3 newForce) {
		stunned = false;
		Rigidbody[] rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody currentRigidbody in rigidbodies) {
			if (currentRigidbody.gameObject.layer == 9) {
				currentRigidbody.useGravity = false;
				currentRigidbody.isKinematic = true;	
			}
		}
		//root.rigidbody.AddForce(newForce, ForceMode.Impulse);
	}
}


