var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.DietPlanner_Api>("Api");

builder.AddNpmApp("web", "../../DP.Web")
    .WithHttpEndpoint(port: 4200, isProxied: false)
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();