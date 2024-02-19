using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCoinDrop : MonoBehaviour
{
    [SerializeField] private Button btn;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        this.btn.onClick.AddListener(() => {
            Instantiate(coinPrefab,spawnPos.position,spawnPos.rotation);
        });
    }
}
