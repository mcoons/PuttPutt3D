using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public lb_BirdController birdController;
    public GameObject puttAudio;
    public GameObject holeAudio;
    public GameObject bounceAudio;

    public AudioSource audioSource;
    public AudioClip[] audioSources;

    Vector3 tmpPosition = new Vector3(0,0,0);
    int x, z;

    void Start()
    {
        EventManager.Instance.OnGameStateChange.AddListener(HandleOnGameStateChange);
        StartCoroutine("BackgroundSounds");
    }

    protected void OnDestroy()
    {
        EventManager.Instance.OnGameStateChange.RemoveListener(HandleOnGameStateChange);
        StopAllCoroutines();
    }

    void HandleOnGameStateChange(GameManager.State newState)
    {
        if (newState == GameManager.State.Moving)
        {
            StartCoroutine("PlayPutt");
        }

        if (newState == GameManager.State.Hole)
        {
            StartCoroutine("PlayHole");
        }
    }

    IEnumerator PlayPutt()
    {
        puttAudio.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        puttAudio.SetActive(false);
    }

    IEnumerator PlayHole()
    {
        holeAudio.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        holeAudio.SetActive(false);
    }

    IEnumerator PlayBounce()
    {
        bounceAudio.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        bounceAudio.SetActive(false);
    }

    IEnumerator BackgroundSounds()
    {
        while (true)
        {
            if (birdController.idealNumberOfBirds > 0)
            {
                yield return new WaitForSeconds(Random.Range(10, 20));
                x = Random.Range(-200, 100);
                z = Random.Range(-50, 250);
                tmpPosition.x = x;
                tmpPosition.z = z;
                transform.position = tmpPosition;
                audioSource.clip = audioSources[Random.Range(0, audioSources.Length)];
                audioSource.Play();
            }
            else
            {
                yield return null;
            }
        }
    }
}

