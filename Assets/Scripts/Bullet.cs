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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PlayerTank":
                {
                    if (BulletType == Contants.BulletType.Player)
                    {
                        //玩家子弹不打玩家
                        return;
                    }
                    else if (BulletType == Contants.BulletType.Enemy)
                    {
                        //敌人子弹打玩家坦克
                        Player player = collision.gameObject.GetComponent<Player>();
                        if (player)
                        {
                            player.SendMessage("OnAttackByEnemy");
                        }
                    }
                    break;

                }

            case "EnemyTank":
                {

                    if (BulletType == Contants.BulletType.Player)
                    {
                        //玩家攻击敌人
                        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                        if (enemy)
                        {

                            //去死吧
                            enemy.SendMessage("Die");
                        }
                    }
                    else if (BulletType == Contants.BulletType.Enemy)
                    {
                        //敌人子弹不要打敌人
                        return;
                    }

                    break;
                }
            case "Bullet":
                {
                    //子弹打到子弹，相互抵消
                    break;
                }
        }
        effectAudio.PlayOneShot(BulletDestoryAudioClip);
        Destroy(gameObject);
    }
}
