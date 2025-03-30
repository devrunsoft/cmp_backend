using System;
using CMPNatural.Application.Commands.Admin;
using CMPNatural.Core.Entities;
using MediatR;
using ScoutDirect.Application.Responses;

namespace CMPNatural
{
	public static class TwoFactorAdmin
	{

		public static async Task<string?> twoFactor(this AdminEntity entity, IMediator _mediator)
		{
            Random random = new Random();
            string code = random.Next(1000, 9999).ToString();
            var result = await _mediator.Send(new AdminTwoFactorCommand() { AdminId = entity.Id, Code = code });

            if (!result.IsSucces())
			{
				return null;
			}

			return code;

        }
	}
}

