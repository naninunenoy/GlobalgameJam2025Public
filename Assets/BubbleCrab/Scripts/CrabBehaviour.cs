using UnityEngine;
using UnityEngine.InputSystem;

namespace BubbleCrab.n5y
{
    public class CrabBehaviour : MonoBehaviour
    {
        static readonly int moving = Animator.StringToHash("Moving");
        static readonly int falling = Animator.StringToHash("Falling");
        [SerializeField] float moveSpeed = 1.0f;
        [SerializeField] PlayerInput playerInput;

        InputAction moveAction;
        Vector2 moveInput;
        Animator _animator;
        Rigidbody _rigidbody;

        void Awake()
        {
            moveAction = playerInput.actions["Move"];
            _animator = GetComponentInChildren<Animator>(true);
            _rigidbody = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMoveCanceled;
        }

        void OnDisable()
        {
            moveAction.performed -= OnMovePerformed;
            moveAction.canceled -= OnMoveCanceled;
        }

        void Update()
        {
            ApplyMove();
            _animator.SetBool(falling, _rigidbody.linearVelocity.y < 0);
        }

        void OnMovePerformed(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
            _animator.SetBool(moving, true);
        }

        void OnMoveCanceled(InputAction.CallbackContext ctx)
        {
            moveInput = Vector2.zero;
            _animator.SetBool(moving, false);
        }

        void ApplyMove()
        {
            if (moveInput != Vector2.zero)
            {
                transform.position += new Vector3(moveInput.x, 0, 0) * moveSpeed * Time.deltaTime;
            }
        }

        public void OnCollisionEnter(Collision other)
        {
            // エリア外落下のリスポーン
            if (other.gameObject.CompareTag("Respawn"))
            {
                transform.position = new Vector3(0, 0.5F, 0);
                return;
            }
        }

        [ContextMenu("Clear 200m")]
        void Clear200m()
        {
            transform.position = new Vector3(0, 200, 0);
            _rigidbody.AddForce(Vector3.up, ForceMode.Impulse);
        }
    }
}
