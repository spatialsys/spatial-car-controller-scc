using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;
using TMPro;

public class SpawnCarButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public Spatial_SCC prefab;
    private Spatial_SCC _currentCar;

    public void ToggleSpawn()
    {
        if (_currentCar != null)
        {
            _currentCar.StopDriving();
            _currentCar = null;
            buttonText.text = "Drive";
        }
        else
        {
            Vector3 pos = SpatialBridge.actorService.localActor.avatar.position;
            Quaternion rot = SpatialBridge.actorService.localActor.avatar.rotation;
            pos += rot * new Vector3(0, 0, 3);
            _currentCar = Instantiate(prefab.gameObject, pos, rot).GetComponent<Spatial_SCC>();
            buttonText.text = "Stop";
        }
    }
}
