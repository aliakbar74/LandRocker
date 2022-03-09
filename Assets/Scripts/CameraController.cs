using System;
using UnityEngine;

public class CameraController :MonoBehaviour, IController {
    [SerializeField] private float speed = 50;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxDistance;
    [SerializeField] private float minDistance;

    private EarthController _earth;
    private float _radius;
    private float _distanceToEarth;
    private Action<float> _onChangeDistance;

    public float DistanceToEarth => _distanceToEarth;

    public void Init(EarthController earth) {
        _radius = earth.Radius;
        _earth = earth;

        Zoom(_earth.Radius + 100);
    }

    private void Update() {
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput != 0)
            Move(verticalInput);
        if (horizontalInput != 0)
            Zoom(horizontalInput);
    }

    private void Move(float direction) {
        var earthPosition = _earth.transform.position;
        transform.RotateAround(earthPosition, Vector3.right, direction * speed * Time.deltaTime);
        var newPos = (transform.position - earthPosition).normalized * _radius + earthPosition;
        transform.position = newPos;

        _distanceToEarth = (transform.position - earthPosition).magnitude - _earth.Radius;
        
        _onChangeDistance?.Invoke(_distanceToEarth);
    }

    private void Zoom(float value) {
        _radius = Mathf.Clamp((_radius + value) * zoomSpeed, minDistance, maxDistance);
        Move(0);
    }

    public void AddListenerOnChangeDistance(Action<float> action) {
        _onChangeDistance += action;
    }
    public void RemoveListenerOnChangeDistance(Action<float> action) {
        if (_onChangeDistance == null) return;
        _onChangeDistance -= action;
    }
}