using Ocelot.Middleware;
using RestSharp;
using System.Text;
using System.Web;
using TimeManager.Gateway.Data.Response;

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
                string endpoint = "http://timemanager.idp:80/api/token/verifytoken/verifytoken";
                var client = new RestClient(endpoint);

                string token = context.Request.Query["token"].ToString();
                
                var request = new RestRequest();
                
                request.AddParameter("token", token);
                Response<int> result = client.Post<Response<int>>(request);

                context.Request.QueryString.Add("userId", result.Data.ToString());

                await next.Invoke();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
