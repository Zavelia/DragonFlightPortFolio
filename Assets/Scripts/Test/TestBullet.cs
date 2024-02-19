using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector2.up * this.moveSpeed * Time.deltaTime);
        this.SelfComeback();
    }

    private void SelfComeback()
    {
        if (this.transform.position.y > 5.2f)
        {
            this.transform.position = new Vector3(this.transform.position.x, -3.2f, this.transform.position.z);
        }
    }
}
