using UnityEngine;

public enum PickupType { Gold, Exp }

public class Pickup : MonoBehaviour
{
    private int amount;
    private PickupType type;
    private Transform player;

    private Vector3 explodeDir;
    private float explodeTime = 0.3f; // time to scatter before magnet
    private float magnetSpeed = 10f; 
    private float timer;

    public void Init(int amount, PickupType type)
    {
        this.amount = amount;
        this.type = type;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // random explosion direction
        explodeDir = Random.insideUnitSphere * 2f; 
        explodeDir.y = 0.5f; // add some upward motion
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < explodeTime)
        {
            // explosion phase
            transform.position += explodeDir * Time.deltaTime * 3f;
        }
        else if (player != null)
        {
            // magnetize to player
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                magnetSpeed * Time.deltaTime
            );

            // check if close enough to collect
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < 1f)
            {
                Collect();
            }
        }
    }

    private void Collect()
    {
        if (type == PickupType.Gold)
        {
            GameManager.Instance.AddMoney(amount);
        }
        else if (type == PickupType.Exp)
        {
            GameManager.Instance.AddExp(amount);
        }

        Destroy(gameObject);
    }
}
