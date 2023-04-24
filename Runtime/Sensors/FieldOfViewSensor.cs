using System;
using UnityEngine;

namespace Dropecho {
  public class FieldOfViewSensor : UpdatingSensor {
    [Range(0, 360), Tooltip("The angle of the FOV (total, so 90 is 45 on both sides).")]
    public float angle = 0;
    [Range(1, 50), Tooltip("The length of the FOV from the entity.")]
    public float range = 1;

    private Collider[] _hits = new Collider[256];

    public bool IsInView(GameObject obj) => sensedObjects.Contains(obj);
    protected override void DetectObjects() {
      var hitCount = Physics.OverlapSphereNonAlloc(transform.position, range, _hits, detectionLayers);

      for (var i = 0; i < hitCount; i++) {
        var obj = _hits[i].gameObject;
        var vectorToTarget = _hits[i].transform.position - transform.position;
        var targetIsWithinFOV = Vector3.Angle(transform.forward, vectorToTarget) < angle / 2;

        var isValidTarget = targetIsWithinFOV && Detectors.IsValidTarget(gameObject, _hits[i], obstructionLayers);

        // Add or remove objects from sensed, as needed.
        if (isValidTarget) {
          if (!sensedObjects.Contains(obj)) {
            sensedObjects.Add(obj);
            onDetection?.Invoke(obj);
          }
        } else if (sensedObjects.Remove(obj)) {
          onDetectionLoss?.Invoke(obj);
        }
      }

      // Check if the existing objects in the list are still within the collider.
      for (var i = sensedObjects.Count - 1; i >= 0; i--) {
        var hitIndex = Array.IndexOf(_hits, sensedObjects[i].GetComponent<Collider>());

        if (hitIndex < 0 || hitIndex >= hitCount) {
          onDetectionLoss?.Invoke(sensedObjects[i]);
          sensedObjects.RemoveAt(i);
        }
      }
    }
  }
}
