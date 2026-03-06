using RubiksCube.Api.Cube.Attributes;
using RubiksCube.Api.Cube.Services.Models;

namespace RubiksCube.Api.Cube.Api.Models;

public class RotateRequest
{
    [CubeStateExactTiles]
    public required CubeState State { get; set; }

    [RotationMove]
    public required string Move { get; set; }
}
