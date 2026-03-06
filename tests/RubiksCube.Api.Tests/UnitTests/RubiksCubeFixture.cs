using RubiksCube.Api.Cube.Services;
using RubiksCube.Api.Cube.Services.Models;

namespace RubiksCube.Api.Tests.UnitTests;

public class RubiksCubeFixture
{
	private static readonly RubiksCubeRotatorService Service = new();

	public static CubeState GetSolved() => CubeState.CreateSolved(RubiksCubeRotatorService.CellsPerFace);

	public static CubeState ApplyMoves(CubeState state, params string[] moves)
	{
		var clone = state.Clone();
		
		foreach (var move in moves)
			Service.RotateCube(clone, move);

		return clone;
	}

	public static bool IsSameState(CubeState a, CubeState b) =>
		IsSequenceEqual(a.Up, b.Up)
		&& IsSequenceEqual(a.Down, b.Down)
		&& IsSequenceEqual(a.Left, b.Left)
		&& IsSequenceEqual(a.Right, b.Right)
		&& IsSequenceEqual(a.Front, b.Front)
		&& IsSequenceEqual(a.Back, b.Back);

	private static bool IsSequenceEqual(char[] array, char[] compareTo) => array.AsSpan().SequenceEqual(compareTo);

	public static string? GetStateDiff(CubeState expected, CubeState actual)
	{
		var names = new[] {
			("Up", expected.Up, actual.Up),
			("Down", expected.Down, actual.Down),
			("Front", expected.Front, actual.Front),
			("Back", expected.Back, actual.Back),
			("Left", expected.Left, actual.Left),
			("Right", expected.Right, actual.Right)
		};

		foreach (var (name, exp, act) in names)
			for (var index = 0; index < RubiksCubeRotatorService.CellsPerFace; index++)
				if (exp[index] != act[index])
					return $"{name}[{index}]: expected '{exp[index]}' actual '{act[index]}'";

		return null;
	}

	public static string FormatStateExploded(CubeState s)
	{
		int row1 = 0;
		int row2 = 3;
		int row3 = 6;

		return $"""
			Up
			{FormatRow(s.Up, row1)}
			{FormatRow(s.Up, row2)}
			{FormatRow(s.Up, row3)}

			Left    Front   Right   Back
			{FormatRow(s.Left, row1)}   {FormatRow(s.Front, row1)}   {FormatRow(s.Right, row1)}   {FormatRow(s.Back, row1)}
			{FormatRow(s.Left, row2)}   {FormatRow(s.Front, row2)}   {FormatRow(s.Right, row2)}   {FormatRow(s.Back, row2)}
			{FormatRow(s.Left, row3)}   {FormatRow(s.Front, row3)}   {FormatRow(s.Right, row3)}   {FormatRow(s.Back, row3)}

			Down
			{FormatRow(s.Down, row1)}
			{FormatRow(s.Down, row2)}
			{FormatRow(s.Down, row3)}
			""";
	}

	private static string FormatRow(char[] face, int start) => $"{face[start]}{face[start + 1]}{face[start + 2]}";
}
