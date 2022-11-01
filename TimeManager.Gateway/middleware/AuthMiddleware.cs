using Ocelot.Authorization;
using Ocelot.Middleware;
using RestSharp;
using System.Net;
using System.Text;
using System.Web;
using TimeManager.Gateway.data.Token;
using TimeManager.Gateway.Data.Response;

namespace TimeManager.Gateway.middleware
{
    public class AuthMiddleware : OcelotPipelineConfiguration
    {
        public AuthMiddleware()
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

                var tokenDTO = new TokenDTO() { token = token};

                request.AddJsonBody(tokenDTO);
                Response<bool> result = client.Post<Response<bool>>(request);


                if (!result.Data)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.WriteAsync("Unauthorized error !");
                    context.Items.SetError(new UnauthorizedError("Unauthorized error !"));
                    return;
                }

                await next.Invoke();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
