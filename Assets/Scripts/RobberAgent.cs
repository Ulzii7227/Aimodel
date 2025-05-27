using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RobberAgent : Agent
{
    public Transform[] chips;
    public Transform escapePoint;
    private Rigidbody rb;
    private int chipsCollected = 0;

    public float moveSpeed = 2.5f;
    public float turnSpeed = 120f;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 0.5f, 0);
        rb.linearVelocity = Vector3.zero;
        chipsCollected = 0;

        foreach (var chip in chips)
            chip.gameObject.SetActive(true);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(chipsCollected);

        foreach (var chip in chips)
        {
            sensor.AddObservation(chip.localPosition);
            sensor.AddObservation(chip.gameObject.activeSelf ? 1.0f : 0.0f);
        }

        sensor.AddObservation(escapePoint.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int move = actions.DiscreteActions[0];
        int turn = actions.DiscreteActions[1];

        if (move == 1)
            rb.AddForce(transform.forward * moveSpeed, ForceMode.VelocityChange);
        else if (move == 2)
            rb.AddForce(-transform.forward * moveSpeed, ForceMode.VelocityChange);

        if (turn == 1)
            transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
        else if (turn == 2)
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);

        AddReward(-0.001f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DataChip") && other.gameObject.activeSelf)
        {
            other.gameObject.SetActive(false);
            chipsCollected++;
            AddReward(1.0f);
        }

        if (other.CompareTag("Escape") && chipsCollected == chips.Length)
        {
            AddReward(3.0f);
            EndEpisode();
        }

        if (other.CompareTag("Guard"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }
}
