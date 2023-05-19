using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TrajectoryRenderer : MonoBehaviour
{
    //Used to control the players movement
    [Header("Editable Values")]
    [SerializeField] float maxMoveDistance;
    [SerializeField] float speed;
    [Header("References")]
    [SerializeField] Transform startPos;
    [SerializeField]  Camera cam;
    [SerializeField] LineRenderer lineRenderer;
    Vector3 endPos;
    private RaycastHit hit;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    { //stop player input while over UI
        
            
        if (Input.GetMouseButtonUp(0))
        {
            InputRelease();
            return;
        }
       
            if (Input.GetMouseButton(0))
            InputHold();
       

    }

   //when holding the mouse or your finger a line will be drawn from the player towards where you are holding.
    void InputHold()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
          
            Debug.Log(hit.transform.name);
            Debug.Log("hit");
            endPos = hit.point;
            endPos.x = Mathf.Clamp(endPos.x, startPos.position.x-maxMoveDistance, startPos.position.x+ maxMoveDistance);
            endPos.z = Mathf.Clamp(endPos.z, startPos.position.z - maxMoveDistance, startPos.position.z + maxMoveDistance);

        }
        endPos.y = startPos.position.y;
       
        lineRenderer.SetPosition(1, endPos);
      
    }
    //when releasing the mouse or your finger a force will be added to the player to shoot them towards where you were holding, the longer the distance of the line the stronger the force 
    void InputRelease()
    {
        if (Time.timeScale == 0)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        lineRenderer.positionCount = 0;
       float dist= Vector3.Distance(startPos.position, endPos);
        rb.AddForce((endPos-startPos.position) *dist*speed,ForceMode.Impulse);
        
    }
}
