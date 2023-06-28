using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_DragCamera : MonoBehaviour
{
   private float deltaX;
   private float deltaY;

   private void LateUpdate()
   {
      if (Input.touchCount > 0)
      {
         Touch touch = Input.GetTouch(0);
         Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

         switch (touch.phase)
         {
            case TouchPhase.Began:
               deltaX = touchPos.x - transform.position.x;
               deltaY = touchPos.y - transform.position.z;
               break;
            
            case TouchPhase.Moved:
               transform.position = Vector3.MoveTowards(transform.position,
                  new Vector3(touchPos.x - deltaX, transform.position.y, touchPos.y - deltaY), 1 * Time.deltaTime);
               break;
            
            case TouchPhase.Ended:
               break;
         }
      }
   }
}
