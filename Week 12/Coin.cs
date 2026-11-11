using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Coin : MonoBehaviour
{
    private float speed;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        transform.localScale = transform.localScale * 0.2f;
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        speed = Random.Range(1f, 2f);

        transform.Translate(Vector3.down * speed * Time.deltaTime);

    }
    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {
        if (whatDidIHit.tag == "Player")
        {
            gameManager.AddScore(1);
            Destroy(this.gameObject);
        }
    }
}
       

