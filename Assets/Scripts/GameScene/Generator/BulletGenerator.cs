using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] private Transform bulletInitPos; // �Ѿ� ���� ��ġ
    [SerializeField] private Transform [] dualBulletInitPoses; // �Ѿ� ���� ��ġ

    private float elapsedTime = 0.0f; //����ð�
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
            if (go == null) //�̹� ������ �Ѿ��� �� ����ϸ�
            {
                go = Instantiate(this.bulletPrefab);
                BulletPoolManager.instance.BulletPool.Add(go);
                go.transform.position = this.bulletInitPos.position;
                Debug.Log("<color=cyan>�Ѿ� ���� ����</color>");
            }
            else if (go != null) //�̹� ������ �Ѿ��̶��
            {
                go.transform.SetParent(null); //�Ŵ��� ������ ����
                go.transform.position = this.bulletInitPos.position;
                go.SetActive(true); // false������ true�� ��ȯ
            }
        }
        else if(isDouble ==true)
        {
            for(int i=0;i<2;i++)
            {
                GameObject go = BulletPoolManager.instance.GetBulletInPool();
                if (go == null) //�̹� ������ �Ѿ��� �� ����ϸ�
                {
                    go = Instantiate(this.bulletPrefab);
                    BulletPoolManager.instance.BulletPool.Add(go);
                    go.transform.position = this.dualBulletInitPoses[i].position;
                    Debug.Log("<color=cyan>�Ѿ� ���� ����</color>");
                }
                else if (go != null) //�̹� ������ �Ѿ��̶��
                {
                    go.transform.SetParent(null); //�Ŵ��� ������ ����
                    go.transform.position = this.dualBulletInitPoses[i].position;
                    go.SetActive(true); // false������ true�� ��ȯ
                }
            }
        }

        
    }
}
