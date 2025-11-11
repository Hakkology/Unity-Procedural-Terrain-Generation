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


    void Update()
    {
        this.transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
        if (Vector3.Distance(this.transform.position, startPosition) > distance) Spawn();
    }


}