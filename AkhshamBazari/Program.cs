using System.Net;
using System.Reflection;
using AkhshamBazari.Controllers;
using AkhshamBazari.Controllers.Base;



HttpListener httpListener = new HttpListener();

const int port = 8080;
httpListener.Prefixes.Add($"http://*:{port}/");

httpListener.Start();

System.Console.WriteLine($"Server started on port {port}...");

while (true)
{
    var context = await httpListener.GetContextAsync();

    var endpointItems = context.Request.Url?.AbsolutePath?.Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    if (endpointItems == null || endpointItems.Any() == false)
    {
        await new HomeController().HomePageAsync(context);
        context.Response.Close();
        continue;
    }

    // get controller class type
    var controllerType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.BaseType == typeof(ControllerBase))
        .FirstOrDefault(t => t.Name.ToLower() == $"{endpointItems[0]}controller");

    if (controllerType == null)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Response.Close();
        continue;
    }

    // get controller's endpoint method
    var controllerMethod = controllerType
        .GetMethods()
        .FirstOrDefault(m => m.Name.ToLower().Contains(context.Request.HttpMethod.ToLower()));

    if (controllerMethod == null)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Response.Close();
        continue;
    }

    // call endpoint method
    var controller = Activator.CreateInstance(controllerType) as ControllerBase;
    var methodCall = controllerMethod.Invoke(controller, parameters: new[] { context });

    // if endpoint method is async -> wait it synchronously
    if (methodCall != null && methodCall is Task asyncMethod) {
        await asyncMethod.WaitAsync(CancellationToken.None);
    }
    
    // send response
    context.Response.Close();
}