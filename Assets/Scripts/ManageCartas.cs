using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageCartas : MonoBehaviour
{
    public GameObject carta;                                            // Carta a ser descartada
    public Sprite novaCarta;                                            // Update da Carta
    private bool primeiraCartaSelecionada, segundaCartaSelecionada;     // Indicadores para cada carta escolhida em cada linha
    private GameObject carta1, carta2;                                  // GameObjects da 1� e 2� carta selecionada 
    private string linhaCarta1, linhaCarta2;                            // Linha da carta selecionada

    bool timerPausado, timerAcionado;                                   // Indicador de pausa ou start no Timer
    float timer;                                                        // Vari�vel do tempo

    public int numTentativas = 0;                                       // N�mero de tentativas na rodada
    int numAcertos = 0;                                                 // N�mero de match de pares acertados
    AudioSource somOK;                                                  // Som de acerto

    int ultimoJogo = 0;                                                 // N�mero de tentativas do �ltimo jogo
    int recorde = 0;                                                    // N�mero do recorde mais recente
   
    // Start is called before the first frame update
    void Start()
    {
        MostraCartas();
        UpdateTentativas();
        somOK = GetComponent<AudioSource>();                            // Retorna o componente somOK        
        ultimoJogo = PlayerPrefs.GetInt("Jogadas", 0);                  // Define a vari�vel ultimoJogo como a PlayerPrefs "Jogadas"
        recorde = PlayerPrefs.GetInt("Recorde", 0);                     // Define a vari�vel recorde como a PlayerPrefs "Recorde"
        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "�ltimo Jogo: " + ultimoJogo;       // Encontra o GameObject "ultimaJogada" e define seu texto com base na vari�vel ultimoJogo
        GameObject.Find("recorde").GetComponent<Text>().text = "Recorde: " + recorde;                   // Encontra o GameObject "recorde" e define seu texto com base na vari�vel recorde
    }

    // Update is called once per frame
    void Update()
    {
        if(timerAcionado)
        {
            timer += Time.deltaTime;            // Soma o tempo corrido na vari�vel timer para cada frame
            print(timer);                       // Imprime o tempo corrido no console
            if (timer > 0.5)                    // Define o intervalo de tempo para "atualiza��o" depois de selecionar duas cartas
            {
                timerPausado = true;            // Define a vari�vel de pausa do timer como verdadeira
                timerAcionado = false;          // Define a vari�vel de continua��o do timer como falsa
                if (carta1.tag == carta2.tag)   // Verifica se as cartas s�o iguais
                {
                    Destroy(carta1);            // Remove a carta 1 do campo
                    Destroy(carta2);            // Remove a carta 2 do campo
                    numAcertos++;               // Soma 1 unidade na varia�vel numAcertos
                    somOK.Play();               // Toca o som de OK
                    if(numAcertos == 13)        // Caso todas cartas sejam acertadas (Necess�rio mudar para outros modos de jogo com mais cartas)
                    {  
                        PlayerPrefs.SetInt("Jogadas", numTentativas);       // Define a PlayerPref "Jogadas" como a vari�vel numTentativas
                        if(numTentativas < recorde)                         // Se ultrapassou o recorde
                        {
                            PlayerPrefs.SetInt("Recorde", numTentativas);   // Define a PlayerPref "Recorde" como a vari�vel numTentativas
                            PlayerPrefs.SetInt("RecordeAntigo", recorde);   // Define a PlayerPref "RecordeAntigo" como a vari�vel recorde
                            SceneManager.LoadScene("Lab3_RecordeBatido");   // Carrega a cena "Lab3_RecordeBatido"
                        }
                        else
                        {
                            SceneManager.LoadScene("Lab3_Fim");     // Carrega a cena "Lab3_Fim"
                        }
                    }
                }
                else
                {
                    carta1.GetComponent<Tile>().EscondeCarta();     // Caso o par seja incorreto elas viram novamente
                    carta2.GetComponent<Tile>().EscondeCarta();     // Caso o par seja incorreto elas viram novamente
                }
                primeiraCartaSelecionada = false;                   // Reinicia os par�metros de carta 1 selecionada
                segundaCartaSelecionada = false;                    // Reinicia os par�metros de carta 2 selecionada
                carta1 = null;                                      // Reinicia o GameObject carta1
                carta2 = null;                                      // Reinicia o GameObject carta2
                linhaCarta1 = "";                                   // Zera o valor de linha da carta 1
                linhaCarta2 = "";                                   // Zera o valor de linha da carta 2
                timer = 0;                                          // Zera a vari�vel timer
            }
        }
    }
    void MostraCartas()
    {
        int[] arrayEmbaralhado = CriaArrayEmbaralhado();            // Cria um array de cartas embaralhadas
        int[] arrayEmbaralhado2 = CriaArrayEmbaralhado();           // Cria outro array (diferente) de cartas embaralhadas
        //Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity);
        //AddUmaCarta();

        for (int i = 0; i <13; i++)
        {
            //AddUmaCarta(i);
            AddUmaCarta(0, i, arrayEmbaralhado[i]);     // Adiciona 13 cartas na linha 0 com arrayEmbaralhado1
            AddUmaCarta(1, i, arrayEmbaralhado2[i]);    // Adiciona 13 cartas na linha 1 com o arrayEmbaralhado2
        }
    }
    void AddUmaCarta(int linha, int rank, int valor)
    {
        GameObject centro = GameObject.Find("centroDaTela");            // Utiliza o GameObject centroDaTela como ponto de refer�ncia para posicionar as cartas
        float escalaCartaOriginal = carta.transform.localScale.x;       // Captura a escala original da carta 
        float fatorEscalaX = (650 * escalaCartaOriginal) / 110.0f;      // Multiplica a escala original por um fator X para defini��o do seu tamanho
        float fatorEscalaY = (945 * escalaCartaOriginal) / 110.0f;      // Multiplica a escala original por um fator Y para defini��o do seu tamanho
        
        //Vector3 novaposicao = new Vector3(centro.transform.position.x + ((rank - 13 / 2) * 1.3f), centro.transform.position.y, centro.transform.position.z);
        //Vector3 novaposicao = new Vector3(centro.transform.position.x + ((rank - 13 / 2) * fatorEscalaX), centro.transform.position.y, centro.transform.position.z); ;
        
        // Define um vetor novaposicao para posicionar a carta na tela com base nos fatores de escala X e Y e o rank da carta, para espa�a-l�s corretamente
        Vector3 novaposicao = new Vector3(centro.transform.position.x + ((rank - 13 / 2) * fatorEscalaX), centro.transform.position.y + ((linha - 2 / 2) * fatorEscalaY), centro.transform.position.z);
        
        //GameObject c = (GameObject)(Instantiate(carta, new Vector3(0, 0, 0), Quaternion.identity));  
        //GameObject c = (GameObject)(Instantiate(carta, new Vector3(rank*1.5f, 0, 0), Quaternion.identity));
        
        GameObject c = (GameObject)(Instantiate(carta, novaposicao, Quaternion.identity));      // Instancia um GameObject "c" para uma carta
        c.tag = "" + (valor+1);             // Define uma tag para essa carta "c" com base na vari�vel valor (n�mero da carta no array)                                                                      
        //c.name = "" + valor;              
        c.name = "" + linha + "_" + valor;  // Define um name para a carta "c" com base na linha (0 ou 1) e seu valor num�rico no array (0 a 12)
        string nomeDaCarta = "";            // Define uma string vazia para o nome da carta
        string numeroCarta = "";            // Define uma string vazia para o n�mero da carta
        /*
        if (rank == 0)
            numeroCarta = "ace";
        else if (rank == 10)
            numeroCarta = "jack";
        else if (rank == 11)
            numeroCarta = "queen";
        else if (rank == 12)
            numeroCarta = "king";
        else numeroCarta = "" + (rank + 1);     // Else if para array ordenado no deck
        */

        //  Define a vari�vel numeroCarta para cartas que n�o s�o num�ricas (�s (ace) = 0; jack (valete) = 10; queen (rainha) = 11; king (rei) = 12)
        if (valor == 0)
            numeroCarta = "ace";
        else if (valor == 10)
            numeroCarta = "jack";
        else if (valor == 11)
            numeroCarta = "queen";
        else if (valor == 12)
            numeroCarta = "king";
        else numeroCarta = "" + (valor + 1);            // Define a vari�vel numeroCarta para cartas num�ricas
                                                        // (neste caso, as cartas t�m seus valores reais iguais aos da array)
        if(linha == 0)
            nomeDaCarta = numeroCarta + "_of_clubs";    // Define as cartas da linha 0 como o naipe paus (clubs)
        else if (linha == 1)
            nomeDaCarta = numeroCarta + "_of_hearts";   // Define as cartas da linha 1 como naipe copas (hearts)

        Sprite s1 = (Sprite)(Resources.Load<Sprite>(nomeDaCarta));  // Define a sprite para as cartas com base no nome da varia�vel nomeDaCarta
                                                                    // (que bate com os nomes das sprites em resources)
        print("S1: " + s1);                             // Imprime o nome da carta final no console
        //GameObject.Find("" + rank).GetComponent<Tile>().setCartaOriginal(s1); 
        //GameObject.Find("" + valor).GetComponent<Tile>().setCartaOriginal(s1);
        GameObject.Find("" + linha + "_" + valor).GetComponent<Tile>().SetCartaOriginal(s1);    // Encontra o GameObject relacionado com a carta definida anteriormente
    }

    public int[] CriaArrayEmbaralhado()                                                 // Cria um array embaralhado
    {
        int[] novoArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};        // Cria um array com 13 n�meros
        int temp;                                                                       // Cria um inteiro temp
        for (int t = 0; t < 13; t++)                                                    // Para cada valor do array
        {
            temp = novoArray[t];                                                        // Define o valor temp como o valor atual do array
            int r = Random.Range(t, 13);                                                // Define uma vari�vel r como um valor aleat�rio indo de t at� 13
            novoArray[t] = novoArray[r];                                                // Troca a posi��o t com a posi��o r   
            novoArray[r] = temp;                                                        // Define a posi��o r como o inteiro temp
        }
        return novoArray;                                                               // Retorna o novoArray
    }

    public void CartaSelecionada(GameObject carta)          // Fun��o utilizada ao clicar na carta/tile                                
    {
        if(!primeiraCartaSelecionada)                       // Caso exista uma primeira carta selecionada
        {
            string linha = carta.name.Substring(0, 1);      // Define a string linha como uma substring do GameObject carta pegando o nome na posi��o 0 com 1 de tamanho    
            primeiraCartaSelecionada = true;                // Define a vari�vel primeiraCartaSelecionada como true
            carta1 = carta;                                 // Define o valor de carta1 como o GameObject carta 
            carta1.GetComponent<Tile>().RevelaCarta();      // Revela a carta selecionada
        }
        else if (primeiraCartaSelecionada && !segundaCartaSelecionada)      // Caso j� exista uma primeira carta selecionada e n�o uma segunda
        {
            string linha = carta.name.Substring(0, 1);      // Define a string linha como uma substring do GameObject carta pegando o nome na posi��o 0 com 1 de tamanho    
            segundaCartaSelecionada = true;                 // Define a vari�vel segundaCartaSelecionada como true
            carta2 = carta;                                 // Define o valor de carta2 como o GameObject carta 
            carta2.GetComponent<Tile>().RevelaCarta();      // Revela a carta selecionada
            VerificaCartas();                               // Chama a fun��o que VerificaCartas
        }
    }

    public void VerificaCartas()                            // Dispara o timer e atualizas as tentativas
    {
        DisparaTimer();                                     // Chama a fun��o que inicia o timer
        numTentativas++;                                    // Adiciona 1 unidade � vari�vel numTentativas
        UpdateTentativas();                                 // Atualiza na tela o valor de tentativas
    }

    public void DisparaTimer()                              // Atualiza as vari�veis timerPausado e timerAcionado para iniciar o timer
    {
        timerPausado = false;                               // Define timerPausado como false
        timerAcionado = true;                               // Define timerAcionado como true
    }

    void UpdateTentativas()
    {
        // Encontra o GameObject "numTentativas" e atualiza ele com base na vari�vel numTentativas
        GameObject.Find("numTentativas").GetComponent<Text>().text = "Tentativas: " + numTentativas;        
    }
}
