using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public System.Action<int,Item.eType,Vector3,string> onGetItem;
    public System.Action onHit;
    public System.Action onDie;
    private Vector2 moveDir;

    private float moveSpeed;
    private PlayerInput playerInput;
    private InputActionMap mainActionMap;
    private InputAction moveAction;

    private int hp = 2;
    private float radius = 3.0f;
    private Animator anim;

    void Start()
    {
        this.anim = this.GetComponent<Animator>(); 
        var receiver = this.GetComponent<MeteoAlertAnimationReceiver>();
        receiver.onFinished = () => {
            this.gameObject.SetActive(false);
        };
        this.moveSpeed = 8.0f;
        #region player Move
        this.playerInput = this.GetComponent<PlayerInput>();
        //ActionMap ����
        this.mainActionMap = this.playerInput.actions.FindActionMap("PlayerActions");
        //Move �׼� ����
        this.moveAction = this.mainActionMap.FindAction("Move");
        //�׼ǿ� �̺�Ʈ ���
        this.moveAction.performed += (context) => {
            Vector2 dir = context.ReadValue<Vector2>();
            //Debug.Log(dir);
            this.moveDir = dir;
        };

        this.moveAction.canceled += (context) => {
            this.moveDir = Vector2.zero;
        };
        #endregion
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�����̱�
        if (this.moveDir == Vector2.left)
        {
            this.transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
        }
        else if(this.moveDir == Vector2.right)
        {
            this.transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);
        }
        //����
        this.transform.position = this.ClampPosition(this.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dragon"))
        {
            Debug.Log("<color=yellow>�÷��̾ �巡��� �浹��</color>");
            Die();
        }
        else if (collision.CompareTag("Item"))
        {
            var type = collision.GetComponent<Item>().Type;
            var value = collision.GetComponent<Item>().Value;
            var getPos = new Vector3(this.transform.position.x, this.transform.position.y + 0.8f,
                this.transform.position.z); //���� ��ġ 
            var itemName = collision.GetComponent<Item>().ItemName;
            this.onGetItem(value,type,getPos,itemName);
            //Debug.LogFormat("<color=cyan>{0} ������ ȹ��</color>", itemName);
        }
        else if (collision.CompareTag("EnemyBullet"))
        {
            this.hp--;
            this.onHit();
            if (this.hp < 0) Die();
            Debug.LogFormat("�÷��̾� HP:{0}", this.hp);
        }
        else if (collision.CompareTag("Meteo"))
        {
            Die();
        }
    }

    private void Die()
    {
        this.onDie();
        this.anim.SetTrigger("Die");
    }

    private Vector2 ClampPosition(Vector2 position)
    {
        return new Vector2(Mathf.Clamp(position.x, -2.7f, 2.7f),
            Mathf.Clamp(position.y, -3.7f, -3.7f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.radius);
    }
}
