using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para mudar de scenes

public class Menu_ListaDeExercicios : MonoBehaviour
{
    public void VoltarMenuPrincipal()
    {
        SceneManager.LoadScene(1);
    }
}
