using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] Material winMaterial;
    [SerializeField] Material looseMaterial;
    [SerializeField] MeshRenderer floorMeshRenderer;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-2.5f, +2.5f), 0f, Random.Range(-3f, +1f));
        targetTransform.localPosition = new Vector3(Random.Range(-2.5f, +2.5f), 0f, Random.Range(1.75f, 3.75f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += moveSpeed * Time.deltaTime * new Vector3(moveX, 0f, moveZ);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxisRaw("Horizontal");
        continousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Goal"))
        {
            SetReward(+1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        else if(other.transform.CompareTag("Wall"))
        {
            SetReward(-1f);
            floorMeshRenderer.material = looseMaterial;
            EndEpisode();
        }
        
    }
}
