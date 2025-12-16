using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Response
{
    public class NoContentResponse : Response
    {
        public NoContentResponse() : base(true, "NoContent")
        {

        }

        public static NoContentResponse Success()
        {
            return new NoContentResponse();
        }
    }
}
