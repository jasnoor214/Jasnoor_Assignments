
create one asp .net core mvc application with name AzureSpookyLoginApp

add one model class into the models folder with the name 

using System.ComponentModel.DataAnnotations;

namespace AzureSpookyLogicApp.Models
{
    public class SpookyRequest
    {
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

    }
}

Go to Home Controller index view 

and remove that desing and keep this design provided below 

@model SpookyRequest
<form method="post" enctype="multipart/form-data">
    <div class="text-center">
        <h1 class="display-4">Welcome to Spooky Request Logic App</h1>
    </div>
    <div class="text-center">
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
            </div>
            <div class="col-6">
                <button type="submit" class="btn btn-success">Apply</button>
            </div>
        </div>
    </div>
</form>

add the following package in Newtonsoft.Json of stable version in the project 

and in program.cs add this   builder.Services.AddHttpClient();

  // Add services to the container.
  builder.Services.AddControllersWithViews();
  builder.Services.AddHttpClient();

Then in the Home controller add this 

       private readonly IHttpClientFactory _httpClientFactory;

   public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
   {
       _logger = logger;
       _httpClientFactory = httpClientFactory;
   }

now from get of index you will go the view where u will fill the form and after filling it will go to post of index which is given 
below 
  [HttpPost]
  public async Task<IActionResult> Index(SpookyRequest spookyrequest)
  {
      spookyrequest.Id = Guid.NewGuid().ToString(); //added 
      using var client = _httpClientFactory.CreateClient();
      //   client.BaseAddress = new Uri("http://localhost:7198/api/");
      var json = JsonConvert.SerializeObject(spookyrequest);
      
      using (var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"))
      {
          HttpResponseMessage response = await client.PostAsync("https://prod-08.centralus.logic.azure.com:443/workflows/9d2bb315c75f4104979d6eced9820173/triggers/When_an_HTTP_request_is_received/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2FWhen_an_HTTP_request_is_received%2Frun&sv=1.0&sig=dj_cs9Dx943x3H_ji2VHHThOGbfmJbrCxJWbCpVOLIw", content);
          string returnValue = await response.Content.ReadAsStringAsync();
      }

      return RedirectToAction(nameof(Index));
  }

you have to copy the code same as it is but shoudl should some parameters 

add this table in azure 
CREATE TABLE [dbo].[SpookyRequests] (
    [Id] [nvarchar](450) NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Email] [nvarchar](max) NOT NULL,
    [Phone] [nvarchar](max) NOT NULL,
  
    CONSTRAINT [PK_SalesRequests1] PRIMARY KEY CLUSTERED (
        [Id] ASC
    )
);
