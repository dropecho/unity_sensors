using System;
using UnityEngine;

namespace Dropecho {
  public enum SensorShape {
    Cube,
    Capsule,
    Sphere
  }
  public class RangeSensor : UpdatingSensor {
    public SensorShape shape;
    [ShowIf(nameof(shape), SensorShape.Sphere, SensorShape.Capsule)]
    public float radius = 1;
    [ShowIf(nameof(shape), SensorShape.Cube)]
    public Vector3 size;
    [ShowIf(nameof(shape), SensorShape.Capsule)]
    public Vector3 origin;
    [ShowIf(nameof(shape), SensorShape.Capsule)]
    public Vector3 offset;

    private Collider[] _hits = new Collider[256];

    protected override void DetectObjects() {
      var hitCount = shape switch {
        SensorShape.Capsule => Physics.OverlapCapsuleNonAlloc(
          transform.TransformPoint(origin), transform.TransformPoint(offset), radius, _hits, detectionLayers
        ),
        SensorShape.Sphere => Physics.OverlapSphereNonAlloc(transform.position, radius, _hits, detectionLayers),
        SensorShape.Cube => Physics.OverlapBoxNonAlloc(transform.position, size * 0.5f, _hits, transform.rotation, detectionLayers)
      };

      // Add or remove objects from sensed, as needed.
      for (var i = 0; i < hitCount; i++) {
        var obj = _hits[i].gameObject;
        
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
