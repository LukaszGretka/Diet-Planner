var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.DietPlanner_Api>("Api");

builder.AddNpmApp("web", "../../DP.Web")
    .WithHttpEndpoint(env: "PORT", port: 4200)
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
            