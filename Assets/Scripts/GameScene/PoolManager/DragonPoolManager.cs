using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonPoolManager : MonoBehaviour
{
    //ȭ��Ʈ �巡���� �̸� ������ ������ ����Ʈ 
    [SerializeField] private List<GameObject> whitePool = new List<GameObject>();
    //��� �巡���� �̸� ������ ������ ����Ʈ 
    [SerializeField] private List<GameObject> goldPool = new List<GameObject>();
    //���� �巡���� �̸� ������ ������ ����Ʈ 
    [SerializeField] private List<GameObject> bossPool = new List<GameObject>();
    //������ �巡���� �̸� ������ ������ ����Ʈ 
    [SerializeField] private List<GameObject> hatchlingPool = new List<GameObject>();
    //������Ʈ Ǯ�� ������ �巡���� �ִ� �� 
    [SerializeField] private int maxDragons = 10;
    [SerializeField] private int maxBossDragons = 3;
    [SerializeField] private int maxHatchling = 16;
    //�̱��� �ν��Ͻ� ����
    public static DragonPoolManager instance=null;
    //�巡�� �����յ� 
    [SerializeField] private GameObject[] dragonPrefabs; //0�� ȭ��Ʈ, 1�� ��� 
    [SerializeField] private GameObject bossDragonPrefab;
    [SerializeField] private GameObject hatchlingPrefab;

    public List<GameObject> WhitePool { get => whitePool;  }
    public List<GameObject> GoldPool { get => goldPool;  }
    public List<GameObject> BossPool { get => bossPool;  }
    public List<GameObject> HatchlingPool { get => hatchlingPool; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
        // ���� ���� ���� ����������
        // ���Ŀ� ���ο� ���� �߰��� �� �ֵ��� ��� 
        DontDestroyOnLoad(gameObject);
        this.CreateDragonPool();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void CreateDragonPool()
    {
        for(int i=0; i<maxDragons; i++)
        {
            GameObject whiteGo = Instantiate(this.dragonPrefabs[0]);
            whiteGo.SetActive(false);
            whiteGo.transform.SetParent(this.transform);
            whitePool.Add(whiteGo);

            GameObject goldGo = Instantiate(this.dragonPrefabs[1]);
            goldGo.SetActive(false);
            goldGo.transform.SetParent(this.transform);
            goldPool.Add(goldGo);
        }

        for(int i=0; i<maxBossDragons; i++)
        {
            GameObject bossGo = Instantiate(this.bossDragonPrefab);
            bossGo.SetActive(false);
            bossGo.transform.SetParent (this.transform);
            bossPool.Add(bossGo);
        }

        for(int i=0; i<maxHatchling; i++)
        {
            GameObject hatchlingGo = Instantiate(this.hatchlingPrefab);
            hatchlingGo.SetActive(false);
            hatchlingGo.transform.SetParent(this.transform);
            hatchlingPool.Add(hatchlingGo);
        }
    }
    //Ǯ���� �巡�� �޾ư��� 
    public GameObject GetWhiteDragonInPool()
    {
        foreach(var white in this.whitePool)
        {
            if(white.activeSelf == false)
            {
                return white;
            }
        }
        return null;
    }

    public GameObject GetGoldDragonInPool()
    {
        foreach (var gold in this.goldPool)
        {
            if (gold.activeSelf == false)
            {
                return gold;
            }
        }
        return null;
    }

    public GameObject GetBossDragonInPool()
    {
        foreach (var boss in this.bossPool)
        {
            if (boss.activeSelf == false)
            {
                return boss;
            }
        }
        return null;
    }

    public GameObject GetHatchlingInPool()
    {
        foreach(var hatchling in this.hatchlingPool)
        {
            if(hatchling.activeSelf == false)
            {
                return hatchling;
            }
        }
        return null;
    }
    //���ƿ��� 
    public void ReleaseDragon(GameObject dragonGo)
    {
        dragonGo.SetActive(false);
        dragonGo.transform.SetParent(this.transform);
    }
}
