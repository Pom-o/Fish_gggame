using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using animation

public class PlayerController : MonoBehaviour
{

    public GameObject readUI;
    PauseMenu pauseMenu;


    //sounds
    public AudioClip soundEffect_electrocuted;
    public AudioClip soundEffect_struggling;
    public AudioClip soundEffect_eat1;
    public AudioClip soundEffect_eat2;
    public AudioClip soundEffect_eat3;
    public AudioClip soundEffect_eat4;
    public AudioClip soundEffect_plastic1;
    public AudioClip soundEffect_plastic2;

    public AudioClip soundEffect_gameOver;
    public AudioClip soundEffect_bottle;






    public Animator animator;
    public GameObject pressQ;
    enum State { Hooked, Electrocuted, Toxiced, Full, Plasticized }
    [SerializeField] private HashSet<State> states = new HashSet<State>();

    // For hooked state
    [SerializeField] GameObject hookBy;
    [SerializeField] float hookEscapePressed = 0;

    // State Effects for speed
    private Dictionary<State, float> stateForSpeedEffect = new Dictionary<State, float>{
        {State.Toxiced, -8 },
        {State.Plasticized, -5 },
        {State.Electrocuted, -9 },
        {State.Full, 2 }
    };


    // For Toxiced state
    [SerializeField] GameObject toxicedBy;

    // For Electrocuted
    float electrocutedDamage = 30;
    float paralysisTime = 3;
    float remainParalysisTime = 0;

    // For Plasticized state
    float plasticizedDamage = 10;
    float decreasedMaxHealthByPlastic = 10;
    float recoverTime = 1;
    float remainRecoverTime = 0;

    // For Full state
    [SerializeField] float fullTime = 2.0f;
    [SerializeField] float remainFullTime;


    //when player "delicious" -> add {speedUp}to speed
    [SerializeField] float speedNormal = 10;
    [SerializeField] float currentSpeed;

    // Enemies realted
    [SerializeField] float hookDamage = 15;


    //when player ate plastic bags[0] / poisoned[1] -> divide the damage cases

    //health control
    public int maxHealth;
    public float currentHealth;
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
        animator.SetBool("FishIsHooked", false);
        pressQ.SetActive(false);

    }

    private void Update()
    {
        //test for healthBar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
            CheckIfGameOver();
        }
        //moveVelocity = (moveInput.normalized +  Vector2.right) * currentSpeed/10;
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * currentSpeed;

        ApplyStateEffects();
        TryToDetectEscapeIfHooked();
        LimitMoveRegion();
        CheckIfGameOver();
    }



    public float RefreshCurrentSpeed()
    {
        float speed = speedNormal;

        foreach (State state in states)
        {
            if (stateForSpeedEffect.ContainsKey(state))
            {
                speed += stateForSpeedEffect[state];
            }
        }
        if (speed < 0)
        {
            speed = 1;
        }
        Debug.Log($"Set speed to {speed}");
        return speed;
    }

    void TakeDamage(float damage)
    {
        currentHealth = healthbar.DecreaseHealth(damage);
        Debug.Log("value of damage is " + damage + " .");
        Debug.Log("current health is " + currentHealth + " .");
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    // public void Pause()
    // {
    //     pauseMenu.SetActive(true);
    //     Time.timeScale = 0f;
    // }

    // public void Resume()
    // {
    //     pauseMenu.SetActive(false);
    //     Time.timeScale = 1;
    // }

    public void Pause()
    {
        //pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        //pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //activate Readletter in UI when encounter with bottle
        if (other.CompareTag("Bottle"))
        {
            //readUI.SetActive(true);
            Pause();
            other.GetComponent<PauseMenu>().SetLetterActive();
            readUI.SetActive(true);
            Destroy(other.gameObject);
            ClearAllEnemies();

        }
        //for damages like plastic bag/ poisoned water / electroized field
        if (other.CompareTag("Food"))
        {
            Debug.Log($"Player collided with {other.gameObject.name} . And this is supposed to be the food");
            ApplyFullState(other.gameObject);
            SoundManager.instance.RandomizesFx(soundEffect_eat1, soundEffect_eat2, soundEffect_eat3, soundEffect_eat4);
        }

        // Hook
        if (other.CompareTag("Hook"))
        {
            TakeDamage(hookDamage);
            other.GetComponent<SelfDestory>().disable();
            hookIfNotHooked(other.gameObject);
            SoundManager.instance.playSingle(soundEffect_struggling);
        }

        // Plastic
        if (other.CompareTag("Plastic"))
        {
            ApplyPlasticizedEffet(other.gameObject);
            SoundManager.instance.RandomizesFx(soundEffect_plastic1, soundEffect_plastic2);
        }


        // Fishnet
        if (other.CompareTag("Fishnet"))
        {
            Debug.Log("Hook by Fishnet");
            hookIfNotHooked(other.gameObject);
            SoundManager.instance.playSingle(soundEffect_struggling);
        }

        // Toxic area
        if (other.CompareTag("ToxicArea") || other.CompareTag("DeadFish"))
        {
            Debug.Log("Entering Fishnet or DeadFish");
            toxicIfNotToxiced(other.gameObject);
        }

        // Electric shocker
        if (other.CompareTag("ElectricShocker"))
        {
            Debug.Log("Hitting Electric Shocker");
            ApplyElectrocutedEffet(other.gameObject);
            SoundManager.instance.playSingle(soundEffect_electrocuted);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ToxicArea") || other.CompareTag("DeadFish"))
        {
            Debug.Log("Exiting  Fishnet Or DeadFish");
            RemoveState(State.Toxiced);
        }
    }

    private string[] EnemyTags =  new string[] {"Fishnet", "DeadFish", "Plastic", "ElectricShocker", "Hook", "ToxicArea" };
    void ClearAllEnemies() { 
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (var go in allObjects) {
            var render = go.GetComponent<Renderer>();
            if (render == null || !render.isVisible) {
                continue;
            }
            if (System.Array.IndexOf(EnemyTags, go.tag) == -1) {
                continue;
            }
            Destroy(go);
        }

    }


    // limit Move region
    float leftBound = -5.2f;
    float rightBound = 4.8f;
    float bottomBound = -4f;
    float topBound = 2.4f;

    void LimitMoveRegion()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBound, rightBound),
            Mathf.Clamp(transform.position.y, bottomBound, topBound),
            transform.position.z);
    }


    // States manager
    void AddState(State state)
    {
        states.Add(state);
        currentSpeed = RefreshCurrentSpeed();
    }

    void RemoveState(State state)
    {
        states.Remove(state);
        currentSpeed = RefreshCurrentSpeed();
    }

    void ApplyStateEffects()
    {
        if (states.Contains(State.Hooked))
        {
            if (hookBy == null)
            {
                EscapeFromHook();
            }
            else
            {
                ApplyHookedState();
            }
        }
        if (states.Contains(State.Toxiced))
        {
            ApplyToxicedEffet();
        }
    }


    // Full State
    void ApplyFullState(GameObject other)
    {
        //use a method to delicious->speedUp + add health ->speedUp disappears
        Destroy(other);
        TakeDamage(-20);
        currentHealth = healthbar.DecreaseMaxHealthByToxic(2);

        remainFullTime += fullTime;
        if (!states.Contains(State.Full))
        {
            AddState(State.Full);
            StartCoroutine(AteCountDownRoutine());
        }
    }

    IEnumerator AteCountDownRoutine()
    {
        while (remainFullTime > 0)
        {
            yield return new WaitForSeconds(1);
            remainFullTime -= 1;
        }
        remainFullTime = 0;
        RemoveState(State.Full);
    }

    // Plasticized State
    void ApplyPlasticizedEffet(GameObject other)
    {
        Destroy(other);
        TakeDamage(plasticizedDamage);
        healthbar.DecreaseMaxHealthByPlastic();

        remainRecoverTime += recoverTime;
        if (!states.Contains(State.Plasticized))
        {
            AddState(State.Plasticized);
            StartCoroutine(RecoverFromPlasticDamage());
        }
    }

    IEnumerator RecoverFromPlasticDamage()
    {
        while (remainRecoverTime > 0)
        {
            yield return new WaitForSeconds(1);
            remainRecoverTime -= 1;
        }
        remainRecoverTime = 0;
        RemoveState(State.Plasticized);
    }

    // Electrocuted State
    void ApplyElectrocutedEffet(GameObject other)
    {
        Destroy(other);
        TakeDamage(electrocutedDamage);

        remainParalysisTime += paralysisTime;
        if (!states.Contains(State.Electrocuted))
        {
            AddState(State.Electrocuted);
            StartCoroutine(ApplyParalizingEffect());
        }
    }

    IEnumerator ApplyParalizingEffect()
    {
        while (remainParalysisTime > 0)
        {
            yield return new WaitForSeconds(1);
            remainParalysisTime -= 1;
        }
        remainParalysisTime = 0;
        RemoveState(State.Electrocuted);
    }

    // Toxic State
    void toxicIfNotToxiced(GameObject other)
    {
        if (states.Contains(State.Toxiced)) return;
        AddState(State.Toxiced);
        toxicedBy = other.gameObject;
    }

    void ApplyToxicedEffet()
    {
        TakeDamage(toxicedBy.GetComponent<ContinuousDamage>().damage * Time.deltaTime);
        currentHealth = healthbar.DecreaseMaxHealthByToxic(toxicedBy.GetComponent<ContinuousDamage>().healthLimitDamage * Time.deltaTime);
    }

    // Hook State
    void hookIfNotHooked(GameObject other)
    {
        if (states.Contains(State.Hooked)) return;
        hookBy = other.gameObject;
        AddState(State.Hooked);

    }
    void ApplyHookedState()
    {
        transform.position = new Vector3(hookBy.transform.position.x, hookBy.transform.position.y, transform.position.z);
        TakeDamage(hookBy.GetComponent<ContinuousDamage>().damage * Time.deltaTime);
        Debug.Log("Press q repeatedly to escape!!!");
    }

    void TryToDetectEscapeIfHooked()
    {
        if (!states.Contains(State.Hooked)) { return; }
        animator.SetBool("FishIsHooked", true);
        pressQ.SetActive(true);
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

    void EscapeFromHook()
    {
        Debug.Log("Escape from hook state successfully");
        RemoveState(State.Hooked);
        hookEscapePressed = 0;
        animator.SetBool("FishIsHooked", false);
        pressQ.SetActive(false);

        hookEscapePressed = 0;
        if (hookBy != null)
        {
            Destroy(hookBy);
            hookBy = null;
        }
    }


    void CheckIfGameOver()
    {
        if (currentHealth <= 0)
        {
            SoundManager.instance.playSingle(soundEffect_gameOver);
            SoundManager.instance.musicSource.Stop();
        }
    }


}


