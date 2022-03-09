using System;
using UnityEngine;

public class CloudController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float distance = 200;
    [SerializeField] private ParticleSystem snowParticle;
    [SerializeField] private ParticleSystem rainParticle;

    private EarthController _earthController;
    private Vector3 _dirToRotate;
    private float _distanceFromEarth;
    public void Init(EarthController earth) {
        _earthController = earth;
        _distanceFromEarth = distance + _earthController.Radius;
        var lookDir = (transform.position - _earthController.transform.position).normalized;
        var dirToEarth = lookDir.normalized;
        _dirToRotate = Quaternion.Euler(0, 90, 0) * dirToEarth;

        transform.rotation = Quaternion.LookRotation(lookDir);
        transform.position = lookDir * _distanceFromEarth;
        
        earth.AddListenerOnChangeWeather(TryToRain);
    }

    private void TryToRain(Weather weather) {
        switch (weather) {
            case Weather.Rain:
                snowParticle.Stop();
                rainParticle.Play();
                break;
            case Weather.Snow:
                snowParticle.Play();
                rainParticle.Stop();
                break;
            case Weather.Dust:
                snowParticle.Stop();
                rainParticle.Stop();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(weather), weather, "As i said before. send a message to god");
        }
    }

    private void Update() {
        OrbitAround();
    }

    private float x, y;

    private void OrbitAround() {
        transform.RotateAround(_earthController.transform.position, _dirToRotate, speed * Time.deltaTime);
    }
}