using UnityEngine;

public class OpenCloseWindow : MonoBehaviour
{
    [Header("Window Setup")]
    [SerializeField] private GameObject window;
    [SerializeField] private RectTransform windowRectTransform;
    [SerializeField] private CanvasGroup windowCanvasGroup;

    public enum AnimateToDiretion
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [Header("Animation Setup")]
    [SerializeField] private AnimateToDiretion openDirection = AnimateToDiretion.Top;
    [SerializeField] private AnimateToDiretion closeDirection = AnimateToDiretion.Bottom;

    [Space]
    [SerializeField] private Vector2 distanceToAnimate = new Vector2(500, 500);
    [SerializeField] private AnimationCurve animatingCurve = AnimationCurve.EaseInOut(0, 0, 5, 5);
    [Range(0, 1f)][SerializeField] private float animationDuration = 0.5f;

    //private fields
    private bool _labOpen;
    private Vector2 _initialPosition;
    private Vector2 _currentPosition;


    private Vector2 _upOffset;
    private Vector2 _downOffset;
    private Vector2 _rightOffset;
    private Vector2 _leftOffset;

    private Coroutine _animationCoroutine;


    private void Start()
    {
        _initialPosition = window.transform.position;

        InitialOffsetPositions();
    }

    private void InitialOffsetPositions()
    {
        _upOffset = new Vector2(0, distanceToAnimate.y);
        _downOffset = new Vector2(0, -distanceToAnimate.y);
        _rightOffset = new Vector2(-distanceToAnimate.x, 0);
        _leftOffset = new Vector2(distanceToAnimate.x, 0);
    }

    // [ContextMenu("Toggle Open Close")]


    private void OnValidate()
    {
        if (window != null)
        {
            windowRectTransform = window.GetComponent<RectTransform>();
            windowCanvasGroup = window.GetComponent<CanvasGroup>();
        }

        distanceToAnimate.x = Mathf.Max(0, distanceToAnimate.x);
        distanceToAnimate.y = Mathf.Max(0, distanceToAnimate.y);

    }
}

//  Code references:
// 1) Title:How to animate your UI with code in Unity | UI Tweening
//    Author:Christina Creates Games
//    Date: 28/08/2025
//    Availiability: https://www.youtube.com/watch?v=AaudFyM3KV0

//This gave me the understanding of the basics of tweening and how to make use of move event based code .