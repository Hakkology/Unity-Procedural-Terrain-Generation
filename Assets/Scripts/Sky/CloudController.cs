using UnityEngine;

public class CloudController : MonoBehaviour
{
    ParticleSystem cloudSystem;
    public Color colour;
    public Color lining;
    bool painted = false;
    public int numberOfParticles;
    public float minSpeed;
    public float maxSpeed;
    public float distance;
    Vector3 startPosition;
    float speed;

    void Start() {

        cloudSystem = this.GetComponent<ParticleSystem>();
        Spawn();
    }

    void Spawn()
    {

        //extend the range of the scale on either side of the manager centre
        float xPos = Random.Range(-0.5f, 0.5f);
        float yPos = Random.Range(-0.5f, 0.5f);
        float zPos = Random.Range(-0.5f, 0.5f);
        transform.localPosition = new Vector3(xPos, yPos, zPos);
        speed = Random.Range(minSpeed, maxSpeed);
        startPosition = transform.position;
    }

    void Paint()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[cloudSystem.particleCount];
        cloudSystem.GetParticles(particles);
        if (particles.Length > 0)
        {

            for (int i = 0; i < particles.Length; ++i)
            {
                float t = i / (float)particles.Length;
                particles[i].startColor = Color.Lerp(lining, colour, t);
            }
            painted = true;
            cloudSystem.SetParticles(particles, particles.Length);
        }
    }
    
    void Paintv2()
    {
        var particles = new ParticleSystem.Particle[cloudSystem.particleCount];
        cloudSystem.GetParticles(particles);

        if (particles.Length == 0) return;

        for (int i = 0; i < particles.Length; i++)
        {
            float t = i / (float)(particles.Length - 1);
            Color col = Color.Lerp(lining, colour, t);
            col.a = 1f; // Alfa sabit
            particles[i].startColor = col;
        }

        cloudSystem.SetParticles(particles, particles.Length);
        painted = true;
    }


    void Update()
    {
        // if (!painted) Paint();
        transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
        if (Vector3.Distance(this.transform.position, startPosition) > distance) Spawn();
    }


}