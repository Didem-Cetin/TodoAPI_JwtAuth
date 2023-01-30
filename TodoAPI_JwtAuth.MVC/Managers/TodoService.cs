using RestSharp;
using TodoAPI_JwtAuth.MVC.Managers.Abstract;
using TodoAPI_JwtAuth.MVC.Models;

namespace TodoAPI_JwtAuth.MVC.Managers
{
    public interface ITodoService : IService<Models.Todo, TodoCreate, Models.Todo>
    {
        RestResponse<string> Authenticate(SignInModel model);
    }
    public class TodoService : ServiceBase<Models.Todo, TodoCreate, Models.Todo>, ITodoService
    {
      public TodoService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {

        }

        public override void SetEndPoints()
        {
            _endpoint = _configuration.GetValue<string>("Services:TodoService:EndPoint");
            _listEndpoint = "/Todo/List";
            _createEndpoint = "/Todo/Create";
            _editEndpoint = "/Todo/Edit";
            _getByIdEndpoint = "/Todo/GetById";
            _deleteEndpoint = "/Todo/Remove";
        }
        public RestResponse<string> Authenticate(SignInModel model)
        {
         RestRequest request= new RestRequest("/Account/SignIn",Method.Post);
            request.AddJsonBody(model);
            return _client.ExecutePost<string>(request);
        }
    }
}
