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
        //Time.time�� �������� ������ ���������� �ð�
        //Repeat �տ� �μ��� ����ؼ� �����ϰ�, posValue���� Ŀ���� ����
        //Time.time * scrollSpeed = Time.time * scrollSpeed- posValue �� �ȴ�. 
        //�� newPos�� Ư������ ���ѵ� ���·� �ݺ��� �ȴ�. 
        //ex) posValue�� 10�̸�, ���⼭�� 0~10���� �ݺ��ȴ�. 
        this.newPos=Mathf.Repeat(Time.time * scrollSpeed, posValue);
        //�Ʒ��� �̵��ϴٰ�, �ٽ� ������ġ�� ���ƿͼ� �ٽ� ��������. 
        transform.position = startPos + Vector3.down * this.newPos;
       
    }

   
}
