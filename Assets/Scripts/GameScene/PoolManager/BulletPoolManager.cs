using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    //총알을 미리 생성해 저장할 리스트
    [SerializeField]
    private List<GameObject> bulletPool = new List<GameObject>();
    //오브젝트 풀에 생성할 총알의 최대 개수 
    [SerializeField] private int maxBullets = 10;
    //싱글톤 인스턴스 선언 
    public static BulletPoolManager instance = null;
    //총알 프리팹
    [SerializeField] private GameObject bulletPrefab;

    #region 속성
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
        // 아직 게임 씬만 구성하지만
        // 추후에 새로운 씬도 추가할 수 있도록 사용 
        DontDestroyOnLoad(gameObject); 
    }
    private void Start()
    {
        this.CreateBulletPool();
    }
    //실행시 총알 만들기
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
    //풀에서 총알 받아가기 
    public GameObject GetBulletInPool()
    {
        foreach(GameObject bullet in this.bulletPool)
        {
            //활성화 상태가 아니면 받아옴
            if (bullet.activeSelf == false)
            {
                return bullet;
            }
        }
        return null;
    }

    //돌아오기 
    public void ReleaseBullet(GameObject bulletGo)
    {
        bulletGo.SetActive(false);
        bulletGo.transform.SetParent(this.transform);
    }
}
