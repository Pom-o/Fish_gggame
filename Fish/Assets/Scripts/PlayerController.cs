using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { Hooked, Electrocuted, FoodPoisoned,  }
    [SerializeField] private HashSet<State> states = new HashSet<State>();

    // For hook state
    [SerializeField] GameObject hookBy;
    [SerializeField] float hookEscapePressed = 0;

    public float currentSpeed;

    //when player "delicious" -> add {speedUp}to speed
    [SerializeField] float speedNormal = 10;
    [SerializeField] float speedUp = 15;

    [SerializeField] float fullTime;


    //when player ate plastic bags[0] / poisoned[1] -> divide the damage cases

    //the damaged divided from speed
    [SerializeField] float damagedSpeed = 5;
    //last for {damageTime}
    [SerializeField] float damageTime = 5f;
    [SerializeField] int escapeCount = 5;

    //health control
    public float currentHealth = 100;
    public int maxHealth;
    public HealthBar healthbar;


    //    [SerializeField] bool hurt;


    private Rigidbody2D rb;
    private Vector2 moveVelocity;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //setup the max health
        healthbar.SetMaxHealth(maxHealth);
        Debug.Log("the current health is " + currentHealth);

        currentSpeed = speedNormal;
    }

    private void Update()
    {
        //test for healthBar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * currentSpeed;

        ApplyStateEffects();
        TryToDetectEscapeIfHooked();
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
        Debug.Log("value of damage is " + damage + " .");
        Debug.Log("current health is " + currentHealth + " .");
    }

    void setSpeedNormal()
    {
        currentSpeed = speedNormal;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //for damages like plastic bag/ poisoned water / electroized field
        if (other.CompareTag("food"))
        {
            Debug.Log($"Player collided with {other.gameObject.name} . And this is supposed to be the food");
            Destroy(other.gameObject);
            //use a method to delicious->speedUp + add health ->speedUp disappears

            StartCoroutine(AteCountDownRoutine());

            TakeDamage(-20);
            //speed up edicator

        }

        if (other.CompareTag("enemy"))
        {
            //enemy is the plastic bag
            currentSpeed = damagedSpeed;
            Debug.Log("current speed is" + currentSpeed);
            //hurt = true;
            TakeDamage(20);
            Destroy(other.gameObject);
            StartCoroutine(RecoverFromDamage());
            //the code of change of state (animation) below:


        }

        if (other.CompareTag("electrocuted")) //paralized & hurt a little? //countdown time
        {
            currentSpeed = 0.1f;

            //the code of change of state (animation) below:
        }

        // Fishnet
        if (other.CompareTag("Fishnet")) {
            Debug.Log("Collide with Fishnet");
            hookIfNotHooked(other.gameObject);
        }

    }


    IEnumerator RecoverFromDamage()
    {
        yield return new WaitForSeconds(damageTime);
        setSpeedNormal();
    }

    IEnumerator AteCountDownRoutine()
    {
        currentSpeed = speedUp;
        //add 20 to healthBar
        TakeDamage(-20);
        yield return new WaitForSeconds(fullTime);
    }

    // States manager
    void ApplyStateEffects() { 
       if (states.Contains(State.Hooked) && hookBy is object) {
            ApplyHookState();
        }
    }

    // Hook State
    public bool isHooked() {
        return states.Contains(State.Hooked);
    }
    void hookIfNotHooked(GameObject other) {
        if (isHooked()) return;
        states.Add(State.Hooked);
        hookBy = other.gameObject;

    }
    void ApplyHookState()
    {
        transform.position = new Vector3(hookBy.transform.position.x, hookBy.transform.position.y, transform.position.z);
        TakeDamage(hookBy.GetComponent<Hookable>().damage * Time.deltaTime);
        Debug.Log("Press q repeatedly to escape!!!");
    }

    void TryToDetectEscapeIfHooked() {
        if (!states.Contains(State.Hooked)) { return; }

        Hookable hookObject = hookBy.GetComponent<Hookable>();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hookEscapePressed += 1;
        }
        if (hookEscapePressed >= hookObject.hookEscapePressTarget)
        {
            EscapeFromHook();
            return;
        }
        if (hookEscapePressed > 0)
        {
            hookEscapePressed -= hookObject.PressStrengthWeakeningRate() * Time.deltaTime;
        }
    }

    void EscapeFromHook() {
        Debug.Log("Escape from hook state successfully");
        states.Remove(State.Hooked);
        Destroy(hookBy);
        hookBy = null;
        hookEscapePressed = 0;
    }


}


