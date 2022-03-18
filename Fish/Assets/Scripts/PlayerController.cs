using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { Hooked, Electrocuted, FoodPoisoned,  Toxiced, SpeedUp, Plasticized }
    [SerializeField] private HashSet<State> states = new HashSet<State>();

    // For hooked state
    [SerializeField] GameObject hookBy;
    [SerializeField] float hookEscapePressed = 0;

    // State Effects for speed
    private Dictionary<State, float> stateForSpeedEffect = new Dictionary<State, float>{
        {State.Toxiced, -8 },
        {State.Plasticized, -5 },
        {State.Electrocuted, -9 },
        {State.SpeedUp, 5 }
    };


    // For Toxiced state
    [SerializeField] GameObject toxicedBy;

    // For speedUp state

    // For Plasticized state


    //when player "delicious" -> add {speedUp}to speed
    [SerializeField] float speedNormal = 10;
    [SerializeField] float currentSpeed;

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


    public float RefreshCurrentSpeed() {
        float speed = speedNormal;

        foreach(State state in states) {
            if (stateForSpeedEffect.ContainsKey(state)) {
                speed += stateForSpeedEffect[state];
            }
        }
        if (speed < 0) {
            speed = 1;
        }
        return speed;
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
        Debug.Log("value of damage is " + damage + " .");
        Debug.Log("current health is " + currentHealth + " .");
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
            AddState(State.Plasticized);
            Debug.Log("current speed is" + currentSpeed);
            //hurt = true;
            TakeDamage(20);
            Destroy(other.gameObject);
            StartCoroutine(RecoverFromDamage());
            //the code of change of state (animation) below:


        }

        //if (other.CompareTag("electrocuted")) //paralized & hurt a little? //countdown time
        //{
        //    //states.Add(State.Electrocuted);

        //    //the code of change of state (animation) below:
        //}

        // Fishnet
        if (other.CompareTag("Fishnet")) {
            Debug.Log("Collide with Fishnet");
            hookIfNotHooked(other.gameObject);
        }

        // Toxic area
        if (other.CompareTag("ToxicArea")) {
            Debug.Log("Entering  Fishnet");
            toxicIfNotToxiced(other.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ToxicArea")) {
            Debug.Log("Exiting  Fishnet");
            RemoveState(State.Toxiced);
        }
    }


    IEnumerator RecoverFromDamage()
    {
        yield return new WaitForSeconds(damageTime);
        RemoveState(State.Plasticized);
    }

    IEnumerator AteCountDownRoutine()
    {
        AddState(State.SpeedUp);
        //add 20 to healthBar
        TakeDamage(-20);
        yield return new WaitForSeconds(fullTime);
        RemoveState(State.SpeedUp);
    }

    // States manager
    void AddState(State state) {
        states.Add(state);
        currentSpeed = RefreshCurrentSpeed();
    }

    void RemoveState(State state) { 
        states.Remove(state);
        currentSpeed = RefreshCurrentSpeed();
    }

    void ApplyStateEffects() { 
       if (states.Contains(State.Hooked) && hookBy is object) {
            ApplyHookState();
        }
       if (states.Contains(State.Toxiced)) {
            ApplyToxicEffet();
        }
    }

    // Toxic State
    void toxicIfNotToxiced(GameObject other)
    {
        if (states.Contains(State.Toxiced)) return;
        AddState(State.Toxiced);
        toxicedBy = other.gameObject;
    }

    void ApplyToxicEffet()
    {
        TakeDamage(toxicedBy.GetComponent<ContinuousDamage>().damage * Time.deltaTime);
    }

    // Hook State
    void hookIfNotHooked(GameObject other) {
        if (states.Contains(State.Hooked)) return;
        AddState(State.Hooked);
        hookBy = other.gameObject;

    }
    void ApplyHookState()
    {
        transform.position = new Vector3(hookBy.transform.position.x, hookBy.transform.position.y, transform.position.z);
        TakeDamage(hookBy.GetComponent<ContinuousDamage>().damage * Time.deltaTime);
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
        RemoveState(State.Hooked);
        Destroy(hookBy);
        hookBy = null;
        hookEscapePressed = 0;
    }


}


