/*--------------------------------------
   Email  : hamza95herbou@gmail.com
   Github : https://github.com/herbou
----------------------------------------*/

using UnityEngine ;
using UnityEngine.Events ;
using UnityEngine.EventSystems ;
using UnityEngine.UI ;

[RequireComponent (typeof(Button))]
public class ButtonPointerExitListener : MonoBehaviour,IPointerExitHandler {

   public UnityEvent onPointerExit ;

   private Button button ;

   private void Awake () {
      button = GetComponent<Button> () ;
   }

   public void OnPointerExit (PointerEventData eventData) {
      if (button.interactable && !object.ReferenceEquals (onPointerExit, null))
         onPointerExit.Invoke () ;
   }

}