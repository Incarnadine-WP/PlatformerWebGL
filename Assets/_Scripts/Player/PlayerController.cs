using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerAnimator _animator;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private LayerMask _ladderLayerMask;

    [Header("Параметры игрока, отдельно от UnitStats")]
    [Space(5)]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _attackRate = 2f;
    [SerializeField] private float _timeToNextAttack;

    private PlayerInput _playerInput;
    private Transform _platform;
    private bool _grounded;
    private bool _isClimbing;
    private bool _isLadder;
    private bool _isFlip = false;
    private Vector2 _inputVector;
    private float _footStepTimer;
    private float _footStepTimerMax = 0.35f;
    private bool _isRuning;

    public UnityAction OnLanding;
    public UnityAction OnInteractAction;
    public UnityAction OnPauseGame;

    public static PlayerController Instance;

    public bool IsLadder => _isLadder && Mathf.Abs(_inputVector.y) > 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _playerInput = new PlayerInput();
        _playerInput.PlayerMovement.Enable();
        SubscribePlayerController();
        _playerInput.PlayerMovement.PauseMenu.performed += PauseGame;
    }

    private void OnEnable()
    {
        OnLanding += _animator.StopJumpAnim;
        _player.OnDamageAction += _animator.PlayPlayerHurtAnim;
        _player.OnDeathAction += _animator.PlayDeathAnim;
    }

    private void OnDisable()
    {
        _playerInput.PlayerMovement.Disable();
        UnsubscribePlayerController();
        _playerInput.PlayerMovement.PauseMenu.performed -= PauseGame;

        OnLanding -= _animator.StopJumpAnim;
        _player.OnDamageAction -= _animator.PlayPlayerHurtAnim;
        _player.OnDeathAction -= _animator.PlayDeathAnim;
    }

    private void Update()
    {
        _inputVector = _playerInput.PlayerMovement.Movement.ReadValue<Vector2>();

        if (IsLadder)
            _isClimbing = true;

        StepsSound();
    }

    private void FixedUpdate()
    {
        AirControl();
        HandleMovement();
        LadderControl();
    }

    public void DisablePlayerController() => _playerInput.PlayerMovement.Disable();

    public void SubscribePlayerController()
    {
        _playerInput.PlayerMovement.Jump.performed += JumpBig;
        _playerInput.PlayerMovement.Jump.canceled += JumpSmall;
        _playerInput.PlayerMovement.Attack.performed += Attack;
        _playerInput.PlayerMovement.Interact.performed += Interact;
    }

    public void UnsubscribePlayerController()
    {
        _playerInput.PlayerMovement.Jump.performed -= JumpBig;
        _playerInput.PlayerMovement.Jump.canceled -= JumpSmall;
        _playerInput.PlayerMovement.Attack.performed -= Attack;
        _playerInput.PlayerMovement.Interact.performed -= Interact;
    }

    private void HandleMovement()
    {
        _player.PlayerRigidbody.velocity = new Vector2(_inputVector.x * _player.PlayerStats.Speed, _player.PlayerRigidbody.velocity.y);
        float speed = _inputVector.x * _player.PlayerStats.Speed;

        FlippingPlayer();
        _animator.PlayRunAnim(Mathf.Abs(speed));

        _isRuning = _inputVector != Vector2.zero;

        // gamepad stick
        if (_inputVector.y < -0.6f)
            if (_platform != null)
                StartCoroutine(DisablePlatformCollision());
    }

    private void StepsSound()
    {
        _footStepTimer -= Time.deltaTime;

        if (_footStepTimer < 0f)
        {
            _footStepTimer = _footStepTimerMax;

            if (_isRuning && _grounded)
                SoundManager.Instance.PlayFootStep(transform.position);
        }
    }

    private IEnumerator DisablePlatformCollision()
    {
        var delay = new WaitForSeconds(0.25f);
        BoxCollider2D platformCollider = _platform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(platformCollider, _player.BoxCollider2D);
        yield return delay;
        Physics2D.IgnoreCollision(platformCollider, _player.BoxCollider2D, false);
    }

    private void Interact(InputAction.CallbackContext obj) => OnInteractAction?.Invoke();
    private void PauseGame(InputAction.CallbackContext obj) => OnPauseGame?.Invoke();

    private void JumpBig(InputAction.CallbackContext context)
    {
        if (context.performed && IsGround() || _isClimbing)
        {
            _player.PlayerRigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _animator.PlayJumpAnim();
            SoundManager.Instance.PlayJumpSound(transform.position);
        }
    }

    private void JumpSmall(InputAction.CallbackContext context)
    {
        Vector2 lowJump = new Vector2(_player.PlayerRigidbody.velocity.x, _player.PlayerRigidbody.velocity.y * 0.5f);
        if (_player.PlayerRigidbody.velocity.y > 0f)
            _player.PlayerRigidbody.velocity = lowJump;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (Time.time >= _timeToNextAttack)
        {
            _timeToNextAttack = Time.time + 1f / _attackRate;
            _animator.PlayAttackAnim();
            SoundManager.Instance.PlayPlayerAttackSound(transform.position, 1f);
        }
    }

    private void FlippingPlayer()
    {
        if (_inputVector.x > 0 && _isFlip)
        {
            _isFlip = false;
            transform.Rotate(0, 180, 0);
        }
        else if (_inputVector.x < 0 && !_isFlip)
        {
            _isFlip = true;
            transform.Rotate(0, 180, 0);
        }
    }

    private bool IsGround()
    {
        float rangeCast = 0.1f;
        return Physics2D.OverlapCircle(_groundCheck.position, rangeCast, _groundLayerMask);
    }

    private void AirControl()
    {
        bool wasGrounded = _grounded;
        _grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, 0.2f, _groundLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _grounded = true;
                if (!wasGrounded)
                    OnLanding.Invoke();
            }
        }
    }

    private void LadderControl()
    {
        if (_isClimbing)
        {
            _player.PlayerRigidbody.gravityScale = 0f;

            _player.PlayerRigidbody.velocity = new Vector2(_player.PlayerRigidbody.velocity.x, _inputVector.y * _player.PlayerStats.Speed);
            _animator.PlayLadderAnim(Mathf.Abs(_inputVector.y));
            _animator.IsGroundTrigger(_isLadder);

        }
        else
        {
            _player.PlayerRigidbody.gravityScale = 3f;
            _animator.PlayLadderAnim(0f);
            _animator.IsGroundTrigger(_isLadder);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ladder ladder))
            if (ladder != null)
                _isLadder = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Ladder ladder))
        {
            if (ladder != null)
            {
                _isLadder = false;
                _isClimbing = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            _platform = collision.transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            _platform = null;
    }
}
