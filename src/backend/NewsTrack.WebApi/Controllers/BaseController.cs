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
            var envelope = new Envelope<T>();

            try
            {
                envelope.Payload = await action();
                envelope.IsSuccessful = true;
            }
            catch(NotFoundException e)
            {
                envelope.ErrorMessage = e.Message;
            }
            catch (Exception)
            {
                throw;
            }

            return envelope;
        }
    }
}
