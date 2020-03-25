using System.IO;
using System.Xml;
using PlanerUek.Storage.Interfaces;
using PlanerUek.Storage.Models;

namespace PlanerUek.Storage.Providers
{
    public class StudentGroupScheduleProvider : IStudentGroupScheduleProvider
    {
        private readonly string _scheduleEndpointTemplate;

        public StudentGroupScheduleProvider(string scheduleEndpointTemplate)
        {
            _scheduleEndpointTemplate = scheduleEndpointTemplate;
        }

        public StudentGroupSchedule GetSchedule(string groupId)
        {
            var xmlDocument = new XmlDocument();
            var endpoint = GetScheduleEndpoint(groupId);
            xmlDocument.Load(endpoint);
            
            return new StudentGroupSchedule();
        }

        private string GetScheduleEndpoint(string groupId) => string.Format(_scheduleEndpointTemplate, groupId);
    }
}