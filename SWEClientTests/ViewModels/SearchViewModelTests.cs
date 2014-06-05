using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWEClient.ViewModels;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace SWEClient.ViewModels.Tests
{
    [TestClass()]
    public class SearchViewModelTests
    {
        string xmlRechnung = @"<Search>
  <Rechnung />
</Search>";
        string xmlPerson = @"<Search>
  <Person />
</Search>";
        string xmlFirma = @"<Search>
  <Firma />
</Search>";

        [TestMethod()]
        public void SearchRechnungTest()
        {
            XElement xml;
            xml =
            new XElement("Search",
                new XElement("Rechnung")
                    );
            Assert.AreEqual(xml.ToString(), xmlRechnung);
        }

        [TestMethod()]
        public void SearchFirmaTest()
        {
            XElement xml;
            xml =
            new XElement("Search",
                new XElement("Firma")
                    );
            Assert.AreEqual(xml.ToString(), xmlFirma);
        }

        [TestMethod()]
        public void SearchPersonTest()
        {
            XElement xml;
            xml =
            new XElement("Search",
                new XElement("Person")
                    );
            Assert.AreEqual(xml.ToString(), xmlPerson);           
        }

        [TestMethod()]
        public void SearchRechnungTest2()
        {
            XElement xml;
            xml =
            new XElement("Search",
                new XElement("Rechnung")
                    );
            Assert.AreNotEqual(xml.ToString(), "");
        }

        [TestMethod()]
        public void SearchFirmaTest2()
        {
            XElement xml;
            xml =
            new XElement("Search",
                new XElement("Firma")
                    );
            Assert.AreNotEqual(xml.ToString(), "");
        }

        [TestMethod()]
        public void SearchPersonTest2()
        {
            XElement xml;
            xml =
            new XElement("Search",
                new XElement("Person")
                    );
            Assert.AreNotEqual(xml.ToString(), "");
        }
    }
}
