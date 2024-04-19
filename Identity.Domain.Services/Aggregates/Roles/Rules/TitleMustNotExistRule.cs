using Identity.Domain.Models.Abstraction.Roles;
using Identity.Domain.Models.Aggregates.Roles.ValueObjects;
using NP.Shared.Domain.Models.SeedWork;
using NP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Services.Aggregates.Roles.Rules
{
    public class TitleMustNotExistRule : IBusinessRuleAsync
    {
        public IRoleManager RoleManager { get; }
        public RoleTitle Title { get; }
        public string FieldName { get; }

        public string Message => string.Format(Validations.AlreadyExistsValueForField, Title.Title, FieldName);

        public async Task<bool> IsBroken()
        {
            var result = await RoleManager.IsTitleExists(Title);

            return result.IsSuccess && result.Value;
        }

        public TitleMustNotExistRule(RoleTitle title, string fieldName, IRoleManager roleManager)
        {
            Title = title;
            FieldName = fieldName;
            RoleManager = roleManager;
        }
    }
}
