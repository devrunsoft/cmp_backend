using System;
using CMPNatural.Core.Entities;

namespace CMPNatural.Core.Models
{
    public class HighLevelSettings
    {
        public string LocationId { get; set; }
        public string Authorization { get; set; }
        public string AuthorizationCustomValues { get; set; }
        public string RestApi { get; set; }
        public string Version { get; set; }

        public void update(GoHighLevel model)
        {
            LocationId = model.LocationId;
            Authorization = model.Authorization;
            RestApi = model.RestApi;
            Version = model.Version;
            AuthorizationCustomValues = model.Authorization;
        }
    }
}

