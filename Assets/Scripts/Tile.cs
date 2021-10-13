using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool tileRevelada = false;      // Indicador da carta virada ou não
    public Sprite originalCarta;            // Sprite da carta desejada
    public Sprite backCarta;                // Sprite do avesso da carta    
    //public Sprite novaCarta;    // Update da Carta

    public Sprite backCartaVermelha;                // Sprite do avesso da carta vermelha
    public Sprite backCartaAzul;                // Sprite do avesso da carta azul


    // Start is called before the first frame update
    void Start()
    {
        // EscondeCarta();                  // Inicia todas as cartas como escondidas
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
        print("Pressinou num Tile");    // Imprime no console toda vez que clica num tile/carta
        /*                              // Aqui não se guardava número de cartas
        if(tileRevelada)
        {
            EscondeCarta();
        }
        else
        {
            RevelaCarta();
        }
        */

        // Encontra o GameObject "gameManager" para utilizar a função CartaSelecionada na carta clicada
        GameObject.Find("gameManager").GetComponent<ManageCartas>().CartaSelecionada(gameObject);       
         
    }
    public void EscondeCarta(string cor = "red")                                  // Define a sprite e a variável tileRevelada para virar a carta de costas
    {
        if (cor == "red")
            backCarta = backCartaVermelha;
        else if (cor == "blue")
            backCarta = backCartaAzul;

        GetComponent<SpriteRenderer>().sprite = backCarta;      // Define a sprite como a parte de trás da carta
        tileRevelada = false;                                   // Define a variável tileRevelada como false
    }
    public void RevelaCarta()                                   // Define a sprite e a variável tileRevelada para virar a carta de frente
    {
        GetComponent<SpriteRenderer>().sprite = originalCarta;  // Define a sprite como a parte de frente da carta
        tileRevelada = true;                                    // Define a variável tileRevelada como true
    }
    public void SetCartaOriginal(Sprite novaCarta)              // Define a carta original como a sprite novaCarta
    {
        originalCarta = novaCarta;                              // Define a variável originalCarta como a sprite novaCarta
    }
}
