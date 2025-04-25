using CMPNatural.Application.Model;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural.Application
{
    public class AdminTermsConditionsEditCommand : TermsConditionsInput, IRequest<CommandResponse<TermsConditions>>
    {
		public AdminTermsConditionsEditCommand(TermsConditionsInput input, long Id)
        {
            this.Type = input.Type;
            this.Content = input.Content;
            this.Enable = input.Enable;
            this.Id = Id;
        }
        public long Id { get; set; }
    }
}

