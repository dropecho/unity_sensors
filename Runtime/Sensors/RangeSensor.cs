using System;
using System.Collections.Generic;
using System.Linq;
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
    private HashSet<Collider> objs = new HashSet<Collider>(256);

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
        if (Detectors.IsValidTarget(gameObject, _hits[i], obstructionLayers)) {
          if (objs.Add(_hits[i])) {
            onDetection?.Invoke(obj);
          }
        } else {
          if (objs.Remove(_hits[i])) {
            onDetectionLoss?.Invoke(obj);
          }
        }
      }

      objs.IntersectWith(new ArraySegment<Collider>(_hits, 0, hitCount));

      foreach (var sensed in sensedObjects) {
        if (!objs.Contains(sensed.GetComponent<Collider>())) {
          onDetectionLoss?.Invoke(sensed);
        }
      }

      sensedObjects.Clear();
      foreach (var obj in objs) {
        sensedObjects.Add(obj.gameObject);
      }
    }
  }
}
