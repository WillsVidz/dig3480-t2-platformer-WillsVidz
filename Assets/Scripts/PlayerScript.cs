using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    Animator anim;
    
    public AudioSource musicSource;
    public AudioClip levelMusic;
    public AudioClip winMusic;
    
    private Rigidbody2D rb2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text lives;
    private int livesValue = 3;
    public float jumpHeight;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    
    // Start is called before the first frame update
    void Start()
    {
        //code below is simply to test second level positioning
        //transform.position = new Vector2(41.78f, 0.06f);

        loseTextObject.SetActive(false);
        rb2d = GetComponent<Rigidbody2D>();
        score.text = "Score: "+scoreValue.ToString();
        lives.text = "Lives: "+livesValue.ToString();
        winTextObject.SetActive(false);

        musicSource.clip = levelMusic;
        musicSource.Play();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rb2d.AddForce(new Vector2 (hozMovement * speed, 0));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement>0)
        {
            Flip();
        }
        
        else if (facingRight == true && hozMovement<0)
        {
            Flip();
        }

        //Walking animation
        if (hozMovement>0 && isOnGround)
        {
            anim.SetInteger("State",1);
        }
        if (hozMovement<0 && isOnGround)
        {
            anim.SetInteger("State",1);
        }
        if (hozMovement==0)
        {
            anim.SetInteger("State",0);
        }

        //Jumping animation
        if (isOnGround==false)
        {
            anim.SetInteger("State",2);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: "+scoreValue.ToString();
            Destroy(other.gameObject);
            LevelAdvance();
        }
        else if(other.gameObject.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: "+livesValue.ToString();
            Destroy(other.gameObject);
            TestLives();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb2d.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }
        }
    }

    //Stop jumping animation when player hits ground
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
        //if (collision.collider.tag == "Ground" && isOnGround)
        //{
            //anim.SetInteger("State",0);
        //}
    //}

    void Update()
    {
        //Press Escape to quit application
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void Flip ()
    {
    facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
    }

    void TestLives()
    {
        if (livesValue<=0)
        {
            loseTextObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    void LevelAdvance()
    {
       if (scoreValue==4)
        {
            transform.position = new Vector2(41.78f, 0.06f);
            livesValue = 3;
            lives.text = "Lives: "+livesValue.ToString();
        }
        else if (scoreValue>=8)
        {
            //switch audio file
            musicSource.Stop();
            musicSource.loop = false;
            musicSource.clip = winMusic;
            musicSource.Play();
            
            //show win state
            speed=0;
            winTextObject.SetActive(true);
        }
    }

}
