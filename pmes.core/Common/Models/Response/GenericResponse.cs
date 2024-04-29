using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pmes.core.Common.Models.Response
{
    public class GenericResponse<T>
    {
        public GenericResponse(T data)
        {
            Data = data;
        }

        public GenericResponse(T data, List<ErrorDetailModel> errors, int statusCode)
        {
            Data = data;
            Errors = errors;
            StatusCode = statusCode;
        }

        public GenericResponse(T data, string message)
        {

            Data = data;
            Message = message;
        }

        public GenericResponse(T data, string message, int statusCode)
        {

            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        public GenericResponse(T data, List<ErrorDetailModel> errors, string message)
        {
            Data = data;
            Errors = errors;
            Message = message;
        }

        public GenericResponse(T data, List<ErrorDetailModel> errors, string message, int statusCode)
        {
            Data = data;
            Errors = errors;
            Message = message;
            StatusCode = statusCode;
        }

        public bool Success => Errors.Count == 0 && StatusCode == 200;
        public bool HasErrors => Errors.Count != 0;
        public string Message { get; private set; }
        public List<ErrorDetailModel> Errors { get; private set; } = [];
        public T Data { get; private set; }
        public int StatusCode { get; private set; }
    }
}
