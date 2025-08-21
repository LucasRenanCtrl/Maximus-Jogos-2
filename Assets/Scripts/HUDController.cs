using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image[] hearts; // Array para armazenar as imagens dos corações
    public Sprite fullHeart; // Sprite do coração cheio
    public Sprite halfHeart; // Sprite do coração pela metade
    public Sprite emptyHeart; // Sprite do coração vazio

    private int currentHealth;

    public static HUDController Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentHealth = PlayerController.Instance.health.currentHealth;
        UpdateHearts();
    }

    // Método que atualiza a interface dos corações
    public void UpdateHearts()
    {
        currentHealth = PlayerController.Instance.health.currentHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth / 2) // Se a posição do coração está totalmente preenchida
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i < currentHealth / 2 + (currentHealth % 2)) // Se o coração está pela metade
            {
                hearts[i].sprite = halfHeart;
            }
            else // Coração vazio
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

}
