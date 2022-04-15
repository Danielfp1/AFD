using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButttonPlus : MonoBehaviour
{
    public GameObject estadoPrefab;
    public GameObject workspace;
    public GameObject enunciado;
 
   public void NovoEstado()
    {
        if (!(workspace.GetComponent<Workspace>().GetQuantosEstados() == 20))
        {
            int i = 0;
            GameObject estadoObj = Instantiate(estadoPrefab, workspace.transform);
            string nometemp = "Q";
            nometemp += i.ToString();
            while (workspace.GetComponent<Workspace>().TemONome(nometemp))
            {
                nometemp = "Q";
                nometemp += i.ToString();;
                i++;
            }
            string[] nomesDosEstados = workspace.GetComponent<Workspace>().GetNomeDosEstados();
            estadoObj.GetComponent<Estado>().SetNomeDoEstado(nometemp);
            workspace.GetComponent<Workspace>().AddEstado(estadoObj);
            enunciado.GetComponent<Enunciado>().AtulizarEstados();
        }
        else
        {
            SSTools.ShowMessage("Não é possivel adicionar mais estados", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }
}
