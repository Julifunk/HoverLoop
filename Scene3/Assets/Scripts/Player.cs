using UnityEngine;

public class Player : MonoBehaviour
{

    public PipeSystem pipeSystem;
    public float velocity;
    public float rotationVelocity;

    private Pipe _currentPipe;
    private float _distanceTraveled;
    
    private float _deltaToRotation;
    private float _systemRotation;
    
    private float _avatarRotation;
    private float _worldRotation;
    private Transform _rotator;
    private Transform _world;
	
    
    
    // Start is called before the first frame update
    void Start()
    {
        _world = pipeSystem.transform.parent;
        _rotator = transform.GetChild(0);
        _currentPipe = pipeSystem.SetupFirstPipe();
        SetupCurrentPipe();
    }

    // Update is called once per frame
    void Update()
    {
        var delta = velocity * Time.deltaTime;
        _distanceTraveled += delta;
        _systemRotation += delta * _deltaToRotation;
        
        if (_systemRotation >= _currentPipe.CurveAngle) {
            delta = (_systemRotation - _currentPipe.CurveAngle) / _deltaToRotation;
            _currentPipe = pipeSystem.SetupNextPipe();
            SetupCurrentPipe();
            _systemRotation = delta * _deltaToRotation;
        }

        pipeSystem.transform.localRotation =
            Quaternion.Euler(0f, 0f, _systemRotation);
        UpdateAvatarRotation();
    }
    
    private void SetupCurrentPipe () {
        _deltaToRotation = 360f / (2f * Mathf.PI * _currentPipe.CurveRadius);
        _worldRotation += _currentPipe.RelativeRotation;
        if (_worldRotation < 0f) {
            _worldRotation += 360f;
        }
        else if (_worldRotation >= 360f) {
            _worldRotation -= 360f;
        }
        _world.localRotation = Quaternion.Euler(_worldRotation, 0f, 0f);
    }
    
    private void UpdateAvatarRotation () {
        _avatarRotation +=
            rotationVelocity * Time.deltaTime * Input.GetAxis("Horizontal");
        if (_avatarRotation < 0f) {
            _avatarRotation += 360f;
        }
        else if (_avatarRotation >= 360f) {
            _avatarRotation -= 360f;
        }
        _rotator.localRotation = Quaternion.Euler(_avatarRotation, 0f, 0f);
    }
    
    public void Die () {
        gameObject.SetActive(false);
    }
}
