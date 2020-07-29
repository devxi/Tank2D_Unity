using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.PlayerLoop;

public class Enemy : MonoBehaviour
{

    public float MoveSpeed = 3.0f;
    public float BulletSeed = 6.0f;
    public Sprite[] TankSprite;

    public GameObject Explode;//死亡爆炸效果
    public GameObject Shild;//护盾
    public GameObject BullerPrefab;

    private AudioSource effectAudio;

    public AudioClip DieAudioClip;
    public AudioClip BoomAudioClip;



    public float ShootCDTime = 0.5f; //射击cd

    private bool canShoot = true;
    private float invincibleTime = 3.0f;//无敌时间
    private bool isInvincible = true;//是否是无敌状态
    private SpriteRenderer sr;

    private Rigidbody2D rigidbody2d;

    private MoveDirection moveDir = MoveDirection.Up;
    private GameObject explodeObj;
    private GameObject shildObj;

    private float timeValChangeDirection;
    private float x = 0.0f;
    private float y = 0.0f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        effectAudio = gameObject.AddComponent<AudioSource>();
        Born();
    }

    void Start()
    {

    }

    void Update()
    {

        //射击🔫
        //Shoot();

        isInvincible = invincibleTime > 0;
        Shild.SetActive(isInvincible);
        invincibleTime -= Time.deltaTime;

        if (!isInvincible)
        {
            Destroy(shildObj);
        }

    }

    private void FixedUpdate()
    {
        Move();
        //transform.Translate(Vector2.right * 1 * MoveSpeed * Time.fixedDeltaTime);
    }

    private void Move()
    {
        SetMoveDir();
        if (timeValChangeDirection >= 4)
        {
            int num = Random.Range(0, 8);
            if (num >= 5)
            {
                y = -1;
                x = 0;
            }
            else if (num == 0)
            {
                y = 1;
                x = 0;
            }

            else if (0<=num && num<=2)
            {
                y = 0;
                x = -1;
            }

            else if (2 <= num && num <= 4)
            {
                y = 0;
                x = 1;
            }
            timeValChangeDirection = 0;
        }
        else
        {
            timeValChangeDirection += Time.fixedDeltaTime;
        }
        DoMove(x, y);
    }

    private void DoMove(float x, float y)
    {
        print("敌人坦克移动 ->x:" + x + ",y:" + y);
        transform.Translate(Vector2.right * x * MoveSpeed * Time.fixedDeltaTime);

        if (x != 0)
        {
            //return;
        }

        transform.Translate(Vector2.up * y * MoveSpeed * Time.fixedDeltaTime);
    }

    private void SetMoveDir()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");



        if (x > 0.0f)
        {
            moveDir = MoveDirection.Right;
            //sr.sprite = TankSprite[1];
            return;
        }

        if (y > 0.0f)
        {
            moveDir = MoveDirection.Up;
            //sr.sprite = TankSprite[0];
            return;
        }

        if (x < -0.0f)
        {
            moveDir = MoveDirection.Left;
            //sr.sprite = TankSprite[3];
            return;
        }


        if (y < -0.0f)
        {
            moveDir = MoveDirection.Down;
            //sr.sprite = TankSprite[2];
            return;
        }

    }

    private void Shoot()
    {
        if (canShoot)
        {
            GameObject bullet = Instantiate(BullerPrefab);
            Rigidbody2D rig = bullet.GetComponent<Rigidbody2D>();
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.BulletType = Contants.BulletType.Enemy;
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

    private void OnAttackByEnemy()
    {
        if (isInvincible)
        {
            return;
        }
        Die();
    }
    private void Die()
    {
        effectAudio.PlayOneShot(BoomAudioClip);
        explodeObj = Instantiate(Explode, transform.position, Quaternion.identity);
        Invoke("DestoryTank", 0.167f);
    }

    private void DestoryTank()
    {
        effectAudio.PlayOneShot(DieAudioClip);
        Destroy(explodeObj);
        Destroy(gameObject);
    }

    private void Born()
    {
        shildObj = Instantiate(Shild);
        if (explodeObj)
        {
            Destroy(explodeObj);
        }
        invincibleTime = 3.0f;
    }

    private IEnumerator ShootCD()
    {
        canShoot = false;
        yield return new WaitForSeconds(ShootCDTime);
        canShoot = true;
    }

}
