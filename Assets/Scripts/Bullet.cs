using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Contants.BulletType BulletType;
    private AudioSource effectAudio;
    public AudioClip BulletDestoryAudioClip;
    private void Awake()
    {
        BulletType = Contants.BulletType.None;
        effectAudio = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (BulletType == Contants.BulletType.Player)
        {
            switch (collision.gameObject.tag)
            {
                case "PlayerTank":
                    {
                        return;
                    }

                case "EnemyTank":
                    {
                        //玩家攻击敌人
                        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                        if (enemy) 
                        {

                            //去死吧
                            enemy.SendMessage("Die");
                        }

                        break;
                    }
            }
        }

        else if (BulletType == Contants.BulletType.Enemy)
        {

        }
         effectAudio.PlayOneShot(BulletDestoryAudioClip);
        Destroy(gameObject);
    }
}
