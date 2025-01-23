using UnityEngine;

public class PlayFootStepParticles : MonoBehaviour
{
    public GameObject walkingParticleSystem;
    public ParticleSystem shoeSoleParticleSystem;
    public GameObject leftFoot;
    public GameObject rightFoot;
    
    public void PlayLeftWalkingParticle()
    {
        //Instantiate(walkingParticleSystem, leftFoot.transform.position, transform.rotation);
        Instantiate(shoeSoleParticleSystem, leftFoot.transform.position, transform.rotation);
    }

    public void PlayRightWalkingParticle()
    {
        //Instantiate(walkingParticleSystem, rightFoot.transform.position, transform.rotation);
        Instantiate(shoeSoleParticleSystem, rightFoot.transform.position, transform.rotation);
    }
}