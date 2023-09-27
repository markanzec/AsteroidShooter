using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
	private float thrustInput;
	private float turnInput;
	public int speed;
	public int turnSpeed;
	public int bulletSpeed;
	public GameObject bullet;
	public Transform gun;
	public int lives;
	public int damageMultiplier;
	public float ufoBulletDamage;
	private float damage = 0.0f;
	private bool canShoot;
	
	//wrapping coordinates
	public float top;
	public float right;
	
	public Rigidbody2D rb;
	public ScreenWrap sw;

	
    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		sw = GetComponent<ScreenWrap>();
		canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        //listen for input
		thrustInput = Input.GetAxis("Vertical");
		turnInput = Input.GetAxis("Horizontal");
		
		//rotate the ship
		//transform.Rotate(Vector3.forward * turnInput * Time.deltaTime * -turnSpeed);
		transform.Rotate(Vector3.forward, turnSpeed * -turnInput * Time.deltaTime);
		
		//check for wrapping
		if (sw.CheckForWrapping(transform.position, top, right))
			transform.position = sw.wrappedPosition;
		
		//check for shooting
		if (Input.GetButtonDown("Jump") && canShoot)
		{
			GameObject firedBullet = Instantiate(bullet, gun.position, gun.rotation);
			firedBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletSpeed);
			Destroy(firedBullet, 5.0f);
		}
		
    }
	
	void FixedUpdate()
	{
		//apply forces/make ship move
		//rb.AddRelativeForce(Vector2.up * thrustInput * speed);
		rb.AddRelativeForce(Vector2.up * thrustInput * speed);
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		float impactForce = other.relativeVelocity.magnitude;
		float impactDamage = impactForce * damageMultiplier;
		damage += impactDamage;
		//Debug.Log("HIT BY RB | " + "Damage: " + damage + " Lives Remaining: " + lives);
		CheckDamage();
		
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("UFO Bullet"))
		{
			damage += ufoBulletDamage;
			Destroy(other.gameObject);
			//Debug.Log("HIT BY BULLET | " + "Damage: " + damage + " Lives Remaining: " + lives);
			CheckDamage();
		}		
	}
	
	void CheckDamage()
	{
		if (damage >= 100)
		{
			lives--;
			rb.velocity = Vector2.zero;
			rb.angularVelocity = 0.0f;
			transform.position = new Vector2(10.0f, 6.0f); // to tell ufo to go away
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<PolygonCollider2D>().enabled = false;
			canShoot = false;
			if (lives == 0)
				GameOver();
			else
				Invoke("Respawn", 2.0f);
		}
	}
	
	void GameOver()
	{
		damage = 100.0f;
		Debug.Log("GAME OVER");
	}
	
	void Respawn()
	{
		damage = 0.0f;
		transform.position = Vector2.zero;
		GetComponent<SpriteRenderer>().enabled = true;
		GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0.2f);
		//Debug.Log("RESPAWNED | " + "Damage: " + damage + " Lives Remaining: " + lives);
		Invoke("EndSpawnProtection", 3.0f);
	}
	
	void EndSpawnProtection()
	{
		GetComponent<PolygonCollider2D>().enabled = true;
		GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1.0f);
		canShoot = true;
	}
}
