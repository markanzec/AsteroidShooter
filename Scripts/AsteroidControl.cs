using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidControl : MonoBehaviour
{
	public string size;
	public int speed;
	public float rotationSpeed;
	
	//wrapping coordinates
	public float top;
	public float right;
	
	public Rigidbody2D rb;
	public ScreenWrap sw;
	
	public GameObject mediumAsteroid;
	public GameObject smallAsteroid;
	//public GameObject impactAnimation;
	
	
    // Start is called before the first frame update
    void Start()
    {	
        rb = GetComponent<Rigidbody2D>();
		sw = GetComponent<ScreenWrap>();

		rb.AddForce(Random.insideUnitCircle * 0);
		rb.AddTorque(0);
		rb.AddForce(Random.insideUnitCircle * speed);
		rb.AddTorque(rotationSpeed);
		
    }

    // Update is called once per frame
    void Update()
    {
        //check for wrapping
		if(sw.CheckForWrapping(transform.position, top, right))
			transform.position = sw.wrappedPosition;
    }
	
	// Bullet hits the asteroid
	void OnTriggerEnter2D(Collider2D other)
	{
		Destroy(other.gameObject);
		//Instantiate(impactAnimation, other.transform.position, transform.rotation);
		Split();
	}
	
	void Split()
	{
		Vector2 location = new Vector2(transform.position.x, transform.position.y);
		switch (size)
		{
			case "Large":
				Instantiate(mediumAsteroid, new(location.x + 0.8f, location.y + 0.8f), transform.rotation);
				Instantiate(mediumAsteroid, location, Quaternion.identity);
				Destroy(gameObject);
				break;
			case "Medium":
				Instantiate(smallAsteroid, new(location.x + 0.35f, location.y + 0.35f), transform.rotation);
				Instantiate(smallAsteroid, location, Quaternion.identity);
				Destroy(gameObject);
				break;
			case "Small":
				Destroy(gameObject);
				break;
		}
	}
}
