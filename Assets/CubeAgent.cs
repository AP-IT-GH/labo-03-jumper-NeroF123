using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;


public class CubeAgent : Agent
{

    public GameObject Obstacle;
    public float maxEpisodeTime = 60f;
    private float episodeTimer;


    //rewards
    public float rewardforLiving = 0.001f;
    public float FinishWithouthGettinghitReward = 1f;
    //penalty
    public float penaltyForHittingObstacle = -2f;
    public float penaltyForjumping = -0.001f;


    private Rigidbody rb;


    public float jumpForce = 3f; // Add jump force



    public override void OnEpisodeBegin() {

        // reset de positie en orientatie van agent
        this.transform.localPosition = new Vector3(0, 0.5f, 0); this.transform.localRotation = Quaternion.identity;
        

        // verplaats de obsatcle naar de pos
        Obstacle.transform.localPosition = new Vector3(0 , 0.5f, Random.Range(15, 20));


        rb = GetComponent<Rigidbody>();

        episodeTimer = maxEpisodeTime;

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target Agent posities
        sensor.AddObservation(this.transform.localPosition.y);
        sensor.AddObservation(Obstacle.transform.position);

        sensor.AddObservation(IsGrounded());



    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {


        // Discrete action for jump
        int jumpAction = actionBuffers.DiscreteActions[0];


        // Jump
        if (jumpAction == 1 && IsGrounded()) // Ensure grounded before jumping
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            AddReward(penaltyForjumping);
        }

        //timer
        episodeTimer -= Time.deltaTime;
        if (episodeTimer <= 0f)
        {
            AddReward(FinishWithouthGettinghitReward);
            EndEpisode();
        }

        //living reward
        AddReward(rewardforLiving);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {


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
