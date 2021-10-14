using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageConfigs : MonoBehaviour
{
    List<string> tiposCartasParaLinhas;
    // Start is called before the first frame update
    void Start()
    {
        tiposCartasParaLinhas = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Recupera qual foi a op��o selecionada pelo jogador e define as configura��es que ser�o utilizadas no jogo
    public void SetSettings()
    {
        Toggle toggle = GameObject.Find("groupConfigs")
            .GetComponent<ToggleGroup>()
            .ActiveToggles()
            .First();

        if (toggle.name == "checkPretas")
            ConfigPretas();
        else if (toggle.name == "checkVermelhas")
            ConfigVermelhas();
        else if (toggle.name == "checkDoisBaralhos")
            ConfigDoisBaralhos();
        else if (toggle.name == "checkTodasCartas")
            ConfigTodasCartas();

        SetTiposCartas(tiposCartasParaLinhas);
    }

    // Configura��o que mostrar� apenas cartas pretas no jogo, mostrando duas linhas sem altern�ncia do tipo da traseira da carta
    void ConfigPretas()
    {
        tiposCartasParaLinhas.Add("_of_clubs");
        tiposCartasParaLinhas.Add("_of_spades");

        SetQuantidadeLinhas(2);

        PlayerPrefs.SetInt("alternarBack", 0);
    }

    // Configura��o que mostrar� apenas cartas vermelhas no jogo, mostrando duas linhas sem altern�ncia do tipo da traseira da carta
    void ConfigVermelhas()
    {
        tiposCartasParaLinhas.Add("_of_hearts");
        tiposCartasParaLinhas.Add("_of_diamonds");

        SetQuantidadeLinhas(2);

        PlayerPrefs.SetInt("alternarBack", 0);
    }

    // Configura��o que mostrar� as cartas pretas e vermelhas no jogo, mostrando duas linhas e com altern�ncia do tipo da traseira da carta
    // Uma linha com traseira azul e outra com traseira vermelha
    void ConfigDoisBaralhos()
    {
        tiposCartasParaLinhas.Add("_of_clubs");
        tiposCartasParaLinhas.Add("_of_clubs");

        SetQuantidadeLinhas(2);

        PlayerPrefs.SetInt("alternarBack", 1);
    }

    // Configura��o com todas as cartas. Ser� mostrado 4 linhas e todos com a mesma traseira
    void ConfigTodasCartas()
    {
        tiposCartasParaLinhas.Add("_of_hearts");
        tiposCartasParaLinhas.Add("_of_clubs");
        tiposCartasParaLinhas.Add("_of_diamonds");
        tiposCartasParaLinhas.Add("_of_spades");

        SetQuantidadeLinhas(4);

        PlayerPrefs.SetInt("alternarBack", 0);
    }

    // M�todo para armazenar a quantidade de linhas que ser� mostrada no jogo
    void SetQuantidadeLinhas(int total)
    {
        PlayerPrefs.SetInt("qtdLinhas", total);
    }

    // Armazena os tipos das cartas que ser�o utilizadas no jogo, como hearts, clubs etc
    void SetTiposCartas(List<string> tiposCartas)
    {
        PlayerPrefs.SetInt("qtdTipos", tiposCartas.Count);
        for (int i = 0; i < tiposCartas.Count; i++)
        {
            PlayerPrefs.SetString("tipo" + i, tiposCartas[i]);
        }
    }
}
