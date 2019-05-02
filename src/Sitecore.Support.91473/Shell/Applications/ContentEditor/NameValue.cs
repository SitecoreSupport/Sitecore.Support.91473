using Sitecore.Diagnostics;
using Sitecore.Text;
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace Sitecore.Support.Shell.Applications.ContentEditor
{
  [UsedImplicitly]
  public class NameValue : Sitecore.Shell.Applications.ContentEditor.NameValue
  {

    public NameValue()
    {
    }

    private void LoadValue()
    {
      if (!this.ReadOnly && !this.Disabled)
      {
        Page handler = HttpContext.Current.Handler as Page;
        NameValueCollection values = (handler == null) ? new NameValueCollection() : handler.Request.Form;
        UrlString str = new UrlString();
        foreach (string str3 in values.Keys)
        {
          int num1;
          if (string.IsNullOrEmpty(str3) || !str3.StartsWith(this.ID + "_Param", StringComparison.InvariantCulture))
          {
            num1 = 0;
          }
          else
          {
            num1 = System.Convert.ToInt32(!str3.EndsWith("_value", StringComparison.InvariantCulture));            
          }
          if (num1 != 0)
          {
            string str4 = values[str3];
            string str5 = values[str3 + "_value"];
            if (!string.IsNullOrEmpty(str4))
            {
              str[Regex.Replace(str4, @"\W", "_")] = str5 ?? string.Empty;
            }
          }
        }
        string str2 = HttpUtility.UrlDecode(str.ToString());
        if (this.Value != str2)
        {
          this.Value = str2;
          this.SetModified();
        }
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull(e, "e");
      if (!Sitecore.Context.ClientPage.IsEvent)
      {
        base.OnLoad(e);
      }
      else
      {
        this.Value = HttpUtility.UrlDecode(this.Value);
        this.LoadValue();
      }
    }
       
  }  
}