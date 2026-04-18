using UnityEngine;
using Unity.Cinemachine;

[SaveDuringPlay]
[AddComponentMenu("Cinemachine/Extensions/Tag Collision Avoidance")]
public class CMTagCollisionAvoidance : CinemachineExtension
{
    [Header("Collision")]
    public string wallTag = "WALLS";
    public LayerMask obstacleLayers = ~0;
    public float cameraRadius = 0.2f;
    public float minDistanceFromTarget = 0.5f;
    public float collisionOffset = 0.1f;
    public float smoothTime = 10f;

    private float currentPull;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Body)
            return;

        if (vcam.Follow == null)
            return;

        Vector3 targetPos = vcam.Follow.position;
        Vector3 camPos = state.RawPosition;

        Vector3 dir = camPos - targetPos;
        float desiredDistance = dir.magnitude;

        if (desiredDistance < 0.001f)
            return;

        dir.Normalize();

        float rayDistance = Mathf.Max(0f, desiredDistance - minDistanceFromTarget);

        Vector3 sphereStart = targetPos + dir * minDistanceFromTarget;

        bool hitWall = false;
        float wantedDistance = desiredDistance;

        if (Physics.SphereCast(
            sphereStart,
            cameraRadius,
            dir,
            out RaycastHit hit,
            rayDistance,
            obstacleLayers,
            QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag(wallTag))
            {
                hitWall = true;

                float distanceFromTargetToHit = Vector3.Distance(targetPos, hit.point);
                wantedDistance = Mathf.Max(
                    minDistanceFromTarget,
                    distanceFromTargetToHit - collisionOffset
                );
            }
        }

        float targetPull = hitWall ? (desiredDistance - wantedDistance) : 0f;

        if (deltaTime < 0f)
            currentPull = targetPull;
        else
            currentPull = Mathf.Lerp(currentPull, targetPull, deltaTime * smoothTime);

        Vector3 finalPos = targetPos + dir * Mathf.Max(minDistanceFromTarget, desiredDistance - currentPull);
        state.RawPosition = finalPos;
    }
}