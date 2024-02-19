using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonHpBar : MonoBehaviour
{
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    private Slider hpBar;

    private GameObject hpBarGo;

    private Dragon dragon;

    private bool onehit=false;

    private void OnDisable()
    {
        this.maxHp = this.GetComponent<Dragon>().MaxHp;
        this.hpBar.value = 1;
        if (this.hpBarGo!=null)hpBarGo.SetActive(false);
    }

    private void OnEnable()
    {
        this.onehit = false;
    }
    // Start is called before the first frame update
    void Awake()
    {
        this.onehit = false;
        var canvas = GameObject.Find("Canvas");
        hpBarGo = Instantiate(hpBarPrefab, canvas.transform);
        this.hpBar = hpBarGo.GetComponent<Slider>();
        hpBarGo.SetActive(false);
        this.maxHp = this.GetComponent<Dragon>().MaxHp;

        this.dragon = this.GetComponent<Dragon>();

        this.dragon.onHit = (hp) => {
            if (onehit == false)
            {
                onehit = true;
                hpBarGo.SetActive(true);
                //Debug.Log("HpBar!");
            }
            this.currentHp = hp;
        };
    }

    // Update is called once per frame
    void Update()
    {
        this.hpBar.value = currentHp / maxHp;
        this.hpBarGo.transform.position = Camera.main.WorldToScreenPoint(
            this.transform.position - new Vector3(0, 0.7f, 0));
    }
}
