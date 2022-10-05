using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public int jumpHeight;
    public GameObject winTextObject;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        winTextObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rb2d.AddForce(new Vector2 (hozMovement * speed, 0));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb2d.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }
        }
    }

    void Update()
    {
        if (scoreValue>=6)
        {
            speed=0;
            winTextObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
