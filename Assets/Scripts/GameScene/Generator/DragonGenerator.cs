using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGenerator : MonoBehaviour
{

    [SerializeField] private Transform[] dragonInitPoses; //�巡�� ���� ��ġ
    [SerializeField] private Transform bossDragonInitPos;

    private float elapsedTime = 0.0f; //����ð�
    [SerializeField] private float intervelTime = 0.3f;
    [SerializeField] private GameObject[] dragonPrefabs; //0�� ȭ��Ʈ, 1�� ��� 
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
        if (go == null) //�̹� ������ �巡���� �� ����ϸ� 
        {
            Debug.LogError("���ο� �巡�� ����!");
            go = Instantiate(bossDragonPrefab);
            DragonPoolManager.instance.BossPool.Add(go);
            go.transform.position = bossDragonInitPos.position;
        }
        else if (go != null)
        {
            go.transform.SetParent(null); //�Ŵ��� ������ ����
            go.transform.position = bossDragonInitPos.position;
            go.SetActive(true); // false������ true�� ��ȯ
        }
    }
    private void CallDragon()
    {
        //5�� �θ� 
        for(int i=0; i<dragonInitPoses.Length; i++)
        {
            GameObject go;
            int rnd = Random.Range(1, 11);
            if (rnd > 7)
            {
                go = DragonPoolManager.instance.GetGoldDragonInPool();
                if (go == null) //�̹� ������ �巡���� �� ����ϸ� 
                {
                    Debug.LogError("���ο� �巡�� ����!");
                    go = Instantiate(this.dragonPrefabs[1]);
                    DragonPoolManager.instance.GoldPool.Add(go);
                    go.transform.position = dragonInitPoses[i].position;
                }
                else if (go != null)
                {
                    go.transform.SetParent(null); //�Ŵ��� ������ ����
                    go.transform.position = dragonInitPoses[i].position;
                    go.SetActive(true); // false������ true�� ��ȯ
                }
            }
            else
            {
                go = DragonPoolManager.instance.GetWhiteDragonInPool();
                if (go == null) //�̹� ������ �巡���� �� ����ϸ� 
                {
                    Debug.LogError("���ο� �巡�� ����!");
                    go = Instantiate(this.dragonPrefabs[0]);
                    DragonPoolManager.instance.WhitePool.Add(go);
                    go.transform.position = dragonInitPoses[i].position;
                }
                else if (go != null)
                {
                    go.transform.SetParent(null); //�Ŵ��� ������ ����
                    go.transform.position = dragonInitPoses[i].position;
                    go.SetActive(true); // false������ true�� ��ȯ
                }
            }
            
        }
    }
}
