First create an fucntion app in azure portal for that steps are there in video u can check it what to select and what to not 

Go to visual studio and create a function app locally now here name given as CalcFunc so this will become the namespace here and inside name space 
  some default classes will be there with name as function1 and in function app u can write as many functions u can okay .

  

so while creating some default code u will get means class name and functionname all will be same as function1 here i am doing sum so i had changed 
the class name and function name as same as sum here and even constructor also i changed as sum here 

while creating app select Httptrigger means your app will be called some body request though http url so i had selected that option from drop down 
if any Blob is called then blob trigger function u have to select okay 

and keep authorization level as anonymous  here  as anybody should be able to access it okay .

  

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CalcFunc;

public class Sum
{
    private readonly ILogger<Sum> _logger;

    public Sum(ILogger<Sum> logger)
    {
        _logger = logger;
    }

    [Function("Sum")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        int x =int.Parse(req.Query["x"]);
        int y = int.Parse(req.Query["y"]);
        int result = x + y;

        return new OkObjectResult(result);
    }
}

so run the above program some url you will get paste that url into browser and type 
http://localhost:7145/api/Sum?x=10&y=20 and type query string values you will get the output now 

so this function i want to publish in fucntion app of azure so from here locally u have to publish into azure function app url 
here website will not bounce back like it was doing in web app you have to go overviww of funciton app and need to brwose the url okay 

same thing url when i am trying i am not getting like local in azure and when i am cicking the publish url there also it is asking me to again 
sign in and on this url also i tride ? and & which i tried for local now next 
so the function i can see in overview left tab and down i can see sum function next so in code and test section i had checked by wrtign x and y values it is working 
but the url is not workingn so locally when u run the code take it from api and paste it online azure it will work so like this u can use azure functions 
  


