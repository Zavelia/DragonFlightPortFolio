using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGenerator : MonoBehaviour
{

    [SerializeField] private Transform[] dragonInitPoses; //드래곤 생성 위치
    [SerializeField] private Transform bossDragonInitPos;

    private float elapsedTime = 0.0f; //경과시간
    [SerializeField] private float intervelTime = 0.3f;
    [SerializeField] private GameObject[] dragonPrefabs; //0이 화이트, 1이 골드 
    [SerializeField] private GameObject bossDragonPrefab;

    public void SummonDragon()
    {
        this.elapsedTime += Time.deltaTime;
        if(this.elapsedTime > intervelTime)
        {
            this.CallDragon();
            this.elapsedTime = 0.0f;
        }
       
    }

    public void SummonBossDragon()
    {
        GameObject go = DragonPoolManager.instance.GetBossDragonInPool();
        if (go == null) //이미 만들어둔 드래곤을 다 사용하면 
        {
            Debug.LogError("새로운 드래곤 생성!");
            go = Instantiate(bossDragonPrefab);
            DragonPoolManager.instance.BossPool.Add(go);
            go.transform.position = bossDragonInitPos.position;
        }
        else if (go != null)
        {
            go.transform.SetParent(null); //매니저 밖으로 나옴
            go.transform.position = bossDragonInitPos.position;
            go.SetActive(true); // false였으니 true로 변환
        }
    }
    private void CallDragon()
    {
        //5번 부름 
        for(int i=0; i<dragonInitPoses.Length; i++)
        {
            GameObject go;
            int rnd = Random.Range(1, 11);
            if (rnd > 7)
            {
                go = DragonPoolManager.instance.GetGoldDragonInPool();
                if (go == null) //이미 만들어둔 드래곤을 다 사용하면 
                {
                    Debug.LogError("새로운 드래곤 생성!");
                    go = Instantiate(this.dragonPrefabs[1]);
                    DragonPoolManager.instance.GoldPool.Add(go);
                    go.transform.position = dragonInitPoses[i].position;
                }
                else if (go != null)
                {
                    go.transform.SetParent(null); //매니저 밖으로 나옴
                    go.transform.position = dragonInitPoses[i].position;
                    go.SetActive(true); // false였으니 true로 변환
                }
            }
            else
            {
                go = DragonPoolManager.instance.GetWhiteDragonInPool();
                if (go == null) //이미 만들어둔 드래곤을 다 사용하면 
                {
                    Debug.LogError("새로운 드래곤 생성!");
                    go = Instantiate(this.dragonPrefabs[0]);
                    DragonPoolManager.instance.WhitePool.Add(go);
                    go.transform.position = dragonInitPoses[i].position;
                }
                else if (go != null)
                {
                    go.transform.SetParent(null); //매니저 밖으로 나옴
                    go.transform.position = dragonInitPoses[i].position;
                    go.SetActive(true); // false였으니 true로 변환
                }
            }
            
        }
    }
}
