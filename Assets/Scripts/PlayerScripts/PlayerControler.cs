using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour 
{
	[Range(0.0f, 10.0f)]
	public float moveSpeed = 3f;
    public float rotateSpeed = 3f;
	public float jumpForce = 600f;

	public int playerHealth = 1;

	public LayerMask whatIsGround;

	public Transform groundCheck;

	[HideInInspector]
	public bool playerCanMove = true;

    [HideInInspector] public bool rolling = false;

	public AudioClip coinSFX;
	public AudioClip deathSFX;
	public AudioClip fallSFX;
	public AudioClip jumpSFX;
	public AudioClip victorySFX;

	private Transform _transform;
    private Quaternion startQuaternion;
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private AudioSource _audio;

	private float xVelocity;
	private float yVelocity;

	private bool facingRight = true;
	public bool isGrounded = false;
	private bool isRunning = false;
    private bool canMidAirJump = false;

	private int _playerLayer;

	private int _platformLayer;
	
	private void Awake () 
	{
		_transform = GetComponent<Transform> ();

	    startQuaternion = _transform.rotation;
		
		_rigidbody = GetComponent<Rigidbody2D> ();
		
		_animator = GetComponent<Animator>();
		
		_audio = GetComponent<AudioSource> ();
		if (_audio==null) 
		{
			_audio = gameObject.AddComponent<AudioSource>();
		}

		_playerLayer = this.gameObject.layer;

		_platformLayer = LayerMask.NameToLayer("Platform");
	}

	private void Update()
	{
		if (!playerCanMove || (Time.timeScale == 0f))
			return;

		xVelocity = Input.GetAxisRaw("Horizontal");

		if (xVelocity != 0) 
		{
			isRunning = true;
		} else {
			isRunning = false;
		}

		_animator.SetBool("Running", isRunning);

        yVelocity = _rigidbody.velocity.y;

		isGrounded = Physics2D.Linecast(_transform.position, groundCheck.position, whatIsGround);

	    if (isGrounded)
	    {
	        canMidAirJump = true;
	    }

		_animator.SetBool("Grounded", isGrounded);

		if((isGrounded || canMidAirJump) && Input.GetKeyDown(KeyCode.Space))
		{
			Jump();

			if (!isGrounded) 
			{
				canMidAirJump = false;
			}
		}

		if(Input.GetKeyUp(KeyCode.Space) && yVelocity > 0f)
		{
			yVelocity = 0f;
		}

		_rigidbody.velocity = new Vector2(xVelocity * moveSpeed, yVelocity);

		Physics2D.IgnoreLayerCollision(_playerLayer, _platformLayer, (yVelocity > 0.0f));

	    rolling = Input.GetButton("Fire2");

	    if (rolling)
	    {
	        _rigidbody.constraints = RigidbodyConstraints2D.None;
	    }
	    else
	    {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            StandUp();
	    }
	}

    public void StandUp()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, startQuaternion, rotateSpeed * Time.deltaTime);
    }

    public void Jump(float scale = 1f)
    {
        yVelocity = 0f;

        _rigidbody.AddForce(new Vector2(0, jumpForce * scale));

        PlaySound(jumpSFX);
    }

	private void LateUpdate()
	{
		Vector3 localScale = _transform.localScale;

		if (xVelocity > 0) // moving right so face right
		{
			facingRight = true;
		} 
		else if (xVelocity < 0) // moving left so face left
        {
			facingRight = false;
		}

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0))) 
		{
			localScale.x *= -1;
		}

		_transform.localScale = localScale;
	}

 	private void FreezeMotion() 
	{
		playerCanMove = false;
		_rigidbody.isKinematic = true;
	}

	private void UnFreezeMotion() 
	{
		playerCanMove = true;
		_rigidbody.isKinematic = false;
	}

	private void PlaySound(AudioClip clip)
	{
		_audio.PlayOneShot(clip);
	}

	public void ApplyDamage (int damage) 
	{
		if (playerCanMove) 
		{
			playerHealth -= damage;

			if (playerHealth <= 0)
            {
				PlaySound(deathSFX);
				StartCoroutine (KillPlayer ());
			}
		}
	}

	public void FallDeath () 
	{
		if (playerCanMove) 
		{
			playerHealth = 0;
			PlaySound(fallSFX);
			StartCoroutine (KillPlayer ());
		}
	}

	IEnumerator KillPlayer()
	{
		if (playerCanMove)
		{
			FreezeMotion();

			_animator.SetTrigger("Death");
			
			yield return new WaitForSeconds(2.0f);

			if (GameManager.gm)
            {
				GameManager.gm.ResetGame();
			}
            else
            {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	public void CollectCoin(int amount) 
	{
		PlaySound(coinSFX);

		if (GameManager.gm)
			GameManager.gm.AddPoints(amount);
	}

	public void Victory() 
	{
		PlaySound(victorySFX);
		FreezeMotion ();
		_animator.SetTrigger("Victory");

		if (GameManager.gm)
			GameManager.gm.LevelCompete();
	}

	public void Respawn(Vector3 spawnloc) 
	{
		UnFreezeMotion();
		playerHealth = 1;
		_transform.parent = null;
		_transform.position = spawnloc;
		_animator.SetTrigger("Respawn");
	}
}
