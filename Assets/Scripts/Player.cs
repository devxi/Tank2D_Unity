using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    public float MoveSpeed = 3.0f;
    public float BulletSeed = 6.0f;
    public Vector2 BornPosition;//出生坐标
    public Sprite[] TankSprite;

    public GameObject Explode;//死亡爆炸效果
    public GameObject Shild;//护盾
    public GameObject BullerPrefab;


    private AudioSource moveAudio;
    private AudioSource effectAudio;

    public AudioClip DieAudioClip;
    public AudioClip EngineDrivingAudioClip;
    public AudioClip EngineIdleAudioClip;
    public AudioClip FireAudioClip;
    public AudioClip BoomAudioClip;

    

    public float ShootCDTime = 0.5f; //射击cd

    private bool canShoot = true;
    private float invincibleTime = 3.0f;//无敌时间
    private bool isInvincible = true;//是否是无敌状态
    private SpriteRenderer sr;
    
    private Rigidbody2D rigidbody2d;
    
    private MoveDirection moveDir = MoveDirection.Up;

    public GameObject shildObj { get; private set; }

    private GameObject explodeObj;

    private Animator animator;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        moveAudio = gameObject.AddComponent<AudioSource>();
        effectAudio = gameObject.AddComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //射击🔫
            Shoot();
        }

   
        isInvincible = invincibleTime > 0;
        invincibleTime -= Time.deltaTime;
        if (!isInvincible)
        {
            Destroy(shildObj);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        SetMoveDir();

        if (x == 0 && y == 0)
        {
            //idle
            moveAudio.clip = EngineIdleAudioClip;
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
            
        }
        else
        {
            moveAudio.clip = EngineDrivingAudioClip;
            if (!moveAudio.isPlaying)
            {
                moveAudio.Play();
            }
            DoMove(x, y);
        }
    }

    private void DoMove(float x, float y)
    {
        transform.Translate(Vector2.right * x * MoveSpeed * Time.fixedDeltaTime);

        if (x != 0)
        {
            return;
        }

        transform.Translate(Vector2.up * y * MoveSpeed * Time.fixedDeltaTime);
    }

    private void SetMoveDir()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (x > 0.0f)
        {
            animator.SetInteger("Float_X", 1);
            moveDir = MoveDirection.Right;
            sr.sprite = TankSprite[1];
            return;
        }

        if (y > 0.0f)
        {
            animator.SetInteger("Float_Y", 1);
            moveDir = MoveDirection.Up;
            sr.sprite = TankSprite[0];
            return;
        }

        if (x < -0.0f)
        {
            animator.SetInteger("Float_X", -1);
            moveDir = MoveDirection.Left;
            sr.sprite = TankSprite[3];
            return;
        }


         if (y < -0.0f)
        {
            animator.SetInteger("Float_Y", -1);
            moveDir = MoveDirection.Down;
            sr.sprite = TankSprite[2];
            return;
        }

    }

    private void Shoot()
    {
        if (canShoot)
        {
            effectAudio.PlayOneShot(FireAudioClip);
            GameObject bullet = Instantiate(BullerPrefab);
            Rigidbody2D rig = bullet.GetComponent<Rigidbody2D>();
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.BulletType = Contants.BulletType.Player;
            switch (moveDir)
            {
                case MoveDirection.Up:
                    {
                        bullet.transform.Rotate(new Vector3(0, 0, 0));
                        bullet.transform.position = new Vector2(transform.position.x, transform.position.y + 0.4f);
                        rig.AddForce(new Vector2(0, 100) * BulletSeed);
                        break;
                    }
                case MoveDirection.Left:
                    {
                        bullet.transform.Rotate(new Vector3(0, 0, 90));
                        bullet.transform.position = new Vector2(transform.position.x - 0.4f, transform.position.y);
                        rig.AddForce(new Vector2(-100, 0) * BulletSeed);
                        break;
                    }
                case MoveDirection.Down:
                    {
                        bullet.transform.Rotate(new Vector3(0, 0, -180));
                        bullet.transform.position = new Vector2(transform.position.x, transform.position.y - 0.4f);
                        rig.AddForce(new Vector2(0, -100) * BulletSeed);
                        break;
                    }
                case MoveDirection.Right:
                    {
                        bullet.transform.Rotate(new Vector3(0, 0, -90));
                        bullet.transform.position = new Vector2(transform.position.x + 0.4f, transform.position.y);
                        rig.AddForce(new Vector2(100, 0) * BulletSeed);
                        break;
                    }
                default:
                    break;
            }



            StartCoroutine(ShootCD());
        }
    }

    private void OnAttackByEnemy() {
        if (isInvincible) {
            return;
        }
        Die();
    }
    private void Die()
    {
        effectAudio.PlayOneShot(BoomAudioClip);
        gameObject.SetActive(false);
        explodeObj = Instantiate(Explode, transform.position, Quaternion.identity);
        Invoke("DestoryTank", 0.167f);
    }

    private void DestoryTank()
    {
        effectAudio.PlayOneShot(DieAudioClip);
        Destroy(explodeObj);
        Destroy(gameObject);
    }

    private IEnumerator ShootCD()
    {
        canShoot = false;
        yield return new WaitForSeconds(ShootCDTime);
        canShoot = true;
    }

}
