using System.ComponentModel.DataAnnotations;

namespace RubiksCube.Api.Cube.Attributes;

public class RotationMoveAttribute : ValidationAttribute
{
    private static readonly HashSet<string> validMoves = ["F", "F'", "R", "R'", "U", "U'", "B", "B'", "L", "L'", "D", "D'"];

	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
		value is string vStr
		&& validMoves.Contains(vStr)
			? ValidationResult.Success
			: new($"Move must be one of: {string.Join(", ", validMoves)}");
}
