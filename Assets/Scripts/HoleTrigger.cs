using System.Collections;
using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    public Transform cameraT;
    public Transform canvasT;

    void Start()
    {
        
    }

    void Update()
    {
        canvasT.LookAt(canvasT.position - (cameraT.position - canvasT.position));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Made Hole");
        canvasT.gameObject.SetActive(true);
        StartCoroutine("DisableHole");
    }

    IEnumerator DisableHole()
    {
        yield return new WaitForSeconds(5);

        canvasT.gameObject.SetActive(false);
    }
}
