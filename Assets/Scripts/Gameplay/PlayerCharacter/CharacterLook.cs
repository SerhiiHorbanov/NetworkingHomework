using System;
using UnityEngine;

public class CharacterLook : MonoBehaviour
{
	[NonSerialized] public Vector3 LookDirectionInEulerAngles;
	[SerializeField] private float _MinLookAngle;
    [SerializeField] private float _MaxLookAngle;
    [SerializeField] public Transform _Viewpoint;
    
	public void RotateLook(Vector2 delta)
	{
		LookDirectionInEulerAngles.y += delta.x;
		float prevX = LookDirectionInEulerAngles.x;
		LookDirectionInEulerAngles.x = Mathf.Clamp(prevX - delta.y, _MinLookAngle, _MaxLookAngle);
		
		SetLook(LookDirectionInEulerAngles);
	}

	public void SetLook(Vector3 lookDirection)
	{
		LookDirectionInEulerAngles = lookDirection;
		
		transform.eulerAngles = new(0, LookDirectionInEulerAngles.y, 0);
		_Viewpoint.localEulerAngles = new(LookDirectionInEulerAngles.x, 0, 0);
	}
}