using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Progra620241_Assets_CamiloRodriguez.Attributes
{
    /*Con este atributo personalizado lo que se quiere es integrar una capa 
     * de sgeuridad por medio de una llave (clave:valor)*/
    /*Con esto se va a decorar ya se a un controller o solo
     ciertos end points a menos que se integre la llave en el encabezado http que lo consuma */
    [AttributeUsage(validOn: AttributeTargets.All )]
    public sealed class ApiKeyAttribute : Attribute, IAsyncActionFilter
    { 

        //Indicamos cual es la llave que se usara, esto estara en el archivo appsetting.json.

        private readonly string _apiKey = "P6ApiKey"; 
    
        public async Task OnActionExecutionAsync(ActionExecutingContext context, 
                                                    ActionExecutionDelegate next)
        {
            /*En esta funcion validamos que el json que llega al WebAPI tenga los
             datos ApiKey validos, caso contrario mmostramos un error en el 
            response del end point*/


            if (!context.HttpContext.Request.Headers.TryGetValue(_apiKey, out var ApiSalida)) 
            {
                //En el caso que no venga info de apikey en el header entonces...
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "The http Request doesn't contain security information"

                };
                return;
                                
            }
            //Pero si viene la info de apikey se procede con ...
            //Ahora que sabemos que la info de apikey viene, hay que validar que sea la correcta 

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var ApiKeyValue = appSettings.GetValue<string>(_apiKey);

            //a este punto tenemos todo lo necesario para hacer la comparacion de valores

            if (ApiKeyValue != null && !ApiKeyValue.Equals(ApiSalida)) 
            {
                context.Result = new ContentResult()
                {
                    StatusCode= 401,
                    Content = "Incorrect ApiKey Data"
                };            
                return;
            }

            await next();
        }


    }
}
