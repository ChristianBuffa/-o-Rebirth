using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Attractor : MonoBehaviour
{
    [SerializeField] private float attractionForce;
    [SerializeField] private float attractionDistance;
    [SerializeField] private bool hasAttraction;
    public enum attractorPolarity
    {
        POSITIVE,
        NEGATIVE,
    }

    public attractorPolarity polarity;

    private float distanceFromAttractor;
    private Rigidbody2D rb;
    
    private const float G = 6.674f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        Attractor[] attractors = FindObjectsOfType<Attractor>();

        foreach(Attractor attractedObj in attractors)
        {
            if (attractedObj.polarity != this.polarity && hasAttraction)
            {
                Attract(attractedObj);
            }
            else if(attractedObj.polarity == this.polarity && hasAttraction)
            {
                Repel(attractedObj);
            }
        }
    }

    private void Attract(Attractor objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract.rb;

        Vector2 direction = rb.position - rbToAttract.position; 
        distanceFromAttractor = direction.magnitude;

        //curveModifier = curve.Evaluate(distanceFromAttractor / attractionDistance);
        float forceMagnitude = G * (attractionForce * rbToAttract.mass) / distanceFromAttractor;
        
        Vector3 force = direction.normalized * forceMagnitude;

        if (distanceFromAttractor < attractionDistance)
        {
            rbToAttract.AddForce(force);    
        }
    }

    private void Repel(Attractor objToRepel)
    {
        Rigidbody2D rbToRepel = objToRepel.rb;

        Vector2 direction = rb.position - rbToRepel.position; 
        distanceFromAttractor = direction.magnitude;

        //curveModifier = curve.Evaluate(distanceFromAttractor / attractionDistance);
        float forceMagnitude = G * (attractionForce * rbToRepel.mass) / distanceFromAttractor;
        
        Vector3 force = -direction.normalized * forceMagnitude;

        if (distanceFromAttractor < attractionDistance)
        {
            rbToRepel.AddForce(force);    
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attractionDistance);
    }
}
