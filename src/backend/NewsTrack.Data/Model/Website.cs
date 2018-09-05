using System;
using Nest;

namespace NewsTrack.Data.Model
{
    public class Website : IDocument
    {
        public Guid Id { get; set; }
        [Keyword(Store = true)]
        public Uri Uri { get; set; }
    }
}