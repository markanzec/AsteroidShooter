using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	private Scene scene;
	private int level;
	
	public GameObject spaceShip;
	public GameObject asteroidLarge;
	public GameObject asteroidMedium;
	public GameObject ufo;

	private GameObject spawnedUfo;

    void Start()
    {
		Application.targetFrameRate = 60;
		scene = SceneManager.GetActiveScene();
		InvokeRepeating("CheckUfoStatus", 2.0f, 2.0f);
		
		level = 1;
		Instantiate(spaceShip, transform.position, Quaternion.identity);
		Instantiate(asteroidLarge, new Vector2(Random.Range(-10.3f, 10.3f), 6.2f), Quaternion.identity);
		spawnedUfo = Instantiate(ufo, new Vector2(Random.Range(-10.3f, 10.3f), 6.2f), Quaternion.identity); 
    }

    void Update()
    {

    }

	void CheckUfoStatus()
	{
		if (!spawnedUfo.activeSelf)
			CheckAsteroidCount();
	}
	
	void CheckAsteroidCount()
	{
		List<GameObject> allObjects = new List<GameObject>();
		scene.GetRootGameObjects(allObjects);
		int asteroidCount = 0;
		foreach (GameObject go in allObjects)
		{
			if (go.CompareTag("Asteroid"))
				asteroidCount++;
		}
		if (asteroidCount == 0)
		{
			Invoke("ChangeLevel", 2.0f);
		}
	}

	void ChangeLevel()
	{
		level++;
		Debug.Log("LEVEL: " + level);
		if (level % 2 == 0)
		{
			for(int i = 0; i < level; i++)
			{
				Instantiate(asteroidMedium, new Vector2(Random.Range(-5.75f, 5.75f), 9.9f), Quaternion.identity);
				Instantiate(asteroidMedium, new Vector2(Random.Range(-5.75f, 5.75f), 9.9f), Quaternion.identity);
			}
		}
		else
		{
			for(int i = 0; i < level; i++)
			{
				Instantiate(asteroidLarge, new Vector2(Random.Range(-10.3f, 10.3f), 6.2f), Quaternion.identity);
			}
		}
		Invoke("ActivateUfo", 10.0f-(level*0.5f));	
	}

	void ActivateUfo()
	{
		spawnedUfo.SetActive(true);
		spawnedUfo.GetComponent<UfoControl>().canShoot = true;
		spawnedUfo.GetComponent<UfoControl>().speed *= 1.10f;
		spawnedUfo.GetComponent<UfoControl>().bulletSpeed += 10;
		spawnedUfo.GetComponent<UfoControl>().hitPoints++;
	}
}
