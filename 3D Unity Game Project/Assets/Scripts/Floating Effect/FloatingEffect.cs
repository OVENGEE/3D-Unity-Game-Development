using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [Tooltip("Transform to apply the floating motion to. If null, uses this GameObject's transform.")]
    public Transform target;

    [Tooltip("Use localPosition (recommended for imported/parented models).")]
    public bool useLocalPosition = true;

    [Tooltip("Which axes to apply the float motion to (1 = enabled).")]
    public Vector3 axis = new Vector3(0f, 1f, 0f);

    [Tooltip("Speed of the floating motion.")]
    public float speed = 2f;

    [Tooltip("Amplitude (height) of the floating motion).")]
    public float height = 0.5f;

    [Tooltip("Optional base offset added to the start position (local or world depending on useLocalPosition).")]
    public Vector3 baseOffset = Vector3.zero;

    private Vector3 startPosition;

    private void Awake()
    {
        if (target == null) target = transform;
        // capture the starting position in the chosen space
        startPosition = useLocalPosition ? target.localPosition : target.position;
        startPosition += baseOffset;
    }

    private void LateUpdate()
    {
        // compute the sin-based offset
        float y = Mathf.Sin(Time.time * speed) * height;
        Vector3 floatOffset = new Vector3(
            axis.x * y,
            axis.y * y,
            axis.z * y
        );

        if (useLocalPosition)
            target.localPosition = startPosition + floatOffset;
        else
            target.position = startPosition + floatOffset;
    }
}