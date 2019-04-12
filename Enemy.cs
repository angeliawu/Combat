//Using statement for unity
using UnityEngine;

//Using statement for UI
using UnityEngine.UI;

//Inherit from MonoBehaviour class
public class Enemy : MonoBehaviour
{
    //Variable for cooldown
    public float cooldown;

    //Variable for ranged timer
    private float rangedTimer;

    //Variable for selector
    public GameObject selector;

    //Variable for health bar
    public Slider healthBar;

    //Variable for health
    private int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        //Ensure enemies don't fire at the very beginning
        rangedTimer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        //Updates based on how much time has passed since last frame
        rangedTimer -= Time.deltaTime;

        //Animation to attack right before spawning projectiles
        if (rangedTimer <= 0.25f)
        {
            //Play the attack animation
            GetComponentInChildren<Animator>().SetTrigger("Attack");
        }

        //Code for spawning objects
        if (rangedTimer <= 0)
        {
            //Reset the timer
            rangedTimer = cooldown;

            //Enemy turns to look at player
            transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);

            //Instantiate prefab where enemy is looking
            GameObject projectile = Instantiate(Resources.Load("Projectile"), transform.position, transform.rotation) as GameObject;

            //Add force to the projectile
            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * 300);
        }
    }
    
    //OnTriggerStay runs every frame while colliding
    private void OnTriggerStay(Collider other)
    {
        //Check for melee tag
        if (other.tag == "Melee")
        {
            //Subtract from enemy health
            health -= 25;

            //Set the health bar
            healthBar.value = health;

            //Check if enemy dies
            if (health <= 0)
            {
                //Destroy enemy when it dies, but do not reload scene
                Destroy(gameObject);
            }
        }
    }
}
