using Abp.Application.Services.Dto;
using System;

namespace ServiceControl.Orders.Dto
{
    public class ExportResultResponse
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}
    