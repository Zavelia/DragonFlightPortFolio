using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    private System.Action onBoss;
    private enum eCoin
    {
        Coin,Ruby,Emerald,Diamond
    }
    private enum eSpecial
    {
        Dual,Double,Magnet,Hyper
    }

    #region Object
    [SerializeField] private Player player; //플레이어
    [SerializeField] private List<Dragon> dragons = new List<Dragon>(); //드래곤들
    [SerializeField] private List<Meteo> meteos = new List<Meteo>();
    [SerializeField] private BulletGenerator bulletGenerator; //총알 생성기 
    [SerializeField] private DragonGenerator dragonGenerator; //드래곤 생성기 
    [SerializeField] private MeteoGenerator meteoGenerator; //메테오 생성기
    [SerializeField] private BackGroundScrolling backgorund; // 배경 
    [SerializeField] private BossDragon bossDragon;
    [SerializeField] private List<Hatchling> hatchlings = new List<Hatchling>();
    public List<Dragon> Dragons { get => dragons; set => dragons = value; }
    public BossDragon BossDragon { get => bossDragon; set => bossDragon = value; }
    public List<Meteo> Meteos { get => meteos; set => meteos = value; }
    public List<Hatchling> Hatchlings { get => hatchlings; set => hatchlings = value; }
    #endregion
    #region Prefabs
    [SerializeField] private GameObject[] dustPrefabs;
    [SerializeField] private GameObject[] coinItemPrefabs;
    [SerializeField] private GameObject[] specialItemPrefabs;
    [SerializeField] private GameObject shockWavePrefab;
    [SerializeField] private GameObject getSpecialItemEffectPrefab;
    [SerializeField] private GameObject meteoPiecesPrefab;
    // 0 gold, 1 ruby, 2 emerald ,3 dia
    [SerializeField] private GameObject[] getItemEffects;
    #endregion
    #region UI
    [SerializeField] private TMP_Text txtHuntPoint;
    [SerializeField] private TMP_Text txtGoldPoint;
    [SerializeField] private TMP_Text txtFlyPoint;
    #endregion
    #region State
    private bool stateHyper = false;
    private bool stateDouble = false;
    private bool stateDual = false;
    private bool stateMagnet = false;
    private bool stateAliveBossDragon = false;
    private bool statePlayerDie = false;
    #endregion
    #region Point
    private int huntPoint = 0;
    private int goldPoint = 0;
    private int flyPoint = 0;
    private int bossSummonPoint = 0;
    #endregion
    #region ElapsedTime
    private float hyperElapsedTime = 0.0f;
    private float doubleElapsedTime = 0.0f;
    private float dualElapsedTime = 0.0f;
    private float magnetElapsedTime = 0.0f;
    #endregion
    #region Magnet
    [SerializeField] private float radius = 3.0f;
    [SerializeField] private float magnetStrength = 5f;
    #endregion
    #region Mark
    [SerializeField] private GameObject booster; //부스터
    [SerializeField] private GameObject doubleScore; //스코어 표시
    [SerializeField] private GameObject magnet; //자석 먹은거 표시
    #endregion
    #region Audio
    [SerializeField] private AudioSource getItemAudio;
    [SerializeField] private AudioSource getHyperAudio;
    [SerializeField] private AudioSource getCoinAudio;
    [SerializeField] private AudioSource getGemAudio;
    [SerializeField] private AudioSource dieAudio;
    [SerializeField] private AudioSource bossDieAudio;
    [SerializeField] private AudioSource bossAlertAudio;
    #endregion
    #region UI
    [SerializeField] private SpriteAtlas atlas;
    [SerializeField] private Image[] heartImages;
    #endregion
    [SerializeField] private Transform coinItem;
    [SerializeField] private Transform specialItem;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject bossAlert;
    private float huntingTime = 0.0f; //보스 잡는데 까지 걸리는 시간 
    private int playerHitCount = 0; //총알 맞은 회수 체크
    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameMain 실행");
        this.DragonsDie();
        this.PlayerGetItem();
        this.PlayerDie();
        this.PlayerHit();
        this.BossDie();
        this.HatchlingDie();
        
        var bossAlertReceiver = this.bossAlert.GetComponent<BossAlertAnimationReceiver>();
        bossAlertReceiver.onFinished = () => {
                this.dragonGenerator.SummonBossDragon();
        };
    }
    
    // Update is called once per frame
    void Update()
    {
        this.MeteoBreak();
        if (this.statePlayerDie == false)
        {
            //총알 발사
            if (this.stateHyper == false)
            {
                this.bulletGenerator.Shoot(this.stateDual);
                //this.mainCamera.GetComponent<Animator>().SetTrigger("Default");
            }
            else
            {
                //this.mainCamera.GetComponent<Animator>().SetTrigger("PlayerDie");
            }
            // 용 소환 
            if (this.bossSummonPoint > 30000)
            {
                this.bossAlert.SetActive(true);
                this.bossAlertAudio.Play();
                this.bossSummonPoint = 0;
                this.stateAliveBossDragon = true;
            }

            if (this.stateAliveBossDragon == false)
            {
                this.dragonGenerator.SummonDragon();
                this.meteoGenerator.SummonMeteo();
            }
            else //보스가 생존하고 있으면 
            {
                this.huntingTime += Time.deltaTime;
                this.bossDragon.transform.position = Vector2.Lerp(bossDragon.transform.position,
                   new Vector2(0f, 3.03f), Time.deltaTime*2.0f);

            }

            this.txtHuntPoint.text = this.huntPoint.ToString();
            this.txtGoldPoint.text = this.goldPoint.ToString();

            this.OnHyperMode();
            this.OnDoubleMode();
            this.OnDualMode();
            this.OnMagnetMode();
            this.txtFlyPoint.text = this.flyPoint.ToString();
        }
    }

    //private IEnumerator CoPlayDieAudio()
    //{
    //    dieAudios[index].SetActive(true);
    //    yield return new WaitForSeconds(1.0f);
    //    dieAudios[index].SetActive(false);
    //    yield return null;
    //    index++;
    //    if(index >= dieAudios.Length)
    //    {
    //        index = 0;
    //    }
    //}
    #region CreateMethod
    //랜덤으로 사망 애니메이션 생성 
    private void CreateDust(Vector3 pos)
    {
        int dustRnd = Random.Range(0, 3); //0~2
        GameObject dust = Instantiate(this.dustPrefabs[dustRnd]);
        dust.transform.position = pos;
        Destroy(dust, 1.0f); //1초후에 삭제 
    }

    private void CreateShockWave(Vector3 pos)
    {
        GameObject shockWave = Instantiate(this.shockWavePrefab);
        shockWave.transform.position = pos;
        Destroy(shockWave, 1.0f);
    }
    //랜덤으로 코인 아이템 생성
    private void CreateCoinItem(Vector3 pos)
    {
        int coinRnd = Random.Range(1, 101); // 1~100
        if(coinRnd >=1 && coinRnd <91) // 1~90까지, 90% 
        {
            int coinInt = (int)eCoin.Coin;
            GameObject coin = Instantiate(this.coinItemPrefabs[coinInt],this.coinItem); //0이 코인 
            coin.transform.position = pos;
        }
        else if(coinRnd >=91 && coinRnd <95) // 91~94 4%
        {
            int rubyInt = (int)eCoin.Ruby;
            GameObject ruby = Instantiate(this.coinItemPrefabs[rubyInt], this.coinItem);
            ruby.transform.position = pos;
        }
        else if(coinRnd >= 95 && coinRnd< 99) //95~98 4%
        {
            int emeraldInt = (int)eCoin.Emerald;
            GameObject emerald = Instantiate(this.coinItemPrefabs[emeraldInt], this.coinItem);
            emerald.transform.position = pos;
        }
        else if(coinRnd == 100) //100 1%
        {
            int diaInt = (int)eCoin.Diamond;
            GameObject diamond = Instantiate(this.coinItemPrefabs[diaInt], this.coinItem);
            diamond.transform.position = pos;
        }
    }
    //랜덤으로 효과 아이템 생성
    private void CreateSpecialItem(Vector3 pos)
    {
        int specialRnd = Random.Range(1, 11); // 1~10
        if (specialRnd >= 1 && specialRnd < 5) // 1~4까지, 40% 
        {
            int dualInt = (int)eSpecial.Dual;
            GameObject dualShot = Instantiate(this.specialItemPrefabs[dualInt], this.specialItem); //0이 코인 
            dualShot.transform.position = pos;
        }
        else if (specialRnd >= 5 && specialRnd < 8) //5~7 30%
        {
            int doubleInt = (int)eSpecial.Double;
            GameObject doubleScore = Instantiate(this.specialItemPrefabs[doubleInt], this.specialItem);
            doubleScore.transform.position = pos;
        }
        else if (specialRnd >= 8 && specialRnd < 10) //8~9 20%
        {
            int magnetInt = (int)eSpecial.Magnet;
            GameObject magnet = Instantiate(this.specialItemPrefabs[magnetInt], this.specialItem);
            magnet.transform.position = pos;
        }
        else if (specialRnd == 10) //10 10%
        {
            int hyperInt = (int)eSpecial.Hyper;
            GameObject hyper = Instantiate(this.specialItemPrefabs[hyperInt], this.specialItem);
            hyper.transform.position = pos;
        }
    }
    private void BossReward(Vector3 deadPos)
    {
        for (int i = 0; i < 30; i++)
        {
            this.CreateCoinItem(deadPos);
        }

        for (int i = 0; i < 5; i++)
        {
            this.CreateSpecialItem(deadPos);
        }
    }
    #endregion
    #region CheckMode
    //하이퍼 모드 판단
    private void OnHyperMode()
    {
        if (this.stateHyper)
        {
            this.hyperElapsedTime += Time.deltaTime;
            this.backgorund.ScrollSpeed = 10f;
            this.flyPoint += 15; //비행 점수 오르는 속도 증가
            foreach (var dragon in dragons)
            {
                dragon.MoveSpeed = 25f;
            }
            foreach(var meteo in meteos)
            {
                meteo.MoveSpeed = 35f;
            }
            this.booster.SetActive(true);
        }
        else
        {
            this.backgorund.ScrollSpeed = 2f;
            this.flyPoint += 3;
            foreach (var dragon in dragons)
            {
                dragon.MoveSpeed = 5f;
            }
            foreach (var meteo in meteos)
            {
                meteo.MoveSpeed = 7f;
            }
            this.booster.SetActive(false);
        }

        if (this.hyperElapsedTime > 3.0f) //여기서 충격파 생성
        {
            this.hyperElapsedTime = 0;
            this.stateHyper = false;
            Vector3 shockInitPos = this.player.transform.position;
            this.CreateShockWave(shockInitPos);
        }
    }
    //점수 더블 판단
    private void OnDoubleMode()
    {
        if (this.stateDouble)
        {
            this.doubleElapsedTime += Time.deltaTime;
            this.doubleScore.SetActive(true);
        }
        else this.doubleScore.SetActive(false);

        if (this.doubleElapsedTime > 10f)
        {
            this.doubleElapsedTime = 0;
            this.stateDouble = false;
        }
    }
    //총알 듀얼 모드 판단
    private void OnDualMode()
    {
        if (this.stateDual)
        {
            this.dualElapsedTime += Time.deltaTime;
        }

        if(this.dualElapsedTime > 10f)
        {
            this.dualElapsedTime = 0;
            this.stateDual = false;
        }
    }
    
    private void OnMagnetMode()
    {
        if (this.stateMagnet)
        {
            this.magnetElapsedTime += Time.deltaTime;
            int layerMask = 1 << LayerMask.NameToLayer("Item");
            Collider2D[] colls = Physics2D.OverlapCircleAll(this.player.transform.position, this.radius, layerMask);
            foreach (Collider2D coll in colls)
            {
                var itemDrop = coll.gameObject.GetComponent<ItemDrop>();
                itemDrop.enabled = false; //켜져 있으면, 자석기능이 안됨
                coll.gameObject.transform.position = Vector2.Lerp(coll.gameObject.transform.position,
                    this.player.transform.position, Time.deltaTime * magnetStrength);
            }
            this.magnet.SetActive(true);
        }
        else this.magnet.SetActive(false);

        if (this.magnetElapsedTime > 10f)
        {
            this.magnetElapsedTime = 0;
            this.stateMagnet=false;
        }
    }
    #endregion
    //사냥 점수 계산
    private void PlustHuntPoint(Dragon.eDragonType type)
    {
        if (type == Dragon.eDragonType.White)
        {
            if (stateDouble)
            {
                this.huntPoint += 100;
                this.bossSummonPoint += 100;
            }
            else
            {
                this.huntPoint += 50;
                this.bossSummonPoint += 50;
            }

        }
        else if (type == Dragon.eDragonType.Gold)
        {
            if (stateDouble)
            {
                this.huntPoint += 200;
                this.bossSummonPoint += 200;
            }
            else 
            {
                this.huntPoint += 100;
                this.bossSummonPoint += 100;
            }

        }
        else if(type == Dragon.eDragonType.Boss)
        {
            if (stateDouble)
            {
                this.bossSummonPoint += 6000;
                this.huntPoint += 6000;
            }
            else
            {
                this.bossSummonPoint += 3000;
                this.huntPoint += 3000;
            }
        }
        else if(type == Dragon.eDragonType.Hatchling)
        {
            if (stateDouble)
            {
                this.bossSummonPoint += 4;
                this.huntPoint += 4;
            }
            else
            {
                this.bossSummonPoint += 2;
                this.huntPoint += 2;
            }
        }
    }
    private void StateChangeByType(Item.eType type)
    {
        if (type == Item.eType.Hyper)
        {
            if (this.stateHyper)
            {
                this.hyperElapsedTime -= 0.5f; //지속시간 연장
                this.getHyperAudio.Play();
            }
            else
            {
                this.stateHyper = true;
                this.getHyperAudio.Play();
            }
        }
        else if (type == Item.eType.Double)
        {
            if (this.stateDouble)
            {
                this.doubleElapsedTime -= 3f;
                this.getItemAudio.Play();
            }
            else
            {
                this.stateDouble = true;
                this.getItemAudio.Play();
            }
        }
        else if (type == Item.eType.Dual)
        {
            if (this.stateDual)
            {
                this.dualElapsedTime -= 3f;
                this.getItemAudio.Play();
            }
            else
            {
                this.stateDual = true;
                this.getItemAudio.Play();
            }
        }
        else if (type == Item.eType.Magnet)
        {
            if (this.stateMagnet)
            {
                this.magnetElapsedTime -= 3f;
                this.getItemAudio.Play();
            }
            else
            {
                this.stateMagnet = true;
                this.getItemAudio.Play();
            }
        }
        else if (type == Item.eType.Coin)
        {
            this.getCoinAudio.Play();
        }
        else if(type == Item.eType.Ruby || type == Item.eType.Emerald || type == Item.eType.Diamond)
        {
            this.getGemAudio.Play();
        }
    }
    private void PlayItemEffect(Item.eType type, Vector3 pos)
    {
        if (type == Item.eType.Coin)
        {
            //따라다니게 하기 위해 생성후, 플레이어 자식으로 만듬
            var effect = Instantiate(getItemEffects[0], this.player.transform);
            effect.transform.position = pos;
            Destroy(effect, 1.0f);
        }

        else if (type == Item.eType.Ruby)
        {
            //따라다니게 하기 위해 생성후, 플레이어 자식으로 만듬
            var effect = Instantiate(getItemEffects[1], this.player.transform);
            effect.transform.position = pos;
            Destroy(effect, 1.0f);
        }

        else if (type == Item.eType.Emerald)
        {
            //따라다니게 하기 위해 생성후, 플레이어 자식으로 만듬
            var effect = Instantiate(getItemEffects[2], this.player.transform);
            effect.transform.position = pos;
            Destroy(effect, 1.0f);
        }

        else if (type == Item.eType.Diamond)
        {
            //따라다니게 하기 위해 생성후, 플레이어 자식으로 만듬
            var effect = Instantiate(getItemEffects[3], this.player.transform);
            effect.transform.position = pos;
            Destroy(effect, 1.0f);
        }
    }
    private void PlaySpecialItemEffect(string itemName,Item.eType type, Vector3 pos)
    {
        if(type == Item.eType.Double || type == Item.eType.Hyper || type==Item.eType.Dual || type == Item.eType.Magnet)
        {
            var Canvas = GameObject.Find("Canvas");
            var specialEffect = Instantiate(this.getSpecialItemEffectPrefab,Canvas.transform);
            specialEffect.transform.position = Camera.main.WorldToScreenPoint(pos);
            TMP_Text text = specialEffect.GetComponentInChildren<TMP_Text>();
            text.text = itemName;
            Destroy(specialEffect, 1.0f);
        }
        
    }
    #region DelegateEvent

    private void MeteoBreak()
    {
        foreach(var meteo in meteos)
        {
            meteo.onBreak = (pos) =>
            {
                GameObject meteoPiecesGo = Instantiate(this.meteoPiecesPrefab);
                meteoPiecesGo.transform.position = pos;
                Destroy(meteoPiecesGo, 1.3f);
            };
        }
    }
    private void DragonsDie()
    {
        foreach (var dragon in dragons)
        {
            dragon.onDie = (deadPos) => {
                this.PlustHuntPoint(dragon.DragonType);
                this.CreateDust(deadPos);
                this.CreateCoinItem(deadPos);
                int rnd = Random.Range(1, 101);  //1~100
                if (rnd > 95) //5프로 
                {
                    this.CreateSpecialItem(deadPos);
                }
            };
        }
    }
    private void PlayerGetItem()
    {
        this.player.onGetItem = (value, type, getPos, itemName) => {
            this.goldPoint += value;
            this.StateChangeByType(type);
            this.PlayItemEffect(type, getPos);
            this.PlaySpecialItemEffect(itemName, type, getPos);
        };
    }
    private void PlayerDie()
    {
        this.player.onDie = () => {
            //Debug.LogError("<color=red>플레이어 사망!!!</color>");
            this.statePlayerDie = true;
            this.backgorund.ScrollSpeed = 0;
            this.mainCamera.GetComponent<Animator>().SetTrigger("PlayerDie");
            int sumPoint = this.flyPoint + this.huntPoint + 1; //update 때문에 1 오차 발생
            int goldPoint = this.goldPoint;
            Debug.LogFormat("<color=yellow>점수:{0} 골드:{1}</color>", sumPoint, goldPoint);
        };
    }

    private void PlayerHit()
    {
        this.player.onHit = () =>
        {
            if (this.playerHitCount == 0)
            {
                this.playerHitCount++;
                heartImages[1].sprite = this.atlas.GetSprite("hp_heart_02");
            }
            else if (this.playerHitCount == 1)
            {
                heartImages[0].sprite = this.atlas.GetSprite("hp_heart_02");
            }
        };
    }

    private void BossDie()
    {
        this.bossDragon.onDie = (deadPos) => {
            this.PlustHuntPoint(this.bossDragon.DragonType);
            this.BossReward(deadPos);
            this.stateAliveBossDragon = false;
            this.bossAlert.SetActive(false);
            Debug.LogFormat("<color=cyan>보스 잡는데 걸린 시간:{0}</color>", this.huntingTime);
            //Debug.LogError("보스 잡음!");
            bossDieAudio.Play();
            foreach (var hatchling in hatchlings)
            {
                hatchling.BossDie();
            }
            this.huntingTime = 0;
            //아직 사망 이펙트 없음
        };
    }
    private void HatchlingDie()
    {
        foreach (var hatchling in hatchlings)
        {
            hatchling.onDie = (deadPos) => {
                this.PlustHuntPoint(hatchling.DragonType);
                this.CreateDust(deadPos);
                this.dieAudio.Play();
            };
        }
    }
    #endregion
}
