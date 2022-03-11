using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelayTime = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip finish;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();   
    }
    
    void OnCollisionEnter(Collision other) 
    {
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
    
    // todo Add Particle FX on Crash
    void StartCrashSequence()
    {
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadDelayTime);
    }
    void FinishSequence()
    {
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadDelayTime);
    }
}
