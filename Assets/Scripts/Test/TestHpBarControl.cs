using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHpBarControl : MonoBehaviour
{
    [SerializeField] private List<Transform> obj;
    [SerializeField] private List<GameObject> hp_bar;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<obj.Count; i++)
        {
            this.hp_bar[i].transform.position = obj[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<obj.Count; i++)
        {
            this.hp_bar[i].transform.position = Camera.main.WorldToScreenPoint
                (obj[i].transform.position - new Vector3(0,0.5f,0));
        }
    }
}
