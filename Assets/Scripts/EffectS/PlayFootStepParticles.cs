using UnityEngine;

public class PlayFootStepParticles : MonoBehaviour
{
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
        ObjectPool.instance.SpawnFootprintEffect(leftFoot.transform.position, leftFoot.transform.rotation);
        Invoke(nameof(Allow), .1f);
    }

    public void PlayRightWalkingParticle()
    {
        if (!canPlay || leftNext) return; 
        
        canPlay = false;
        leftNext = true;
        //Instantiate(walkingParticleSystem, rightFoot.transform.position, transform.rotation);
        ObjectPool.instance.SpawnFootprintEffect(rightFoot.transform.position, rightFoot.transform.rotation);

        Invoke(nameof(Allow), .1f);
    }

    void Allow()
    {
        canPlay = true;
    }
}