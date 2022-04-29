using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public GameObject puttAudio;
    public GameObject holeAudio;

    public AudioSource audioSource;
    public AudioClip[] audioSources;

    Vector3 tmpPosition = new Vector3(0,0,0);
    int x, z;

    void Start()
    {
        Debug.Log("Audio is subscribing");
        EventManager.Instance.OnGameStateChange.AddListener(HandleOnGameStateChange);
        StartCoroutine("BackgroundSounds");
    }

    void HandleOnGameStateChange(GameManager.State newState)
    {
        Debug.Log("In Audio Handler with new state of " + newState);
        if (newState == GameManager.State.Moving)
        {
            StartCoroutine("PlayPutt");
        }

        if (newState == GameManager.State.Win)
        {
            StartCoroutine("PlayHole");
        }
    }

    IEnumerator PlayPutt()
    {
        Debug.Log("In PlayPutt");
        puttAudio.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        puttAudio.SetActive(false);
    }

    IEnumerator PlayHole()
    {
        Debug.Log("In PlayHole");
        holeAudio.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        holeAudio.SetActive(false);
    }


    IEnumerator BackgroundSounds()
    {

        while (true)
        {

            yield return new WaitForSeconds(Random.Range(5, 10));
            x = Random.Range(-200, 100);
            z = Random.Range(-50, 250);
            tmpPosition.x = x;
            tmpPosition.z = z;
            transform.position = tmpPosition;
            audioSource.clip = audioSources[Random.Range(0, audioSources.Length)];
            audioSource.Play();


        }


    }

}





// using UnityEngine;
// using System.Collections;
 
// public class RandomSoundsScript : MonoBehaviour
//{

//    public AudioSource randomSound;

//    public AudioClip[] audioSources;

//    // Use this for initialization
//    void Start()
//    {

//        CallAudio();
//    }


//    void CallAudio()
//    {
//        Invoke("RandomSoundness", 10);
//    }

//    void RandomSoundness()
//    {
//        randomSound.clip = audioSources[Random.Range(0, audioSources.Length)];
//        randomSound.Play();
//        CallAudio();
//    }
//}