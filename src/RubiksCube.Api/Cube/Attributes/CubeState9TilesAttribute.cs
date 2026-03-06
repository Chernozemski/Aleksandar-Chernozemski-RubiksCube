using RubiksCube.Api.Cube.Services;
using RubiksCube.Api.Cube.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace RubiksCube.Api.Cube.Attributes;

public class CubeStateExactTilesAttribute : ValidationAttribute
{
	private const int cellsPerFace = RubiksCubeRotatorService.CellsPerFace;

	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
		(value is CubeState c)
		&& c.Up != null && c.Up.Length == cellsPerFace
		&& c.Down != null && c.Down.Length == cellsPerFace
		&& c.Left != null && c.Left.Length == cellsPerFace
		&& c.Right != null && c.Right.Length == cellsPerFace
		&& c.Front != null && c.Front.Length == cellsPerFace
		&& c.Back != null && c.Back.Length == cellsPerFace
			? ValidationResult.Success
			: new ValidationResult($"Invalid cube state: all six faces must have exactly {cellsPerFace} cells");
}