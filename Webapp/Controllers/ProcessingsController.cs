using Microsoft.AspNetCore.Mvc;
using WebappDb;

namespace Webapp.Controllers
{
    public class ProcessingsController : Controller
    {
        private readonly WebappdbContext db;

        public ProcessingsController(WebappdbContext context)
        {
            db = context;
        }
    }
}
