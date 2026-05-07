using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Inimigo : MonoBehaviour
{
    private static readonly int VelocidadeHash = Animator.StringToHash("Velocidade");
    private static readonly int MachucadoHash = Animator.StringToHash("Machucado");
    private static readonly int VivoHash = Animator.StringToHash("Vivo");
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);
    private static WaitForSeconds _waitForSeconds0_1 = new WaitForSeconds(0.1f);

    [Header("Configurações")]
    public float moveSpeed = 2f;
    public int maxHealth = 2;
    public float knockbackForce = 5f;
    public int Damage = 10;
    [SerializeField] bool movingRight = true;

    [Header("Patrulha")]
    public Transform pointA;
    public Transform pointB;
    public float pointReachedDistance = 0.2f;

    private bool vivo = true;
    private bool isKnockBacked = false;
    private Transform currentTarget;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // Define o primeiro alvo da patrulha
        if (pointA != null && pointB != null)
        {
            currentTarget = movingRight ? pointB : pointA;
        }
    }

    void Update()
    {
        if (isKnockBacked || !vivo) return;

        Move();
    }

    void Move()
    {
        // Se existirem pontos de patrulha, usa patrulha
        if (pointA != null && pointB != null && currentTarget != null)
        {
            PatrolBetweenPoints();
        }
        else
        {
            // Se não houver pontos, mantém o comportamento antigo
            float direction = movingRight ? 1 : -1;
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
            MirrorSprite(direction);
            anim.SetFloat(VelocidadeHash, Mathf.Abs(rb.velocity.x));
        }
    }

    void PatrolBetweenPoints()
    {
        float direction = Mathf.Sign(currentTarget.position.x - transform.position.x);

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        MirrorSprite(direction);
        anim.SetFloat(VelocidadeHash, Mathf.Abs(rb.velocity.x));

        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= pointReachedDistance)
        {
            if (currentTarget == pointA)
            {
                currentTarget = pointB;
                movingRight = true;
            }
            else
            {
                currentTarget = pointA;
                movingRight = false;
            }
        }
    }

    private void MirrorSprite(float moveInput)
    {
        if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Se estiver usando patrulha por pontos, não precisa inverter ao bater no chão
        if (pointA == null || pointB == null)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Inimigo"))
            {
                movingRight = !movingRight;
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.TakeDamage(Damage);
        }
    }

    public void EfeitoDeRecuo()
    {
        isKnockBacked = true;

        float knockbackDirection = movingRight ? -1 : 1;
        Vector2 force = new(knockbackDirection * knockbackForce, 0);

        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(ResetKnockback());
    }

    IEnumerator ResetKnockback()
    {
        yield return _waitForSeconds0_5;
        isKnockBacked = false;
    }

    public void EfeitoDePiscar()
    {
        StartCoroutine(Piscar());
    }

    IEnumerator Piscar()
    {
        Color corOriginal = spriteRenderer.color;
        Color corTransparente = new(corOriginal.r, corOriginal.g, corOriginal.b, 0.5f);

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = corTransparente;
            yield return _waitForSeconds0_1;
            spriteRenderer.color = corOriginal;
            yield return _waitForSeconds0_1;
        }
    }

    public void AnimacaoDeDano()
    {
        anim.SetTrigger(MachucadoHash);
        StartCoroutine(ResetMachucado());
    }

    IEnumerator ResetMachucado()
    {
        yield return _waitForSeconds0_5;
        anim.ResetTrigger(MachucadoHash);
    }

    internal void AnimacaoDeMorte()
    {
        vivo = false;

        rb.isKinematic = true;
        col.enabled = false;

        anim.SetBool(VivoHash, vivo);
        EfeitoDePiscar();

        Destroy(gameObject, 3);
    }

    private void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.15f);
            Gizmos.DrawSphere(pointB.position, 0.15f);
        }
    }
}