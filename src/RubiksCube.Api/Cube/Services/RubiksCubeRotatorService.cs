using RubiksCube.Api.Cube.Services.Models;

namespace RubiksCube.Api.Cube.Services;

public class RubiksCubeRotatorService
{
	public const int CellsPerFace = 9;
	private const int StripLength = 3;

	private static readonly int[] topIndexes = [0, 1, 2];
	private static readonly int[] leftIndexes = [0, 3, 6];
	private static readonly int[] rightIndexes = [2, 5, 8];
	private static readonly int[] bottomIndexes = [6, 7, 8];

	private static readonly int[] topIndexesReversed = [.. topIndexes.Reverse()];
	private static readonly int[] leftIndexesReversed = [.. leftIndexes.Reverse()];
	private static readonly int[] rightIndexesReversed = [.. rightIndexes.Reverse()];
	private static readonly int[] bottomIndexesReversed = [.. bottomIndexes.Reverse()];

	private static readonly int[] clockwiseSelfSpin = [
														6, 3, 0,
														7, 4, 1,
														8, 5, 2];

	private static readonly int[] ccwSelfSpin = [.. clockwiseSelfSpin.Reverse()];

	public void RotateCube(CubeState state, string move)
	{
		var cubeIndexes = GetCubeIndexesToMove(state, move);
		var (left, up, right, down, facing) = cubeIndexes;

		Span<char> leftStrip = stackalloc char[StripLength];
		Span<char> upStrip = stackalloc char[StripLength];
		Span<char> rightStrip = stackalloc char[StripLength];
		Span<char> downStrip = stackalloc char[StripLength];
		Span<char> facingSnapshot = stackalloc char[facing.Face.Length];

		for (int stripIndex = 0; stripIndex < StripLength; stripIndex++)
		{
			leftStrip[stripIndex] = left.Face[left.Indexes[stripIndex]];
			upStrip[stripIndex] = up.Face[up.Indexes[stripIndex]];
			rightStrip[stripIndex] = right.Face[right.Indexes[stripIndex]];
			downStrip[stripIndex] = down.Face[down.Indexes[stripIndex]];
		}

		facing.Face.AsSpan().CopyTo(facingSnapshot);

		for (int stripIndex = 0; stripIndex < StripLength; stripIndex++)
		{
			left.Face[left.Indexes[stripIndex]] = downStrip[stripIndex];
			up.Face[up.Indexes[stripIndex]] = leftStrip[stripIndex];
			right.Face[right.Indexes[stripIndex]] = upStrip[stripIndex];
			down.Face[down.Indexes[stripIndex]] = rightStrip[stripIndex];
		}

		for (int cellIndex = 0; cellIndex < facing.Face.Length; cellIndex++)
			facing.Face[cellIndex] = facingSnapshot[facing.Indexes[cellIndex]];
	}

	private static CubeIndexes GetCubeIndexesToMove(CubeState state, string move) =>
		move switch
		{
			"F" => ((state.Left, rightIndexes),
					(state.Up, bottomIndexes),
					(state.Right, leftIndexes),
					(state.Down, topIndexes),
					(state.Front, clockwiseSelfSpin)),

			"F'" => ((state.Right, leftIndexesReversed),
					(state.Up, bottomIndexesReversed),
					(state.Left, rightIndexes),
					(state.Down, topIndexes),
					(state.Front, ccwSelfSpin)),

			"R" => ((state.Front, rightIndexesReversed),
					(state.Up, rightIndexesReversed),
					(state.Back, leftIndexes),
					(state.Down, rightIndexesReversed),
					(state.Right, clockwiseSelfSpin)),

			"R'" => ((state.Back, leftIndexesReversed),
					(state.Up, rightIndexes),
					(state.Front, rightIndexes),
					(state.Down, rightIndexes),
					(state.Right, ccwSelfSpin)),

			"U" => ((state.Left, topIndexes),
					(state.Back, topIndexes),
					(state.Right, topIndexes),
					(state.Front, topIndexes),
					(state.Up, clockwiseSelfSpin)),

			"U'" => ((state.Right, topIndexesReversed),
					(state.Back, topIndexesReversed),
					(state.Left, topIndexesReversed),
					(state.Front, topIndexesReversed),
					(state.Up, ccwSelfSpin)),

			"B" => ((state.Right, rightIndexes),
					(state.Up, topIndexes),
					(state.Left, leftIndexesReversed),
					(state.Down, bottomIndexesReversed),
					(state.Back, clockwiseSelfSpin)),

			"B'" => ((state.Left, leftIndexes),
					(state.Up, topIndexesReversed),
					(state.Right, rightIndexesReversed),
					(state.Down, bottomIndexes),
					(state.Back, ccwSelfSpin)),

			"L" => ((state.Back, rightIndexes),
					(state.Up, leftIndexesReversed),
					(state.Front, leftIndexesReversed),
					(state.Down, leftIndexesReversed),
					(state.Left, clockwiseSelfSpin)),

			"L'" => ((state.Front, leftIndexesReversed),
					(state.Up, leftIndexesReversed),
					(state.Back, rightIndexes),
					(state.Down, leftIndexesReversed),
					(state.Left, ccwSelfSpin)),

			"D" => ((state.Left, bottomIndexes),
					(state.Front, bottomIndexes),
					(state.Right, bottomIndexes),
					(state.Back, bottomIndexes),
					(state.Down, clockwiseSelfSpin)),

			"D'" => ((state.Right, bottomIndexes),
					(state.Front, bottomIndexes),
					(state.Left, bottomIndexes),
					(state.Back, bottomIndexes),
					(state.Down, ccwSelfSpin)),

			_ => throw new ArgumentOutOfRangeException(nameof(move), move, "Invalid move")
		};
}
