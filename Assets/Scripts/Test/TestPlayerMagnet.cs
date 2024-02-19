using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMagnet : MonoBehaviour
{
    private float magnetElapsedTime = 0.0f;
    [SerializeField] private bool stateMagnet = false;
    [SerializeField] private float radius = 1.0f;
    [SerializeField] private GameObject magnet;

    [SerializeField]
    private float magnetStrength = 5f;
    [SerializeField] private float distanceStretch = 10f;
    [SerializeField] private int magnetDirection = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.OnMagnetMode();
    }

    private void OnMagnetMode()
    {
        if (this.stateMagnet)
        {
            int layerMask = 1 << LayerMask.NameToLayer("Item");
            Collider2D[] colls = Physics2D.OverlapCircleAll(this.transform.position, this.radius, layerMask);
            if (colls.Length > 0)
            {
                Debug.LogError("검출완료!");
            }
            foreach (Collider2D coll in colls)
            {
                var rb = coll.gameObject.GetComponent<Rigidbody2D>();
                var itemDrop=coll.gameObject.GetComponent<ItemDrop>();
                itemDrop.enabled = false;
                Vector2 directionToMagnet = this.transform.position - coll.transform.position;
                float distance = Vector2.Distance(this.transform.position, coll.transform.position);
                float magnetDistanceStr = (distanceStretch / distance) * magnetStrength;
                rb.AddForce(magnetDistanceStr * (directionToMagnet * magnetDirection), ForceMode2D.Force);
            }
            this.magnet.SetActive(true);
        }
        else this.magnet.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.radius);
    }
}
