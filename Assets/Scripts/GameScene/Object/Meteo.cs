using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : MonoBehaviour
{
    public System.Action <Vector3> onBreak;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private GameObject meteoAlert;
    private bool meteoMove = false;

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    private void Awake()
    {
        GameMain gameMain = GameObject.FindAnyObjectByType<GameMain>();
        gameMain.Meteos.Add(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        var receiver = meteoAlert.GetComponent<MeteoAlertAnimationReceiver>();
        receiver.onFinished = () => {
            meteoMove=true;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (meteoMove)
        {
            this.meteoAlert.SetActive(false);
            this.transform.Translate(Vector2.down * this.moveSpeed * Time.deltaTime);
        }
        this.SelfDestory();
    }

    private void SelfDestory()
    {
        if (this.transform.position.y < -6.0f)
        {
            Destroy(this.gameObject);
            GameMain gameMain = GameObject.FindAnyObjectByType<GameMain>();
            gameMain.Meteos.Remove(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Booster") ||
            collision.CompareTag("Shock"))
        {
            if(collision.tag != "Shock") this.onBreak(this.transform.position);
            Destroy(this.gameObject);
            GameMain gameMain = GameObject.FindAnyObjectByType<GameMain>();
            gameMain.Meteos.Remove(this);
        }
        
    }
}
