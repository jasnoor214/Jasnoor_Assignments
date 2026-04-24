
create an project with the name AzureFunctionsTangyWeb of asp.net core mvc and solution name should be AzureFunctionExample means inside the solution this is my first project 
with the name AzureFunctionsTangyWeb okay 

so set this as a start up project and check this project is running or not .

so now go to models  folder of this project and add this model like this 


  namespace AzureFunctionsTangyWeb.Models
{
    public class SalesRequest
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
    }
}

Now go to index view of home controller and delete all content over there and add this design for the above model 

@model SalesRequest
<form method="post" enctype="multipart/form-data">
    <div class="text-center">
        <h1 class="display-4">Welcome</h1>
        <p>Enter your details to apply today!</p>
        <div class="row py-1">
            <div class="col-3">
                Name:
            </div>
            <div class="col-6">
                <input asp-for="Name" class="form-control" />
            </div>
        </div>
        <div class="row py-1">
            <div class="col-3">
                Email:
            </div>
            <div class="col-6">
                <input asp-for="Email" class="form-control" />
            </div>
        </div>
        <div class="row py-1">
            <div class="col-3">
                Phone:
            </div>
            <div class="col-6">
                <input asp-for="Phone" class="form-control" />
            </div>
        </div>
        <div class="row py-1">
            <div class="col-3">
                Upload Resume:
            </div>
            <div class="col-6">
                <input type="file" class="form-control pt-1" name="file" />
            </div>
        </div>

        <div class="row py-1">
            <div class="col-3">
            </div>
            <div class="col-6">
                <button type="submit" class="btn btn-success">Apply</button>
            </div>
        </div>
    </div>
</form>


Run the application and see the design of this index page once which is created based on above model 

Right now if i click button it is not working as post index method i had not written it here so will write later now

Next Now in the solution i want to add new project of azure function app so search in text box azure function and select the template and give the name as TangyAzureFunc to this function 

select .NET 8.0 isolated (Long Term Support) u select 

  here u can think azure function as some kind of endpoint and how that endpoint has to be triggered ...so select httptrigger here as option and select anaonomous option next 

  using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TangyAzureFunc;

public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function("Function1")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}

so a default code will be created like this ....above 

now i am changing function name like this [Function("OnSalesUploadWriteToQueue")] it means when i fill the form and do submit that data has to be stored in queue first in first out 
and also go to file and rename the file in solution as same and doing this the file will look like this now 

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TangyAzureFunc;

public class OnSalesUploadWriteToQueue
{
    private readonly ILogger<OnSalesUploadWriteToQueue> _logger;

    public OnSalesUploadWriteToQueue(ILogger<OnSalesUploadWriteToQueue> logger)
    {
        _logger = logger;
    }

    [Function("OnSalesUploadWriteToQueue")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}

so everywhere it looks like this now as shown above next 

so now what i want to do check the slide 1 in day 65 so now create an  azure storage account under  data storage check queues section will be there so there here you can add messages in queue and 
you can retrive it from there so some listner has to be added to listen to messages which are getting added to queue so from azure portal i am not doing anything just right now u can go to access keys of 
storage account and take connection string from there 

I am having a sotrage account with me i am using access key of connection string of that storage account and paste that in local.settings.json 

  {
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "paste it here ",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}

  
Now once test the function means as we have changed the name of function and file so this url which u are getting has to be invoked with from the fron end of that index page 

 http://localhost:7009/api/OnSalesUploadWriteToQueue  so this the url and it has to be called from index front end as from front end i want to put it in storage account 

Now go to program.cs of web project  AzureFunctionsTangyWeb 

  // Add services to the container.
  builder.Services.AddControllersWithViews();
  builder.Services.AddHttpClient();  //add this line after above line 


Now in the Home controller //added written there code added 


using System.Diagnostics;
using AzureFunctionsTangyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureFunctionsTangyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _httpClientFactory; //added

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory; //added
        }

        public IActionResult Index()
        {
            return View();
        }
//added 
        [HttpPost]
        public async Task<IActionResult> Index(SalesRequest salesRequest)
        {
            using var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(" http://localhost:7009/api/");
            await client.GetAsync("OnSalesUploadWriteToQueue");
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
Now run multiple projects now at a time if u are getting any port error 

go to tanzy function app properties ,debug and open the profile url and u can chnage the port if it is not working otherwsie leave it 

next just click the apply button the function app is getting called 

againn click agin the function is executed 

so right now it is calling the function app and it is working fine 

Now some of the details which we populate in form needs to passed to azure function now so sales request details has to be passsed to azure functions now so pass the values now from the web project now 

so code change again happens in index post method now 

install nugget package Newtonsoft.Json any stable version 

  [HttpPost]
  public async Task<IActionResult> Index(SalesRequest salesRequest)
  {
      using var client = _httpClientFactory.CreateClient();
      client.BaseAddress = new Uri("http://localhost:7009/api/");
      using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest), System.Text.Encoding.UTF8, "application/json"))
      {
          HttpResponseMessage response = await client.PostAsync("OnSalesUploadWriteToQueue", content);
          string returnValue = await response.Content.ReadAsStringAsync();
      }

      return RedirectToAction(nameof(Index));
  }

here  i will explain the code here i am taking the content and i am converting it into json format by doing seriaiztion in order to transfer data online it has to converted to json format 

earlier the method was get now it has been chnaged to post as in the body i want to send the object and return that in the form of string 

so now add Models folder in Azure function here also create a class of SalesRequest as i need to read it here also 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangyAzureFunc.Models
{
    public class SalesRequest
    {

        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
    }
}


go and add id also in web app post function 

  [HttpPost]
  public async Task<IActionResult> Index(SalesRequest salesRequest)
  {
      salesRequest.Id = Guid.NewGuid().ToString(); //added 
      using var client = _httpClientFactory.CreateClient();
      client.BaseAddress = new Uri("http://localhost:7009/api/");
      using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest), System.Text.Encoding.UTF8, "application/json"))
      {
          HttpResponseMessage response = await client.PostAsync("OnSalesUploadWriteToQueue", content);
          string returnValue = await response.Content.ReadAsStringAsync();
      }

      return RedirectToAction(nameof(Index));
  }



Next install package Microsoft.Azure.Functions.Worker.Extensions.Storage.Queues using nugget in azure function app 

also install Newtonsoft.Json  package from nugget 

so now azure function has to read that object and pass it to queue so i need to bind this function with the queue 

so now i will do coding for azure function now where i have not written any code here 



using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TangyAzureFunc.Models;

namespace TangyAzureFunc;

public class OnSalesUploadWriteToQueue
{
    private readonly ILogger<OnSalesUploadWriteToQueue> _logger;

    public OnSalesUploadWriteToQueue(ILogger<OnSalesUploadWriteToQueue> logger) 
    {
        _logger = logger;
    }
    [Function("OnSalesUploadWriteToQueue")]
    [QueueOutput("SalesRequestOutBound", Connection = "AzureWebJobsStorage")]
    public async Task<SalesRequest> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        SalesRequest? data = JsonConvert.DeserializeObject<SalesRequest>(requestBody);
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return data ?? new SalesRequest();
    }
}

Now run again  multiple functions it will automatically creates a view here 

so now i can see the queeue created here here i am not wantedly entering valeus in file as i had not written code for that 


Now go to localsettings.json of azure function 

and add this 

 "AzureSqlDatabase": "Server=LAPTOP-4G8BHPK9\\SQLEXPRESS;Database=AzureFundamentals;TrustServerCertificate=True;Trusted_Connection=True;"
and change server name and add one database in that server with AzureFundamentals 

create database AzureFundamentals

and keep remaining same okay ..and add this table locally 


CREATE TABLE [dbo].[SalesRequests] (
    [Id] [nvarchar](450) NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Email] [nvarchar](max) NOT NULL,
    [Phone] [nvarchar](max) NOT NULL,
    [Status] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_SalesRequests] PRIMARY KEY CLUSTERED (
        [Id] ASC
    )
);

Next add this table in remote also means in azure also okay ..
next show slide 2 which tells what task u are doing 

Now add Data Folder in function project and in that add one class ApplicationDbContext 
Now install 3 packages of entity framework here of 8.0.24 okay 

Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools

  using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangyAzureFunc.Models;

namespace TangyAzureFunc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<SalesRequest> SalesRequests { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SalesRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}


then in program.cs of function app see added line s


using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TangyAzureFunc.Data;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();


string connectionString = Environment.GetEnvironmentVariable("AzureSqlDatabase"); //added


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));//added 

builder.Build().Run();

Now i have to work on final trigger where queue trigger will happen and my database which is local gets populated and here  i can add many functions in function app there is no limit 
so now i am adding second function like this 

No need to create new projet right click the project and add new azure function now and give name as OnQueueTriggerUpdateDatabase

so let everthing be default select queue trigger and add it 

using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TangyAzureFunc;

public class OnQueueTriggerUpdateDatabase
{
    private readonly ILogger<OnQueueTriggerUpdateDatabase> _logger;

    public OnQueueTriggerUpdateDatabase(ILogger<OnQueueTriggerUpdateDatabase> logger)
    {
        _logger = logger;
    }

    [Function(nameof(OnQueueTriggerUpdateDatabase))]
    public void Run([QueueTrigger("myqueue-items", Connection = "")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
    }
}

will look like this 

 and now chnage the code like this 

  using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using TangyAzureFunc.Data;
using TangyAzureFunc.Models;

namespace TangyAzureFunc;

public class OnQueueTriggerUpdateDatabase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<OnQueueTriggerUpdateDatabase> _logger;

    public OnQueueTriggerUpdateDatabase(ILogger<OnQueueTriggerUpdateDatabase> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [Function(nameof(OnQueueTriggerUpdateDatabase))]
    public void Run([QueueTrigger("SalesRequestOutBound")] QueueMessage message)
    {
        string messageBody = message.Body.ToString();
        SalesRequest? salesRequest = JsonConvert.DeserializeObject<SalesRequest>(messageBody);

        if (salesRequest != null)
        {
            salesRequest.Status = "";
            _dbContext.SalesRequests.Add(salesRequest);
            _dbContext.SaveChanges();
        }
        else
        {
            _logger.LogWarning("Failed to deserialize the message body into a SalesRequest object.");
        }
    }
}

next if u want to see the same in azure sql db means comment and put like this your azure connection


  
{
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "paste here ",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    //"AzureSqlDatabase": "Server=LAPTOP-4G8BHPK9\\SQLEXPRESS;Database=AzureFundamentals;TrustServerCertificate=True;Trusted_Connection=True;",
    "AzureSqlDatabase": "Server=tcp:raghuserver.database.windows.net,1433;Initial Catalog=raghudb;Persist Security Info=False;User ID=raghuadmin;Password=SqlServer#123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}

Now it will go to azure sql okay 
