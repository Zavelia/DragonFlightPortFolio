using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum eType
    {
        Dual, Double, Magnet, Hyper,
        Coin,Ruby,Emerald,Diamond
    }
    [SerializeField] private eType type; 
    public eType Type { get { return this.type; } }

    public int Value { get => value; set => this.value = value; }
    public string ItemName { get => itemName; set => itemName = value; }

    private int value;

    private string itemName;
    // Start is called before the first frame update
    void Start()
    {
        if(this.type == eType.Coin) { this.value = 1; }
        else if(this.type == eType.Ruby) { this.value = 10; }
        else if(this.type == eType.Emerald) { this.value = 20; }
        else if(this.type == eType.Diamond) { this.value = 30; }
        this.SetItemName();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.ClampPosition(this.transform.position);
        this.SelfDestory();
    }

    private Vector2 ClampPosition(Vector2 position)
    {
        return new Vector2(Mathf.Clamp(position.x, -2.7f, 2.7f), position.y);
    }

    private void SelfDestory()
    {
        if (this.transform.position.y < -5.7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    private void SetItemName()
    {
        if(this.type == eType.Dual)
        {
            this.itemName = string.Format("듀얼샷");
        }
        else if(this.type == eType.Double)
        {
            this.itemName = string.Format("더블\n스코어");
        }
        else if(this.type == eType.Magnet)
        {
            this.itemName = string.Format("자석");
        }
        else if(this.type == eType.Hyper)
        {
            this.itemName = string.Format("하이퍼 플라이트");
        }
    }
}
