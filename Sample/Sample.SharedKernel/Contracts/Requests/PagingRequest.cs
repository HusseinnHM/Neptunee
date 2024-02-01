using System.ComponentModel;

namespace Sample.SharedKernel.Contracts.Requests;

public record PagingRequest
{
    [DefaultValue(1)] public int PageIndex { get; set; } = 1;
    [DefaultValue(10)] public int PageSize { get; set; } = 10;
}