using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace Embervalle.Core.Localization;


internal class LocalizationManager
{
    
    
    public const string DEFAULT_CULTURE_CODE = "en-EN";

    
    public static List<CultureInfo> GetSupportedCultures()
    {
        
        List<CultureInfo> supportedCultures = new List<CultureInfo>();

        
        Assembly assembly = Assembly.GetExecutingAssembly();

        
        ResourceManager resourceManager = new ResourceManager("Embervalle.Core.Localization.Resources", assembly);

        
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

        foreach (CultureInfo culture in cultures)
        {
            try
            {
                
                var resourceSet = resourceManager.GetResourceSet(culture, true, false);
                if (resourceSet != null)
                {
                    supportedCultures.Add(culture);
                }
            }
            catch (MissingManifestResourceException)
            {
                
            }
        }

        
        supportedCultures.Add(CultureInfo.InvariantCulture);

        return supportedCultures;
    }

    
    public static void SetCulture(string cultureCode)
    {
        if (string.IsNullOrEmpty(cultureCode))
            throw new ArgumentNullException(nameof(cultureCode), "A culture code must be provided.");

        
        CultureInfo culture = new CultureInfo(cultureCode);

        
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}