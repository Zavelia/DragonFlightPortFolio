using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float moveSpeed;
    Player player;
    Vector3 playerPos;
    Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        this.moveSpeed = 1.0f;
        this.player = GameObject.FindAnyObjectByType<Player>();
        if(this.player != null)
        {
            this.playerPos = this.player.transform.position;
        }
        else this.playerPos = Vector3.zero;
        
        this.direction = this.playerPos - this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(this.direction * Time.deltaTime * moveSpeed);
        this.SelfDestory();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            Destroy(this.gameObject);
        }
    }

    private void SelfDestory()
    {
        if (this.transform.position.y < -5.7f)
        {
            Destroy(this.gameObject);
        }
    }
}
