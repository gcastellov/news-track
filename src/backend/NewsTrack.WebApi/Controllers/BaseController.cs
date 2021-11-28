using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsTrack.Domain.Exceptions;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.Controllers
{
    public class BaseController : Controller
    {
        protected async Task<IActionResult> Execute<T>(Func<Task<T>> action) where T : class
        {
            var envelope = await Envelope(action);
            return Ok(envelope);
        }

        protected async Task<Envelope<T>> Envelope<T>(Func<Task<T>> action) where T : class
        {
            try
            {
                return new Envelope<T>
                {
                    Payload = await action(),
                    IsSuccessful = true
                };
            }
            catch(NotFoundException)
            {
                return new Envelope<T>
                {
                    IsSuccessful = false,
                    Error = new Error
                    {
                        Message = "Entity not found",
                        Code = (uint)Error.Codes.NotFound
                    }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
