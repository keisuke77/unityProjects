using UnityEngine;
 
public class GetClickedGameObject : MonoBehaviour {
 
 public static GameObject clickedGameObject;
 public GameObject effect;
 public bool _2D;
    void Update () {
 
        if (Input.GetMouseButtonDown(0)) {
 
            clickedGameObject = null;
 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_2D)
            {
                 RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
 
            if (hit2d) {
                clickedGameObject = hit2d.transform.gameObject;

            } 
            }else
            {
                  RaycastHit hit = new RaycastHit();
 
            if (Physics.Raycast(ray, out hit)) {
                clickedGameObject = hit.collider.gameObject;
            }
            }
           
effect.Instantiate(clickedGameObject.transform);
            
 
            Debug.Log(clickedGameObject);
        }
    }
}