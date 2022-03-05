using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int _hp = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damg)
    {
        _hp -= damg;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log($"{collision.gameObject}");
    //    if (collision.CompareTag("DamageObject"))
    //    {
    //        _hp--;
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log($"{collision.gameObject}");
    //    if (collision.CompareTag("DamageObject"))
    //    {
    //        _hp--;
    //    }
    //}
}
