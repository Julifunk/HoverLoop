using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PipeSystem : MonoBehaviour
{
    public Pipe pipePrefab;

    public int pipeCount;
    public int emptyPipeCount;


    private Pipe[] _pipes;

    private void Awake () {
        _pipes = new Pipe[pipeCount];
        for (var i = 0; i < _pipes.Length; i++) {
            var pipe = _pipes[i] = Instantiate<Pipe>(pipePrefab);
            pipe.transform.SetParent(transform, false);
            pipe.Generate(i > emptyPipeCount);
            if (i > 0)
            {
                pipe.AlignWith(_pipes[i - 1]);
            }
        }
        AlignNextPipeWithOrigin();
    }

    public Pipe SetupFirstPipe()
    {
        transform.localPosition = new Vector3(0f, -_pipes[1].CurveRadius);
        return _pipes[1];
    }
    
    public Pipe SetupNextPipe () {
        ShiftPipes();
        AlignNextPipeWithOrigin();
        _pipes[_pipes.Length - 1].Generate();
        _pipes[_pipes.Length - 1].AlignWith(_pipes[_pipes.Length - 2]);
        transform.localPosition = new Vector3(0f, -_pipes[1].CurveRadius);
        return _pipes[1];
    }
    
    private void ShiftPipes () {
        var temp = _pipes[0];
        for (int i = 1; i < _pipes.Length; i++) {
            _pipes[i - 1] = _pipes[i];
        }
        _pipes[_pipes.Length - 1] = temp;
    }
    
    private void AlignNextPipeWithOrigin () {
        var transformToAlign = _pipes[1].transform;
        for (var i = 0; i < _pipes.Length; i++) {
            if (i != 1) {
                _pipes[i].transform.SetParent(transformToAlign);
            }
        }
		
        transformToAlign.localPosition = Vector3.zero;
        transformToAlign.localRotation = Quaternion.identity;
		
        for (var i = 0; i < _pipes.Length; i++) {
            if (i != 1) {
                _pipes[i].transform.SetParent(transform);
            }
        }
    }
}
