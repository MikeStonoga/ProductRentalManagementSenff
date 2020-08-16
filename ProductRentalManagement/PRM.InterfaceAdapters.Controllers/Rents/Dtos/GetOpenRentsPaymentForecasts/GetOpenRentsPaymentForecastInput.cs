using System;
using PRM.UseCases.Rents.GetOpenRentsPaymentForecasts;

namespace PRM.InterfaceAdapters.Controllers.Rents.Dtos.GetOpenRentsPaymentForecasts
{
    public class GetOpenRentsPaymentForecastInput : GetOpenRentsPaymentForecastRequirement
    {
        public new DateTime? TargetDate { get; set; }
        public GetOpenRentsPaymentForecastInput()
        {
        }
    }
}