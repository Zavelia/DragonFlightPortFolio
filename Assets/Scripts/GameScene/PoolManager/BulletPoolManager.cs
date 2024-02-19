using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    //�Ѿ��� �̸� ������ ������ ����Ʈ
    [SerializeField]
    private List<GameObject> bulletPool = new List<GameObject>();
    //������Ʈ Ǯ�� ������ �Ѿ��� �ִ� ���� 
    [SerializeField] private int maxBullets = 10;
    //�̱��� �ν��Ͻ� ���� 
    public static BulletPoolManager instance = null;
    //�Ѿ� ������
    [SerializeField] private GameObject bulletPrefab;

    #region �Ӽ�
    public List<GameObject> BulletPool { get => bulletPool; }
    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this.gameObject);
        }
        // ���� ���� ���� ����������
        // ���Ŀ� ���ο� ���� �߰��� �� �ֵ��� ��� 
        DontDestroyOnLoad(gameObject); 
    }
    private void Start()
    {
        this.CreateBulletPool();
    }
    //����� �Ѿ� �����
    private void CreateBulletPool()
    {
        for(int i=0; i<maxBullets; i++)
        {
            GameObject bulletGo = Instantiate(this.bulletPrefab);
            bulletGo.SetActive(false);
            bulletGo.transform.SetParent(this.transform);
            bulletPool.Add(bulletGo);

        }
    }
    //Ǯ���� �Ѿ� �޾ư��� 
    public GameObject GetBulletInPool()
    {
        foreach(GameObject bullet in this.bulletPool)
        {
            //Ȱ��ȭ ���°� �ƴϸ� �޾ƿ�
            if (bullet.activeSelf == false)
            {
                return bullet;
            }
        }
        return null;
    }

    //���ƿ��� 
    public void ReleaseBullet(GameObject bulletGo)
    {
        bulletGo.SetActive(false);
        bulletGo.transform.SetParent(this.transform);
    }
}
