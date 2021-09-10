using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PlanetGenerator[] _planets;
    [SerializeField] private Transform _shipSpawnPoint;

    public int _planetIndex;
    private Ship _ship;
    private Music _music;

    void Start()
    {
        _music = FindObjectOfType<Music>();
        _ship = FindObjectOfType<Ship>();
        
        // Start at a random planet
        _planetIndex = Random.Range(0, _planets.Length - 1);
        
        // Activate the starting planet
        MoveToNextPlanet();
    }

    private void ActivateCurrentPlanet()
    {
        // Randomize the planet and show it
        _planets[_planetIndex].GeneratePlanet();
        _planets[_planetIndex].gameObject.SetActive(true);
        
        // Spawn the objects on the planet
        _planets[_planetIndex].GetComponent<PlanetObjectSpawner>().SpawnObjects();
    }

    public void MoveToNextPlanet()
    {
        // Turn off all the planets
        foreach (var planet in _planets)
            planet.gameObject.SetActive(false);
        
        // Cycle through the planets
        ++_planetIndex;
        if (_planetIndex >= _planets.Length)
            _planetIndex = 0;
        
        // Activate the new planet
        ActivateCurrentPlanet();
        
        // Start the music for this planet
        _music.StartTrack(_planetIndex, 3f);
        
        // Position the ship far out in orbit
        _ship.transform.position = _shipSpawnPoint.position;
        _ship.transform.rotation = _shipSpawnPoint.rotation;
    }
}
