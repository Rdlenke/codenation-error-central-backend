using System;
using System.Collections.Generic;
using System.Text;

namespace ErrorCentral.Domain.SeedWork
{
    public class Response<T> 
    {
        public Response(T data, bool success, string[] errors)
        {
            Data = data;
            Success = success;
            Errors = errors;
        }

        public Response(bool success, string[] errors)
        {
            Success = success;
            Errors = errors;
        }

        public string[] Errors { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
