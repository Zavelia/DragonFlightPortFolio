using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoGenerator : MonoBehaviour
{
    [SerializeField] private GameObject meteoPrefab;
    [SerializeField] private GameObject meteoAlertPrefab;
    [SerializeField] private AudioSource meteoAlertAudio;
    Vector2 meteoRandomPos;
    private float elapsedTime = 0.0f;
    [SerializeField] private float intervelTime = 0.3f;
    public void SummonMeteo()
    {
        this.elapsedTime += Time.deltaTime;
        if(this.elapsedTime > intervelTime)
        {
            this.CreateMeteo();
            this.elapsedTime = 0.0f;
        }
    }

    private void CreateMeteo()
    {
        Player player = GameObject.FindAnyObjectByType<Player>();
        //플레이어 위치에 메테오가 소환되도록함
        this.meteoRandomPos = new Vector2(player.transform.position.x, 6.2f);
        GameObject go = Instantiate(meteoPrefab);
        go.transform.position = this.meteoRandomPos;
        meteoAlertAudio.Play();
    }
}
