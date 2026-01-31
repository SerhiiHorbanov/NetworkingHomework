using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacterMovement : MonoBehaviour
{
    [SerializeField] private float _MaxSpeed;
    [SerializeField] private float _Acceleration;

    private Rigidbody _rigidbody;
    
    public Vector3 _GlobalMoveDir;

    public void SetMoveDirFromRelative(Vector2 relativeDir)
    {
        _GlobalMoveDir = transform.forward * relativeDir.y + transform.right * relativeDir.x;
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        DoWalkingMovement();
    }
    
    private void DoWalkingMovement()
    {
        Vector3 wishedHorizontalVelocity = _GlobalMoveDir * _MaxSpeed;
        
        Vector3 currentHorizontalVelocity = _rigidbody.linearVelocity;
        currentHorizontalVelocity.y = 0;
        
        Vector3 wishedVelocityDelta = wishedHorizontalVelocity - currentHorizontalVelocity;
        
        float maxVelocityDelta = _Acceleration * Time.fixedDeltaTime;
        float maxVelocityDeltaSquared = maxVelocityDelta * maxVelocityDelta;

        if (wishedVelocityDelta.sqrMagnitude > maxVelocityDeltaSquared)
        {
            _rigidbody.AddForce(wishedVelocityDelta, ForceMode.VelocityChange);
            return;
        }

        Vector3 velocityDelta = _GlobalMoveDir * maxVelocityDelta;
        
        _rigidbody.AddForce(velocityDelta, ForceMode.VelocityChange);
    }
}
