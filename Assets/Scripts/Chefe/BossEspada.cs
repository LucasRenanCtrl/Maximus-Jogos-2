using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEspada : MonoBehaviour
{
    public float swordSpeed = 8f;
    public float swordReturnSpeed = 12f;
    bool swordReturning = false;
    Vector2 direction;
    Transform boss;
    Rigidbody2D swordRb;

    void Start()
    {
        Rigidbody2D swordRb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void IniciarAtaque(Transform bossTransform, Vector2 atkDirection)
    {
        boss = transform;
        direction = atkDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (!swordReturning)
        {
            swordRb.velocity = direction * swordSpeed;
            // Se a espada já andou um certo tempo, começa a voltar
            if (Vector2.Distance(boss.position, gameObject.transform.position) > 5f)
            {
                swordReturning = true;
            }
        }
        else
        {
            // Espada volta em direção ao boss
            Vector2 dirBack = (boss.position - gameObject.transform.position).normalized;
            swordRb.velocity = dirBack * swordReturnSpeed;

            // Se chegou perto, destrói
            if (Vector2.Distance(boss.position, gameObject.transform.position) < 0.5f)
            {
                // boss.GetComponent<BossAtaques>().EspadaVoltou();
                Destroy(gameObject);
                swordReturning = false;
            }
        }
    }
}
