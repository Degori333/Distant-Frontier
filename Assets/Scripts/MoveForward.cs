using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private readonly float speed = 30.0f;
    [SerializeField] private readonly float horizontalBoundry = 30.0f;
    [SerializeField] private readonly float verticalBoundry = 20.0f;

    // Update is called once per frame
    void Update()
    {
        // Move object forward
        transform.Translate(Vector3.up * Time.deltaTime * speed);

        // Destroy object that goes too far
        if (transform.position.x > horizontalBoundry || transform.position.x < -horizontalBoundry ||
            transform.position.z > verticalBoundry || transform.position.z < -verticalBoundry)
        {
            Destroy(gameObject);
        }
    }
}
