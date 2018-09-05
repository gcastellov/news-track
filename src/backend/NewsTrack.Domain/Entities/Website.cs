using System;

namespace NewsTrack.Domain.Entities
{
    public class Website
    {
        public Guid Id { get; set; }
        public Uri Uri { get; set; }
    }
}