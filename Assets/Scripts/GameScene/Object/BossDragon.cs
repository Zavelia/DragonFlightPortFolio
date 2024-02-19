using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDragon : Dragon
{
    private enum eState
    {
        Idle,Attack,Summon
    }
    private eState state;
    private int deathCount = 1;
    private Animator anim;
    private float elapsedTime = 0.0f;

    public new int MaxHp
    {
        get { return base.maxHp; }
    }
    private BossAnimationReceiver receiver;
    [SerializeField] private GameObject bossBulletPrefab;
    [SerializeField] private GameObject hatchlingPrefab;

    [SerializeField] private Transform[] bulletInitPoses; //0이 왼쪽, 1이 중앙, 2가 오른쪽
    [SerializeField] private Transform[] hatchlingInintPoses;
    private void Awake()
    {
        Debug.Log("BossDragon 실행");
        GameMain gameMain = GameObject.FindAnyObjectByType<GameMain>();
        gameMain.BossDragon = this;
        this.anim = this.GetComponent<Animator>();
        this.receiver = this.GetComponent<BossAnimationReceiver>();
        this.anim.SetTrigger("Idle");
        this.state = eState.Idle;
        base.maxHp = 30;
    }
    private void OnEnable()
    {
        if (this.deathCount == 0)
        {
            base.maxHp = 30;
        }
        else base.maxHp = 30 * (deathCount-1);
        Debug.LogFormat("<color=red>Death Count:{0}</color>",this.deathCount);
        this.hp = base.maxHp;
        this.state = eState.Idle; 
    }
    private void OnDisable()
    {
        this.deathCount++;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.hp = base.maxHp;
        this.receiver.onFinished = () => {
            this.state = eState.Idle;
            this.anim.SetTrigger("Idle");
            this.elapsedTime = 0.0f;
        };
        this.receiver.onAttackLeft = () =>
        {
            GameObject bullet=Instantiate(this.bossBulletPrefab);
            bullet.transform.position = this.bulletInitPoses[0].position;
        };

        this.receiver.onAttackRight = () =>
        {
            GameObject bullet = Instantiate(this.bossBulletPrefab);
            bullet.transform.position = this.bulletInitPoses[2].position;
        };

        this.receiver.onAttackFront = () =>
        {
            for(int i = 0; i < 3; i++)
            {
                GameObject bullet = Instantiate(this.bossBulletPrefab);
                bullet.transform.position = this.bulletInitPoses[1].position;
            }
        };

        this.receiver.onSummon = () => {
            this.CallHatchling();
        };
    }

    private void CallHatchling()
    {
        for (int i = 0; i < hatchlingInintPoses.Length; i++)
        {
            GameObject hatchling = DragonPoolManager.instance.GetHatchlingInPool();
            hatchling.transform.SetParent(null); //매니저 밖으로 나옴
            hatchling.SetActive(true); // false였으니 true로 변환
           
            hatchling.transform.position = Vector2.Lerp(hatchling.transform.position,
            this.hatchlingInintPoses[i].position, 1.0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(this.state == eState.Idle) //Idle 상태일때만 시간 계산
        {
            this.elapsedTime += Time.deltaTime;
            
        }
        else if(this.state == eState.Attack)
        {
            this.anim.SetTrigger("Attack");
        }
        else if(this.state == eState.Summon)
        {
            this.anim.SetTrigger("Summon");
        }

        if(this.elapsedTime > 3.0f)
        {
            this.elapsedTime = 0.0f;
            int rndState =Random.Range(1, 101); //1~2 즉, Attack이나 Summon 둘중하나
            if(rndState < 91) 
            {
                this.state = eState.Attack;
            }
            else
            {
                this.state = eState.Summon;
            }
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
        this.onHit(this.hp);
        if (this.hp <= 0)
        {
            this.Die();
        }
    }

    private void Die()
    {
        this.deadPos = this.transform.position; //죽었을 때의 좌표를 저장
        onDie(this.deadPos);
        DragonPoolManager.instance.ReleaseDragon(this.gameObject);
    }
}
