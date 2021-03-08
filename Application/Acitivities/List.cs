using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Acitivities
{
    public class List
    {
        public class Query : IRequest<Result<List<ActivityDTO>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ActivityDTO>>>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                this.userAccessor = userAccessor;
                this.mapper = mapper;
                this.context = context;
            }

            public async Task<Result<List<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await context.Activities
                .ProjectTo<ActivityDTO>(mapper.ConfigurationProvider, 
                    new {currentUsername = userAccessor.GetUsername()})
                .ToListAsync(cancellationToken);

                return Result<List<ActivityDTO>>.Success(activities);
            }
        }
    }
}