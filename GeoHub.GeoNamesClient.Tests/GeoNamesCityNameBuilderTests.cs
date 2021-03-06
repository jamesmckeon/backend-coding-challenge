﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using GeoHub.GeoNamesClient;
using GeoHub.Logic;

namespace GeoHub.GeoNamesClient.Tests
{

    [TestFixture]
    public class GeoNamesCityNameBuilderTests
    {
        protected GeoNamesCityNameBuilder Target { get; set; }

        [SetUp]
        public void SetUp()
        {
            Target = new GeoNamesCityNameBuilder();
        }

        [Test]
        public void BuildName_NotUs_SkipsState()
        {
            var data = new GeonameEntry() { Name = "Test Name", AdminCode1 = "1AAAA", CountryCode = "XX" };
            var expected = "Test Name, XX";

            Assert.AreEqual(expected, Target.BuildName(data));

        }

        [Test]
        public void BuildName_US_IncludesState()
        {
            var data = new GeonameEntry() { Name = "Test Name", AdminCode1 = "Test State", CountryCode = "US" };
            var expected = "Test Name, Test State, US";

            Assert.AreEqual(expected, Target.BuildName(data));

        }
    }
}
