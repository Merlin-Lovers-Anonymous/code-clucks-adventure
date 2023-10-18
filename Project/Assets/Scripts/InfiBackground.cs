using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
   private Transform cameraTransform;
   private Vector3 lastCameraPosition;

private void Start() {
    cameraTransform = Camera.main.transform;
    lastCameraPosition = cameraTransform.position;
}
private void Update() {
Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
transform.position += deltaMovement;
lastCameraPosition = cameraTransform.position;
}
    }