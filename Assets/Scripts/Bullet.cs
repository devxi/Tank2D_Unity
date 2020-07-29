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
            switch (collision.gameObject.tag)
            {
                case "PlayerTank":
                    {
                        //敌人子弹打玩家坦克
                        Player player = collision.gameObject.GetComponent<Player>();
                        if (player)
                        {
                            player.SendMessage("OnAttackByEnemy");
                        }
                        break;
                    }

                case "EnemyTank":
                    {
                        //敌人子弹不要打敌人
                        return;
                    }
            }
        }
         effectAudio.PlayOneShot(BulletDestoryAudioClip);
        Destroy(gameObject);
    }
}
