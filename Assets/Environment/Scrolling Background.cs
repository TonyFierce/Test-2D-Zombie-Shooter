using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Transform player;
    public float parallaxEffectMultiplier = 0.5f;

    public float length;
    private float _startPosition;

    void Start()
    {
        _startPosition = transform.position.x; // Store the starting position of the background
    }

    void Update()
    {
        // Calculate parallax effect based on player movement
        float temp = (player.position.x * (1 - parallaxEffectMultiplier));
        float distance = (player.position.x * parallaxEffectMultiplier);

        // Move the background based on player's movement
        transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);

        // Reposition the background if it moves out of view
        if (temp > _startPosition + length)
        {
            _startPosition += length * 2; // Move it to the right of the next background
        }
        else if (temp < _startPosition - length)
        {
            _startPosition -= length * 2; // Move it to the left of the next background
        }
    }
}
