using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEyeChange : MonoBehaviour
{
    [SerializeField] private Button btnAngry;
    [SerializeField] private Button btnDefault;

    [SerializeField] private Sprite defaultLeftEye;
    [SerializeField] private Sprite defaultRightEye;

    [SerializeField] private Sprite angryLeftEye;
    [SerializeField] private Sprite angrytRightEye;

    [SerializeField] private SpriteRenderer goldLeftRender;
    [SerializeField] private SpriteRenderer goldRightRender;

    [SerializeField] private SpriteRenderer whiteLeftRender;
    [SerializeField] private SpriteRenderer whiteRightRender;

    // Start is called before the first frame update
    void Start()
    {
      
        this.btnAngry.onClick.AddListener(() => {
            Debug.Log("화남 실행");
            this.goldLeftRender.sprite = this.angryLeftEye;
            this.goldRightRender.sprite = this.angrytRightEye;

            whiteLeftRender.sprite = this.angryLeftEye;
            whiteRightRender.sprite = this.angrytRightEye;
        });
        this.btnDefault.onClick.AddListener(() =>{
            Debug.Log("보통 실행");
            this.goldLeftRender.sprite = this.defaultLeftEye;
            this.goldRightRender.sprite = this.defaultRightEye;

            whiteLeftRender.sprite = this.defaultLeftEye;
            whiteRightRender.sprite = this.defaultRightEye;
        });
    }
}
