using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
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

        public StudentGroupSchedule GetCurrentSchedule(string groupId)
        {
            var xmlDocument = new XmlDocument();
            var endpoint = GetScheduleEndpoint(groupId);
            xmlDocument.Load(endpoint);

            using TextReader reader = new StringReader(xmlDocument.InnerXml);
            var serializer = new XmlSerializer(typeof(StudentGroupSchedule));

            var result = (StudentGroupSchedule) serializer.Deserialize(reader);
            result.ScheduleClasses.RemoveAll(x => x.Date < DateTime.Now || x.Type.ToLower().Contains("przeniesienie"));

            return result;
        }

        private string GetScheduleEndpoint(string groupId) => string.Format(_scheduleEndpointTemplate, groupId);
    }
}