using System.Collections.Generic;
using UnityEngine;

public static class Detectors {
  // public static GameObject[] Sphere(
  //   Transform detector,
  //   float range,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0) {
  //   return Sphere(detector.position, range, detectionMask, obstructionMask, detector.gameObject);
  // }

  // public static GameObject[] Sphere(
  //   Vector3 origin,
  //   float range,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0,
  //   GameObject excludedObject = null) {
  //   var sensedObjects = new List<GameObject>();
  //   SphereNonAlloc(sensedObjects, origin, range, detectionMask, obstructionMask, excludedObject);
  //   return sensedObjects.ToArray();
  // }

  // public static void SphereNonAlloc(
  //   IList<GameObject> sensedObjects,
  //   Transform detector,
  //   float range,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0) {
  //   SphereNonAlloc(sensedObjects, detector.position, range, detectionMask, obstructionMask, detector.gameObject);
  // }

  // public static void SphereNonAlloc(
  //   IList<GameObject> sensedObjects,
  //   Vector3 origin,
  //   float range,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0,
  //   GameObject excludedObject = null) {
  //   sensedObjects.Clear();

  //   foreach (var collider in Physics.OverlapSphere(origin, range, detectionMask)) {
  //     var isValidTarget =
  //       (collider.gameObject.layer & detectionMask) == 0 &&
  //       !IsThisOrChild(excludedObject, collider) &&
  //       !IsObstructed(origin, collider.transform.position, obstructionMask);

  //     if (isValidTarget) {
  //       sensedObjects.Add(collider.gameObject);
  //     }
  //   }
  // }

  // public static GameObject[] Box(
  //   Transform detector,
  //   Vector3 size,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0) {
  //   return Box(detector.position, size, detectionMask, obstructionMask, detector.gameObject);
  // }

  // public static GameObject[] Box(
  //   Vector3 center,
  //   Vector3 size,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0,
  //   GameObject excludedObject = null) {
  //   var sensedObjects = new List<GameObject>();
  //   BoxNonAlloc(sensedObjects, center, size, detectionMask, obstructionMask, excludedObject);
  //   return sensedObjects.ToArray();
  // }

  // public static void BoxNonAlloc(
  //   IList<GameObject> sensedObjects,
  //   Vector3 center,
  //   Vector3 size,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0,
  //   GameObject excludedObject = null) {
  //   sensedObjects.Clear();
  //   foreach (var collider in Physics.OverlapBox(center, size * 0.5f, Quaternion.identity, detectionMask)) {
  //     var isValidTarget =
  //       (collider.gameObject.layer & detectionMask) == 0 &&
  //       !IsThisOrChild(excludedObject, collider) &&
  //       !IsObstructed(center, collider.transform.position, obstructionMask);

  //     if (isValidTarget) {
  //       sensedObjects.Add(collider.gameObject);
  //     }
  //   }
  // }

  // public static GameObject[] FOV(
  //   Transform detector,
  //   float range,
  //   float angle,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0) {
  //   return FOV(detector.position, detector.forward, range, angle, detectionMask, obstructionMask);
  // }

  // public static GameObject[] FOV(
  //   Vector3 origin,
  //   Vector3 forward,
  //   float range,
  //   float angle,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0,
  //   GameObject excludedObject = null) {
  //   var sensedObjects = new List<GameObject>();
  //   FOVNonAlloc(sensedObjects, origin, forward, range, angle, detectionMask, obstructionMask, excludedObject);
  //   return sensedObjects.ToArray();
  // }

  // public static void FOVNonAlloc(
  //  IList<GameObject> sensedObjects,
  //  Transform detector,
  //  float range,
  //  float angle,
  //  [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //  int obstructionMask = 0) {
  //   FOVNonAlloc(sensedObjects, detector.position, detector.forward, range, angle, detectionMask, obstructionMask, detector.gameObject);
  // }

  // public static void FOVNonAlloc(
  //   IList<GameObject> sensedObjects,
  //   Vector3 origin,
  //   Vector3 forward,
  //   float range,
  //   float angle,
  //   [UnityEngine.Internal.DefaultValue("AllLayers")] int detectionMask,
  //   int obstructionMask = 0,
  //   GameObject excludedObject = null) {
  //   sensedObjects.Clear();

  //   foreach (var collider in Physics.OverlapSphere(origin, range, detectionMask)) {
  //     var vectorToTarget = collider.transform.position - origin;
  //     var targetIsWithinFOV = Vector3.Angle(forward, vectorToTarget) < angle / 2;

  //     var isValidTarget =
  //       (collider.gameObject.layer & detectionMask) == 0 &&
  //       !IsThisOrChild(excludedObject, collider) &&
  //       targetIsWithinFOV &&
  //       !IsObstructed(origin, collider.transform.position, obstructionMask);

  //     if (isValidTarget) {
  //       sensedObjects.Add(collider.gameObject);
  //     }
  //   }
  // }

  public static bool IsNotThisObject(GameObject exclude, Collider collider) =>
    (collider?.transform?.IsChildOf(exclude.transform) ?? false) == false;

  public static bool IsVisible(Vector3 position, Vector3 target, LayerMask obstructionMask) =>
    !Physics.Linecast(position, target, obstructionMask);

  public static bool IsValidTarget(GameObject sensor, Collider target, LayerMask obstructionMask) =>
    IsNotThisObject(sensor, target) && IsVisible(sensor.transform.position, target.transform.position, obstructionMask);
}
