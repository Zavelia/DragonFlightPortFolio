using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//속도가 강할수록 멀리 날아감 
//Rad -> 라디안, 여기서는 각도를 의미 

//circle-yellow가 자석 이펙트임 
public class ItemDrop : MonoBehaviour
{
    // Start is called before the first frame update

    #region SpeedParametor
    
    [SerializeField] private float floatVelo = 0f; //최초 속도 
    [SerializeField] private float gravity = 9.82f; //중력
    private double velocity = 0f; //속도 
    private float radian = 0f; //라디안 값 

    private float maxH = 0f; //최고 높이 

    [SerializeField] private float positionX;
    [SerializeField] private float positionY;

    float time = 0f;

    private double velo_X = 0f;
    private double velo_Y = 0f;

    bool SimulationStart = false;

    float fx = 0f;
    float fy = 0f;

    #endregion
    private Item.eType type;

    void Start()
    {
        this.radian = UnityEngine.Random.Range(45, 136);
        this.velocity = UnityEngine.Random.Range(2, 5); //2~4
        this.positionX = this.transform.position.x;
        this.positionY = this.transform.position.y;
        this.type = this.GetComponent<Item>().Type;
        
    }

    // Update is called once per frame
    void Update()
    {
        this.ParabolicMotion();
    }
    //포물선 운동 계산, 시간에 따라 계산하기에, Update안에서 호출
    private void ParabolicMotion()
    {
        if (this.transform.position.y < -3f)
        {
            this.SimulationStart = false;
        }
        this.time += Time.deltaTime;
        //포물선 운동 
        this.velo_X = velocity * Mathf.Cos(radian * Mathf.Deg2Rad) * time; //속도의 x 성분
        this.velo_Y = velocity * Mathf.Sin(radian * Mathf.Deg2Rad) * time; //속도의 y 성분

        //포물선 운동 식 구현 
        double xx = velo_X;
        double yy = velo_Y - (0.5 * gravity * Mathf.Pow(time, 2));

        fx = Convert.ToSingle(xx);
        fy = Convert.ToSingle(yy);

        float CurrentX = this.transform.position.x;
        float CurrentY = this.transform.position.y;

        if (CurrentY > maxH)
        {
            maxH = CurrentY;
        }

        this.transform.position = new Vector2(fx + positionX, fy + positionY);
        if(this.type == Item.eType.Coin || this.type == Item.eType.Hyper)
        {
            
        }
        else
        {
            ///회전 
            float angle = Mathf.Atan2(fy + positionY, fx + positionX) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}