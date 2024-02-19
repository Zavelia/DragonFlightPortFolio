using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] private Transform bulletInitPos; // 총알 생성 위치
    [SerializeField] private Transform [] dualBulletInitPoses; // 총알 생성 위치

    private float elapsedTime = 0.0f; //경과시간
    [SerializeField] private float intervelTime = 0.3f;
    [SerializeField] private GameObject bulletPrefab;
    
    public void Shoot(bool isDouble)
    {
        this.elapsedTime += Time.deltaTime;
        if(this.elapsedTime > intervelTime)
        {
            this.CallBullet(isDouble);
            this.elapsedTime = 0.0f;
        }
    }

    private void CallBullet(bool isDouble)
    {
        
        if (isDouble == false)
        {
            GameObject go = BulletPoolManager.instance.GetBulletInPool();
            if (go == null) //이미 만들어둔 총알을 다 사용하면
            {
                go = Instantiate(this.bulletPrefab);
                BulletPoolManager.instance.BulletPool.Add(go);
                go.transform.position = this.bulletInitPos.position;
                Debug.Log("<color=cyan>총알 동적 생성</color>");
            }
            else if (go != null) //이미 만들어둔 총알이라면
            {
                go.transform.SetParent(null); //매니저 밖으로 나옴
                go.transform.position = this.bulletInitPos.position;
                go.SetActive(true); // false였으니 true로 변환
            }
        }
        else if(isDouble ==true)
        {
            for(int i=0;i<2;i++)
            {
                GameObject go = BulletPoolManager.instance.GetBulletInPool();
                if (go == null) //이미 만들어둔 총알을 다 사용하면
                {
                    go = Instantiate(this.bulletPrefab);
                    BulletPoolManager.instance.BulletPool.Add(go);
                    go.transform.position = this.dualBulletInitPoses[i].position;
                    Debug.Log("<color=cyan>총알 동적 생성</color>");
                }
                else if (go != null) //이미 만들어둔 총알이라면
                {
                    go.transform.SetParent(null); //매니저 밖으로 나옴
                    go.transform.position = this.dualBulletInitPoses[i].position;
                    go.SetActive(true); // false였으니 true로 변환
                }
            }
        }

        
    }
}
