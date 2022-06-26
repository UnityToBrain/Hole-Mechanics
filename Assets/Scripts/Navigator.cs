using System;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    private bool MoveByTouch;
    private Vector3 _mouseStartPos, PlayerStartPos;
    [SerializeField] [Range(0f,100f)]private float maxAcceleration;
    
    [Header("Gravity")] 
    
    [Range(1f,1000f)]public float power = 1f; // gravity power
    [Range(-10f, 10f)] public float upOrDown; // direction of gravity
    [Range(1f,10f)]public float forceRange = 1f; // range of gravity

    public ForceMode forceMode; // force type
    public LayerMask layerMask; // determines which layer should be affected by gravity
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MoveByTouch = true;
            
            Plane plane = new Plane(Vector3.up, 0f);
        
            float Distance;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (plane.Raycast(ray,out Distance))
            {
                _mouseStartPos = ray.GetPoint(Distance);
                PlayerStartPos = transform.position;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MoveByTouch = false;
        }
        
        if (MoveByTouch)
        {
            Plane plane = new Plane(Vector3.up, 0f);
            float Distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (plane.Raycast(ray,out Distance))
            {
                Vector3 MousePos = ray.GetPoint(Distance);
                Vector3 move = MousePos - _mouseStartPos;
                Vector3 navigator = PlayerStartPos + move;

                // navigator.x = Mathf.Clamp(navigator.x, -5f, 5f);
                // navigator.z = Mathf.Clamp(navigator.z, -5f, 5f);

                navigator = Vector3.ClampMagnitude(navigator, 10f);
                
                transform.position = Vector3.Lerp(transform.position,navigator,Time.deltaTime * maxAcceleration);
       
            }
        }
    }

    private void FixedUpdate()
    {
        Gravity(transform.position,forceRange,layerMask);
    }

    private void Gravity(Vector3 gravitySource, float range, LayerMask layerMask)
    {
        Collider[] objs = Physics.OverlapSphere(gravitySource, range, layerMask);

        for (int i = 0; i < objs.Length; i++)
        {
            Rigidbody rbs = objs[i].GetComponent<Rigidbody>();
            
            Vector3 forceDirection = new Vector3(gravitySource.x,upOrDown,gravitySource.z) - objs[i].transform.position;
            
            rbs.AddForceAtPosition(power * forceDirection.normalized,gravitySource);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,forceRange);
    }
}

