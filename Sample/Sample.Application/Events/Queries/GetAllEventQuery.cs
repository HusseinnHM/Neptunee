using Neptunee.Handlers.Requests;
using Neptunee.OperationResponse;
using Sample.Application.Events.Queries.Responses;
using Sample.SharedKernel.Contracts.Requests;

namespace Sample.Application.Events.Queries;

public record GetAllEventQuery : PagingRequest, INeptuneeRequest<Operation<List<GetAllEventResponse>>>;

