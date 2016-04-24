using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.Controllers;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Migrations;
using apartmenthostService.Models;
using apartmenthostService.Tests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TestContext = apartmenthostService.Tests.Infrastructure.TestContext;

namespace apartmenthostService.Tests.ControllersTest
{
    /// <summary>
    /// Summary description for TestArticleController
    /// </summary>
    [TestClass]
    public class TestArticleController
    {

        private readonly IApartmenthostContext _testContext;
        private TestDbPopulator _testDb;

        public TestArticleController()
        {
          _testContext = new TestContext();
          _testDb = new TestDbPopulator(_testContext);
          }

        
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// TEST POST api/Articles/
        /// Должен возвращать ОК и Article.Id при успехе
        /// </summary>
        [TestMethod]
        public void PostArticle_ShouldReturnOkResult()
        {
            var controller = new ArticleApiController(_testContext)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var item = _testDb.GetTestArticleDto();

            var result = TestHelper.ParseResponse(controller.PostArticle(item));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode,HttpStatusCode.OK.ToString());
            Assert.AreEqual(result.IsSuccessStatusCode,true);
            Assert.AreEqual(result.ResponseCode,RespH.SRV_CREATED);
            Assert.IsNotNull(result.ResponseData[0]);
        }

        /// <summary>
        /// TEST POST api/Articles/
        /// Должен возвращать BadRequest если не указан входной объект
        /// </summary>
        [TestMethod]
        public void PostArticle_ShoulReturnBadResulIfObjectIsNull()
        {
            var controller = new ArticleApiController(_testContext)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var result = TestHelper.ParseResponse(controller.PostArticle(null));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode,HttpStatusCode.BadRequest.ToString());
            Assert.AreEqual(result.IsSuccessStatusCode,false);
            Assert.AreEqual(result.ResponseCode,RespH.SRV_ARTICLE_NULL);
        }

        /// <summary>
        /// TEST POST api/Articles/
        /// Должен возвращать BadRequest со списком полей
        /// если не укзаны Name, Title, Text, Type, Lang
        /// </summary>
        [TestMethod]
        public void PostArticle_ShoulReturnBadResulIfLackOfFields()
        {
            var controller = new ArticleApiController(_testContext)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            var item = _testDb.GetTestArticleDto();
            item.Name = null;
            item.Text = null;
            item.Title = null;
            item.Type = null;
            item.Lang = null;
            var result = TestHelper.ParseResponse(controller.PostArticle(item));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest.ToString());
            Assert.AreEqual(result.IsSuccessStatusCode, false);
            Assert.AreEqual(result.ResponseCode, RespH.SRV_ARTICLE_REQUIRED);
            Assert.IsTrue(result.ResponseData.Contains("Name"));
            Assert.IsTrue(result.ResponseData.Contains("Title"));
            Assert.IsTrue(result.ResponseData.Contains("Text"));
            Assert.IsTrue(result.ResponseData.Contains("Type"));
            Assert.IsTrue(result.ResponseData.Contains("Lang"));
        }

        [TestMethod]
        public void Test()
        {
            
           

        }
    }
}
