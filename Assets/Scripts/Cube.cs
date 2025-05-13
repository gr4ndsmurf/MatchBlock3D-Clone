using UnityEngine;

public class Cube : MonoBehaviour
{
    public int value;
    public GameObject nextCube;
    public Rigidbody rb;
    public int ID;

    [SerializeField] private float instantiateSpeed = 1200;
    [SerializeField] private float speed = 3000;

    private bool hasMerged = false;

    void Awake()
    {
        ID = GetInstanceID();
        rb = GetComponent<Rigidbody>();
    }

    public void CreatingForce()
    {
        rb.AddForce((Vector3.up + Vector3.forward) * instantiateSpeed);
    }

    public void SendCube()
    {
        rb.AddForce(Vector3.forward * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasMerged) return; // Zaten birleştiyse bir daha işlem yapma

        if (collision.gameObject.CompareTag("Cube"))
        {
            rb.freezeRotation = false;
            if (collision.gameObject.TryGetComponent(out Cube cube))
            {
                if (cube.value == value && !cube.hasMerged)
                {
                    // Yalnızca daha büyük ID'li olan yeni küp oluşturur
                    if (ID < cube.ID) return;

                    hasMerged = true;
                    cube.hasMerged = true;

                    // Yeni küp oluştur
                    if (nextCube != null)
                    {
                        GameObject temp = Instantiate(nextCube, transform.position, Quaternion.identity);
                        if (temp.TryGetComponent(out Cube newCube))
                        {
                            newCube.CreatingForce();
                        }
                    }

                    Destroy(cube.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}
