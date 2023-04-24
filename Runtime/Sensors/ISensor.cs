using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dropecho {
  public enum SensorUpdateMode {
    Update,
    FixedUpdate,
    Rate,
    Manual
  }

  public interface ISensor {
    IList<GameObject> sensedObjects { get; }
    GameObject GetClosestDetectedObject();

    UnityEvent<GameObject> onDetection { get; }
    UnityEvent<GameObject> onDetectionLoss { get; }
  }

  public abstract class Sensor : MonoBehaviour, ISensor {
    public IList<GameObject> sensedObjects { get; private set; } = new List<GameObject>();

    [field: SerializeField]
    public UnityEvent<GameObject> onDetection { get; set; }
    [field: SerializeField]
    public UnityEvent<GameObject> onDetectionLoss { get; set; }


    [field: SerializeField]
    public LayerMask detectionLayers { get; private set; }
    [field: SerializeField, Tooltip("The layers that will obstruct the view.")]
    public LayerMask obstructionLayers { get; private set; }

    public GameObject GetClosestDetectedObject() {
      var closestDistance = float.PositiveInfinity;
      GameObject closestObject = null;

      foreach (var sensedObject in sensedObjects) {
        var distanceToObject = Vector3.Distance(sensedObject.transform.position, transform.position);
        if (distanceToObject < closestDistance) {
          closestDistance = distanceToObject;
          closestObject = sensedObject;
        }
      }

      return closestObject;
    }
  }

  public abstract class UpdatingSensor : Sensor {

    [field: SerializeField]
    public SensorUpdateMode updateMode { get; private set; }
    [field:
      SerializeField,
      Range(0, 60),
      ShowIf(nameof(updateMode), SensorUpdateMode.Rate),
      Tooltip("This is the rate at which the sensor updated when in Rate update mode.")
    ]
    public float updateRate { get; private set; } = 0.5f;
    // void OnEnable() => StartCoroutine(UpdateCR());
    void Update() => DetectObjects();

    // YieldInstruction GetUpdateTimingForMode() {
    //   return updateMode switch {
    //     SensorUpdateMode.Rate => new WaitForSeconds(updateRate),
    //     SensorUpdateMode.FixedUpdate => new WaitForFixedUpdate(),
    //     _ => null
    //   };
    // }

    // IEnumerator UpdateCR() {
    //   while (true) {
    //     DetectObjects();
    //     yield return GetUpdateTimingForMode();
    //   }
    // }

    protected abstract void DetectObjects();
  }
}
