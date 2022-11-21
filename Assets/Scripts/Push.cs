using UnityEngine;

public class Push : MonoBehaviour
{
    public LayerMask push_layer;
    public bool can_push;
    [Range(0.5f, 5f)] public float strength = 1.1f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (can_push) PushRigidBodies(hit);
    }

    private void PushRigidBodies(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;
        var bodyLayerMask = 1 << body.gameObject.layer;
        if ((bodyLayerMask & push_layer.value) == 0) return;
        if (hit.moveDirection.y < -0.3f) return;
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
        body.AddForce(pushDir * strength, ForceMode.Impulse);
    }
}
