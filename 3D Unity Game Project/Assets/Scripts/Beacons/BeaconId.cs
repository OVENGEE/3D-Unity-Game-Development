using UnityEngine;

public class BeaconId : MonoBehaviour, IBeacon
{
    [SerializeField] private GameType beaconGameType; 
    public GameType gameType { get; set; }

    private void Awake()
    {
        gameType = beaconGameType;
        // Debug.Log($"{name} Awake: beaconGameType = {beaconGameType}, gameType = {gameType}");
        var beaconMesh = this.GetComponent<MeshRenderer>();
        beaconMesh.enabled = false;
    }
}
