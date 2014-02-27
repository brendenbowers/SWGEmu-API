using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;

namespace OAuth2.Server.Validators
{
    public class ValidateScope : AbstractValidator<OAuth2.DataModels.Scope>
    {
        public Model.IScopeModel ScopeModel { get; set; }

        public ValidateScope()
        {
            CascadeMode = ServiceStack.FluentValidation.CascadeMode.StopOnFirstFailure;

            RuleSet(ServiceStack.ServiceInterface.ApplyTo.Post, () =>
                {
                    RuleFor(r => r.scope_name).Cascade(CascadeMode).NotEmpty()
                        .Must(scope_name => !ScopeModel.ScopeExists(scope_name)).WithErrorCode("ShouldNotExist").WithMessage("Scope with name already exists");
                    RuleFor(r => r.description).NotEmpty();
                });
            RuleSet(ServiceStack.ServiceInterface.ApplyTo.Delete | ServiceStack.ServiceInterface.ApplyTo.Put | ServiceStack.ServiceInterface.ApplyTo.Patch, () =>
                {
                    RuleFor(r => r.scope_name).NotEmpty();
                });
        }
    }
}