using UnityEngine;
using System.Collections;

public class TransformMapper : MonoBehaviour {
	
	public Transform sourceTransform;
	public Vector3 positionAmount = Vector3.one;
	public float rotationAmount = 0.0f;
	public bool worldSpace = false;

	
	void Start () {
	
	}
	
	void Update () {
		
		if (!worldSpace) {
			
			Vector3 localPos;
			localPos.x = sourceTransform.localPosition.x * positionAmount.x;
			localPos.y = sourceTransform.localPosition.y * positionAmount.y;
			localPos.z = sourceTransform.localPosition.z * positionAmount.z;
			transform.localPosition = localPos;
			
			transform.localRotation = Quaternion.Lerp(Quaternion.identity, sourceTransform.localRotation, rotationAmount);

		} else {
			
			Vector3 worldPos;
			worldPos.x = sourceTransform.position.x * positionAmount.x;
			worldPos.y = sourceTransform.position.y * positionAmount.y;
			worldPos.z = sourceTransform.position.z * positionAmount.z;
			transform.position = worldPos;
			
			transform.rotation = Quaternion.Lerp(Quaternion.identity, sourceTransform.rotation, rotationAmount);

		}
	}
}
