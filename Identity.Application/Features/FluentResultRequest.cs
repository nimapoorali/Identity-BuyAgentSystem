using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Features
{
    public class FluentResultRequest<T> : IRequest<Result<T>>
    {
        public FluentResultRequest() : base() { }

    }

    public class FluentResultRequest : IRequest<Result>
    {
        public FluentResultRequest() : base() { }

    }
}
