using UnityEngine;

public class PlayFootStepParticles : MonoBehaviour
{
    public GameObject walkingParticleSystem;
    public ParticleSystem shoeSoleParticleSystem;
    public GameObject leftFoot;
    public GameObject rightFoot;

    private bool canPlay = true;
    private bool leftNext = true; 
    public void PlayLeftWalkingParticle()
    {
        if (!canPlay || !leftNext) return; 
        
        canPlay = false;
        leftNext = false;
        //Instantiate(walkingParticleSystem, leftFoot.transform.position, transform.rotation);
        Instantiate(shoeSoleParticleSystem, leftFoot.transform.position, transform.rotation);
        Invoke(nameof(Allow), .1f);
    }

    public void PlayRightWalkingParticle()
    {
        if (!canPlay || leftNext) return; 
        
        canPlay = false;
        leftNext = true;
        //Instantiate(walkingParticleSystem, rightFoot.transform.position, transform.rotation);
        Instantiate(shoeSoleParticleSystem, rightFoot.transform.position, transform.rotation);
        Invoke(nameof(Allow), .1f);
    }

    void Allow()
    {
        canPlay = true;
    }
}