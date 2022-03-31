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

    public void NovaTrans()
    {
        GameObject estado1 = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        workspaceCanvas.GetComponent<Workspace>().SetNovaTransFlag(true);
        //GameObject estado2 = workspaceCanvas.GetComponent<Workspace>().GetEstadoAlvo();
        //workspaceCanvas.GetComponent<Workspace>().SetNovaTransFlag(false);
        //Debug.Log(estado1.GetComponent<Estado>().getNomeDoEstado());
        //Debug.Log(estado2.GetComponent<Estado>().getNomeDoEstado());
    }

    public void Final()
    {
        GameObject estado = workspaceCanvas.GetComponent<Workspace>().GetEstadoAtual();
        if (!estado.transform.GetChild(2).gameObject.activeSelf)
        {
            estado.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            estado.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

}
