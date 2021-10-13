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

    public List<string> GetTiposCartas()
    {
        return tiposCartasParaLinhas;
    }

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

    void ConfigPretas()
    {
        tiposCartasParaLinhas.Add("_of_clubs");
        tiposCartasParaLinhas.Add("_of_spades");

        SetQuantidadeLinhas(2);

        PlayerPrefs.SetInt("alternarBack", 0);
    }

    void ConfigVermelhas()
    {
        tiposCartasParaLinhas.Add("_of_hearts");
        tiposCartasParaLinhas.Add("_of_diamonds");

        SetQuantidadeLinhas(2);

        PlayerPrefs.SetInt("alternarBack", 0);
    }

    void ConfigDoisBaralhos()
    {
        tiposCartasParaLinhas.Add("_of_clubs");
        tiposCartasParaLinhas.Add("_of_clubs");

        SetQuantidadeLinhas(2);

        PlayerPrefs.SetInt("alternarBack", 1);
    }

    void ConfigTodasCartas()
    {
        tiposCartasParaLinhas.Add("_of_hearts");
        tiposCartasParaLinhas.Add("_of_clubs");
        tiposCartasParaLinhas.Add("_of_diamonds");
        tiposCartasParaLinhas.Add("_of_spades");

        SetQuantidadeLinhas(4);

        PlayerPrefs.SetInt("alternarBack", 0);
    }


    void SetQuantidadeLinhas(int total)
    {
        PlayerPrefs.SetInt("qtdLinhas", total);
    }
    void SetTiposCartas(List<string> tiposCartas)
    {
        PlayerPrefs.SetInt("qtdTipos", tiposCartas.Count);
        for (int i = 0; i < tiposCartas.Count; i++)
        {
            PlayerPrefs.SetString("tipo" + i, tiposCartas[i]);
        }
    }
}
