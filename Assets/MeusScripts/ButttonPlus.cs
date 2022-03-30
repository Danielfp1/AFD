using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButttonPlus : MonoBehaviour
{
    public GameObject estadoPrefab;
    public GameObject workspaceCanvas;
 
    // Update is called once per frame

   public void NovoEstado()
    {
        SSTools.ShowMessage("Novo Estado Adicionado", SSTools.Position.bottom, SSTools.Time.threeSecond);
        GameObject estadoObj = Instantiate(estadoPrefab,workspaceCanvas.transform);
        string nometemp = "Q";
        nometemp = nometemp + (workspaceCanvas.GetComponent<Workspace>().getQuantosEstados().ToString());
        estadoObj.GetComponent<Estado>().setNomeDoEstado(nometemp);
        workspaceCanvas.GetComponent<Workspace>().addEstado(estadoObj);

    }
}
