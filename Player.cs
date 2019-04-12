//Using statement for Unity
using UnityEngine;

//Using statement for AI
using UnityEngine.AI;

//Using statement for UI
using UnityEngine.UI;

//Using statement for scene management
using UnityEngine.SceneManagement;

//Inherit from MonoBehaviour class
public class Player : MonoBehaviour
{
    //Variable for NavMesh agent
    private NavMeshAgent myAgent;

    //Variable for enemy
    private GameObject currentEnemy;

    //Variable for health
    private int health = 100;

    //Variable for health bar
    public Slider healthBar;

    //Reference to animator on charater
    public Animator characterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //Refer to NavMeshAgent attached to this game object
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Specify speed based on NavMesh agent
        characterAnimator.SetFloat("Speed", myAgent.velocity.magnitude);

        //Ray from mouse to world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Raycast object stores info about things hit by raycast
        RaycastHit hit;

        //Option for arrow key movement
        myAgent.destination = transform.position + 10 * (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));

        //Check for mouse input and if the raycast hit something
        if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
        {
            //Update destination for NavMesh agent
            myAgent.destination = hit.point;

            //Deselect the previous enemy selected
            if (currentEnemy != null)
            {
                //Make color yellow if enemy is deselected
                currentEnemy.GetComponent<Enemy>().selector.GetComponent<MeshRenderer>().material.color = Color.yellow;

                //Reset the current enemy
                currentEnemy = null;
            }

            //Check if an enemy is hit
            if (hit.collider.tag == "Enemy")
            {
                //Set the current enemy to targeted enemy
                currentEnemy = hit.collider.gameObject;

                //Turn the cubes red if targeting an enemy
                hit.collider.GetComponent<Enemy>().selector.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }

        //Check if targeting an enemy while walking and make sure it is close enough to attack
        if (currentEnemy != null && characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Walking")
            && Vector3.Distance(transform.position, currentEnemy.transform.position) < 1.5)
        {
            //Prevent from sliding forward while punching
            myAgent.destination = transform.position;

            //Face enemy while punching
            transform.LookAt(currentEnemy.transform.position);

            //Play the punch animation to attack
            characterAnimator.SetTrigger("Punch");
        }
    }

    //OnTriggerEnter function processes collision
    private void OnTriggerEnter(Collider other)
    {
        //Check for damage tag
        if (other.tag == "Damage")
        {
            //BloodSplat effect when projectile hits player
            Instantiate(Resources.Load("BloodSplat"), other.transform.position, other.transform.rotation);

            //Destroy projectile when it hits player
            Destroy(other.gameObject);

            //Health decreases
            health -= 10;

            //Set the health bar
            healthBar.value = health;

            //Check if player dies
            if (health <= 0)
            {
                //Reload scene if player dies
                SceneManager.LoadScene(0);
            }
        }
    }
}
