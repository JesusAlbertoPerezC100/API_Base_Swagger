using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Collateral.SupplierPortal.Apis.Core.Dto
{
    public class ResponseMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ResponseMessage(bool success = true, string message = null)
        {
            Success = success;
            Message = message;
        }

        public void AddFieldError(string fieldName, string error)
        {
            Success = false;

            if (Errors == null)
                Errors = new List<ErrorModel>();
            ((List<ErrorModel>)Errors).Add(new ErrorModel { FieldName = fieldName, Message = error });
        }

        public void AddFieldError(ErrorModel error)
        {
            if (error == null)
                return;

            if (Errors == null)
                Errors = new List<ErrorModel>();

            Success = false;

            ((List<ErrorModel>)Errors).Add(error);
        }

        public ResponseMessage AddError(ResponseMessage error)
        {
            if (error == null || error.Success || error.Errors.Any())
                return error;

            List<ErrorModel> list;

            if (Errors == null)
                Errors = new List<ErrorModel>();

            list = Errors as List<ErrorModel>;

            list.AddRange(error.Errors);
            Errors = list;
            Message = error.Message;
            Success = false;
            return this;
        }

        public IEnumerable<ErrorModel> Errors { get; private set; }
    }
}