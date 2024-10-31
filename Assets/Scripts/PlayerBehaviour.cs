using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private GameBehavior _gameManager;
    [SerializeField] float MoveSpeed = 10f;
    [SerializeField] float RotateSpeed = 75f;
    private float _vInput;
    private float _hInput;
    public float JumpVelocity = 5f;
    private bool _isJumping;
    private Rigidbody _rb;
    [SerializeField] float DistanceToGround = 0.1f;
    [SerializeField] LayerMask GroundLayer;
    private CapsuleCollider _col;
    public GameObject Bullet;
    [SerializeField] float BulletSpeed = 30f;
    private bool _isShooting;
    [SerializeField] AudioSource Shooting;

    private void Start()
    { 
        _col= GetComponent<CapsuleCollider>(); 
        _rb = GetComponent<Rigidbody>(); 
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
        
    }
    void Update()
    {
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;
        _isJumping |= Input.GetKeyDown(KeyCode.J);
        _isShooting |= Input.GetKeyDown(KeyCode.Space);
     
        this.transform.Translate(Vector3.forward * _vInput * Time.deltaTime);
        this.transform.Rotate(Vector3.up * _hInput * Time.deltaTime);
     
    }
     void FixedUpdate()
    {
        if (IsGrounded() &&_isJumping) 
        { 
            _rb.AddForce(Vector3.up * JumpVelocity, ForceMode.Impulse);
        }
        _isJumping = false;
        Vector3 rotation = Vector3.up * _hInput;
        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);
        _rb.MovePosition(this.transform.position + this.transform.forward * _vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation*angleRot);
        if (_isShooting)
        {
            GameObject newBullet= Instantiate(Bullet,this.transform.position + new Vector3(0,0,1),this.transform.rotation);
            Rigidbody BulletRB= newBullet.GetComponent<Rigidbody>();
            BulletRB.velocity = this.transform.forward *BulletSpeed;
            Shooting.Play();

        }
        _isShooting = false;

    }
      private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3 (_col.bounds.center.x,_col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center,capsuleBottom,DistanceToGround,GroundLayer,QueryTriggerInteraction.Ignore);
       return grounded;
    } 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Enemy" || collision.gameObject.name == "Bullet(Clone)")
        {
            _gameManager.PlayerHP -= 1;
        }
    }
}
