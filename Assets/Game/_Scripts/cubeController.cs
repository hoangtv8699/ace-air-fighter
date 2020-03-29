using UnityEngine;
using System.Collections;
using System;
public class cubeController : MonoBehaviour {


	public Animator cubeAnimator;
	public Animation animator;

	public enum  RotationStates{
		left,right
	};

	public RotationStates currentRotation;
	void OnEnable(){
		
		OnpointerDown.buttonDown += OnbuttonClick;
	}
	public	void OnbuttonClick(System.Object buttonname,EventArgs args)
	{
		if (buttonname.ToString ().Contains ("left")) {
			LeftRotate ();
		} else if (buttonname.ToString ().Contains ("right")) {
			RightRotate ();
		}
	}

	void LeftRotate()
	{

		if (currentRotation != RotationStates.left) {
			cubeAnimator.SetTrigger ("Left");
			currentRotation = RotationStates.left;
		}
	}

	void RightRotate()
	{
		if (currentRotation != RotationStates.right) {
			cubeAnimator.SetTrigger ("Right");
			currentRotation = RotationStates.right;
		}
	}

	 
}
