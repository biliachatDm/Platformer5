using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Entity
{
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource damageSound;
    [SerializeField] private AudioSource missAttack;
    [SerializeField] private AudioSource attack;
    public string HName;
    
    [SerializeField] private float speed = 3f; //скорость движения
    [SerializeField] private int health = 5; // текущие жизни
    //[SerializeField] private Image[] hearts;
    [SerializeField] private float jumpForce = 6f; //сила прижка
    private bool isGrounded = false;
    [SerializeField] private Text heroNameText;

    //[SerializeField] private Sprite heart;

    public bool isAttacking = false;
    public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;
    public Text lifes;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public static Hero Instance { get; set; }
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        Instance = this;
        HName = PlayerPrefs.GetString("hName");
        heroNameText.text = HName;
        lives = 5;
        health = lives;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isRecharged = true;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded && !isAttacking) State = States.idle;
        
        if (!isAttacking && Input.GetButton("Horizontal"))
            Run();
        if (!isAttacking && isGrounded && Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButtonDown("Fire1"))
            Attack();

        lifes.text = lives.ToString();
    }

    private IEnumerator AttackAnimation()
    {
        yield return new WaitForSeconds(0.1f); //0.4
        isAttacking = false;            
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }

    private IEnumerator EnemyOnAttack(Collider2D enemy)
    {
        SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
        enemyColor.color = new Color(1f, 0.4375f, 0.4375f);
        yield return new WaitForSeconds(0.2f);
        enemyColor.color = new Color(1, 1, 1);


    }

    private void Run()
    {
        if (isGrounded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        sprite.flipX = dir.x < 0.0f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        jumpSound.Play();
        Debug.Log(HName);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    private void CheckGround (){
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;

        if (!isGrounded)
            State = States.jump;
    }

    public override void GetDamage()
    {
        lives -= 1;
        damageSound.Play();
        if (lives == 0)
        {
            Die();
            lifes.text = "0";
        }
        Debug.Log(lives);
    }

    private void Attack()
    {
        if (isGrounded && isRecharged)
        {
            State = States.attack;
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCoolDown());
        }
    }
    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
        if (colliders.Length == 0)
        {
            missAttack.Play();
        } else
        {
            attack.Play();
        }
        
        for (int i=0; i<colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
            StartCoroutine(EnemyOnAttack(colliders[i]));
        }
    }
}

public enum States
{
    idle,
    run,
    jump,
    attack
}
