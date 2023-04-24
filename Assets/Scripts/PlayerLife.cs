using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] int lives = 3;
    bool dead = false;

    [SerializeField] Material fullLifeMaterial;
    [SerializeField] Material halfLifeMaterial;
    [SerializeField] Material lastLifeMaterial;
    [SerializeField] Text livesCount;
    [SerializeField] AudioSource obstacleSound;
    [SerializeField] AudioSource deathSound;

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void Update()
    {
        if (transform.position.y < -1f && !dead)
        {
            Die();
        }
        if (lives == 3)
        {
            meshRenderer.material = fullLifeMaterial;
        }
        else if (lives == 2)
        {
            meshRenderer.material = halfLifeMaterial;
        }
        else if (lives == 1)
        {
            meshRenderer.material = lastLifeMaterial;
        }
        livesCount.text = "Lives: " + lives;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            lives--;
            Destroy(other.gameObject);
            if (lives == 0)
            {
                Die();
            }else{
                obstacleSound.Play();

            }
        }
    }

    void Die()
    {
        lives = 0;
        dead = true;

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<PlayerMovement>().enabled = false;
        Invoke(nameof(ReloadLevel), 1f);
        deathSound.Play();
    }

    void ReloadLevel()
    {
        dead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
