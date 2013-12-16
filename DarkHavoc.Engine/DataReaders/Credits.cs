using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DarkHavoc.Engine.DataReaders
{
    public class CreditsTask
    {
        public string Title;
        public string Name;

        public CreditsTask(string title, string name)
        {
            Title = title;
            Name = name;
        }
    }

    public class CreditsMajorTask
    {
        public string Name;
        public List<CreditsTask> Tasks;

        public CreditsMajorTask(string name, List<CreditsTask> tasks)
        {
            Name = name;
            Tasks = tasks;
        }
    }

    public class CreditsSpecialThanks
    {
        public List<string> Thanks;

        public CreditsSpecialThanks(List<string> thanks)
        {
            Thanks = thanks;
        }
    }

    public class Credits
    {
        private string fileName;
        private XDocument document;

        public string Title;
        public string Subtitle;

        public bool ShowLogo;

        public List<CreditsMajorTask> MajorTasks;

        public CreditsSpecialThanks SpecialThanks;

        public Credits(string file)
        {
            fileName = file;

            document = XDocument.Load(file);

            var creditsRoot = document.Element("JoshoCredits");

            ShowLogo = Convert.ToBoolean(creditsRoot.Attribute("ShowLogo").Value);

            Title = creditsRoot.Element("Title").Attribute("Text").Value;
            Subtitle = creditsRoot.Element("Subtitle").Attribute("Text").Value;

            MajorTasks = new List<CreditsMajorTask>();

            var majorTasksDescendants = creditsRoot.Descendants("MajorTask");

            foreach (var element in majorTasksDescendants)
            {
                string majorTaskTitle = element.Attribute("Title").Value;

                var tasksElementDescendants = element.Descendants("Task");

                List<CreditsTask> tempCreditsTaskList = new List<CreditsTask>();

                foreach (var task in tasksElementDescendants)
                {
                    string taskTitle = task.Attribute("Title").Value;
                    string taskName = task.Attribute("Name").Value;

                    CreditsTask myTask = new CreditsTask(taskTitle, taskName);

                    tempCreditsTaskList.Add(myTask);
                }

                CreditsMajorTask majorTask = new CreditsMajorTask(majorTaskTitle, tempCreditsTaskList);

                MajorTasks.Add(majorTask);
            }

            var specialThanksDescendants = creditsRoot.Descendants("SpecialThanks");

            List<string> specialThanksContents = new List<string>();

            foreach (var element in specialThanksDescendants)
            {
                specialThanksContents.Add(element.Value);
            }

            SpecialThanks = new CreditsSpecialThanks(specialThanksContents);
        }
    }
}
