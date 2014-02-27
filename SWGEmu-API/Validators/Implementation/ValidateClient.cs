using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;

namespace OAuth2.Server.Validators
{
    public class ValidateClient : AbstractValidator<OAuth2.DataModels.Client>
    {
        public Model.IClientModel   ClientModel { get; set; } 

        public ValidateClient()
        {
            CascadeMode = ServiceStack.FluentValidation.CascadeMode.StopOnFirstFailure;

            RuleSet(ServiceStack.ServiceInterface.ApplyTo.Post, () =>
                {
                    RuleFor(r => r.id).Cascade(CascadeMode).NotEmpty()
                        .Must(id => !ClientModel.ClientExists(id)).WithErrorCode("ShouldNotExist").WithMessage("Client with ID already exists");
                    RuleFor(r => r.contact_email).NotEmpty();                    
                    RuleFor(r => r.description).NotEmpty();                    
                    RuleFor(r => r.name).NotEmpty();
                    RuleFor(r => r.redirect_uri).NotNull().Must(cur => Uri.IsWellFormedUriString(cur, UriKind.Absolute)).WithErrorCode("InvalidURI").WithMessage("Invalid Redirect URI");
                    RuleFor(r => r.secret).NotEmpty().When(r => r.type == DataModels.ClientTypes.web_application);
                    RuleFor(r => r.allowed_scope).NotNull();                   
                });
            RuleSet(ServiceStack.ServiceInterface.ApplyTo.Delete | ServiceStack.ServiceInterface.ApplyTo.Put | ServiceStack.ServiceInterface.ApplyTo.Patch, () =>
                {
                    RuleFor(r => r.id).NotNull();
                    //RuleFor(r => r.id).Must(id => !string.IsNullOrWhiteSpace(id) && !ClientModel.ClientExists(id)).WithErrorCode("ShouldExist").WithMessage("Invalid Client ID");
                });
        }

    }
}