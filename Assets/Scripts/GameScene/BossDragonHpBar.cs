using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDragonHpBar : MonoBehaviour
{
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    private Slider hpBar;

    private GameObject hpBarGo;

    private BossDragon bossDragon;

    private bool onehit=false;
    private void OnDisable()
    {
        this.hpBar.value = 1;
        if (this.hpBarGo != null) hpBarGo.SetActive(false);
    }
    private void OnEnable()
    {
        this.maxHp = this.GetComponent<BossDragon>().MaxHp;
        this.onehit = false;
    }
    void Awake()
    {
        this.onehit = false;
        Debug.Log("BossDragonHpBar ½ÇÇà");
        var canvas = GameObject.Find("Canvas");
        hpBarGo = Instantiate(hpBarPrefab, canvas.transform);
        this.hpBar = hpBarGo.GetComponent<Slider>();
        hpBarGo.SetActive(false);
        this.maxHp = this.GetComponent<BossDragon>().MaxHp;

        this.bossDragon = this.GetComponent<BossDragon>();

        this.bossDragon.onHit = (hp) => {
            if (onehit == false)
            {
                onehit = true;
                hpBarGo.SetActive(true);
            }
            this.currentHp = hp;
        };
    }

    // Update is called once per frame
    void Update()
    {
        this.hpBar.value = currentHp / maxHp;
        this.hpBarGo.transform.position = Camera.main.WorldToScreenPoint(
            this.transform.position - new Vector3(0, 1.5f, 0));

    }
}
