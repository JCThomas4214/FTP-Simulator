using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace Stardome.UITest
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class LoginTest
    {
        public LoginTest()
        {
        }


        /// <summary>
        /// This method test for the login failure scenario
        /// </summary>
        [TestMethod]
        public void LoginFailure()
        {

            this.UIMap.LoginFailure();
            this.UIMap.LoginFailureAssert();
        }


        /// <summary>
        /// This method test for the successful login of Admin
        /// </summary>
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\LoginData.csv", "LoginData#csv", DataAccessMethod.Sequential), DeploymentItem("LoginData.csv"), TestMethod]
        public void AdminLoginSuccess()
        {
            this.UIMap.AdminLoginSuccess();
            this.UIMap.AssertAdminLogin();
            this.UIMap.AdminLogOff();

        }

        /// <summary>
        /// This method test for the successful login of Producer
        /// </summary>
        [TestMethod]
        public void ProducerLoginSuccess()
        {
            this.UIMap.ProducerLoginSuccess();
            this.UIMap.AssertProducerLoginSuccess();
            this.UIMap.ProducerLogOff();

        }

        /// <summary>
        /// This method test for the successful login of Clients
        /// </summary>
        [TestMethod]
        public void ClientLoginSuccess()
        {

            this.UIMap.ClientLoginSuccess();
            this.UIMap.AssertClientLogin();
            this.UIMap.ClientLogOff();

        }


        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
