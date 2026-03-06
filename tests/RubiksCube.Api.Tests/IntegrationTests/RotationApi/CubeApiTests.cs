using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using RubiksCube.Api.Cube.Services.Models;
using RubiksCube.Api.Tests.UnitTests;

namespace RubiksCube.Api.Tests.IntegrationTests.RotationApi;

public class CubeApiTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _client = factory.CreateClient();

	[Fact]
	public async Task Get_returns_200_and_solved_cube()
	{
		var response = await _client.GetAsync("/api/cube");
		response.EnsureSuccessStatusCode();
		var state = await response.Content.ReadFromJsonAsync<CubeState>();
		
		Assert.NotNull(state);
		Assert.Equal(9, state.Up.Length);
		Assert.Equal(9, state.Down.Length);
		Assert.Equal(9, state.Front.Length);
		Assert.Equal(9, state.Back.Length);
		Assert.Equal(9, state.Left.Length);
		Assert.Equal(9, state.Right.Length);
		Assert.All(state.Up, c => Assert.Equal('W', c));
		Assert.All(state.Front, c => Assert.Equal('G', c));
		Assert.All(state.Right, c => Assert.Equal('R', c));
		Assert.All(state.Back, c => Assert.Equal('B', c));
		Assert.All(state.Down, c => Assert.Equal('Y', c));
		Assert.All(state.Left, c => Assert.Equal('O', c));
	}

	[Fact]
	public async Task Post_with_valid_move_returns_200_and_modified_state()
	{
		var getResponse = await _client.GetAsync("/api/cube");
		getResponse.EnsureSuccessStatusCode();
		var initialState = await getResponse.Content.ReadFromJsonAsync<CubeState>();

		Assert.NotNull(initialState);

		var postResponse = await _client.PostAsJsonAsync("/api/cube", new { State = initialState, Move = "F" });
		postResponse.EnsureSuccessStatusCode();
		var afterState = await postResponse.Content.ReadFromJsonAsync<CubeState>();
		
		Assert.NotNull(afterState);
		Assert.All(afterState.Front, c => Assert.Equal('G', c));
		Assert.Equal('O', afterState.Up[6]);
		Assert.Equal('O', afterState.Up[7]);
		Assert.Equal('O', afterState.Up[8]);
	}

	[Fact]
	public async Task Post_with_invalid_move_returns_400_and_validation_message()
	{
		var response = await _client.PostAsJsonAsync("/api/cube", new { State = RubiksCubeFixture.GetSolved(), Move = "X" });

		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		var content = await response.Content.ReadAsStringAsync();
		
		Assert.Contains("Move must be one of", content);
	}

	[Fact]
	public async Task Post_with_empty_move_returns_400()
	{
		var response = await _client.PostAsJsonAsync("/api/cube", new { State = RubiksCubeFixture.GetSolved(), Move = "" });
		
		Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
	}

	[Fact]
	public async Task Post_R_prime_then_R_returns_to_same_as_initial()
	{
		var getResponse = await _client.GetAsync("/api/cube");
		getResponse.EnsureSuccessStatusCode();
		var initial = await getResponse.Content.ReadFromJsonAsync<CubeState>();

		Assert.NotNull(initial);

		var move1Response = await _client.PostAsJsonAsync("/api/cube", new { State = initial, Move = "R'" });
		move1Response.EnsureSuccessStatusCode();
		var afterMove1State = await move1Response.Content.ReadFromJsonAsync<CubeState>();

		Assert.NotNull(afterMove1State);

		var move2Response = await _client.PostAsJsonAsync("/api/cube", new { State = afterMove1State, Move = "R" });
		move2Response.EnsureSuccessStatusCode();
		var afterMove2State = await move2Response.Content.ReadFromJsonAsync<CubeState>();

		Assert.NotNull(afterMove2State);
		Assert.True(RubiksCubeFixture.IsSameState(initial, afterMove2State));
	}
}
