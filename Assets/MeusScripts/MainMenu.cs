using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para mudar de scenes

public class MainMenu : MonoBehaviour
{
    public void MenuOptionListaDeExercicios()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FecharJogo()
    {
        Debug.Log("Fechando Aplicação");
        Application.Quit();  
    }

    public void Sair()
    {
        Debug.Log("Saindo");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
