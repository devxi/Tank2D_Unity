using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{


    public Tilemap TileMapTank;

    public Grid GridTank;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject go = collision.gameObject;
        if (!go.CompareTag("Bullet")) {
            //只有子弹才能销毁TileMap障碍物
            return;
        }

        var worldPos = collision.transform.position;
        //var a = collision.GetContacts();

        Debug.Log("世界坐标:" + this.transform.position);
        Vector3Int cellPos = GridTank.WorldToCell(worldPos);

        if (TileMapTank.HasTile(cellPos))
        {
            Debug.Log("销毁tilemap 障碍物," + cellPos);

            //销毁指定tile
            TileMapTank.SetTile(cellPos, null);
        }
        else
        {
            //bug 
              //Debug.LogError("找不到tile" + cellPos + "vec3:" + collision.transform.position);
        }

    }
}
