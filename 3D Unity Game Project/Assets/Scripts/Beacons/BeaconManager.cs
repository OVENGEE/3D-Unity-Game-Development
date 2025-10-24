using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeaconManager : MonoBehaviour
{
    public GameObject[] beacons;

    //Dictionary and list declaration
    private Dictionary<IGame, GameObject> beaconDictionary;


    private void OnEnable()
    {
        TicketManager.OnAvailableGames += TriggerAvailableBeacon;
    }

    private void OnDisable()
    {
        TicketManager.OnAvailableGames -= TriggerAvailableBeacon;
    }

    private void TriggerAvailableBeacon(List<IGame> games)
    {
        Debug.Log($"TriggerAvailableBeacon called. games count: {(games == null ? 0 : games.Count)}");
        HideAllBeacons();
        beaconDictionary = new Dictionary<IGame, GameObject>();

        if (games.Count == 0) return;
        if (beacons.Length == 0) return;

        for (int i = 0; i < games.Count; i++)
        {
            var game = games[i];
            for (int j = 0; j < beacons.Length; j++)
            {
                var beaconData = beacons[j].GetComponent<BeaconId>();
                if(game.gameType == beaconData.gameType)
                {
                    beaconDictionary[game] = beacons[j];
                    var beaconMesh = beacons[j]?.GetComponent<MeshRenderer>();
                    beaconMesh.enabled = true;
                }
            }
        }
    }

    

    private void HideAllBeacons()
    {
        foreach( var beacon in beacons)
        {
            var beaconMesh = beacon.GetComponent<MeshRenderer>();
            beaconMesh.enabled = false;
        }
    }

    

}
