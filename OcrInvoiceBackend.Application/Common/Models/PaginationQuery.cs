using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Common.Models
{
    public record PaginationQuery(int Page, int PageSize, string SortingOrders);
}
