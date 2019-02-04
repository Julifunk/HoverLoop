using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeObstacleSpiralPlacer : PipeObstacleGenerator
{
    public PipeObstacle[] obstaclePrefabs;

    public override void GenerateItems (Pipe pipe) {
        var start = (Random.Range(0, pipe.pipeSegmentCount) + 0.5f);
        var direction = Random.value < 0.5f ? 1f : -1f;

        var angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
        for (var i = 0; i < pipe.CurveSegmentCount; i++) {
            var item = Instantiate<PipeObstacle>(
                obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
            var pipeRotation =
                (start + i * direction) * 360f / pipe.pipeSegmentCount;
            item.Position(pipe, i * angleStep, pipeRotation);
        }
    }
}
