using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDragonHpBar : MonoBehaviour
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.hpBar.value = currentHp/maxHp;
    }
}
