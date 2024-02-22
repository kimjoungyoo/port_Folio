using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitEffectDestroy : MonoBehaviour
{
    private float particleDuration;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem particleSystem = this.GetComponent<ParticleSystem>();
        particleDuration = particleSystem.main.duration;
        Invoke("DestroyParticleEffect", particleDuration);
    }
    private void DestroyParticleEffect()
    {
        Destroy(gameObject);
    }
}
