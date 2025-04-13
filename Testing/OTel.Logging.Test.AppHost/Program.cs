using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<OTel_Logging_Test_App>("Test-App");

builder.Build().Run();