using UnityEngine;

public class SwipeRotation : MonoBehaviour
{
    // Adjust this value to control the rotation speed
    public float rotationSpeed = 30.0f;

    public Transform objectToRotatr;

    private FixedTouchField touchField;

    private void Start()
    {
        touchField = GetComponent<FixedTouchField>();
    }

    private void Update()
    {
        // Check if the touch field is pressed
        if (touchField.Pressed)
        {
            // Get the horizontal movement from the touch field
            float swipeDistance = touchField.TouchDist.x;

            // Rotate the object based on the swipe direction
            if (swipeDistance > 0)
            {
                // Swipe from left to right
                objectToRotatr.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }
            else if (swipeDistance < 0)
            {
                // Swipe from right to left
                objectToRotatr.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
