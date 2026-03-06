using Microsoft.AspNetCore.Mvc;
using RubiksCube.Api.Cube.Api.Models;
using RubiksCube.Api.Cube.Services;
using RubiksCube.Api.Cube.Services.Models;

namespace RubiksCube.Api.Cube.Api;

public static class CubeEndpoints
{
	public static void MapCubeEndpoints(this IEndpointRouteBuilder app)
	{
		app.MapGet("/api/cube", GetInitial)
			.Produces<CubeState>(StatusCodes.Status200OK);

		app.MapPost("/api/cube", PostRotate)
			.Produces<CubeState>(StatusCodes.Status200OK)
			.Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest);
	}

	private static IResult GetInitial() =>
		Results.Ok(CubeState.CreateSolved(RubiksCubeRotatorService.CellsPerFace));

	private static IResult PostRotate(RubiksCubeRotatorService service, [FromBody] RotateRequest request)
	{
		var state = request.State;
		service.RotateCube(state, request.Move);
		return Results.Ok(state);
	}
}
