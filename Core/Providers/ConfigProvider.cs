using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Core.Common;
using JetBrains.Annotations;
using Model.Runtime;
using Tools.Extentions;
using Tools.ResourcesSupport;

namespace Core.Providers
{
    internal static class ConfigManager
    {
        #region Main section
        private static XDocument _settings;

        private static XDocument Settings => _settings ?? (_settings = XDocument.Load(@"Core\RESTConfiguration.xml"));

        private static XElement GetConfigEntity(string sectionType, string name)
        {
            if (Settings.Root == null) throw new XmlException(AppResources.GetResource("ConfigRootElError"));
            var el = Settings.Root;
            //if (el == null) throw new XmlException(AppResources.ConfigMainElError);
            return el.DescendantsAndSelf().Where(x => x.Name == sectionType)
                .FirstOrDefault(element =>
                    (element.Attribute("Name") ?? new XAttribute("Name", string.Empty)).Value == name);
        }
        #endregion Main section

        #region ConferenceHelpers
        private static uint _conferenceId;

        public static XElement GetConference([NotNull] string name)
        {
            return GetConfigEntity("Conference", name);
        }

        public static uint GetConferenceId()
        {
            if (_conferenceId != 0) return _conferenceId;
            var conference = GetConference(ProjectConsts.DefaultConferenceName);
            //current approach used for hardcoded one Conference without selection
            if (conference == null) throw new XmlException(AppResources.GetResource("ConfigNoConferenciesError"));
            var value = conference.Attribute("ID");
            if (value == null) throw new XmlException(AppResources.GetResource("ConfigConferenceIDError"));
            var result = uint.Parse(value.Value);
            if (result <= 0) throw new ArgumentException(AppResources.GetResource("ConfigCompIDUnexpectedError"));

            _conferenceId = result;
            return _conferenceId;
        }
        #endregion ConferenceHelpers

        #region ServicesHelper

        private static string _mainServiceUrl;
        private static List<ServiceMethodModel> _mainServiceMethods;
        public static XElement GetService([NotNull] string name)
        {
            return GetConfigEntity("Service", name);
        }

        public static List<ServiceMethodModel> GetServiceMethods([NotNull] string name)
        {
            var service = GetConfigEntity("Service", name);
            if (service == null) return new List<ServiceMethodModel>(0);
            return (from method in service.DescendantsAndSelf().Where(x => x.Name == "Method")
                    let methodNameAttr = method.Attribute("Name")
                    where methodNameAttr != null
                    let methodName = methodNameAttr.Value
                    where !string.IsNullOrEmpty(methodName)
                    let pathAttr = method.Attribute("PathMask")
                    where pathAttr != null
                    let path = pathAttr.Value
                    where !string.IsNullOrEmpty(path)
                    let protocolAttr = method.Attribute("Protocol")
                    where protocolAttr != null
                    let protocol = protocolAttr.Value
                    where !string.IsNullOrEmpty(protocol)
                    select new ServiceMethodModel
                    {
                        Name = methodName,
                        PathMask = path,
                        Protocol = HttpMethodExtention.GetMethod(protocol)
                    }).ToList();
        }

        public static string GetMainServiceUrl()
        {
            if (!string.IsNullOrEmpty(_mainServiceUrl)) return _mainServiceUrl;
            var mainService = GetService(ProjectConsts.MainServiceName);
            if (mainService == null) throw new XmlException(AppResources.GetResource("ConfigMainServiceMissingError"));
            var url = mainService.Attribute("Url");
            if (url == null) throw new XmlException(AppResources.GetResource("ConfigMainServiceUrlSectionMissingError"));
            var urlValue = url.Value;
            if (string.IsNullOrEmpty(urlValue))
                throw new ArgumentException(AppResources.GetResource("ConfigMainServiceUrlBrockenError"));

            _mainServiceUrl = urlValue;
            return _mainServiceUrl;
        }

        public static List<ServiceMethodModel> GetMainServiceMethods()
        {
            if (_mainServiceMethods != null) return _mainServiceMethods;

            var methods = GetServiceMethods(ProjectConsts.MainServiceName);
            if (!methods.Any()) throw new XmlException(AppResources.GetResource("ConfigMainServiceMethodMissingError"));
            _mainServiceMethods = methods;

            return _mainServiceMethods;
        }
        #endregion ServicesHelper
    }
}
