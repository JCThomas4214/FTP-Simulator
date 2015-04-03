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
    public class UserProfileTest
    {
        public UserProfileTest()
        {
        }

        [TestMethod]
        public void ProfileResetPassword()
        {

            
            BrowserWindow browzer = BrowserWindow.Launch("http://localhost:2129/");
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            this.UIMap.ProfilePasswordReset();
        }


        [TestMethod]
        public void ForgotPasswordTest()
        {
            BrowserWindow browzer = BrowserWindow.Launch("http://localhost:2129/");
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            this.UIMap.ForgotPassword();
            this.UIMap.ForgotPasswordAssert();
        }

        [TestMethod]
        public void ForgotPasswordInvalidEmailTest()
        {
            BrowserWindow browzer = BrowserWindow.Launch("http://localhost:2129/");
            Playback.PlaybackSettings.WaitForReadyLevel = WaitForReadyLevel.Disabled;
            this.UIMap.ForgotPasswordInvalidEmail();
            this.UIMap.ForgotPasswordInvalidEmailAssert();

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
