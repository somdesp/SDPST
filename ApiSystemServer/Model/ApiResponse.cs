using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace ApiSystemServer.Model
{
    public class ApiResponse
    {
        public IEnumerable<ModelError> errors;
        public string Status { get; set; }
        public string SuccessMessage { get; set; }

        public ApiResponse(string status, ModelStateDictionary modelState)
        {
            this.errors = modelState.Values.SelectMany(e => e.Errors);
            this.Status = status;
        }

        public ApiResponse(string status, string successMessage)
        {
            this.SuccessMessage = successMessage;
            this.Status = status;
        }
    }
}
