using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int lives;
    private float speed;
    private int weaponType;
    private bool invincible = false;
    private bool hasShield = false;
    private bool isDead; // New bool variable that will help with triggering the "Game Over" state, checked in LoseALife()
    private GameManager gameManager;

    private float horizontalInput;
    private float verticalInput;

    private float verticalTopScreenLimit = 0;
    private float verticalBottomScreenLimit = -3.5f;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject thrusterPrefab;
    public GameObject shieldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        speed = 5.0f;
        weaponType = 1;
        gameManager.ChangeLivesText(lives);
        shieldPrefab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    public void LoseALife()
    {
        IEnumerator InvincibleOff()
        {
            yield return new WaitForSeconds(2f);
            invincible = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            gameManager.PlaySound(2);
        }
        //Do I have a shield? If yes: do not lose a life, but instead deactivate the shield's visibility
        //If not: lose a life
        //lives = lives - 1;
        //lives -= 1;
        if (hasShield == true)
        {
            hasShield = false;
            shieldPrefab.SetActive(false);
            gameManager.ManagePowerupText(0);
            gameManager.PlaySound(2);
            return;
        }
        if (invincible)
        {
            return;
        }
        lives--;
        gameManager.ChangeLivesText(lives);
        invincible = true;
        StartCoroutine(InvincibleOff());
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
        // Destroys player object, this will be the game over state
        if (lives == 0 && !isDead) // If the player is not dead when lives == 0, immediately set isDead to true and trigger Game Over
        {
            isDead = true;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            gameManager.GameOver();
            Destroy(this.gameObject);
        }
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        speed = 5f;
        thrusterPrefab.SetActive(false);
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
    }

    IEnumerator WeaponPowerDown()
    {
        yield return new WaitForSeconds(3f);
        weaponType = 1;
        gameManager.ManagePowerupText(0);
        gameManager.PlaySound(2);
    }


    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {
        if (whatDidIHit.tag == "Powerup")
        {
            Destroy(whatDidIHit.gameObject);
            int whichPowerup = 4;
            gameManager.PlaySound(1);
            switch (whichPowerup)
            {
                case 1:
                    //Picked up speed
                    speed = 10f;
                    StartCoroutine(SpeedPowerDown());
                    thrusterPrefab.SetActive(true);
                    gameManager.ManagePowerupText(1);
                    break;
                case 4:
                    //Picked up shield
                    if (hasShield == false)
                        hasShield = true;
                    shieldPrefab.SetActive(true);
                    gameManager.ManagePowerupText(4);
                    break;
            }
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (weaponType)
            {
                case 1:
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.Euler(0, 0, 45));
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(0, 0, -45));
                    break;
            }
        }
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);

        float horizontalScreenSize = gameManager.horizontalScreenSize;
        float verticalScreenSize = gameManager.verticalScreenSize;

        if (transform.position.x <= -horizontalScreenSize || transform.position.x > horizontalScreenSize)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }

        if (transform.position.y >= verticalTopScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        if (transform.position.y <= verticalBottomScreenLimit)

        {
            transform.position = new Vector3(transform.position.x, -3.5f, 0);
        }
    }
}