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

        public Response(T data, bool success)
        {
            Data = data;
            Success = success;
        }

        public string[] Errors { get; private set; }
        public bool Success { get; private set; }
        public T Data { get; private set; }
    }
}
