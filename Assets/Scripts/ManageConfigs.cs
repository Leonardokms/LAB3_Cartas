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

    // Recupera qual foi a opção selecionada pelo jogador e define as configurações que serão utilizadas no jogo
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

    // Configuração que mostrará apenas cartas pretas no jogo, mostrando duas linhas sem alternância do tipo da traseira da carta
    void ConfigPretas()
    {
        tiposCartasParaLinhas.Add("_of_clubs");
        tiposCartasParaLinhas.Add("_of_spades");

        SetQuantidadeLinhas(2);

        PlayerPrefs.SetInt("alternarBack", 0);
    }

    // Configuração que mostrará apenas cartas vermelhas no jogo, mostrando duas linhas sem alternância do tipo da traseira da carta
    void ConfigVermelhas()
    {
        tiposCartasParaLinhas.Add("_of_hearts");
        tiposCartasParaLinhas.Add("_of_diamonds");

        SetQuantidadeLinhas(2);

        PlayerPrefs.SetInt("alternarBack", 0);
    }

    // Configuração que mostrará as cartas pretas e vermelhas no jogo, mostrando duas linhas e com alternância do tipo da traseira da carta
    // Uma linha com traseira azul e outra com traseira vermelha
    void ConfigDoisBaralhos()
    {
        tiposCartasParaLinhas.Add("_of_clubs");
        tiposCartasParaLinhas.Add("_of_clubs");

        SetQuantidadeLinhas(2);

        PlayerPrefs.SetInt("alternarBack", 1);
    }

    // Configuração com todas as cartas. Será mostrado 4 linhas e todos com a mesma traseira
    void ConfigTodasCartas()
    {
        tiposCartasParaLinhas.Add("_of_hearts");
        tiposCartasParaLinhas.Add("_of_clubs");
        tiposCartasParaLinhas.Add("_of_diamonds");
        tiposCartasParaLinhas.Add("_of_spades");

        SetQuantidadeLinhas(4);

        PlayerPrefs.SetInt("alternarBack", 0);
    }

    // Método para armazenar a quantidade de linhas que será mostrada no jogo
    void SetQuantidadeLinhas(int total)
    {
        PlayerPrefs.SetInt("qtdLinhas", total);
    }

    // Armazena os tipos das cartas que serão utilizadas no jogo, como hearts, clubs etc
    void SetTiposCartas(List<string> tiposCartas)
    {
        PlayerPrefs.SetInt("qtdTipos", tiposCartas.Count);
        for (int i = 0; i < tiposCartas.Count; i++)
        {
            PlayerPrefs.SetString("tipo" + i, tiposCartas[i]);
        }
    }
}
