using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class CreatePlan_P0 : PageObject
    {
        By inputName = By.Id("planName");
        By inputDescription = By.Id("planDescription");
        By inputWeeks = By.Id("planWeeks");
        By inputHealthIssues = By.Id("planHealth");
        By buttonSubmit = By.Id("Submit");
        By buttonModifyClasses = By.Id("buttonModifyClasses");
        By tableSelectedClasses = By.Id("tableOfPlanClasses");
        By errorPanel = By.Id("errorsShown");
        By buttonDialogOK = By.Id("Button_DialogOK");

        public CreatePlan_P0(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void FillPlanData(string name, string description, int weeks, string healthIssues)
        {
            _output.WriteLine("Robot: Iniciando carga de datos del plan...");
            
            System.Threading.Thread.Sleep(2000);

            Action<By, string, string> BlazorSendKeys = (locator, value, fieldName) =>
            {
                try
                {
                    _output.WriteLine($"   > Escribiendo en {fieldName}: {value}");

                    var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                    var element = wait.Until(d => d.FindElement(locator));

                    IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                    js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
                    System.Threading.Thread.Sleep(500); 
                    js.ExecuteScript("arguments[0].click();", element);
                    js.ExecuteScript("arguments[0].focus();", element);

                    element.SendKeys(Keys.Control + "a");
                    element.SendKeys(Keys.Backspace);
                    element.SendKeys(value);
                    element.SendKeys(Keys.Tab);
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Error en campo {fieldName}: {ex.Message}");
                    throw;
                }
            };

            BlazorSendKeys(By.Id("planName"), name, "Nombre");

            if (!string.IsNullOrEmpty(description))
            {
                BlazorSendKeys(By.Id("planDescription"), description, "Descripción");
            }
            else
            {
                var el = _driver.FindElement(By.Id("planDescription"));
                el.Click();
                el.SendKeys(Keys.Control + "a");
                el.SendKeys(Keys.Backspace);
                el.SendKeys(Keys.Tab);
            }

            BlazorSendKeys(By.Id("planWeeks"), weeks.ToString(), "Semanas");
            if (!string.IsNullOrWhiteSpace(healthIssues))
            {
                try
                {
                    // Solo entra aquí si realmente hay texto que escribir
                    BlazorSendKeys(By.Id("planHealthIssues"), healthIssues, "Salud");
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Aviso: No se encontró el campo opcional de salud: {ex.Message}");
                }
            }
            else
            {
                _output.WriteLine("Salud es null o vacío, saltando campo...");
            }
        }

        public void SubmitPlan()
        {
            _output.WriteLine("Intentando pulsar el botón de Create Plan...");
         By submitBtn = By.XPath("//button[@id='Submit' or contains(text(), 'Create Plan') or contains(@class, 'btn-primary')]");

            try
            {
                System.Threading.Thread.Sleep(1000);

                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var element = wait.Until(d => d.FindElement(submitBtn));

                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                System.Threading.Thread.Sleep(500);
                js.ExecuteScript("arguments[0].click();", element);

                _output.WriteLine("Botón Create Plan pulsado con éxito.");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error fatal al buscar el botón de envío: {ex.Message}");
                throw;
            }
        }

       public void ConfirmPlanSubmission()
{
    _output.WriteLine("🤖 Robot: Buscando el botón de confirmación por texto exacto...");

    // 1. Espera necesaria para que el diálogo termine de dibujarse
    System.Threading.Thread.Sleep(2500); 

    try 
    {
        // 2. Buscamos un botón que diga EXACTAMENTE "Save" (o el texto de tu botón)
        // El punto (.) indica que buscamos el texto dentro del botón.
        // Solo buscamos dentro de la clase modal para no irnos a la Home.
        By confirmBtnXPath = By.XPath("//div[contains(@class, 'modal-content')]//button[normalize-space(.)='Save' or normalize-space(.)='Ok' or normalize-space(.)='Proceed']");

        var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        var element = wait.Until(d => {
            var el = d.FindElement(confirmBtnXPath);
            return (el.Displayed && el.Enabled) ? el : null;
        });

        _output.WriteLine($"   > ¡Botón '{element.Text}' localizado! Clicando...");

        // 3. Click con JavaScript (el más fiable para evitar redirecciones raras)
        IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
        js.ExecuteScript("arguments[0].click();", element);
        
        _output.WriteLine("✅ ¡Click en confirmación realizado!");
        
        // 4. Pausa para que la API responda antes de que el Test mire la URL
        System.Threading.Thread.Sleep(4000); 
    }
    catch (Exception ex)
    {
        _output.WriteLine($"❌ Error: No se pudo clicar el botón. {ex.Message}");
        // Si falla, intentamos pulsar Enter, que en los diálogos suele ser el botón por defecto
        new OpenQA.Selenium.Interactions.Actions(_driver).SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
    }
}

        public void ModifyClasses()
        {
            
            By modifySelector = By.XPath("//button[contains(@id, 'buttonModifyClasses') or contains(text(), 'Modify') or contains(text(), 'Back')]");

            try
            {
                
                System.Threading.Thread.Sleep(2000);

                var element = _driver.FindElement(modifySelector);
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                js.ExecuteScript("arguments[0].click();", element);

                _output.WriteLine("Botón Modificar pulsado con éxito.");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"No se encontró el botón de modificar: {ex.Message}");
                throw;
            }
        }
        public bool CheckListOfItems(List<string[]> expectedItems)
        {
            return CheckBodyTable(expectedItems, tableSelectedClasses);
        }

        public string GetErrorMessage()
        {
            try
            {
                WaitForBeingVisible(errorPanel);
                return _driver.FindElement(errorPanel).Text;
            }
            catch (WebDriverTimeoutException)
            {
                return "";
            }
        }
    }
}