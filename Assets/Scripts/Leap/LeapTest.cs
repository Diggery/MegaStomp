using UnityEngine;
using System.Collections;
using Leap;

public class LeapTest : MonoBehaviour {
	
	Controller leapController;
	public Vector3 tipPosition;
	public GameObject position;
	public Vector3 lastTipPosition;

	void Start () {
		leapController = new Controller();
	}
	
	
	void Update () {
		
		if(leapController.IsConnected){
			Frame frame = leapController.Frame(); //The latest frame
            Frame previous = leapController.Frame(1); //The previous frame	
			
			PointableList pointableList = frame.Pointables;
			
			if(previous.IsValid && frame.IsValid){
				tipPosition = frame.Pointables.Frontmost.TipPosition.ToUnity() - previous.Pointables.Frontmost.TipPosition.ToUnity();
				tipPosition.y = 0f;
				position.transform.position = position.transform.position + tipPosition*Time.deltaTime;
			}
			
			
			
			/*foreach(Pointable pointable in pointableList){
				if(pointable.TipPosition.IsValid()){
					tipPosition = pointable.TipPosition.ToUnity();
					position.transform.position = tipPosition;
				}
			}*/
		}
	}
}
