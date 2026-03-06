using Microsoft.AspNetCore.Diagnostics;
using RubiksCube.Api.Cube.Api;
using RubiksCube.Api.Cube.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.AddProblemDetails()
	.AddOpenApi()
	.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddSingleton<RubiksCubeRotatorService>()
	.AddValidation()
	.AddCors(opts =>
		opts.AddDefaultPolicy(pol =>
			pol.WithOrigins(
					builder.Configuration
						.GetSection("Cors:FrontEndOrigin")
						.Get<string>()
					?? throw new ArgumentNullException("origins", "CORS configuration missing"))
				.AllowAnyMethod()
				.AllowAnyHeader()
				.SetPreflightMaxAge(TimeSpan.FromDays(1)))
	);

var app = builder.Build();

app.UseExceptionHandler(c =>
	c.Run(async ctx =>
	{
		var feature = ctx.Features.Get<IExceptionHandlerPathFeature>();
		var ex = feature?.Error;
		var statusCode = ex is BadHttpRequestException bad
			? bad.StatusCode
			: StatusCodes.Status500InternalServerError;
		await Results.Problem(detail: ex?.Message, statusCode: statusCode).ExecuteAsync(ctx);
	}));

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger()
		.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", "Rubik's Cube API v1"));
}

app.UseCors();
app.MapCubeEndpoints();

app.Run();
