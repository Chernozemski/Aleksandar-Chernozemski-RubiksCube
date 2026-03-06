using RubiksCube.Api.Cube.Services.Models;

namespace RubiksCube.Api.Tests.UnitTests;

public class RubiksCubeTests
{
	[Fact]
	public void CreateSolved_returns_solved_cube_all_faces_same_color()
	{
		var state = RubiksCubeFixture.GetSolved();

		Assert.All(state.Up, c => Assert.Equal('W', c));
		Assert.All(state.Down, c => Assert.Equal('Y', c));
		Assert.All(state.Front, c => Assert.Equal('G', c));
		Assert.All(state.Back, c => Assert.Equal('B', c));
		Assert.All(state.Left, c => Assert.Equal('O', c));
		Assert.All(state.Right, c => Assert.Equal('R', c));
	}

	[Theory]
	[InlineData("F", "F", "F", "F")]
	[InlineData("R", "R", "R", "R")]
	[InlineData("U", "U", "U", "U")]
	[InlineData("D", "D", "D", "D")]
	[InlineData("B", "B", "B", "B")]
	[InlineData("L", "L", "L", "L")]
	public void Rotate_same_face_four_times_returns_to_identity(string move1, string move2, string move3, string move4)
	{
		var solved = RubiksCubeFixture.GetSolved();

		var after = RubiksCubeFixture.ApplyMoves(solved, move1, move2, move3, move4);

		Assert.True(RubiksCubeFixture.IsSameState(solved, after));
	}

	[Theory]
	[InlineData("F", "F'")]
	[InlineData("R", "R'")]
	[InlineData("U", "U'")]
	[InlineData("D", "D'")]
	[InlineData("B", "B'")]
	[InlineData("L", "L'")]
	public void Rotate_face_then_prime_returns_to_identity(string move, string prime)
	{
		var solved = RubiksCubeFixture.GetSolved();

		var after = RubiksCubeFixture.ApplyMoves(solved, move, prime);

		Assert.True(RubiksCubeFixture.IsSameState(solved, after));
	}

	[Theory]
	[InlineData("F", "Front", 'G')]
	[InlineData("F'", "Front", 'G')]
	[InlineData("R", "Right", 'R')]
	[InlineData("R'", "Right", 'R')]
	[InlineData("U", "Up", 'W')]
	[InlineData("D", "Down", 'Y')]
	[InlineData("D'", "Down", 'Y')]
	[InlineData("B", "Back", 'B')]
	[InlineData("B'", "Back", 'B')]
	[InlineData("L", "Left", 'O')]
	public void After_single_move_rotated_face_stays_solid_color(string move, string faceName, char expectedColor)
	{
		var solved = RubiksCubeFixture.GetSolved();

		var after = RubiksCubeFixture.ApplyMoves(solved, move);
		var face = GetFace(after, faceName);

		Assert.All(face, c => Assert.Equal(expectedColor, c));
	}

	private static char[] GetFace(CubeState state, string faceName) => faceName switch
	{
		"Up" => state.Up,
		"Down" => state.Down,
		"Front" => state.Front,
		"Back" => state.Back,
		"Left" => state.Left,
		"Right" => state.Right,
		_ => throw new ArgumentOutOfRangeException(nameof(faceName), faceName, null)
	};

	[Fact]
	public void F_rotates_Front_face_in_place_red_stays_on_Front()
	{
		var state = RubiksCubeFixture.GetSolved().Clone();
		state.Front[3] = 'R';

		var after = RubiksCubeFixture.ApplyMoves(state, "F");

		Assert.Equal('R', after.Front[1]);
	}

	[Fact]
	public void Simple_rotations_matches_expected_state()
	{
		var solved = RubiksCubeFixture.GetSolved();
		var singleMoveExpected = new (string move, CubeState expected)[]
		{
			("F", new CubeState
			{
				Up = "WWWWWWOOO".ToCharArray(),
				Left = "OOYOOYOOY".ToCharArray(),
				Front = "GGGGGGGGG".ToCharArray(),
				Right = "WRRWRRWRR".ToCharArray(),
				Back = "BBBBBBBBB".ToCharArray(),
				Down = "RRRYYYYYY".ToCharArray()
			}),
			("F'", new CubeState
			{
				Up = "WWWWWWRRR".ToCharArray(),
				Left = "OOWOOWOOW".ToCharArray(),
				Front = "GGGGGGGGG".ToCharArray(),
				Right = "YRRYRRYRR".ToCharArray(),
				Back = "BBBBBBBBB".ToCharArray(),
				Down = "OOOYYYYYY".ToCharArray()
			}),
		};

		foreach (var (move, expected) in singleMoveExpected)
		{
			var actual = RubiksCubeFixture.ApplyMoves(solved, move);
			var diff = RubiksCubeFixture.GetStateDiff(expected, actual);
			Assert.True(diff is null, $"After {move} from solved: {diff}");
		}
	}

	[Fact]
	public void Multiple_moves_then_inverses_returns_to_identity()
	{
		var solved = RubiksCubeFixture.GetSolved();

		var after = RubiksCubeFixture.ApplyMoves(solved, "F", "R", "U", "B'", "L", "D'", "D", "L'", "B", "U'", "R'", "F'");

		Assert.True(RubiksCubeFixture.IsSameState(solved, after));
	}

	[Fact]
	public void Scramble_then_solve_returns_to_solved()
	{
		var solved = RubiksCubeFixture.GetSolved();

		var scrambled = RubiksCubeFixture.ApplyMoves(solved, "F", "R", "U", "B'", "L");
		var backToSolved = RubiksCubeFixture.ApplyMoves(scrambled, "L'", "B", "U'", "R'", "F'");

		Assert.False(RubiksCubeFixture.IsSameState(solved, scrambled));
		var diff = RubiksCubeFixture.GetStateDiff(solved, backToSolved);
		Assert.True(diff is null, $"Difference: {diff}. Actual state:\n{RubiksCubeFixture.FormatStateExploded(backToSolved)}");
	}

	[Fact]
	public void Scramble_rotate_then_returns_expected_state()
	{
		var solved = RubiksCubeFixture.GetSolved();
		solved.Down[0] = 'W';
		solved.Down[1] = 'G';
		solved.Down[2] = 'R';

		var after = RubiksCubeFixture.ApplyMoves(solved, "F");

		var expected = new CubeState
		{
			Up = "WWWWWWOOO".ToCharArray(),
			Left = "OOWOOGOOR".ToCharArray(),
			Front = "GGGGGGGGG".ToCharArray(),
			Right = "WRRWRRWRR".ToCharArray(),
			Back = "BBBBBBBBB".ToCharArray(),
			Down = "RRRYYYYYY".ToCharArray()
		};

		var diff = RubiksCubeFixture.GetStateDiff(expected, after);
		Assert.True(diff is null, $"After F from solved: {diff}");

		var afterPrime = RubiksCubeFixture.ApplyMoves(after, "F'");

		diff = RubiksCubeFixture.GetStateDiff(solved, afterPrime);
		Assert.True(diff is null, $"After F' from after F: {diff}");
	}

	[Fact]
	public void Rotate_complex_returns_expected_state()
	{
		var solved = RubiksCubeFixture.GetSolved();

		(string step, CubeState expected)[] steps = [
			("F", new CubeState
			{
				Up = "WWWWWWOOO".ToCharArray(),
				Left = "OOYOOYOOY".ToCharArray(),
				Front = "GGGGGGGGG".ToCharArray(),
				Right = "WRRWRRWRR".ToCharArray(),
				Back = "BBBBBBBBB".ToCharArray(),
				Down = "RRRYYYYYY".ToCharArray()
			}),
			("R'", new CubeState
			{
				Up = "WWBWWBOOB".ToCharArray(),
				Left = "OOYOOYOOY".ToCharArray(),
				Front = "GGWGGWGGO".ToCharArray(),
				Right = "RRRRRRWWW".ToCharArray(),
				Back = "YBBYBBRBB".ToCharArray(),
				Down = "RRGYYGYYG".ToCharArray()
			}),
			("U", new CubeState
			{
				Up = "OWWOWWBBB".ToCharArray(),
				Left = "GGWOOYOOY".ToCharArray(),
				Front = "RRRGGWGGO".ToCharArray(),
				Right = "YBBRRRWWW".ToCharArray(),
				Back = "OOYYBBRBB".ToCharArray(),
				Down = "RRGYYGYYG".ToCharArray()
			}),
			("B'", new CubeState
			{
				Up = "OOGOWWBBB".ToCharArray(),
				Left = "YGWYOYGOY".ToCharArray(),
				Front = "RRRGGWGGO".ToCharArray(),
				Right = "YBORRWWWW".ToCharArray(),
				Back = "YBBOBBOYR".ToCharArray(),
				Down = "RRGYYGWRB".ToCharArray()
			}),
			("L", new CubeState
			{
				Up = "ROGBWWBBB".ToCharArray(),
				Left = "GYYOOGYYW".ToCharArray(),
				Front = "ORROGWBGO".ToCharArray(),
				Right = "YBORRWWWW".ToCharArray(),
				Back = "YBWOBYOYR".ToCharArray(),
				Down = "RRGGYGGRB".ToCharArray()
			}),
			("D'", new CubeState
			{
				Up = "ROGBWWBBB".ToCharArray(),
				Left = "GYYOOGBGO".ToCharArray(),
				Front = "ORROGWWWW".ToCharArray(),
				Right = "YBORRWOYR".ToCharArray(),
				Back = "YBWOBYYYW".ToCharArray(),
				Down = "GGBRYRRGG".ToCharArray()
			}),
			("L'", new CubeState
			{
				Up = "OOGOWWWBB".ToCharArray(),
				Left = "YGOYOGGOB".ToCharArray(),
				Front = "GRRRGWRWW".ToCharArray(),
				Right = "YBORRWOYR".ToCharArray(),
				Back = "YBBOBBYYR".ToCharArray(),
				Down = "WGBYYRWGG".ToCharArray()
			}),
			("D", new CubeState
			{
				Up = "OOGOWWWBB".ToCharArray(),
				Left = "YGOYOGYYR".ToCharArray(),
				Front = "GRRRGWGOB".ToCharArray(),
				Right = "YBORRWRWW".ToCharArray(),
				Back = "YBBOBBOYR".ToCharArray(),
				Down = "WYWGYGGRB".ToCharArray()
			}),
			("F'", new CubeState
			{
				Up = "OOGOWWYRR".ToCharArray(),
				Left = "YGBYOBYYW".ToCharArray(),
				Front = "RWBRGOGRG".ToCharArray(),
				Right = "WBOYRWWWW".ToCharArray(),
				Back = "YBBOBBOYR".ToCharArray(),
				Down = "OGRGYGGRB".ToCharArray()
			}),
			("R", new CubeState
			{
				Up = "OOBOWOYRG".ToCharArray(),
				Left = "YGBYOBYYW".ToCharArray(),
				Front = "RWRRGGGRB".ToCharArray(),
				Right = "WYWWRBWWO".ToCharArray(),
				Back = "RBBWBBGYR".ToCharArray(),
				Down = "OGOGYOGRY".ToCharArray()
			}),
			("U'", new CubeState
			{
				Up = "BOGOWROOY".ToCharArray(),
				Left = "RBBYOBYYW".ToCharArray(),
				Front = "YGBRGGGRB".ToCharArray(),
				Right = "RWRWRBWWO".ToCharArray(),
				Back = "WYWWBBGYR".ToCharArray(),
				Down = "OGOGYOGRY".ToCharArray()
			}),
			("B", new CubeState
			{
				Up = "RBOOWROOY".ToCharArray(),
				Left = "GBBOOBBYW".ToCharArray(),
				Front = "YGBRGGGRB".ToCharArray(),
				Right = "RWYWRRWWG".ToCharArray(),
				Back = "GWWYBYRBW".ToCharArray(),
				Down = "OGOGYORYY".ToCharArray()
			}),

		];

		var state = solved;
		foreach (var (step, expected) in steps)
		{
			state = RubiksCubeFixture.ApplyMoves(state, step);
			var diff = RubiksCubeFixture.GetStateDiff(expected, state);
			Assert.True(diff is null, $"{step} should produce the exact expected cube state. Difference: {diff}. Actual state:\n{RubiksCubeFixture.FormatStateExploded(state)}");
		}
	}
}