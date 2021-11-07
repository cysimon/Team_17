using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRespond : MonoBehaviour
{
    private void Update()
    {
        Unit thisUnit = this.gameObject.GetComponent<Unit>();
        if (Input.GetMouseButtonDown(0) && thisUnit.currentArmor>0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Something was clicked!");
                //EventBus.Publish<clickEvent>(new clickEvent());
            }
        }
    }

}
