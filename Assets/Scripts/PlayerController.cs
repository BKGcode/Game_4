using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("3D Movement")]
    [SerializeField] float _moveSpeed = 8f;
    [SerializeField] float _rotationSpeed = 500f;
    [SerializeField] float _acceleration = 12f;
    
    [Header("3D Interaction")]
    [SerializeField] LayerMask _interactableLayer;
    [SerializeField] float _interactionRadius = 2f;

    private Rigidbody _rb;
    private Vector3 _currentVelocity;
    private Collider[] _interactablesBuffer = new Collider[5];

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        HandleMovement();
        CheckInteractions3D();
    }

    void HandleMovement()
    {
        Vector3 input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Movimiento en XZ con rotación suave
        if (input != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                _rotationSpeed * Time.deltaTime
            );
        }

        Vector3 targetVelocity = input * _moveSpeed;
        _currentVelocity = Vector3.MoveTowards(
            _currentVelocity, 
            targetVelocity, 
            _acceleration * Time.deltaTime
        );

        _rb.velocity = _currentVelocity;
    }

    void CheckInteractions3D()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _interactionRadius,
                _interactablesBuffer,
                _interactableLayer
            );

            if (hitCount > 0)
            {
                Interactable interactable = _interactablesBuffer[0].GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.ExecuteInteraction();
                    Debug.Log($"🔄 Interacted with: {_interactablesBuffer[0].name}");
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _interactionRadius);
    }
}
