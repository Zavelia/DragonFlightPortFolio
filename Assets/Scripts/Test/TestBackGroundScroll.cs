using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBackGroundScroll : MonoBehaviour
{
    [SerializeField] 
    float scrollSpeed = 0.5f;
    [SerializeField] float posValue;

    Vector3 startPos;
    float newPos;
    // Start is called before the first frame update
    void Start()
    {
      this.startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Time.time은 프레임이 시작한 순간부터의 시간
        //Repeat 앞에 인수가 계속해서 증가하고, posValue보다 커지는 순간
        //Time.time * scrollSpeed = Time.time * scrollSpeed- posValue 가 된다. 
        //즉 newPos는 특정값이 제한된 상태로 반복이 된다. 
        //ex) posValue가 10이면, 여기서는 0~10까지 반복된다. 
        this.newPos=Mathf.Repeat(Time.time * scrollSpeed, posValue);
        //아래로 이동하다가, 다시 원래위치로 돌아와서 다시 내려간다. 
        transform.position = startPos + Vector3.down * this.newPos;
       
    }

   
}
