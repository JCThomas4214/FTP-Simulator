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
    public class SiteSettingsTest
    {
        public SiteSettingsTest()
        {

        }

        [TestMethod]
        public void UpdateSiteSettingsTest()
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
            Keyboard.SendKeys(uIPassword, "password");

            UITestControl uILoginBtn = new UITestControl(browzer);
            uILoginBtn.TechnologyName = "Web";
            uILoginBtn.SearchProperties.Add("ControlType", "Button");
            uILoginBtn.SearchProperties.Add("Type", "submit");
            Mouse.Click(uILoginBtn);

           // Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.UIThreadOnly;
           
            HtmlHyperlink uIManageSettings = new HtmlHyperlink(browzer);
            uIManageSettings.TechnologyName = "Web";
            uIManageSettings.SearchProperties.Add("ControlType", "Hyperlink");
            uIManageSettings.SearchProperties.Add("TagName", "A");
            uIManageSettings.SearchProperties.Add("Innertext", "Manage Settings");
            Mouse.Click(uIManageSettings);

            UITestControl uISiteName = new UITestControl(browzer);
            uISiteName.TechnologyName = "Web";
            uISiteName.SearchProperties.Add("ControlType", "Edit");
            uISiteName.SearchProperties.Add("Name", "[0].Value");
            uISiteName.SetFocus();
            // Delete existing content in the control and insert new content.
            SendKeys.SendWait("^{HOME}");   // Move to start of control
            SendKeys.SendWait("^+{END}");   // Select everything
            SendKeys.SendWait("{DEL}");     // Delete selection
            Keyboard.SendKeys(uISiteName, "Stardome1");
            

            UITestControl uIFilePath = new UITestControl(browzer);
            uIFilePath.TechnologyName = "Web";
            uIFilePath.SearchProperties.Add("ControlType", "Edit");
            uIFilePath.SearchProperties.Add("Name", "[1].Value");
            uIFilePath.SetFocus();
            // Delete existing content in the control and insert new content.
            SendKeys.SendWait("^{HOME}");   // Move to start of control
            SendKeys.SendWait("^+{END}");   // Select everything
            SendKeys.SendWait("{DEL}");     // Delete selection
            Keyboard.SendKeys(uIFilePath, "C:\\Stardome1");

            UITestControl uIUpdate = new UITestControl(browzer);
            uILoginBtn.TechnologyName = "Web";
            uILoginBtn.SearchProperties.Add("ControlType", "Button");
            uILoginBtn.SearchProperties.Add("Type", "submit");
            uILoginBtn.SearchProperties.Add("DisplayText", "Update");
            Mouse.Click(uILoginBtn);

            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.AllThreads;

            UITestControl uIUpdateSuccessfully = new UITestControl(browzer);
            uIUpdateSuccessfully.TechnologyName = "Web";
            uIUpdateSuccessfully.SearchProperties.Add("ControlType", "Label");
            uIUpdateSuccessfully.SearchProperties.Add("LabelFor", "lblUpdateMesssage");
            uIUpdateSuccessfully.SearchProperties.Add("TagName", "LABEL");

            Assert.AreEqual("Label", uIUpdateSuccessfully.ControlType.ToString());

            HtmlHyperlink uILogOff = new HtmlHyperlink(browzer);
            uILogOff.TechnologyName = "Web";
            uILogOff.SearchProperties.Add("ControlType", "Hyperlink");
            uILogOff.SearchProperties.Add("TagName", "A");
            uILogOff.SearchProperties.Add("Innertext", "Log off");
            Mouse.Click(uILogOff);
           

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
