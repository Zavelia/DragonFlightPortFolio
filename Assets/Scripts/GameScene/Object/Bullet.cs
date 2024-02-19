using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        this.moveSpeed = 10.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Translate(Vector2.up * this.moveSpeed * Time.deltaTime);
        this.SelfComeback();
    }

    //Å×½ºÆ®
    private void SelfComeback()
    {
        if (this.transform.position.y > 5.86f)
        {
            BulletPoolManager.instance.ReleaseBullet(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dragon"))
        {
            BulletPoolManager.instance.ReleaseBullet(this.gameObject);
        }
    }
}
