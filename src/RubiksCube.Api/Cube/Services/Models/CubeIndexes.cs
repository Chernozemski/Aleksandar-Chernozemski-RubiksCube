namespace RubiksCube.Api.Cube.Services.Models;

public record struct CubeIndexes((char[] Face, int[] Indexes) Left,
								 (char[] Face, int[] Indexes) Up,
								 (char[] Face, int[] Indexes) Right,
								 (char[] Face, int[] Indexes) Down,
								 (char[] Face, int[] Indexes) Facing)
{
	public static implicit operator CubeIndexes(((char[] Face, int[] Indexes) Left,
												(char[] Face, int[] Indexes) Up,
												(char[] Face, int[] Indexes) Right,
												(char[] Face, int[] Indexes) Down,
												(char[] Face, int[] Indexes) Facing) value) =>
		new(value.Left, value.Up, value.Right, value.Down, value.Facing);
}