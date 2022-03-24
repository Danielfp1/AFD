using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para mudar de scenes

public class MainMenu_Professor : MonoBehaviour
{
    public void MeusExercicios()
    {
        SceneManager.LoadScene(7);
    }
    public void Sair()
    {
        Debug.Log("Saindo");
        SceneManager.LoadScene(0);
    }
    public void CadastrarExercicios()
    {
        SceneManager.LoadScene(6);
    }
}
