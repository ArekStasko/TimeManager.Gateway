using Ocelot.Middleware;
using RestSharp;
using System.Text;
using System.Web;

namespace TimeManager.Gateway.middleware
{
    public class CustomMiddleware : OcelotPipelineConfiguration
    {
        public CustomMiddleware()
        {
            AuthenticationMiddleware = async (ctx, next) =>
            {
                await ProcessRequest(ctx, next);
            };
        }

        public async Task ProcessRequest(HttpContext context, System.Func<Task> next)
        {
            try
            {
                string endpoint = "http://timemanager.idp:80/api/Auth/IsAuth/isauth";
                var client = new RestClient(endpoint);

                string token = context.Request.Query["token"].ToString();
                
                var request = new RestRequest();
                
                request.AddParameter("token", token);
                int result = client.Post<int>(request);
                
                await next.Invoke();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
