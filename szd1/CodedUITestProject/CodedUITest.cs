using System;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Input;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
using Microsoft.VisualStudio.TestTools.UITesting.DirectUIControls;
using Microsoft.VisualStudio.TestTools.UITesting.WindowsRuntimeControls;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace CodedUITestProject
{
    /// <summary>
    /// Summary description for CodedUITest1
    /// </summary>
    [CodedUITest(CodedUITestType.WindowsStore)]
    public class CodedUITest
    {
        public CodedUITest()
        {
        }

        [TestMethod]
        public void CodedUITestMethod1()
        {
            //var app = XamlWindow.Launch("tile-P~b07035b1-30d9-4a7c-81de-b16376ee8713_vw2e7bp5pxcky!App");
            Gesture.Tap(UIMap.UISzd1Window.UIFillominoButtonButton);
            Gesture.Tap(UIMap.UISzd1Window.UILevelChooserComboBox);
            UIMap.UISzd1Window.UILevelChooserComboBox.SelectedItem = "5x5";
            Gesture.Tap(UIMap.UISzd1Window.UIReloadButton);
            Gesture.Tap(UIMap.UISzd1Window.UIAlgorithmChooserComboBox);
            UIMap.UISzd1Window.UIAlgorithmChooserComboBox.SelectedItem = "Backtrack";
            Gesture.Tap(UIMap.UISzd1Window.UISolveButton);
            Gesture.Tap(UIMap.UISzd1Window.UIBackButton);
            Gesture.Tap(UIMap.UISzd1Window.UISokobanButtonButton);
            Gesture.Tap(UIMap.UISzd1Window.UILevelChooserComboBox);
            UIMap.UISzd1Window.UILevelChooserComboBox.SelectedItem = "0";
            Gesture.Tap(UIMap.UISzd1Window.UIReloadButton);
            Gesture.Tap(UIMap.UISzd1Window.UIAlgorithmChooserComboBox);
            UIMap.UISzd1Window.UIAlgorithmChooserComboBox.SelectedItem = "Q-Learning";
            Gesture.Tap(UIMap.UISzd1Window.UISolveButton);
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
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }
        private TestContext testContextInstance;

        public UIMap UIMap {
            get {
                if (this.map == null)
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
