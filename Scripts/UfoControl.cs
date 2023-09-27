using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoControl : MonoBehaviour
{
	private Vector2 direction;
	public float speed;
	public Rigidbody2D rb;
	private Transform transformShip;
	public GameObject ufoBullet;
	public int bulletSpeed;
	public int hitPoints;
	public bool canShoot = true;
	
	private int timesHit;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		transformShip = GameObject.FindWithTag("Player").transform;
		InvokeRepeating("Shoot", 5.0f, 5.0f);
		timesHit = 0;
    }

    // Update is called once per frame
    void Update()
    {
		transform.Rotate(Vector3.forward, 90.0f * Time.deltaTime);
		if (timesHit >= hitPoints)
		{
			gameObject.SetActive(false);
			canShoot = false;
			timesHit = 0;
            transform.position = new Vector2(Random.Range(-10.0f, 10.0f), 8.0f);
		}		
    }
	
	void FixedUpdate()
	{
		direction = (transformShip.position - transform.position).normalized;
		rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
	}
	
	void Shoot()
	{
		if (canShoot)
		{
			GameObject firedUfoBullet = Instantiate(ufoBullet, transform.position, transform.rotation);
			firedUfoBullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);
			Destroy(firedUfoBullet, 10.0f);
		}	
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		timesHit += 2;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Bullet"))
		{
			timesHit++;
			Destroy(other.gameObject);
		}
	}
}
