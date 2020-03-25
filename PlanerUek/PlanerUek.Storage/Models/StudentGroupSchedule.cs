using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlanerUek.Storage.Models
{
    [XmlRoot(ElementName="plan-zajec")]
    public class StudentGroupSchedule {
        [XmlElement(ElementName="okres")]
        public List<Period> Periods { get; set; }
        
        [XmlElement(ElementName="zajecia")]
        public List<ScheduleClass> ScheduleClasses { get; set; }
        
        [XmlAttribute(AttributeName="typ")]
        public string Type { get; set; }
        
        [XmlAttribute(AttributeName="id")]
        public string Id { get; set; }
        
        [XmlAttribute(AttributeName="nazwa")]
        public string Name { get; set; }
        
        [XmlAttribute(AttributeName="od")]
        public DateTime FromDate { get; set; }
        
        [XmlAttribute(AttributeName="do")]
        public DateTime ToDate { get; set; }
    }

    [XmlRoot(ElementName="okres")]
    public class Period {
        [XmlAttribute(AttributeName="od")]
        public DateTime From { get; set; }
        
        [XmlAttribute(AttributeName="do")]
        public DateTime To { get; set; }
        
        [XmlAttribute(AttributeName="nazwa")]
        public string Name { get; set; }
        
        [XmlAttribute(AttributeName="wybrany")]
        public string Chosen { get; set; }
    }

    [XmlRoot(ElementName="zajecia")]
    public class ScheduleClass
    {
        private string _toHour;
        
        [XmlElement(ElementName="termin")]
        public DateTime Date { get; set; }
        
        [XmlElement(ElementName="dzien")]
        public string WeekDay { get; set; }
        
        [XmlElement(ElementName="od-godz")]
        public string FromHour { get; set; }

        [XmlElement(ElementName = "do-godz")]
        public string ToHour
        {
            get => _toHour;
            set => _toHour = value.Substring(0, 5);
        }
        
        [XmlElement(ElementName="przedmiot")]
        public string Subject { get; set; }
        
        [XmlElement(ElementName="typ")]
        public string Type { get; set; }
        
        [XmlElement(ElementName="nauczyciel")]
        public Teacher Teacher { get; set; }
        
        [XmlElement(ElementName="sala")]
        public string ClassRoom { get; set; }
        
        [XmlElement(ElementName="uwagi")]
        public string Notes { get; set; }
    }

    [XmlRoot(ElementName="nauczyciel")]
    public class Teacher {
        [XmlAttribute(AttributeName="moodle")]
        public string Moodle { get; set; }
        
        [XmlText]
        public string Text { get; set; }
    }
}