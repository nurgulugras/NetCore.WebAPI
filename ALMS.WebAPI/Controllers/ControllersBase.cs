using System;
using System.Collections.Generic;
using System.Net;
using ALMS.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ALMS.WebAPI.Controllers
{
    /// <summary>
    /// Base of Endpoint Controllers
    /// </summary>
    public class ControllersBase : ControllerBase
    {
        /// <summary>
        /// AutoMapper
        /// </summary>
        protected readonly IMapper Mapper;
        /// <summary>
        /// DI Service Provider
        /// </summary>
        protected readonly IServiceProvider ServiceProvider;
        /// <summary>
        /// Authorisation Service
        /// </summary>

        /// <summary>
        /// Base of Endpoint Controllers
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ControllersBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Mapper = ServiceProvider.GetService<IMapper>();
        }

        /// <summary>
        /// Return result
        /// </summary>
        /// <param name="operationResult"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public ActionResult<ApiResponseParameter<TModel>> APIResult<TModel>(OperationResult<TModel> operationResult)
        {
            var result = new ApiResponseParameter<TModel>();
            if (operationResult.IsSuccess)
            {
                result.DataModel = operationResult.DataModel;
                result.HttpStatusCode = IsDefaultValue<TModel>(operationResult.DataModel) ? HttpStatusCode.NoContent : HttpStatusCode.OK;
                result.ResultType = IsDefaultValue<TModel>(operationResult.DataModel) ? ResultType.NoContent : ResultType.Success;
                result.Message = operationResult.Message;
            }
            else
            {
                result.HttpStatusCode = IsDefaultValue<HttpStatusCode>(operationResult.HttpStatusCode) ? HttpStatusCode.BadRequest : operationResult.HttpStatusCode;
                result.ResultType = ResultType.Fail;
                result.Message = operationResult.Message;
            }
            return Ok(result);
        }

        /// <summary>
        /// Return result with dto object data model
        /// </summary>
        /// <param name="operationResult"></param>
        /// <typeparam name="TModel">Original Object</typeparam>
        /// <typeparam name="TDtoModel">DTO Object of TModel</typeparam>
        /// <returns></returns>
        public ActionResult<ApiResponseParameter<TDtoModel>> APIResult<TModel, TDtoModel>(OperationResult<TModel> operationResult)
        {
            var newOperationResult = new OperationResult<TDtoModel>()
            {
                DataModel = IsDefaultValue<TModel>(operationResult.DataModel) ? default(TDtoModel) : Mapper.Map<TDtoModel>(operationResult.DataModel),
                HttpStatusCode = operationResult.HttpStatusCode,
                IsSuccess = operationResult.IsSuccess,
                Message = operationResult.Message,
                NoContent = operationResult.NoContent,
                ResultType = operationResult.ResultType
            };
            return APIResult<TDtoModel>(newOperationResult);
        }

        /// <summary>
        /// Return result with dto object data model
        /// </summary>
        /// <param name="resultData"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public ActionResult<ApiResponseParameter<TModel>> APIResult<TModel>(TModel resultData)
        {
            var result = new ApiResponseParameter<TModel>();
            result.DataModel = resultData;
            result.HttpStatusCode = resultData == null ? HttpStatusCode.NoContent : HttpStatusCode.OK;
            result.ResultType = resultData == null ? ResultType.NoContent : ResultType.Success;
            return Ok(result);
        }

        /// <summary>
        /// Return result with dto object data model
        /// </summary>
        /// <param name="resultData"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TDtoModel"></typeparam>
        /// <returns></returns>
        public ActionResult<ApiResponseParameter<TDtoModel>> APIResult<TModel, TDtoModel>(TModel resultData)
        {
            var result = new ApiResponseParameter<TDtoModel>();
            var data = IsDefaultValue<TModel>(resultData) ? default(TDtoModel) : Mapper.Map<TDtoModel>(resultData);
            result.DataModel = data;
            result.HttpStatusCode = resultData == null ? HttpStatusCode.NoContent : HttpStatusCode.OK;
            result.ResultType = resultData == null ? ResultType.NoContent : ResultType.Success;
            return Ok(result);
        }

        /// <summary>
        /// Return result for BadRequest
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected ActionResult APIBadRequestResult(string errorMessage = "Invalid request parameter!")
        {
            var result = new ApiResponseParameter<bool>();
            result.HttpStatusCode = HttpStatusCode.BadRequest;
            result.ResultType = ResultType.Fail;
            result.Message = errorMessage;
            return Ok(result);
        }

        /// <summary>
        /// Return result for Unauthorized
        /// </summary>
        /// <returns></returns>
        protected ActionResult APIUnauthorizedRequestResult()
        {
            return Unauthorized();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private bool IsDefaultValue<T>(T obj)
        {
            return EqualityComparer<T>.Default.Equals(obj, default(T));
        }
    }
}