using System;
using CMPNatural.Core.Entities;

namespace CMPNatural.Core.Models
{
    public class HighLevelSettings
    {
        public string LocationId { get; set; } = null!;
        public string Authorization { get; set; } = null!;
        public string AuthorizationCustomValues { get; set; } = null!;
        public string RestApi { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string UpdateContactApi { get; set; } = null!;
        public string ForgotPasswordApi { get; set; } = null!;
        public string ActivationLinkApi { get; set; } = null!;

        public void update(GoHighLevel model)
        {
            LocationId = model.LocationId;
            Authorization = model.Authorization;
            RestApi = model.RestApi;
            Version = model.Version;
            AuthorizationCustomValues = model.Authorization;
            UpdateContactApi = model.UpdateContactApi;
            ForgotPasswordApi = model.ForgotPasswordApi;
            ActivationLinkApi = model.ActivationLinkApi;
        }
    }
}

