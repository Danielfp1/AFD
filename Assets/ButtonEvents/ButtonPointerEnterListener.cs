/*--------------------------------------
   Email  : hamza95herbou@gmail.com
   Github : https://github.com/herbou
----------------------------------------*/

using UnityEngine ;
using UnityEngine.Events ;
using UnityEngine.EventSystems ;
using UnityEngine.UI ;

[RequireComponent (typeof(Button))]
public class ButtonPointerEnterListener : MonoBehaviour,IPointerEnterHandler {

   public UnityEvent onPointerEnter ;

   private Button button ;

   private void Awake () {
      button = GetComponent<Button> () ;
   }

   public void OnPointerEnter (PointerEventData eventData) {
      if (button.interactable && !object.ReferenceEquals (onPointerEnter, null))
         onPointerEnter.Invoke () ;
   }

}