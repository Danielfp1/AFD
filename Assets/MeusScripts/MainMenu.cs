using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para mudar de scenes

public class MainMenu : MonoBehaviour
{
    public void MenuOptionListaDeExercicios()
    {
        SceneManager.LoadScene("ListaDeExercicios");
    }

    public void MenuOptionMeusProjetos()
    {
        SceneManager.LoadScene("MeusProjetos_Aluno");
    }

    public void FecharJogo()
    {
        Debug.Log("Fechando Aplicação");
        Application.Quit();  
    }

    public void Sair()
    {
        Debug.Log("Saindo");
        SceneManager.LoadScene("LoginECadastro");
    }

}
