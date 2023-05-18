using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TrajectoryRenderer : MonoBehaviour
{

    [SerializeField] Transform startPos;
    
    [SerializeField]  Camera cam;
    Vector3 endPos;
    [SerializeField] LineRenderer lineRenderer;
    private RaycastHit hit;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //stop player input while over UI
        
            
        if (Input.GetMouseButtonUp(0))
        {
            InputRelease();
            return;
        }
       
            if (Input.GetMouseButton(0))
            InputHold();
       

    }

   
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
            endPos.x = Mathf.Clamp(endPos.x, startPos.position.x-2.5f, startPos.position.x+2.5f);
            endPos.z = Mathf.Clamp(endPos.z, startPos.position.z - 2.5f, startPos.position.z + 2.5f);

        }
        endPos.y = startPos.position.y;
       
        lineRenderer.SetPosition(1, endPos);
      
    }
    void InputRelease()
    {
        if (Time.timeScale == 0)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        lineRenderer.positionCount = 0;
       float dist= Vector3.Distance(startPos.position, endPos);
        rb.AddForce((endPos-startPos.position) *dist,ForceMode.Impulse);
        
    }
}
