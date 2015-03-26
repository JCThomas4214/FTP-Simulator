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
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;


namespace Stardome.UITest
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest]
    public class AuthenticationTests
    {
        public AuthenticationTests()
        {
        }

        [TestMethod]
        public void LoginFailure()
        {
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            BrowserWindow browzer = BrowserWindow.Launch("http://localhost:2129/");
            
            UITestControl uIUserName = new UITestControl(browzer);
            uIUserName.TechnologyName = "Web";
            uIUserName.SearchProperties.Add("ControlType", "Edit");
            uIUserName.SearchProperties.Add("Id", "UserName");
            Keyboard.SendKeys(uIUserName, "ab");

            UITestControl uIPassword = new UITestControl(browzer);
            uIPassword.TechnologyName = "Web";
            uIPassword.SearchProperties.Add("ControlType", "Edit");
            uIPassword.SearchProperties.Add("Id", "Password");
            Keyboard.SendKeys(uIPassword, "123456");

            UITestControl uILoginBtn = new UITestControl(browzer);
            uILoginBtn.TechnologyName = "Web";
            uILoginBtn.SearchProperties.Add("ControlType", "Button");
            uILoginBtn.SearchProperties.Add("Type", "submit");
            Mouse.Click(uILoginBtn);


            HtmlCustom uILoginFailure = new HtmlCustom(browzer);
            uILoginFailure.TechnologyName = "Web";
            uILoginFailure.SearchProperties.Add("ControlType", "Custom");
            uILoginFailure.SearchProperties.Add("TagName", "LI");
           
            Assert.AreEqual("LI", uILoginFailure.TagName);

        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\LoginData.csv", "LoginData#csv", DataAccessMethod.Sequential), DeploymentItem("LoginData.csv"), TestMethod]
        public void LoginSuccess()
        {
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            BrowserWindow browzer = BrowserWindow.Launch("http://localhost:2129/");

            UITestControl uIUserName = new UITestControl(browzer);
            uIUserName.TechnologyName = "Web";
            uIUserName.SearchProperties.Add("ControlType", "Edit");
            uIUserName.SearchProperties.Add("Id", "UserName");
            Keyboard.SendKeys(uIUserName, TestContext.DataRow[0].ToString());

            UITestControl uIPassword = new UITestControl(browzer);
            uIPassword.TechnologyName = "Web";
            uIPassword.SearchProperties.Add("ControlType", "Edit");
            uIPassword.SearchProperties.Add("Id", "Password");
            Keyboard.SendKeys(uIPassword, TestContext.DataRow[1].ToString());

            UITestControl uILoginBtn = new UITestControl(browzer);
            uILoginBtn.TechnologyName = "Web";
            uILoginBtn.SearchProperties.Add("ControlType", "Button");
            uILoginBtn.SearchProperties.Add("Type", "submit");
            Mouse.Click(uILoginBtn);

            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.UIThreadOnly;
            HtmlHyperlink uIAssertUserName = new HtmlHyperlink(browzer);
            uIAssertUserName.TechnologyName = "Web";
            uIAssertUserName.SearchProperties.Add("ControlType", "Hyperlink");
            uIAssertUserName.SearchProperties.Add("TagName", "A");
            Assert.AreEqual(TestContext.DataRow[0].ToString(), uIAssertUserName.InnerText);

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
    }
}
