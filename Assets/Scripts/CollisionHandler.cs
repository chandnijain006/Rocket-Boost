using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashSFX;
   [SerializeField] AudioClip successSFX;
   [SerializeField] ParticleSystem successParticles;
   [SerializeField] ParticleSystem crashParticles;
   AudioSource audioSource;
   bool isControllable = true;
   bool isColliadable = true;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RespondToDebugKeys();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!isControllable || !isColliadable){return;}

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Player is at the start point");
                break;

            case "Finish":
                StartNextLevelSequence();
                break;

            default:
                StartCrashSequence();
                break;
        }
    }

    void StartNextLevelSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }
    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = ++currentScene;
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        SceneManager.LoadScene(nextScene);
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void RespondToDebugKeys()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if(Keyboard.current.cKey.wasPressedThisFrame)
        {
            isColliadable = !isColliadable;
        }
    }
}
