using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMagnet : MonoBehaviour
{
    [SerializeField] private Vector2 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.Lerp(this.transform.position,
            newPosition, Time.deltaTime * 1.5f);
        if (Mathf.Abs(this.newPosition.y - this.transform.position.y) < 0.05f)
        {
            this.transform.position = newPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
        }
    }
}
