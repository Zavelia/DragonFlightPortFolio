using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScrolling : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private float endPosY; //전환되는 좌표
    Vector3 startPos;

    public float ScrollSpeed { get => scrollSpeed; set => scrollSpeed = value; }

    // Start is called before the first frame update
    void Start()
    {
        this.startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector2.down*Time.deltaTime*this.scrollSpeed);
        this.SelfComeback();
    }

    private void SelfComeback()
    {
        if(this.transform.position.y < endPosY)
        {
            this.transform.position = this.startPos;
        }
    }
}
