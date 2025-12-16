using KutuphaneService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneService.Response
{
    public class Response
    {
        public bool IsSuccess { get; protected set; }

        public string Message { get; protected set; }

        public Response(bool isSucces, string message)
        {
            IsSuccess = isSucces;
            Message = message;
        }

        public static Response Success(string message = "")
        {
            return new Response(true, message);
        }

        public static Response Error(string message = "")
        { 
            return new Response(false, message); 
        }
    }
}
