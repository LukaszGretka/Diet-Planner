var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.DietPlanner_Api>("Api");

builder.AddNpmApp("web", "../../DP.Web")
    .WithUrl("http://localhost:4200")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();