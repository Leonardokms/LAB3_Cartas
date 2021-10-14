using System;
using System.Linq;
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

    
    int quantidadeLinhas;                                               // Quantidade de linhas que ser� mostrada na Scene
    int quantidadeCartas = 13;                                          // Quantidade de cartas para cada linha
    List<Carta> cartasEscolhidas;                                       // Lista das cartas escolhidas pelo jogador
    List<string> tiposCartas;                                           // Tipo da carta do jogador (clubs, hearts etc.)
    bool alternarBack;                                                  // Op��o para habilitar a traseira da carta ficar diferente entre as linhas
    public class Carta // GameObjects das cartas selecionadas pelo jogador
    {
        public string Linha { get; set; } // Posi��o da linha da carta
        public bool Selecionada { get; set; } // Indicador de que a carta foi selecionada
        public GameObject GameObject { get; set; } // O GameObject atrelado � carta
        public Carta()
        {
            Selecionada = false;
        }

        // Reinicializa os atributos da carta ao final da rodada
        // Ou seja, no inicio da rodada, o jogador n�o selecionou nenhuma carta, por isso tudo est� como vazio, false ou nulo
        public void Reset()
        {
            Linha = string.Empty;
            Selecionada = false;
            GameObject = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetConfiguracao();  // Estabelece a configura��o escolhida inicialmente pelo jogador
        MostraCartas(); // Mostra as cartas na Scene
        UpdateTentativas(); // Atualiza a quantidade de tentativas na Scene
        somOK = GetComponent<AudioSource>();                            // Retorna o componente somOK        
        ultimoJogo = PlayerPrefs.GetInt("Jogadas", 0);                  // Define a vari�vel ultimoJogo como a PlayerPrefs "Jogadas"
        recorde = PlayerPrefs.GetInt("Recorde", 0);                     // Define a vari�vel recorde como a PlayerPrefs "Recorde"
        GameObject.Find("ultimaJogada").GetComponent<Text>().text = "�ltimo Jogo: " + ultimoJogo;       // Encontra o GameObject "ultimaJogada" e define seu texto com base na vari�vel ultimoJogo
        GameObject.Find("recorde").GetComponent<Text>().text = "Recorde: " + recorde;                   // Encontra o GameObject "recorde" e define seu texto com base na vari�vel recorde
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (timerAcionado) // Se o timer � acionado, ent�o come�a a contagem para remover ou esconder as cartas
            {
                timer += Time.deltaTime; // Soma no timer os segundos do ultimo frame

                if (timer > 0.5) // Quando o timer passar de 0.5s ser� atualizada as cartas 
                {
                    timerAcionado = false; // Desbilita a contagem do timer 
                    timer = 0; // Reseta os segundos do timer

                    string tag = cartasEscolhidas.First().GameObject.tag; // Pega a primeira tag das cartas escolhidas do jogador
                    bool todasIguais = cartasEscolhidas.All(c => c.GameObject.tag == tag); // Se todas as cartas forem iguais a primeira carta, fica verdadeiro, ou seja, todas elas possuem a mesma tag

                    foreach (var c in cartasEscolhidas) // Percorre a lista das cartas selecionadas pelo jogador
                    {
                        if (todasIguais) // Se todas as cartas forem iguais, elas ser�o removidas da Scene
                            Destroy(c.GameObject);
                        else                        
                            EsconderCarta(c.GameObject); // Caso contr�rio, elas ser�o escondidas

                        c.Reset(); // Reinicializa os atributos da carta selecionada
                    }

                    if (todasIguais) // Se todas forem iguais, aumenta a quantidade de acertos e toca som de confirma��o
                    {
                        numAcertos++;
                        somOK.Play();
                    }

                    UpdateTentativas(); // Para cada rodada, atualiza a quantidade de tentativas feitas pelo jogador
                }
            }

            if (numAcertos == quantidadeCartas) // Se a quantidade de acertos forem iguais a quantidade de cartas nas linhas, finaliza a partida
            {
                PlayerPrefs.SetInt("Jogadas", numTentativas);       // Define a PlayerPref "Jogadas" como a vari�vel numTentativas
                if (numTentativas < recorde || recorde == 0)         // Se ultrapassou o recorde. 
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
        catch (Exception ex)
        {
            print(ex.Message);
        }

    }

    // Inicializa as variaveis de configura��o de acordo com as op��es escolhidas pelo jogador
    void SetConfiguracao()
    {
        tiposCartas = RecuperaTiposCartas(); // Recupera  os tipos das cartas dependendo da configura��o do jogo (hearts, clubs etc)
        quantidadeLinhas = PlayerPrefs.GetInt("qtdLinhas"); // Pega a quantidade de linhas dado a op��o escolhida
        cartasEscolhidas = CriarCartas(quantidadeLinhas); // Cria o objeto que lida com as cartas selecionadas pelo jogador na partida 
        alternarBack = GetAlternarBack(); // Recupera a op��o de alternar o sprite da traseira da carta dependendo da op��o escolhida pelo jogador
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

    /* Novos metodos - Refactory */

    // Monta uma lista dos tipos que ser�o usados na partida, podendo variar de acordo com a configura��o feita
    List<string> RecuperaTiposCartas()
    {
        List<string> tipos = new List<string>();

        int total = PlayerPrefs.GetInt("qtdTipos");

        for (int i = 0; i < total; i++)
        {
            string tipo = PlayerPrefs.GetString("tipo" + i);
            tipos.Add(tipo);
        }

        return tipos;
    }

    // Cria a lista para armazenar as cartas que ser�o selecionadas pelo jogador
    List<Carta> CriarCartas(int total)
    {
        List<Carta> cartas = new List<Carta>();

        for (int i = 0; i < total; i++)
            cartas.Add(new Carta());

        return cartas;
    }

    // Mostra as cartas na Scene
    void MostraCartas()
    {
        List<int[]> cartasEmbaralhadas = CriarCartasEmbaralhadas(); // Recupera as listas do indices das cartas de forma embaralhada

        // Para cada linha adiciona uma carta com determinado valor, que no caso foi embaralhado acima para n�o ter um padr�o muito claro
        for (int i = 0; i < quantidadeLinhas; i++) 
        {
            for (int j = 0; j < quantidadeCartas; j++)
                AdicionarUmaCarta(i, j, cartasEmbaralhadas[i][j]);
        }
    }

    // Deixa a traseira da carta na frente para esconde-la
    void EsconderCarta(GameObject carta)
    {
        if (alternarBack) // Se a op��o de altern�ncia das traseiras for ligado, ir� alternar os sprites da traseira entre as linhas 
        {
            // Dependendo da linha seleciona uma traseira azul ou vermelha
            string linha = carta.name.Substring(0, 1);
            int numeroLinha = Convert.ToInt32(linha);

            if (numeroLinha % 2 == 1)
                carta.GetComponent<Tile>().EscondeCarta("blue");
            else
                carta.GetComponent<Tile>().EscondeCarta("red");
            
            return;
        }

        // Se a op��o de altern�ncia estiver desligada, seleciona a traseira padr�o
        carta.GetComponent<Tile>().EscondeCarta();
    }

    // Retora a configura��o da altern�ncia da traseira, indicando se ela est� ligada ou n�o
    bool GetAlternarBack()
    {
        int alterna = PlayerPrefs.GetInt("alternarBack");

        if (alterna == 0)
            return false;

        return true;
    }

    // Cria o conjunto de cartas para cada linha que ser� apresentada
    List<int[]> CriarCartasEmbaralhadas()
    {
        List<int[]> cartas = new List<int[]>();
        for (int i = 0; i < quantidadeLinhas; i++)
            cartas.Add(CriarArrayEmbaralhado());

        return cartas;
    }

    // Utiliza o algoritmo de Fisher-Yates para criar um array como valores embaralhados
    int[] CriarArrayEmbaralhado()
    {
        int[] novoArray = Enumerable.Range(0, quantidadeCartas).ToArray();
        for (int i = 0; i < quantidadeCartas; i++)
        {
            int temp = novoArray[i];
            int r = UnityEngine.Random.Range(i, quantidadeCartas);
            novoArray[i] = novoArray[r];
            novoArray[r] = temp;
        }

        return novoArray;
    }

    // Estabelece a carta do baralho que ser� mostrada quando o jogador clicar nela
    void AdicionarUmaCarta(int linha, int rank, int valor)
    {
        GameObject c = ClonarCarta(linha, rank, valor);

        string nomeCarta = GetNomeCarta(linha, valor);

        Sprite s = Resources.Load<Sprite>(nomeCarta);
        GameObject.Find(c.name).GetComponent<Tile>().SetCartaOriginal(s);
        EsconderCarta(c);
    }

    // Cria um clone do Prefab Tile, para mostrar diversas cartas na Scene
    GameObject ClonarCarta(int linha, int rank, int valor)
    {
        Vector3 novaPosicao = GetPosicaoClone(linha, rank);

        GameObject c = Instantiate(carta, novaPosicao, Quaternion.identity);
        c.tag = valor.ToString();
        c.name = linha + "_" + rank + "_" + valor;

        return c;
    }

    // Calcula a posi��o que carta ficar� na Scene 
    Vector3 GetPosicaoClone(int linha, int indice)
    {
        float escalaX = carta.transform.localScale.x;
        float escalaY = carta.transform.localScale.y;

        float fatorEscalaX = (400 * escalaX) / 130.0f;
        float fatorEscalaY = (600 * escalaY) / 130.0f;

        float deslocamentoX = (indice - (quantidadeCartas / 2)) * fatorEscalaX;
        float deslocamentoY = (linha - (quantidadeLinhas / 2)) * fatorEscalaY;

        GameObject centro = GameObject.Find("centroDaTela");
        float posX = centro.transform.position.x + deslocamentoX;
        float posY = centro.transform.position.y + deslocamentoY;

        return new Vector3(posX, posY, 0);
    }

    // Recupera o nome da carta pelo o valor atribu�do � ela
    string GetNomeCarta(int linha, int valor)
    {
        string numeroCarta;

        if (valor == 0)
            numeroCarta = "ace";
        else if (valor == 10)
            numeroCarta = "jack";
        else if (valor == 11)
            numeroCarta = "queen";
        else if (valor == 12)
            numeroCarta = "king";
        else
            numeroCarta = (valor + 1).ToString();

        return numeroCarta + tiposCartas[linha];
    }

    // Quando o jogador selecionar uma carta, ela ser� revelada na Scene, al�m de ser adicionada a lista das cartas selecionadas do jogador, indicando a linha dela e o GameObject atrelado
    // Caso a quantidade de cartas for igual ao tamanho da lista, que no caso o seu tamanho tem a mesma quantidade de linhas, ent�o chama a fun��o de verificar cartas que validar� se as cartas foram iguais ou n�o
    public void CartaSelecionada(GameObject carta)
    {
        if (!CartaValida(carta)) return;

        int qtdCartasEscolhidas = 0;
        foreach (var escolhida in cartasEscolhidas)
        {
            qtdCartasEscolhidas += 1;
            if (!escolhida.Selecionada)
            {
                escolhida.Selecionada = true;
                escolhida.Linha = carta.name.Substring(0, 1);
                escolhida.GameObject = carta;
                escolhida.GameObject.GetComponent<Tile>().RevelaCarta();
                break;
            }
        }

        if (qtdCartasEscolhidas == cartasEscolhidas.Count)
            VerificaCartas();
    }

    // Valida se a carta escolhida pelo jogador � v�lida de entrar na lista das escolhidas
    // O nome dela deve ser diferente das que j� existem e n�o pode ser escolhida ap�s ter escolhido o par
    bool CartaValida(GameObject carta)
    {
        if (cartasEscolhidas == null || carta == null) return false;

        var cartasSelecionadas = cartasEscolhidas.Where(c => c.GameObject != null).ToList();

        return cartasSelecionadas.All(c => c.GameObject.name != carta.name) && !timerAcionado;
    }
}
