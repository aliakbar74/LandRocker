using System;
using DefaultNamespace;
using UnityEngine;

public class EarthController : MonoBehaviour {
    [SerializeField] private float radius = 200;
    [SerializeField] private float spinSpeed;
    [SerializeField] private ParticleSystem dustParticle;

    private CameraController _cameraController;
    private UiController _uiController;
    private CloudController[] _cloudControllers;
    private Weather _currentWeather;
    private float _lastDistance;
    private Action<Weather> _onChangeWeather;

    public float Radius => radius;
    public CameraController CameraController => _cameraController;

    private void Awake() {
        SetRadius();

        _cameraController = FindObjectOfType<CameraController>();
        _uiController = FindObjectOfType<UiController>();
        _cloudControllers = FindObjectsOfType<CloudController>();

        _cameraController.Init(this);
        _uiController.Init(this);
        foreach (var cloudController in _cloudControllers) {
            cloudController.Init(this);
        }

        _cameraController.AddListenerOnChangeDistance(CheckWeather);
        
        CheckWeather(_cameraController.DistanceToEarth);
    }

    private void SetRadius() {
        transform.localScale = Vector3.one * radius * 2;
    }

    private void Update() {
        Spin();
    }

    private void Spin() {
        var rotation = transform.rotation;
        transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, rotation.eulerAngles.y + spinSpeed * Time.deltaTime, rotation.eulerAngles.z);
    }

    private void CheckWeather(float distance) {
        if (distance >= 500) {
            ChangeWeather(Weather.Snow);
            return;
        }

        if (distance >= 300) {
            ChangeWeather(Weather.Rain);
            return;
        }

        ChangeWeather(Weather.Dust);
    }

    private void ChangeWeather(Weather weather) {
        if (_currentWeather == weather) return;
        _currentWeather = weather;

        switch (weather) {
            case Weather.Dust:
                dustParticle.Play();
                break;
            case Weather.Rain:
                dustParticle.Stop();
                break;
            case Weather.Snow:
                dustParticle.Stop();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(weather), weather, "Send a message to god");
        }

        _onChangeWeather?.Invoke(_currentWeather);
    }

    public void AddListenerOnChangeWeather(Action<Weather> action) {
        _onChangeWeather += action;
    }

    public void RemoveListenerOnChangeWeather(Action<Weather> action) {
        if (_onChangeWeather == null) return;
        _onChangeWeather -= action;
    }
}