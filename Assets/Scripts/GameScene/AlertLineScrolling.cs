using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertLineScrolling : MonoBehaviour
{
    private enum eType
    {
        Left,Right
    }
    [SerializeField] private eType type;
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private float endPosY; //전환되는 좌표
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        this.startPos = this.GetComponent<RectTransform>().anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if(this.type == eType.Left)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
           rectTransform.anchoredPosition.y + scrollSpeed);
        }
        else if(this.type == eType.Right)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
           rectTransform.anchoredPosition.y - scrollSpeed);
        }
        this.SelfComeback(rectTransform);
    }

    private void SelfComeback(RectTransform rect)
    {
        if(this.type == eType.Right)
        {
            if (rect.anchoredPosition.y < endPosY)
            {
                rect.anchoredPosition = this.startPos;
            }
        }
        else if(this.type == eType.Left)
        {
            if (rect.anchoredPosition.y > endPosY)
            {
                rect.anchoredPosition = this.startPos;
            }
        }
        
    }
}
