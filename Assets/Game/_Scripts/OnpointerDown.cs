using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System ;


public class OnpointerDown : MonoBehaviour ,IPointerDownHandler,IPointerUpHandler
{

	public static EventHandler buttonDown, buttonUp	; 

	public void OnPointerDown (PointerEventData data)
	{

		if (buttonDown != null)
			buttonDown (gameObject.name, null);

	}

	public void OnPointerUp (PointerEventData data)
	{

		if (buttonUp != null)
			buttonUp (gameObject.name, null);
	}

}
