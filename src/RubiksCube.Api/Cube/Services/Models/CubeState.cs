namespace RubiksCube.Api.Cube.Services.Models;

public class CubeState
{
	public char[] Up { get; set; } = [];
	public required char[] Down { get; set; }
	public required char[] Left { get; set; }
	public required char[] Right { get; set; }
	public required char[] Front { get; set; }
	public required char[] Back { get; set; }

	public CubeState Clone() => new()
	{
		Up = (char[])Up.Clone(),
		Down = (char[])Down.Clone(),
		Left = (char[])Left.Clone(),
		Right = (char[])Right.Clone(),
		Front = (char[])Front.Clone(),
		Back = (char[])Back.Clone()
	};

	public static CubeState CreateSolved(int cellsPerFace) => new()
	{
		Up = FillFace('W', cellsPerFace),
		Down = FillFace('Y', cellsPerFace),
		Left = FillFace('O', cellsPerFace),
		Right = FillFace('R', cellsPerFace),
		Front = FillFace('G', cellsPerFace),
		Back = FillFace('B', cellsPerFace)
	};

	private static char[] FillFace(char color, int cellsPerFace) =>
		[.. Enumerable.Repeat(color, cellsPerFace)];
}
