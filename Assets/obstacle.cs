using System.Collections;
using UnityEngine;

public class obstacle : MonoBehaviour
{

    public float speed = 5f;
    public float moveDistance = 10f;
    public Transform player;
    private Vector3 startPosition;
    private bool isResetting = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(5, 10); 
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.right * speed * Time.deltaTime);



        if (player.position.z > transform.position.z && !isResetting)
        {
            StartCoroutine(ResetAfterDelay(1f)); 
        }

    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        isResetting = true;
        yield return new WaitForSeconds(delay);
        transform.position = startPosition;
        isResetting = false;
    }

}
        
