using UnityEngine;

public class obstacle : MonoBehaviour
{

    public float speed = 5f;
    public float moveDistance = 10f;
    public Transform player;
    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
        {
            // Reset the position back to the start
            transform.position = startPosition;
        }

        if (player.position.x > transform.position.x) 
        {
            transform.position = startPosition;
        }

    }

}
        
