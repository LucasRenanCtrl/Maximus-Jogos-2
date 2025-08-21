using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image[] hearts; // Array para armazenar as imagens dos cora��es
    public Sprite fullHeart; // Sprite do cora��o cheio
    public Sprite halfHeart; // Sprite do cora��o pela metade
    public Sprite emptyHeart; // Sprite do cora��o vazio

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

    // M�todo que atualiza a interface dos cora��es
    public void UpdateHearts()
    {
        currentHealth = PlayerController.Instance.health.currentHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth / 2) // Se a posi��o do cora��o est� totalmente preenchida
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i < currentHealth / 2 + (currentHealth % 2)) // Se o cora��o est� pela metade
            {
                hearts[i].sprite = halfHeart;
            }
            else // Cora��o vazio
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

}
