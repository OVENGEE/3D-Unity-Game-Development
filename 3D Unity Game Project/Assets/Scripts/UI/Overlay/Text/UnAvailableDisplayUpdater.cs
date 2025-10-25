using UnityEngine;
using TMPro;
public class UnAvailableDisplayUpdater : MonoBehaviour
{
    private Camera camera;
    private TextMeshProUGUI unavailableText;
    void Start()
    {
        unavailableText = GetComponent<TextMeshProUGUI>();
        camera = Camera.main;
    }

    private void OnEnable()
    {
        PanelController.OnUnAvailableGame += UpdateAvailabilityInfo;
    }

    private void OnDisable()
    {
        PanelController.OnUnAvailableGame -= UpdateAvailabilityInfo;
    }
    

    private void UpdateAvailabilityInfo()
    {
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray,out hit,5f))
        {
            if(hit.collider.TryGetComponent(out IGame game))
            {
                string message = $"{game.GameName} needs {game.RequiredTickets} tickets";
                unavailableText.text = message;
            }
        }
    }
    
}
