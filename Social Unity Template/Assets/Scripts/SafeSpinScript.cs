using System.Collections.Generic;
using UnityEngine;

public class SafeSpinScript : MonoBehaviour
{
    //[SerializeField] private float amplitude = 1.0f;

    [SerializeField] private float rotationSpeed = 50f;
    public List<GameObject> blackList;

    //[SerializeField] private float frequency = 0.2f;
    private Safe _safe;

    // Update is called once per frame

    private void Start()
    {
        blackList = new List<GameObject>();
        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}