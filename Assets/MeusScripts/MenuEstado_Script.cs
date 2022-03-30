using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEstado_Script : MonoBehaviour
{
    public GameObject workspaceCanvas;
    public GameObject menuEstadoObj;

    // Update is called once per frame


    public void OnMouseOver()
    {
        //Debug.Log("O mouse esta em cima do menu");
    }

    public void OnMouseExit()
    {
        menuEstadoObj.SetActive(false);
    }


    //Botões
    public void DeletarEstado()
    {
        GameObject estado = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        Destroy(estado);
        Debug.Log("Morre!");
        menuEstadoObj.SetActive(false);
    }

}
