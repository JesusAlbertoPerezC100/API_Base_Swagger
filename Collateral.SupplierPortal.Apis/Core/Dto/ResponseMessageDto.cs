namespace Collateral.SupplierPortal.Apis.Core.Dto
{
    public class ResponseMessage<TDto> : ResponseMessage
    {
        public TDto Dto { get; set; }
    }

    public static class ResponseMessageExtend
    {
        public static ResponseMessage AddError(this ResponseMessage response, string error)
        {
            if (response == null)
                return null;

            response.Success = false;
            response.Message = error;
            return response;
        }

        public static ResponseMessage<TDto> AddError<TDto>(this ResponseMessage<TDto> response, string error)
        {
            if (response == null)
                return null;

            response.Success = false;
            response.Message = error;

            return response;
        }

        public static ResponseMessage<TDto> Add<TDto>(this ResponseMessage<TDto> response, TDto dto)
        {
            if (response == null || dto == null)
                return null;

            response.Dto = dto;

            return response;
        }
    }
}