using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelayTime = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip finish;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem finishParticles;

    AudioSource audioSource;

    bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }
    
    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning) { return; }

        switch (other.gameObject.tag)
        {
            case "Finish":
                Debug.Log("You made it to the end!!!");
                FinishSequence();
                audioSource.PlayOneShot(finish);
                break;
            //case "Fuel":
                //Debug.Log("You have refueled.");
                //break;
            case "Friendly":
                Debug.Log("This thing is friendly.");
                break;
            default:
                Debug.Log("You Crashed!!!");
                StartCrashSequence();
                audioSource.PlayOneShot(crash);
                break;
        }
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    // Function loads next level on completing current level, or if final level completed reloads first level
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    
    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadDelayTime);
    }

    void FinishSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        finishParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadDelayTime);
    }
}
