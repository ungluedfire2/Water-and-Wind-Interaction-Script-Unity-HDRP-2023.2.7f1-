using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
[RequireComponent(typeof(Rigidbody))] 

public class waterAndWindInterction : MonoBehaviour
{
    [Header("Componets")]
    public Rigidbody rb;
    public WindZone wind;
    public WaterDeformer waterDeformer;
    public WaterSurface water;


    [Header("Variables")]
    public float depthBefSub;
    public float displacementAmt;

    [Min(1)]
    public int floaters; 

    public float waterDrag;
    public float waterAngluarDrag;
    
    WaterSearchParameters Search;
    WaterSearchResult SearchResult;

    private Vector3 windDirection;


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForceAtPosition(Physics.gravity / floaters, transform.position, ForceMode.Acceleration);
        Search.startPositionWS = transform.position;

        water.ProjectPointOnWaterSurface(Search, out SearchResult);


        if (transform.position.y < SearchResult.projectedPositionWS.y)
        {
            float displacementMulti = Mathf.Clamp01(SearchResult.projectedPositionWS.y - transform.position.y / depthBefSub) * displacementAmt;
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMulti, 0f), transform.position, ForceMode.Acceleration);
            rb.AddForce(displacementMulti * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddTorque(displacementMulti * -rb.angularVelocity * waterAngluarDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
    private void Update()
    {
        windDirection = wind.transform.forward;

        rb.velocity += windDirection * wind.windMain * Time.deltaTime;

        waterDeformer.transform.forward = -rb.velocity;
        waterDeformer.bowWaveElevation = wind.windMain / 10;
    }
}
