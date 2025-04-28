using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;


public class CubeAgent : Agent
{

    public GameObject Obstacle;
    public Transform TargetZone;
    public float targetTime;


    //rewards
    public float rewardForTargetZone = 5f;
    //penalty
    public float penaltyForFalling = -1f;
    public float penaltyForTimeout = -1f;
    public float penaltyForHittingObstacle = -1f;

    //add small rewards for getting closer, make them aware of distance later too
    public float distanceMultiplier = 0.1f;
    private Vector3 previousPosition;
    private Rigidbody rb;
    public float jumpForce = 5f; // Add jump force



    public override void OnEpisodeBegin() {

        // reset de positie en orientatie als de agent gevallen is
        
        

        this.transform.localPosition = new Vector3(-20, 0.5f, 0); this.transform.localRotation = Quaternion.identity;
        

     // verplaats de obsatcle naar de pos
        Obstacle.transform.localPosition = new Vector3(Random.Range(-10, 0) , 0.5f, 0);

        targetTime = 60.0f;
        rb = GetComponent<Rigidbody>();

        previousPosition = this.transform.localPosition;

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target Agent posities
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(TargetZone.localPosition);

        sensor.AddObservation(IsGrounded());

        //disttances
        sensor.AddObservation(Vector3.Distance(this.transform.localPosition, TargetZone.localPosition));


    }

    public float speedMultiplier = 0.1f;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Continuous actions for movement and rotation
        float forwardAction = actionBuffers.ContinuousActions[0]; // Forward/Backward
        float sideAction = actionBuffers.ContinuousActions[1]; // Left/Right
        float rotationAction = actionBuffers.ContinuousActions[2]; // Rotation

        // Discrete action for jump
        int jumpAction = actionBuffers.DiscreteActions[0];

        Vector3 controlSignal = new Vector3(sideAction, 0, forwardAction);

        // Apply movement
        transform.Translate(controlSignal * speedMultiplier);

        // Apply rotation
        transform.Rotate(Vector3.up, rotationAction * 90 * Time.deltaTime);

        // Jump
        if (jumpAction == 1 && Mathf.Abs(rb.linearVelocity.y) < 0.01f) // Ensure grounded before jumping
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Beloningen
        float distanceToTargetZone = Vector3.Distance(this.transform.localPosition, TargetZone.localPosition);
        float previousDistanceToTargetZone = Vector3.Distance(previousPosition, TargetZone.localPosition);


        targetTime -= Time.deltaTime;

        
            // Phase 2: Move to target zone

            if (distanceToTargetZone < previousDistanceToTargetZone)
            {
                AddReward(distanceMultiplier * (previousDistanceToTargetZone - distanceToTargetZone));
            }


        // Update previous position
        previousPosition = this.transform.localPosition;


        // Te laat
        if (targetTime <= 0.0f)
        {
            AddReward(penaltyForTimeout);
            EndEpisode();
        }

        // gevallen
        if (this.transform.localPosition.y < 0)
        {
            AddReward(penaltyForFalling);
            EndEpisode();
        }

        //wander penalty
        float movementPenalty = -0.0001f;
        AddReward(movementPenalty);



    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical"); // Forward/Backward
        continuousActionsOut[1] = Input.GetAxis("Horizontal"); // Left/Right
        continuousActionsOut[2] = Input.GetAxis("Mouse X"); // Rotation

        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0; // Jump
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            Debug.Log("Reached Target Zone (Trigger)!");
            AddReward(rewardForTargetZone);
            EndEpisode();
        }

        if (other.CompareTag("Obstacle")) 
        {
            Debug.Log("Hit Obstacle!");
            AddReward(penaltyForHittingObstacle);

        }

    }

    private bool IsGrounded()
    {
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            return true;
        }

        return false;

    }


}
