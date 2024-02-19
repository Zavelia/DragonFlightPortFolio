using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoinMagnet : MonoBehaviour
{
    [SerializeField]
    private float magnetStrength = 5f;
    [SerializeField] private float distanceStretch = 10f;
    [SerializeField] private int magnetDirection = -1;
    [SerializeField] bool looseMagnet=true;

    private Rigidbody2D rb;
    private Transform magnetTrans;
    private bool isMagnetInZone;
    // Start is called before the first frame update
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.isMagnetInZone)
        {
            Vector2 directionToMagnet = this.magnetTrans.position - this.transform.position;
            float distance = Vector2.Distance(magnetTrans.position,this.transform.position);
            float magnetDistanceStr = (distanceStretch / distance) * magnetStrength;
            this.rb.AddForce(magnetDistanceStr * (directionToMagnet * magnetDirection), ForceMode2D.Force);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Magnet"))
        {
            magnetTrans = collision.transform;
            this.isMagnetInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Magnet") && looseMagnet)
        {
            isMagnetInZone = false;
        }
    }
}
