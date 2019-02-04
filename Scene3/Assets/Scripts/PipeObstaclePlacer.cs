using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeObstaclePlacer : PipeObstacleGenerator
{
    public PipeObstacle[] obstaclePrefabs;

    public override void GenerateItems (Pipe pipe) {
        var angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
        for (var i = 0; i < pipe.CurveSegmentCount; i++) {
            var obstacle = Instantiate<PipeObstacle>(
                obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
            var pipeRotation =
                (Random.Range(0, pipe.pipeSegmentCount) + 0.5f) *
                360f / pipe.pipeSegmentCount;
            obstacle.Position(pipe, i * angleStep, pipeRotation);
        }
    }
}
