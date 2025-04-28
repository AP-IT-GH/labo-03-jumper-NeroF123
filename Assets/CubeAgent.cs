using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;


public class CubeAgent : Agent
{

    public GameObject Obstacle;
    public float episodeLength = 20f;


    //rewards
    public float rewardForSurvivingObstacle = 0.1f;
    public float rewardforLiving = 0.001f;
    //penalty
    public float penaltyForFalling = -1f;
    public float penaltyForHittingObstacle = -2f;


    private Vector3 previousPosition;
    private Rigidbody rb;


    public float jumpForce = 10f; // Add jump force



    public override void OnEpisodeBegin() {

        // reset de positie en orientatie van agent
        this.transform.localPosition = new Vector3(0, 0.5f, 0); this.transform.localRotation = Quaternion.identity;
        

        // verplaats de obsatcle naar de pos
        Obstacle.transform.localPosition = new Vector3(0 , 0.5f, Random.Range(15, 20));


        rb = GetComponent<Rigidbody>();

        previousPosition = this.transform.localPosition;

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target Agent posities
        sensor.AddObservation(this.transform.localPosition);
        //sensor.AddObservation(obstacle.transform.position);

        sensor.AddObservation(IsGrounded());



    }

    public float speedMultiplier = 0.1f;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Continuous actions for movement and rotation
        //float forwardAction = actionBuffers.ContinuousActions[0]; // Forward/Backward
        //float sideAction = actionBuffers.ContinuousActions[1]; // Left/Right
        //float rotationAction = actionBuffers.ContinuousActions[2]; // Rotation

        // Discrete action for jump
        int jumpAction = actionBuffers.DiscreteActions[0];

        //Vector3 controlSignal = new Vector3(sideAction, 0, forwardAction);

        // Apply movement
        //transform.Translate(controlSignal * speedMultiplier);

        // Apply rotation
        //transform.Rotate(Vector3.up, rotationAction * 90 * Time.deltaTime);

        // Jump
        if (jumpAction == 1 && IsGrounded()) // Ensure grounded before jumping
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }





        // Update previous position
        previousPosition = this.transform.localPosition;



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
        //var continuousActionsOut = actionsOut.ContinuousActions;
        //continuousActionsOut[0] = Input.GetAxis("Vertical"); // Forward/Backward
        //continuousActionsOut[1] = Input.GetAxis("Horizontal"); // Left/Right
        //continuousActionsOut[2] = Input.GetAxis("Mouse X"); // Rotation

        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1 : 0; // Jump
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Obstacle")) 
        {
            Debug.Log("Hit Obstacle!");
            AddReward(penaltyForHittingObstacle);
            EndEpisode();
        }

    }

    private bool IsGrounded()
    {

        return Physics.Raycast(transform.position, Vector3.down, 0.55f);

    }



}
