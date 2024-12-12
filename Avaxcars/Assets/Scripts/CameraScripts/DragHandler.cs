
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler{


	public void OnDrag(PointerEventData data){


		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().OnDrag (data);

	}

	public void OnEndDrag(PointerEventData data){


	}

}