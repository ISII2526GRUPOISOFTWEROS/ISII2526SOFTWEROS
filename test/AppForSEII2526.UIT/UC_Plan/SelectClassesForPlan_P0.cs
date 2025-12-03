using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class SelectClassesForPlan_P0 : PageObject
    {
        By Name = By.Id("className");
        public SelectClassesForPlan_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        { }
            public void SearchClasses(string name) {
                //wait for the webelement to be clickable
                WaitForBeingClickable(Name);
                _driver.FindElement(Name).SendKeys(name);
                    }
        }
    }

