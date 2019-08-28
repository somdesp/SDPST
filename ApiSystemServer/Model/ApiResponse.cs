using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace ApiSystemServer.Model
{
    public class ApiResponse
    {
        public IEnumerable<ModelError> errors;
        public string status { get; set; }
        public string successMessage { get; set; }

        public ApiResponse(string status, ModelStateDictionary modelState)
        {
            this.errors = modelState.Values.SelectMany(e => e.Errors);
            this.status = status;
        }

        public ApiResponse(string status, string successMessage)
        {
            this.successMessage = successMessage;
            this.status = status;
        }
    }
}
