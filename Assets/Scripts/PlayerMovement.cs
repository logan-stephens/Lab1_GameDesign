using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    private Rigidbody2D marioBody;
    public JumpOverGoomba jumpOverGoomba;


    // global variables
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // Start is called before the first frame update
    void Start(){
        marioSprite = GetComponent<SpriteRenderer>();

        // other instructions
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        GameOver.SetActive(false);
        gameOverScoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update(){
              // toggle state
      if (Input.GetKeyDown("a") && faceRightState){
          faceRightState = false;
          marioSprite.flipX = true;
      }

      if (Input.GetKeyDown("d") && !faceRightState){
          faceRightState = true;
          marioSprite.flipX = false;
      }

      // other instructions
    }


    public float upSpeed = 10;
    private bool onGroundState = true;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }


    // FixedUpdate is called 50 times a second
    public float maxSpeed = 20;

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        // other instructions
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.linearVelocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            marioBody.linearVelocity = Vector2.zero;
        }

        if (Input.GetKeyDown("space") && onGroundState){
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with goomba!");
            // hide one show the other?
            gameOverScoreText.text = "Score: " + jumpOverGoomba.score;
            GameOver.SetActive(true);
            HUDscore.SetActive(false);
            Time.timeScale = 0.0f;
        }
    }


    // just added
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public GameObject HUDscore;
    public GameObject GameOver;
    public TextMeshProUGUI gameOverScoreText;
    // other methods

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    

    public void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
        // reset score
        gameOverScoreText.text = "Score: 0";
        GameOver.SetActive(false);
        HUDscore.SetActive(true);
        jumpOverGoomba.score = 0;
    }

    

    

    
}