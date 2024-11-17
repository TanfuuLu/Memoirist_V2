var builder = DistributedApplication.CreateBuilder(args);

var usernameDb = builder.AddParameter("usernameDb", true);
var passwordDb = builder.AddParameter("passwordDb", true);
var postgreConfig = builder.AddPostgres("postgres", userName: usernameDb, password: passwordDb, port: 8001)
		   .WithImage("postgres")
		   .WithPgAdmin()
		   .WithDataVolume("memoiristVolume", isReadOnly: false);
var writerDb = postgreConfig.AddDatabase("writerDb");// writer service db
var authenDb = postgreConfig.AddDatabase("authenDb");//yarp gateway authen db 
var storyDb = postgreConfig.AddDatabase("storyDb");// story service db 
var postDb = postgreConfig.AddDatabase("postDb");

var rabbitUser = builder.AddParameter("MQUsername");
var rabbitPass = builder.AddParameter("MQPassword", secret: true);
var rabbitConfig = builder.AddRabbitMQ("rabbitMess", password: rabbitPass, userName: rabbitUser, port: 5672)
			.WithManagementPlugin()
			.WithDataVolume("rabbitVolume", isReadOnly: false);


var writerProj =  builder.AddProject<Projects.Memoirist_V2_WriterService>("memoirist-v2-writerservice")
	.WithReference(writerDb)
	.WithReference(rabbitConfig)
	.WithEndpoint("https", endpoint => endpoint.IsProxied = false);

var storyProj = builder.AddProject<Projects.Memoirist_V2_StoryService>("memoirist-v2-storyservice")
	.WithReference(storyDb)
	.WithEndpoint("https", endpoint => endpoint.IsProxied = false)
	.WithReference(rabbitConfig);

var postProj = builder.AddProject<Projects.Memoirist_V2_PostService>("memoirist-v2-postservice")
	.WithReference(postDb)
	.WithReference(rabbitConfig)
	.WithEndpoint("https", endpoint => endpoint.IsProxied = false);

var yarpProj = builder.AddProject<Projects.Memoirist_V2_YarpGateway>("memoirist-v2-yarpgateway")
	.WithEndpoint("https", endpoint => endpoint.IsProxied = false)
	.WithReference(rabbitConfig)
	.WithReference(writerProj)
	.WithReference(storyProj)
	.WithReference(postProj)
	.WithReference(authenDb)// add authenDb for yarp gateway
	.WithReference(writerDb)
	.WithReference(storyDb)
	.WithReference(postDb)
	;//add writerDb for yarp gateway


builder.AddProject<Projects.Memoirist_V2_MigrationService>("memoirist-v2-migrationservice")
	.WithReference(storyDb)
	.WithReference(authenDb)
	.WithReference(writerDb)
	.WithReference(postDb);

builder.Build().Run();
