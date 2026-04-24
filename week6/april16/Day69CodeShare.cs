
Azure Cosmos DB is mainly considered a NoSQL database for semi-structured data, especially when you use its document model with JSON items. Microsoft describes Azure Cosmos DB as schema-free or schema-agnostic for NoSQL data, and its data modeling guidance says it is designed to store and query unstructured and semi-structured data.


   https://microsoftedge.github.io/Demos/json-dummy-data/

   Take this data :https://microsoftedge.github.io/Demos/json-dummy-data/64KB.json
   https://codeblogmoney.com/json-example-with-data-types-including-json-array/  //for complex scenarios ask chat gpt how  i can get this data so here company stores data in one format 
// so that format it is giving it to u in the form of web api and in web api data format typ will be json only.



  
Demo from visual studio 
------------------------
create an resouce of azure cosmos db with the name raghucosmosdb 

and then go to Data Explorer section and create a database name with DemoDB and by clicking on same database add container inside this database which is nothing but a table here 

items and partion key as id and manual u select and say OK now for creating container okay 

so here id i had taken as partion key and here id is also primary key of table also 

now in cosmos db only come to settings and in that go to keys and from there copy and paste the follwing things in notepad for reference 


URI (endpoint ) 

https://raghucosmosdb.documents.azure.com:443/

copy now primary key over here 

not pasting as github will give error so paste it in visual studio from cosmos portal

  then copy primary connection string here 

not pasting as github will give error so paste it in visual studio from cosmos portal

so all these is neeed to establish connection with asp.net core mvc code which will talk to cosmos db okay here 



now come to visual studio and open and create asp.net core mvc project now okay ...with name CosmosDB-Demo 

Add required paramters over here in app settings file first 

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "CosmosDb": {
    "Endpoint": "https://raghucosmosdb.documents.azure.com:443/",
    "PrimaryKey": "",//not pasting as github will give error so paste it in visual studio from cosmos portal

     "DatabaseName": "DemoDB",
    "ContainerName": "items"
  }
}


next add necessary packages required for the application 

in the search just type newton and then add this below package of version 8.0.24 only install as it is compatible with .net core 8.0 which we are using it okay 
so this is required becasue cosmos db is primary json based structured document okay 


Microsoft.AspNetCore.Mvc.NewtonsoftJson 

again in search type cosmos and install the package Microsoft.Azure.Cosmos anything which is latest u can install here 


Now go to Models folder and add a model with the name Itemmodel

using Newtonsoft.Json;

namespace CosmosDB_Demo.Models
{
    public class Itemmodel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}

here as we store in json format all things we have to write like this only 

Now i want to add data layer so create one folder with the name Data and in that add an class with the name CosmosDbService and 

  so the complete code of CosmosDbService class will be like this 


using Microsoft.Azure.Cosmos;
using CosmosDB_Demo.Models;
namespace CosmosDB_Demo.Data
{
    
        public class CosmosDbService
        {
            private Container _container;

            public CosmosDbService(CosmosClient cosmosClient,
                string databaseName, string containerName)
            {
                _container = cosmosClient.GetContainer(databaseName, containerName);
            }

            public async Task AddItemAsync(Itemmodel item)
            {
                await _container.CreateItemAsync(item, new PartitionKey(item.Id));
            }


            public async Task<Itemmodel> GetItemAsync(string id)
            {
                ItemResponse<Itemmodel> response = await
                    _container.ReadItemAsync<Itemmodel>(id, new PartitionKey(id));

                return response.Resource;
            }


            public async Task<IEnumerable<Itemmodel>> GetItemsAsync(string queryString)
            {
                var query = _container.GetItemQueryIterator<Itemmodel>(new QueryDefinition(queryString));
                List<Itemmodel> results = new List<Itemmodel>();

                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    results.AddRange(response.ToList());
                }

                return results;
            }


            public async Task UpdateItemAsync(string id, Itemmodel item)
            {
                await _container.UpsertItemAsync(item, new PartitionKey(id));
            }

            public async Task DeleteItemAsync(string id)
            {
                await _container.DeleteItemAsync<Itemmodel>(id, new PartitionKey(id));
            }



    }
    
}

next go to program.cs file 

on the top add following namespaces 

using CosmosDB_Demo.Data;
using Microsoft.Azure.Cosmos;

and then add code after this below line to impelement dependency 


   builder.Services.AddControllersWithViews(); //after this add 

   builder.Services.AddSingleton<CosmosDbService>(options =>
   {
       var _configuration = builder.Configuration;

       var cosmosClient = new CosmosClient(
           _configuration["CosmosDb:Endpoint"],
           _configuration["CosmosDb:PrimaryKey"]);

       return new CosmosDbService(
           cosmosClient,
           _configuration["CosmosDb:DatabaseName"],
           _configuration["CosmosDb:ContainerName"]);
   });


so this is the settings which i had done in my json file that only i am using it here okay

Now just build and run the application once to check whether any error is there on not all things are proeprly set or not for that purpose we have to do this

  
  so till now i checked it is working fine next task now add one Empty MVC controller ItemsController and write the code like this 


using Microsoft.AspNetCore.Mvc;
using CosmosDB_Demo.Models;
using CosmosDB_Demo.Data;

namespace CosmosDB_Demo.Controllers
{
    public class ItemsController : Controller
    {
        private readonly CosmosDbService _cosmosDbService;

        public ItemsController(CosmosDbService cosmosdbservice)
        {
            _cosmosDbService = cosmosdbservice;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _cosmosDbService.GetItemsAsync("SELECT * FROM c");
            return View(items);
        }
       
        public async Task<IActionResult> Details(string id)
        {
            var item = await _cosmosDbService.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Itemmodel item)
        {
            if (ModelState.IsValid)
            {
                await _cosmosDbService.AddItemAsync(item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var item = await _cosmosDbService.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Itemmodel item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateItemAsync(id, item);
                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var item = await _cosmosDbService.GetItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _cosmosDbService.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(string query)
        {
            var items = await _cosmosDbService.GetItemsAsync($"SELECT * FROM c WHERE CONTAINS(LOWER(c.name),'{query}')");
            return View("Index", items);
        }
    }
}


next add index view empty one and add the follwing code 

Index.cshtml
-------------
@model IEnumerable<Itemmodel>

<h2>Items</h2>

<form asp-action="Search" method="get">
    <input type="text" name="query" placeholder="Search items..." />
    <input type="submit" value="Search" />
</form>

<table class="table">
    <thead>
        <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Category</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>@item.Name</td>
                <td>@item.Description</td>
                <td>@item.Category</td>
                
                <td>
                    <a asp-action="Edit" asp-route-id="@item.Id">Edit</a> |
                    <a asp-action="Details" asp-route-id="@item.Id">Details</a> |
                    <a asp-action="Delete" asp-route-id="@item.Id">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>

<a asp-action="Create">Create New</a>


now run and see Items/index whether upto this is working or not okay 


Create view 
-------------
@model Itemmodel

@{
    ViewData["Title"] = "Create Item";
}

<h2>Create Item</h2>

<form asp-action="Create">
    <div class="form-group">
        <label asp-for="Id" class="control-label"></label>
        <input asp-for="Id" class="form-control" />
        <span asp-validation-for="Id" class="text-danger"></span>
    </div>

    <div class="form-group">
        <label asp-for="Name" class="control-label"></label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>

    <div class="form-group">
        <label asp-for="Description" class="control-label"></label>
        <textarea asp-for="Description" class="form-control"></textarea>
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="Category" class="control-label"></label>
        <input asp-for="Category" class="form-control" />
        <span asp-validation-for="Category" class="text-danger"></span>
    </div>

    <div class="form-group">
        <input type="submit" value="Create" class="btn btn-primary" />
    </div>
</form>

        <div>
            <a asp-action="Index">Back to List</a>
        </div>

next same manner add Details view 
Details view 
--------------
@model Itemmodel

@{
    ViewData["Title"] = "Item Details";
}

<h2>Item Details</h2>

<div>
    <h4>Item</h4>
    <hr />
    <dl class="row">
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.Id)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.Id)
        </dd>

        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.Name)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.Name)
        </dd>
        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.Description)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.Description)
        </dd>

        <dt class="col-sm-2">
            @Html.DisplayNameFor(model => model.Category)
        </dt>
        <dd class="col-sm-10">
            @Html.DisplayFor(model => model.Category)
        </dd>
    </dl>
</div>

<div>
    <a asp-action="Edit" asp-route-id="@Model.Id" class="btn btn-warning">Edit</a> |
    <a asp-action="Index" class="btn btn-secondary">Back to List</a>
</div>

Edit view 
------------
@model Itemmodel

@{
    ViewData["Title"] = "Edit Item";
}

<h2>Edit Item</h2>

<form asp-action="Edit">
    <div class="form-group">
        <label asp-for="Id" class="control-label"></label>
        <input asp-for="Id" class="form-control" readonly />
        <span asp-validation-for="Id" class="text-danger"></span>
    </div>

    <div class="form-group">
        <label asp-for="Name" class="control-label"></label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>

    <div class="form-group">
        <label asp-for="Description" class="control-label"></label>
        <textarea asp-for="Description" class="form-control"></textarea>
        <span asp-validation-for="Description" class="text-danger"></span>
    </div>
    <div class="form-group">
        <label asp-for="Category" class="control-label"></label>
        <input asp-for="Category" class="form-control" />
        <span asp-validation-for="Category" class="text-danger"></span>
    </div>

    <div class="form-group">
        <input type="submit" value="Save" class="btn btn-primary" />
    </div>
</form>

<div>
    <a asp-action="Index">Back to List</a>
</div>

Delete view 
----------
@model Itemmodel

@{
    ViewData["Title"] = "Delete Item";
}

<h2>Delete Item</h2>

<h3>Are you sure you want to delete this item?</h3>

<form asp-action="DeleteConfirmed">
    <input type="hidden" asp-for="Id" />

    <div>
        <h4>Item</h4>
        <hr />
        <dl class="row">
            <dt class="col-sm-2">
                @Html.DisplayNameFor(model => model.Id)
            </dt>
            <dd class="col-sm-10">
                @Html.DisplayFor(model => model.Id)
            </dd>

            <dt class="col-sm-2">
                @Html.DisplayNameFor(model => model.Name)
            </dt>
            <dd class="col-sm-10">
                @Html.DisplayFor(model => model.Name)
            </dd>

            <dt class="col-sm-2">
                @Html.DisplayNameFor(model => model.Description)
            </dt>
            <dd class="col-sm-10">
                @Html.DisplayFor(model => model.Description)
            </dd>

            <dt class="col-sm-2">
                @Html.DisplayNameFor(model => model.Category)
            </dt>
            <dd class="col-sm-10">
                @Html.DisplayFor(model => model.Category)
            </dd>
         </dl>
   </div>


            @* <div class="form-group"> *@
            <input type="submit" value="Delete" class="btn btn-danger" /> |
            <a asp-action="Index" class="btn btn-secondary">Back to List</a>
            @* </div> *@
</form>



