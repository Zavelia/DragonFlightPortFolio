using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatchling : Dragon
{
    private void OnEnable()
    {
        this.maxHp = 4;
        this.hp = this.maxHp;
        this.moveSpeed = 0.1f;
    }
    private void Awake()
    {
        GameMain gameMain = GameObject.FindAnyObjectByType<GameMain>();
        gameMain.Hatchlings.Add(this);
        var audio = GameObject.Find("Audio");
        this.audioGo = Instantiate(base.dragonDie, audio.transform);
    }
    // Start is called before the first frame update
    void Start()
    {
        this.maxHp = 4;
        this.hp = this.maxHp;
        this.moveSpeed = 0.1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Translate(Vector2.down * Time.deltaTime * this.moveSpeed);
        this.SelfComeback();
    }

    private void SelfComeback()
    {
        if (this.transform.position.y < -5.7f)
        {
            DragonPoolManager.instance.ReleaseDragon(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            this.HitDamage();
        }
    }

    private void HitDamage()
    {
        this.hp -= 1;
        if (this.hp <= 0)
        {
            this.Die();
        }
    }

    private void Die()
    {
        this.deadPos = this.transform.position; //Á×¾úÀ» ¶§ÀÇ ÁÂÇ¥¸¦ ÀúÀå
        onDie(this.deadPos);
        DragonPoolManager.instance.ReleaseDragon(this.gameObject);
    }

    public void BossDie()
    {
        this.maxHp = 0;
        this.deadPos = this.transform.position; //Á×¾úÀ» ¶§ÀÇ ÁÂÇ¥¸¦ ÀúÀå
        onDie(this.deadPos);
        DragonPoolManager.instance.ReleaseDragon(this.gameObject);
    }
}
