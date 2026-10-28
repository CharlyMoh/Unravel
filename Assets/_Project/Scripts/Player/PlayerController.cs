using UnityEngine;
using Unravel.Core;

namespace Unravel.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Parámetros de Movimiento")]
        [Tooltip("Velocidad normal de desplazamiento")]
        [SerializeField] private float walkSpeed = 6f;

        [Tooltip("Velocidad reducida en modo sigilo")]
        [SerializeField] private float crouchSpeed = 2.5f;

        [Tooltip("Fuerza de impulso vertical")]
        [SerializeField] private float jumpForce = 11f;

        [Header("Detección de Suelo")]
        [Tooltip("Transform ubicado en los pies del personaje")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Sistema Acústico")]
        [Tooltip("Radio de la onda de sonido al caminar")]
        [SerializeField] private float walkNoiseRadius = 4f;

        [Tooltip("Radio de la onda al correr o aterrizar de un salto")]
        [SerializeField] private float runNoiseRadius = 8f;

        private Rigidbody2D _rb;
        private Collider2D _col;
        private float _horizontalInput;
        private bool _isGrounded;
        private bool _isCrouching;

        public bool IsCrouching => _isCrouching;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
        }

        private void Update()
        {
            // Lectura de inputs en el ciclo visual
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _isCrouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

            // Salto: solo si toca el suelo y no está agachado
            if (Input.GetButtonDown("Jump") && _isGrounded && !_isCrouching)
            {
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
                // Saltar produce un sonido fuerte inmediato
                GameEvents.EmitNoise(transform.position, runNoiseRadius);
            }

            // Emisión de ruido continuo si está en movimiento
            HandleAcousticEmission();
        }

        private void FixedUpdate()
        {
            // Verificación física del suelo usando OverlapCircle
            if (groundCheck != null)
            {
                _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            }

            // Aplicación de velocidad física horizontal
            float targetSpeed = _isCrouching ? crouchSpeed : walkSpeed;
            _rb.linearVelocity = new Vector2(_horizontalInput * targetSpeed, _rb.linearVelocity.y);
        }

        private void HandleAcousticEmission()
        {
            // Si está agachado (paso amortiguado), en el aire o quieto, no emite sonido
            if (_isCrouching || Mathf.Abs(_horizontalInput) < 0.05f || !_isGrounded)
                return;

            // Al caminar emite la onda acústica
            GameEvents.EmitNoise(transform.position, walkNoiseRadius);
        }

        // Dibuja en la vista de escena la esfera de detección de suelo
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}