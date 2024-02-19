using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public System.Action <Vector3> onDie;
    public System.Action <int>onHit;
    public enum eDragonType
    {
        White,Gold,Boss,Hatchling
    }
    [SerializeField] private eDragonType dragonType;
    protected float moveSpeed;
    protected int maxHp;
    protected int hp;
    protected Vector3 deadPos;

    [SerializeField] protected GameObject dragonDie;
    protected GameObject audioGo;
    private bool isCreate=false;

    public eDragonType DragonType { get => dragonType; set => dragonType = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int Hp { get => hp; set => hp = value; }

    private void OnEnable()
    {
        if (this.dragonType == eDragonType.White)
        {
            this.maxHp = 1;
            this.hp = this.maxHp;
        }
        else if (this.dragonType == eDragonType.Gold)
        {
            this.maxHp = 3;
            this.hp = this.maxHp;
        }
    }

    private void Awake()
    {
        GameMain gameMain = GameObject.FindAnyObjectByType<GameMain>();
        gameMain.Dragons.Add(this);
        var audio = GameObject.Find("Audio");
        this.audioGo = Instantiate(this.dragonDie,audio.transform);
    }
    // Start is called before the first frame update
    void Start()
    {
        this.moveSpeed = 5f;
        this.hp = this.maxHp;
        if(this.dragonType == eDragonType.White)
        {
            this.maxHp = 1;
        }
        else if(this.dragonType == eDragonType.Gold)
        {
            this.maxHp = 3;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Translate(Vector2.down * Time.deltaTime * this.moveSpeed);
        this.SelfComeback();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            this.HitDamage();
        }
        else if (collision.CompareTag("Booster") || collision.CompareTag("Shock"))
        {
            this.Die();
        }
    }

    private void OnDisable()
    {
        if (this.audioGo != null)
        {
            var audio = audioGo.GetComponent<AudioSource>();
            if (this.isCreate) audio.Play();
        }
        this.isCreate = true;
    }

    private void HitDamage()
    {
        this.hp -= 1;
        this.onHit(this.hp);
        if (this.hp <= 0)
        {
            this.deadPos = this.transform.position; //Á×¾úÀ» ¶§ÀÇ ÁÂÇ¥¸¦ ÀúÀå
            onDie(this.deadPos);
            //StartCoroutine(CoDieAudioPlay());
            DragonPoolManager.instance.ReleaseDragon(this.gameObject);
        }
    }

    private void Die()
    {
        this.hp = 0;
        if (this.hp <= 0)
        {
            this.deadPos = this.transform.position; //Á×¾úÀ» ¶§ÀÇ ÁÂÇ¥¸¦ ÀúÀå
            onDie(this.deadPos);
            //StartCoroutine(CoDieAudioPlay());
            DragonPoolManager.instance.ReleaseDragon(this.gameObject);
        }
    }

    private void SelfComeback()
    {
        if (this.transform.position.y < -5.7f)
        {
            DragonPoolManager.instance.ReleaseDragon(this.gameObject);
        }
    }
}
